using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Nodes;

public class EnderecoService
{
    private static readonly HttpClient httpClient = new HttpClient();

    public async Task<string> BuscarCoordenadasAsync(string json)
    {
        try
        {
            // Parse como JsonNode para poder alterar o JSON
            var root = JsonNode.Parse(json)?.AsObject();
            if (root is null || root["endereco"] is null) return json;

            var endereco = root["endereco"]!.AsObject();

            string rua = endereco["rua"]?.ToString()?.Trim() ?? "";
            string numero = endereco["numero"]?.ToString()?.Trim() ?? "";
            string bairro = endereco["bairro"]?.ToString()?.Trim() ?? "";
            string cidade = endereco["cidade"]?.ToString()?.Trim() ?? "";
            string cep = endereco["cep"]?.ToString()?.Trim() ?? "";

            string enderecoCompleto = cep.StartsWith("00000")
                ? $"{rua}, {numero} - {bairro} - {cidade}"
                : $"{rua}, {numero} - {bairro} - {cidade} - {cep}";

            string enderecoEncoded = Uri.EscapeDataString(enderecoCompleto);

            string url = $"https://zippy-api.onrender.com/api/Localizacao?endereco={enderecoEncoded}";
            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("accept", "*/*");

            var response = await httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
                return json; // Mantém o original se a API falhar

            string resultado = await response.Content.ReadAsStringAsync();

            // Tenta extrair latitude/longitude
            var geoJson = JsonDocument.Parse(resultado);
            var geoRoot = geoJson.RootElement;

            if (geoRoot.TryGetProperty("latitude", out var latProp) &&
                geoRoot.TryGetProperty("longitude", out var lonProp))
            {
                var coordenadas = new JsonObject
                {
                    ["latitude"] = latProp.GetDouble(),
                    ["longitude"] = lonProp.GetDouble()
                };

                // Substitui ou cria o campo "coordenadas"
                root["coordenadas"] = coordenadas;
            }

            return root.ToJsonString(new JsonSerializerOptions { WriteIndented = true });
        }
        catch (Exception ex)
        {
            return $"{{ \"erro\": \"{ex.Message}\" }}";
        }
    }

    private string RemoverCepInvalido(string endereco)
    {
        var partes = endereco.Split('-');
        if (partes.Length == 3 && partes[2].Trim().StartsWith("00000"))
        {
            return $"{partes[0].Trim()} - {partes[1].Trim()}";
        }
        return endereco;
    }
}

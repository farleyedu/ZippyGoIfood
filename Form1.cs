using Microsoft.Web.WebView2.Core;
using System;
using System.Text.Json;
using System.Windows.Forms;

namespace ZippyGoIfood
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            var env = await CoreWebView2Environment.CreateAsync(null, "UserData", null);
            await webView21.EnsureCoreWebView2Async(env);

            webView21.CoreWebView2.WebMessageReceived += WebView_WebMessageReceived;

            webView21.CoreWebView2.Navigate("https://gestordepedidos.ifood.com.br/#/home/orders");
        }

        private async void WebView_WebMessageReceived(object sender, CoreWebView2WebMessageReceivedEventArgs e)
        {
            string json = e.WebMessageAsJson;

            var confirm = MessageBox.Show("Deseja enviar esse pedido para a API?", "Confirmação", MessageBoxButtons.YesNo);
            if (confirm == DialogResult.Yes)
            {
                await EnviarPedidoParaApiAsync(json);
            }
        }

        private async void btnCapturar_Click(object sender, EventArgs e)
        {
            string script = System.IO.File.ReadAllText(@"D:\TI\Aplicativos\back end\ZippyGoIfood\capturaPedido.js");
            var DadosPedido = await webView21.CoreWebView2.ExecuteScriptAsync(script);
        }

        private async Task EnviarPedidoParaApiAsync(string json)
        {
            using var client = new HttpClient();
//            string json1 = @"{
//  ""id"": null,
//  ""peidoIdIfood"": ""8387"",
//  ""displayId"": ""8387"",
//  ""criadoEm"": null,
//  ""previsaoEntrega"": ""2025-06-12T00:41:00.000Z"",
//  ""horarioEntrega"": ""2025-06-12T00:41:00.000Z"",
//  ""horarioSaida"": null,
//  ""codigoRetirada"": ""9541 5464"",
//  ""coordenadas"": {
//    ""latitude"": 1,
//    ""longitude"": 2
//  },
//  ""cliente"": {
//    ""nome"": ""Farley Eduardo"",
//    ""telefone"": """",
//    ""documento"": """",
//    ""localizador"": """"
//  },
//  ""endereco"": {
//    ""rua"": ""Alameda dos Mandarins, 500 - Gran Ville - Uberlândia●00000000\nBloco 05 AP 08"",
//    ""numero"": """",
//    ""bairro"": """",
//    ""cidade"": ""Uberlândia"",
//    ""estado"": """",
//    ""cep"": """",
//    ""complemento"": """"
//  },
//  ""itens"": [
//    {
//      ""nome"": ""Molho de alho"",
//      ""quantidade"": 1,
//      ""precoUnitario"": 0.1,
//      ""precoTotal"": 0.1
//    }
//  ]
//}";

            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

            try
            {
                 var response = await client.PostAsync("https://localhost:7137/api/Pedido/PedidoIfood", content);
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("✅ Pedido enviado com sucesso!");
                }
                else
                {
                    var erro = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"❌ Erro ao enviar: {response.StatusCode}\n{erro}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ Exceção:\n" + ex.Message);
            }
        }
    }
}

using Microsoft.Web.WebView2.Core;
using System;
using System.Text.Json;
using System.Windows.Forms;

namespace ZippyGoIfood
{
    public partial class Form1 : Form
    {
        readonly EnderecoService enderecoService;
        public Form1(EnderecoService enderecoService)
        {
            this.enderecoService = enderecoService ?? throw new ArgumentNullException(nameof(enderecoService));
            InitializeComponent();
        }
        private async void Form1_Load(object sender, EventArgs e)
        {
            var env = await CoreWebView2Environment.CreateAsync(null, "UserData", null);
            await webView21.EnsureCoreWebView2Async(env);

            webView21.CoreWebView2.WebMessageReceived += WebView_WebMessageReceived;

            webView21.CoreWebView2.Navigate("https://gestordepedidos.ifood.com.br/#/home/orders/now");
        }

        private async void WebView_WebMessageReceived(object sender, CoreWebView2WebMessageReceivedEventArgs e)
        {
            string json = e.WebMessageAsJson;

            var confirm = MessageBox.Show("Deseja enviar esse pedido para a API?", "Confirmação", MessageBoxButtons.YesNo);
            if (confirm == DialogResult.Yes)
            {
                var updatejsonCord = await enderecoService.BuscarCoordenadasAsync(json);
                await EnviarPedidoParaApiAsync(updatejsonCord);
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

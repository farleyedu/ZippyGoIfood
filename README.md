# 🤖 ZippyGo Ifood Robot

Este projeto é um **robô em Windows Forms (.NET 8)** que acessa automaticamente o painel do iFood, extrai os dados dos pedidos aceitos e envia para a API da plataforma ZippyGo.

---

## 🚀 Funcionalidades

- Abre o painel do iFood automaticamente com login já salvo.
- Extrai dados de pedidos aceitos e cancelados.
- Lê nome do cliente, endereço, horário, itens e coordenadas.
- Envia os dados para a API ZippyGo via POST.

---

## 🧱 Tecnologias

- C# com Windows Forms (.NET 8)
- WebView2 (Microsoft Edge)
- JavaScript injetado para scraping do DOM
- Integração com API via `HttpClient`

---

## 🔗 Endpoints

### Envio de pedidos para API:
POST https://zippy-api.onrender.com/api/Pedido/PedidoIfood

css
Copiar
Editar

**Exemplo de payload enviado:**
```json
{
  "peidoIdIfood": "8387",
  "displayId": "8387",
  "previsaoEntrega": "2025-06-12T00:41:00.000Z",
  "horarioEntrega": "2025-06-12T00:41:00.000Z",
  "codigoRetirada": "9541 5464",
  "coordenadas": {
    "latitude": -19.933,
    "longitude": -43.937
  },
  "cliente": {
    "nome": "Farley Eduardo",
    "telefone": "",
    "documento": "",
    "localizador": ""
  },
  "endereco": {
    "rua": "Alameda dos Mandarins, 500 - Gran Ville -"
  },
  "itens": [
    {
      "nome": "Pizza Calabresa",
      "quantidade": 1
    }
  ]
}
🧪 Como usar
Clone o repositório:

bash
Copiar
Editar
git clone https://github.com/farleyedu/ZippyGoIfood.git
Abra com o Visual Studio.

Compile e rode o projeto.

O app abrirá o painel do iFood automaticamente e começará a ler os pedidos em tempo real.

⚠️ Observações
O login precisa estar salvo previamente no navegador do WebView2.

A comunicação é feita via HttpClient, com logs no terminal indicando sucesso ou falha.

Os cookies podem ser lidos automaticamente ou importados via arquivo .json.

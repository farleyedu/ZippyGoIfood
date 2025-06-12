(function () {
    const pedido = {
        id: null,
        peidoIdIfood: null,
        displayId: null,
        criadoEm: null,
        previsaoEntrega: null,
        horarioEntrega: null,
        horarioSaida: null,
        codigoRetirada: null,
        coordenadas: null,
        cliente: {},
        endereco: {},
        itens: []
    };

    const titulo = document.querySelector('h2.BaseHeading-sc-x85g2i-0');
    if (titulo) {
        const texto = titulo.innerText.trim();
        const partes = texto.split(' ');
        pedido.displayId = partes[0];
        pedido.peidoIdIfood = partes[0].replace('#', '');
        pedido.cliente.nome = partes.slice(1).join(' ');
    }

    const feitoMatch = document.body.innerText.match(/Feito às (\d{2}:\d{2})/);
    if (feitoMatch) {
        const [hora, minuto] = feitoMatch[1].split(':');
        const agora = new Date();
        agora.setHours(parseInt(hora), parseInt(minuto), 0, 0);
        pedido.criadoEm = agora.toISOString();
    }

    const entrega = document.querySelector('b.sc-fGdiLE');
    if (entrega) {
        const [h, m] = entrega.innerText.trim().split(':');
        const dt = new Date();
        dt.setHours(parseInt(h), parseInt(m), 0, 0);
        pedido.previsaoEntrega = dt.toISOString();
        pedido.horarioEntrega = dt.toISOString();
    }

    const enderecoSpan = Array.from(document.querySelectorAll('span'))
        .find(el => el.innerText.includes('Gran Ville'));
    if (enderecoSpan) {
        pedido.endereco = {
            rua: enderecoSpan.innerText.trim(),
            bairro: '',
            numero: '',
            cidade: 'Uberlândia',
            complemento: ''
        };
    }

    const cod = document.querySelector('[data-test-id="localizer-id"] b');
    if (cod) pedido.codigoRetirada = cod.innerText.trim();

    const itens = [];
    const itensList = document.querySelectorAll('li.sc-biptUy');
    itensList.forEach(li => {
        const nome = li.querySelector('.SectionItem__label--bold')?.innerText.trim();
        const preco = li.querySelector('[data-test-id="item-price"]')?.innerText.trim().replace('R$', '').replace(',', '.');
        if (nome && preco) {
            itens.push({
                nome: nome,
                preco: parseFloat(preco),
                quantidade: 1
            });
        }
    });
    pedido.itens = itens;

    window.chrome.webview.postMessage(pedido);
})();

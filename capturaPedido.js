(function () {
    const pedido = {
        id: null,
        pedidoIdIfood: null,
        displayId: null,
        criadoEm: null,
        previsaoEntrega: null,
        horarioEntrega: null,
        horarioSaida: null,
        localizador: null,
        coordenadas: null,
        cliente: {},
        endereco: {},
        itens: [],
        tipoPagamento: null
    };

    // ID do pedido + nome do cliente
    const titulo = document.querySelector('h2.BaseHeading-sc-x85g2i-0');
    if (titulo) {
        const texto = titulo.innerText.trim();
        const partes = texto.split(' ');
        pedido.displayId = partes[0];
        pedido.pedidoIdIfood = partes[0].replace('#', '');
        pedido.cliente.nome = partes.slice(1).join(' ');
    }

    // Horário de criação do pedido (convertido para UTC)
    const feitoMatch = document.body.innerText.match(/Feito às (\d{2}):(\d{2})/);
    if (feitoMatch) {
        const local = new Date();
        local.setHours(parseInt(feitoMatch[1]), parseInt(feitoMatch[2]), 0, 0);
        pedido.criadoEm = new Date(local.getTime() - 3 * 60 * 60 * 1000).toISOString();
    }

    // Telefone via regex
    const telMatch = document.body.innerText.match(/(\d{4} \d{3} \d{4})/);
    if (telMatch) {
        pedido.cliente.telefone = telMatch[1];
    }

    // Previsão de entrega (convertido para UTC)
    const entrega = document.querySelector('b.sc-fGdiLE');
    if (entrega) {
        const [h, m] = entrega.innerText.trim().split(':');
        const local = new Date();
        local.setHours(parseInt(h), parseInt(m), 0, 0);
        const utc = new Date(local.getTime() - 3 * 60 * 60 * 1000);
        pedido.previsaoEntrega = utc.toISOString();
        pedido.horarioEntrega = utc.toISOString();
    }

    // Endereço
    const inlineItems = document.querySelectorAll('.DetailsDeliverySectionItem .InlineList .InlineItem');
    if (inlineItems.length >= 2) {
        // Endereço principal
        let textoEndereco = '';
        inlineItems[0].childNodes.forEach(node => {
            if (node.nodeType === Node.TEXT_NODE) {
                textoEndereco += node.textContent;
            }
        });
        textoEndereco = textoEndereco.trim();

        const partes = textoEndereco.split(' - ');
        if (partes.length === 3) {
            const [ruaNumero, bairro, cidade] = partes;
            const match = ruaNumero.match(/^(.*?),\s*(\d+)$/);
            if (match) {
                pedido.endereco.rua = match[1];
                pedido.endereco.numero = match[2];
                pedido.endereco.bairro = bairro.trim();
                pedido.endereco.cidade = cidade.trim();
            }
        }

        // CEP
        const cepSpan = inlineItems[1].querySelector('span');
        if (cepSpan) {
            pedido.endereco.cep = cepSpan.innerText.trim();
        }

        // Complemento
        const complementoItem = document.querySelectorAll('.DetailsDeliverySectionItem .InlineList')[1]?.querySelector('.InlineItem');
        if (complementoItem) {
            let textoComp = '';
            complementoItem.childNodes.forEach(node => {
                if (node.nodeType === Node.TEXT_NODE) {
                    textoComp += node.textContent;
                }
            });
            pedido.endereco.complemento = textoComp.trim();
        }
    }

    // Código de retirada
    const cod = document.querySelector('[data-test-id="localizer-id"] b');
    if (cod) {
        pedido.localizador = cod.innerText.trim();
    }

    // Itens do pedido
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

    // Tipo de pagamento
    const pagamentoSection = document.querySelector('.Summary__item--expandable .InlineList');
    if (pagamentoSection) {
        const pagamentoText = Array.from(pagamentoSection.querySelectorAll('.InlineItem'))
            .map(el => el.childNodes[0]?.textContent.trim())
            .filter(Boolean)
            .join(' - ');
        pedido.tipoPagamento = pagamentoText;
    }

    // Envia o pedido
    window.chrome.webview.postMessage(pedido);
})();

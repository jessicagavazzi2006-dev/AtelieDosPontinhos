// Chave usada para salvar o carrinho no navegador
const CART_STORAGE_KEY = 'AtelieDosPontinhos_Cart';

// 1. Obter os itens atuais do carrinho
function getCart() {
    const cart = localStorage.getItem(CART_STORAGE_KEY);
    return cart ? JSON.parse(cart) : [];
}

// 2. Adicionar um produto ao carrinho (Sem mexer na API por enquanto)
function adicionarAoCarrinho(productId, name, price, imageUrl) {
    let cart = getCart();

    // Verifica se o produto já está no carrinho
    const existingItem = cart.find(item => item.productId === productId);

    if (existingItem) {
        existingItem.quantity += 1;
    } else {
        cart.push({
            productId: productId,
            name: name,
            price: price,
            imageUrl: imageUrl,
            quantity: 1
        });
    }

    localStorage.setItem(CART_STORAGE_KEY, JSON.stringify(cart));
    alert(`${name} adicionado ao carrinho!`);
    atualizarContadorCarrinho();
}

// 3. Atualizar o contador visual no menu superior (opcional)
function atualizarContadorCarrinho() {
    const cart = getCart();
    const totalItens = cart.reduce((sum, item) => sum + item.quantity, 0);
    const badge = document.getElementById('cart-badge');
    if (badge) {
        badge.innerText = totalItens;
    }
}

// Chame a função ao carregar a página para atualizar o ícone do topo
document.addEventListener("DOMContentLoaded", atualizarContadorCarrinho);
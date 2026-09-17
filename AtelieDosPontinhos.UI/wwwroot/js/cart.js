// Chave usada para salvar o carrinho no navegador
const CART_STORAGE_KEY = 'AtelieDosPontinhos_Cart';

// 1. Obter os itens atuais do carrinho
function getCart() {
    const cart = localStorage.getItem(CART_STORAGE_KEY);
    return cart ? JSON.parse(cart) : [];
}

// 2. Adicionar um produto ao carrinho via localStorage (função de suporte)
function adicionarAoCarrinho(productId, name, price, imageUrl) {
    let cart = getCart();

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
    atualizarContadorCarrinho();
}

// 3. Atualizar o contador visual no menu superior
function atualizarContadorCarrinho() {
    const cart = getCart();
    const totalItens = cart.reduce((sum, item) => sum + item.quantity, 0);
    const badge = document.getElementById('cartCount');
    if (badge) {
        badge.innerText = totalItens;
    }
}

document.addEventListener("DOMContentLoaded", atualizarContadorCarrinho);
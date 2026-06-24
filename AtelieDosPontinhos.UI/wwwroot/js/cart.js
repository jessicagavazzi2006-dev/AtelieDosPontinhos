function getCart() {
    return JSON.parse(localStorage.getItem("cart")) || [];
}

function saveCart(cart) {
    localStorage.setItem("cart", JSON.stringify(cart));
}

function addToCart(id, name, price) {

    let cart = getCart();

    let item = cart.find(x => x.id === id);

    if (item) {
        item.quantity++;
    } else {
        cart.push({
            id: id,
            name: name,
            price: price,
            quantity: 1
        });
    }

    saveCart(cart);
    updateCartCount();
}

function updateCartCount() {

    let cart = getCart();

    let total = cart.reduce((sum, item) => sum + item.quantity, 0);

    let el = document.getElementById("cartCount");

    if (el) el.innerText = total;
}

document.addEventListener("DOMContentLoaded", updateCartCount);
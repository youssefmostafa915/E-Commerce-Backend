// Cart page functionality
document.addEventListener('DOMContentLoaded', function() {
    loadCartPage();
});

// Load cart page content
function loadCartPage() {
    const cartContent = document.getElementById('cart-content');
    const emptyCart = document.getElementById('empty-cart');

    if (cart.length === 0) {
        cartContent.style.display = 'none';
        emptyCart.style.display = 'block';
        return;
    }

    cartContent.style.display = 'block';
    emptyCart.style.display = 'none';

    displayCartItems();
}

// Display cart items
function displayCartItems() {
    const cartContent = document.getElementById('cart-content');

    let html = `
        <div class="cart-header">
            <div>Product</div>
            <div>Price</div>
            <div>Quantity</div>
            <div>Total</div>
        </div>
    `;

    let subtotal = 0;

    cart.forEach(item => {
        const itemTotal = item.price * item.quantity;
        subtotal += itemTotal;

        html += `
            <div class="cart-item">
                <div style="display: flex; align-items: center; gap: 15px;">
                    <div class="cart-item-image">
                        <img src="${item.imageUrl || 'https://via.placeholder.com/80x80?text=No+Image'}" alt="${item.name}" onerror="this.src='https://via.placeholder.com/80x80?text=No+Image'">
                    </div>
                    <div class="cart-item-info">
                        <h4>${item.name}</h4>
                    </div>
                </div>
                <div class="cart-item-price">$${item.price.toFixed(2)}</div>
                <div>
                    <div class="quantity-selector" style="justify-content: center;">
                        <button class="quantity-btn" onclick="updateCartItemQuantity('${item.id}', ${item.quantity - 1})">-</button>
                        <input type="number" class="quantity-input" value="${item.quantity}" min="1" onchange="updateCartItemQuantity('${item.id}', parseInt(this.value))" style="width: 50px;">
                        <button class="quantity-btn" onclick="updateCartItemQuantity('${item.id}', ${item.quantity + 1})">+</button>
                    </div>
                </div>
                <div style="display: flex; align-items: center; justify-content: space-between;">
                    <div>$${itemTotal.toFixed(2)}</div>
                    <button class="remove-btn" onclick="removeFromCart('${item.id}'); loadCartPage();" style="margin-left: 10px;">Remove</button>
                </div>
            </div>
        `;
    });

    const tax = subtotal * 0.1; // 10% tax
    const shipping = subtotal > 50 ? 0 : 9.99; // Free shipping over $50
    const total = subtotal + tax + shipping;

    html += `
        <div class="cart-summary">
            <div class="summary-row">
                <span>Subtotal:</span>
                <span>$${subtotal.toFixed(2)}</span>
            </div>
            <div class="summary-row">
                <span>Tax (10%):</span>
                <span>$${tax.toFixed(2)}</span>
            </div>
            <div class="summary-row">
                <span>Shipping:</span>
                <span>${shipping === 0 ? 'Free' : '$' + shipping.toFixed(2)}</span>
            </div>
            <div class="summary-row summary-total">
                <span>Total:</span>
                <span>$${total.toFixed(2)}</span>
            </div>
        </div>
        <div class="cart-actions">
            <a href="products.html" class="btn-secondary">Continue Shopping</a>
            <button id="checkout-btn" class="btn-primary" onclick="handleCheckout()">Proceed to Checkout</button>
        </div>
    `;

    cartContent.innerHTML = html;
}

// Handle checkout
async function handleCheckout() {
    if (!currentUser) {
        showNotification('Please login to checkout', 'error');
        window.location.href = 'login.html';
        return;
    }

    if (cart.length === 0) {
        showNotification('Your cart is empty', 'error');
        return;
    }

    // Here you would typically send the cart to the API to create an order
    // For now, just show a success message
    showNotification('Order placed successfully! (This is a demo)');
    clearCart();
    loadCartPage();
}
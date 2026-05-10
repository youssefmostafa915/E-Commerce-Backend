// API Configuration
const API_BASE_URL = 'http://localhost:5000/api'; // Update this to match your API URL

// Global state
let cart = [];
let currentUser = null;
let products = [];

// Initialize the app
document.addEventListener('DOMContentLoaded', function() {
    initializeEventListeners();
    loadProducts();
    loadCartFromStorage();
    updateCartDisplay();
    loadUserFromStorage();
});

// Initialize event listeners
function initializeEventListeners() {
    // Modal controls - only if elements exist
    const cartBtn = document.getElementById('cart-btn');
    const loginBtn = document.getElementById('login-btn');
    const cartModal = document.getElementById('cart-modal');
    const loginModal = document.getElementById('login-modal');

    if (cartBtn && cartModal) {
        cartBtn.addEventListener('click', () => openModal(cartModal));
    }

    if (loginBtn && loginModal) {
        loginBtn.addEventListener('click', () => openModal(loginModal));
    }

    // Close modals - only if elements exist
    document.querySelectorAll('.close').forEach(closeBtn => {
        closeBtn.addEventListener('click', () => closeModal());
    });

    // Auth tabs - only if elements exist
    const loginTab = document.getElementById('login-tab');
    const registerTab = document.getElementById('register-tab');
    const loginForm = document.getElementById('loginForm');
    const registerForm = document.getElementById('registerForm');

    if (loginTab && registerTab && loginForm && registerForm) {
        loginTab.addEventListener('click', () => switchAuthTab('login'));
        registerTab.addEventListener('click', () => switchAuthTab('register'));

        // Forms
        loginForm.addEventListener('submit', handleLogin);
        registerForm.addEventListener('submit', handleRegister);
    }

    // Cart actions - only if elements exist
    const checkoutBtn = document.getElementById('checkout-btn');
    const clearCartBtn = document.getElementById('clear-cart-btn');

    if (checkoutBtn) {
        checkoutBtn.addEventListener('click', handleCheckout);
    }

    if (clearCartBtn) {
        clearCartBtn.addEventListener('click', clearCart);
    }

    // Shop now button - only if element exists
    const shopNowBtn = document.getElementById('shop-now-btn');
    if (shopNowBtn) {
        shopNowBtn.addEventListener('click', () => {
            document.getElementById('products').scrollIntoView({ behavior: 'smooth' });
        });
    }

    // Close modal when clicking outside
    window.addEventListener('click', (e) => {
        const cartModal = document.getElementById('cart-modal');
        const loginModal = document.getElementById('login-modal');
        if ((cartModal && e.target === cartModal) || (loginModal && e.target === loginModal)) {
            closeModal();
        }
    });
}

// Modal functions
function openModal(modal) {
    modal.style.display = 'block';
    document.body.style.overflow = 'hidden';
}

function closeModal() {
    cartModal.style.display = 'none';
    loginModal.style.display = 'none';
    document.body.style.overflow = 'auto';
}

// Auth tab switching
function switchAuthTab(tab) {
    loginTab.classList.toggle('active', tab === 'login');
    registerTab.classList.toggle('active', tab === 'register');
    loginForm.classList.toggle('active', tab === 'login');
    registerForm.classList.toggle('active', tab === 'register');
}

// API helper functions
async function apiRequest(endpoint, options = {}) {
    const url = `${API_BASE_URL}${endpoint}`;
    const config = {
        headers: {
            'Content-Type': 'application/json',
            ...options.headers
        },
        ...options
    };

    // Add authorization header if user is logged in
    if (currentUser && currentUser.token) {
        config.headers.Authorization = `Bearer ${currentUser.token}`;
    }

    try {
        const response = await fetch(url, config);
        const data = await response.json();
        return { ok: response.ok, status: response.status, data };
    } catch (error) {
        console.error('API request failed:', error);
        return { ok: false, error: error.message };
    }
}

// Load products from API
async function loadProducts() {
    const productsGrid = document.getElementById('products-grid');
    const loading = document.getElementById('loading');

    if (!productsGrid) return; // Only load products if products grid exists

    if (loading) loading.style.display = 'block';
    productsGrid.innerHTML = '';

    try {
        const result = await apiRequest('/products');

        if (result.ok) {
            products = result.data;
            displayProducts(products);
        } else {
            // Fallback to sample data if API is not available
            console.warn('API not available, using sample data');
            loadSampleProducts();
        }
    } catch (error) {
        console.error('Error loading products:', error);
        // Fallback to sample data
        loadSampleProducts();
    }

    if (loading) loading.style.display = 'none';
}

// Load sample products as fallback
function loadSampleProducts() {
    const sampleProducts = [
        {
            id: '1',
            name: 'Wireless Headphones',
            price: 99.99,
            description: 'High-quality wireless headphones with noise cancellation',
            imageUrl: 'https://via.placeholder.com/300x200?text=Headphones',
            stockQuantity: 50,
            category: 'Electronics'
        },
        {
            id: '2',
            name: 'Smart Watch',
            price: 199.99,
            description: 'Feature-rich smartwatch with health tracking',
            imageUrl: 'https://via.placeholder.com/300x200?text=Smart+Watch',
            stockQuantity: 30,
            category: 'Electronics'
        },
        {
            id: '3',
            name: 'Laptop',
            price: 899.99,
            description: 'Powerful laptop for work and entertainment',
            imageUrl: 'https://via.placeholder.com/300x200?text=Laptop',
            stockQuantity: 15,
            category: 'Computers'
        },
        {
            id: '4',
            name: 'Phone Case',
            price: 19.99,
            description: 'Protective case for your smartphone',
            imageUrl: 'https://via.placeholder.com/300x200?text=Phone+Case',
            stockQuantity: 100,
            category: 'Accessories'
        }
    ];

    products = sampleProducts;
    displayProducts(products);
}

// Display products
function displayProducts(products) {
    const productsGrid = document.getElementById('products-grid');
    if (!productsGrid) return;

    productsGrid.innerHTML = '';

    products.forEach(product => {
        const productCard = document.createElement('div');
        productCard.className = 'product-card';
        productCard.innerHTML = `
            <div class="product-image">
                <img src="${product.imageUrl}" alt="${product.name}" onerror="this.src='https://via.placeholder.com/300x200?text=No+Image'">
            </div>
            <div class="product-info">
                <div class="product-name">${product.name}</div>
                <div class="product-price">$${product.price.toFixed(2)}</div>
                <div class="product-description">${product.description}</div>
                <button class="add-to-cart-btn" onclick="addToCart('${product.id}')">
                    <i class="fas fa-cart-plus"></i> Add to Cart
                </button>
            </div>
        `;
        productsGrid.appendChild(productCard);
    });
}

// Cart functions
function addToCart(productId, quantity = 1) {
    const product = products.find(p => p.id === productId);
    if (!product) return;

    const existingItem = cart.find(item => item.id === productId);
    if (existingItem) {
        existingItem.quantity += quantity;
    } else {
        cart.push({
            id: product.id,
            name: product.name,
            price: product.price,
            quantity: quantity,
            imageUrl: product.imageUrl
        });
    }

    saveCartToStorage();
    updateCartDisplay();
    showNotification(`${product.name} added to cart!`);
}

function removeFromCart(productId) {
    cart = cart.filter(item => item.id !== productId);
    saveCartToStorage();
    updateCartDisplay();
}

function updateCartItemQuantity(productId, newQuantity) {
    if (newQuantity <= 0) {
        removeFromCart(productId);
        return;
    }

    const item = cart.find(item => item.id === productId);
    if (item) {
        item.quantity = newQuantity;
        saveCartToStorage();
        updateCartDisplay();
    }
}

function clearCart() {
    cart = [];
    saveCartToStorage();
    updateCartDisplay();
}

function updateCartDisplay() {
    // Update cart count - only if element exists
    const cartCount = document.getElementById('cart-count');
    if (cartCount) {
        const totalItems = cart.reduce((sum, item) => sum + item.quantity, 0);
        cartCount.textContent = totalItems;
    }

    // Update cart modal - only if elements exist
    const cartItems = document.getElementById('cart-items');
    const cartTotal = document.getElementById('cart-total');

    if (cartItems && cartTotal) {
        cartItems.innerHTML = '';
        let total = 0;

        if (cart.length === 0) {
            cartItems.innerHTML = '<p>Your cart is empty</p>';
        } else {
            cart.forEach(item => {
                const itemTotal = item.price * item.quantity;
                total += itemTotal;

                const cartItem = document.createElement('div');
                cartItem.className = 'cart-item';
                cartItem.innerHTML = `
                    <div class="cart-item-info">
                        <div class="cart-item-name">${item.name}</div>
                        <div class="cart-item-price">$${item.price.toFixed(2)} each</div>
                    </div>
                    <div class="cart-item-controls">
                        <div class="quantity-controls">
                            <button class="quantity-btn" onclick="updateCartItemQuantity('${item.id}', ${item.quantity - 1})">-</button>
                            <span>${item.quantity}</span>
                            <button class="quantity-btn" onclick="updateCartItemQuantity('${item.id}', ${item.quantity + 1})">+</button>
                        </div>
                        <div>$${itemTotal.toFixed(2)}</div>
                        <button class="remove-btn" onclick="removeFromCart('${item.id}')">Remove</button>
                    </div>
                `;
                cartItems.appendChild(cartItem);
            });
        }

        cartTotal.textContent = total.toFixed(2);
    }
}

// Cart persistence
function saveCartToStorage() {
    localStorage.setItem('cart', JSON.stringify(cart));
}

function loadCartFromStorage() {
    const savedCart = localStorage.getItem('cart');
    if (savedCart) {
        cart = JSON.parse(savedCart);
    }
}

// Authentication functions
async function handleLogin(e) {
    e.preventDefault();

    const email = document.getElementById('login-email').value;
    const password = document.getElementById('login-password').value;

    const result = await apiRequest('/user/login', {
        method: 'POST',
        body: JSON.stringify({ email, password })
    });

    if (result.ok) {
        currentUser = {
            email: email,
            token: result.data.token // Assuming the API returns a token
        };
        localStorage.setItem('user', JSON.stringify(currentUser));
        updateAuthDisplay();
        closeModal();
        showNotification('Login successful!');
    } else {
        showNotification('Login failed. Please check your credentials.', 'error');
    }
}

async function handleRegister(e) {
    e.preventDefault();

    const name = document.getElementById('register-name').value;
    const email = document.getElementById('register-email').value;
    const phone = document.getElementById('register-phone').value;
    const password = document.getElementById('register-password').value;

    const result = await apiRequest('/user/register', {
        method: 'POST',
        body: JSON.stringify({
            name,
            email,
            phone,
            password
        })
    });

    if (result.ok) {
        showNotification('Registration successful! Please login.');
        switchAuthTab('login');
    } else {
        showNotification('Registration failed. Please try again.', 'error');
    }
}

function updateAuthDisplay() {
    const loginBtn = document.getElementById('login-btn');
    if (!loginBtn) return;

    if (currentUser) {
        loginBtn.innerHTML = '<i class="fas fa-user"></i> Logout';
        loginBtn.onclick = handleLogout;
    } else {
        loginBtn.innerHTML = '<i class="fas fa-user"></i> Login';
        loginBtn.onclick = () => {
            const loginModal = document.getElementById('login-modal');
            if (loginModal) openModal(loginModal);
        };
    }
}

function handleLogout() {
    currentUser = null;
    localStorage.removeItem('user');
    updateAuthDisplay();
    showNotification('Logged out successfully');
}

// Checkout function
async function handleCheckout() {
    if (!currentUser) {
        showNotification('Please login to checkout', 'error');
        openModal(loginModal);
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
    closeModal();
}

// Notification system
function showNotification(message, type = 'success') {
    // Create notification element
    const notification = document.createElement('div');
    notification.className = `notification ${type}`;
    notification.textContent = message;

    // Style the notification
    notification.style.cssText = `
        position: fixed;
        top: 20px;
        right: 20px;
        padding: 15px 20px;
        border-radius: 5px;
        color: white;
        font-weight: bold;
        z-index: 3000;
        animation: slideIn 0.3s ease-out;
    `;

    if (type === 'success') {
        notification.style.backgroundColor = '#27ae60';
    } else {
        notification.style.backgroundColor = '#e74c3c';
    }

    document.body.appendChild(notification);

    // Remove after 3 seconds
    setTimeout(() => {
        notification.style.animation = 'slideOut 0.3s ease-out';
        setTimeout(() => {
            document.body.removeChild(notification);
        }, 300);
    }, 3000);
}

// Load user from storage on page load
function loadUserFromStorage() {
    const savedUser = localStorage.getItem('user');
    if (savedUser) {
        currentUser = JSON.parse(savedUser);
        updateAuthDisplay();
    }
}

// Initialize user on load
loadUserFromStorage();

// Add notification animations to CSS dynamically
const style = document.createElement('style');
style.textContent = `
    @keyframes slideIn {
        from { transform: translateX(100%); opacity: 0; }
        to { transform: translateX(0); opacity: 1; }
    }
    @keyframes slideOut {
        from { transform: translateX(0); opacity: 1; }
        to { transform: translateX(100%); opacity: 0; }
    }
`;
document.head.appendChild(style);
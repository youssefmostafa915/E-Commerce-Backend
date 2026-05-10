// Product details page functionality
let currentProduct = null;

document.addEventListener('DOMContentLoaded', function() {
    const urlParams = new URLSearchParams(window.location.search);
    const productId = urlParams.get('id');

    if (productId) {
        loadProductDetails(productId);
        loadRelatedProducts();
    } else {
        showProductNotFound();
    }
});

// Load product details
async function loadProductDetails(productId) {
    const productDetail = document.getElementById('product-detail');

    try {
        const result = await apiRequest(`/products/${productId}`);

        if (result.ok) {
            currentProduct = result.data;
            displayProductDetails(currentProduct);
        } else {
            // Try to find in sample products
            const sampleProduct = getSampleProduct(productId);
            if (sampleProduct) {
                currentProduct = sampleProduct;
                displayProductDetails(currentProduct);
            } else {
                showProductNotFound();
            }
        }
    } catch (error) {
        console.error('Error loading product details:', error);
        const sampleProduct = getSampleProduct(productId);
        if (sampleProduct) {
            currentProduct = sampleProduct;
            displayProductDetails(currentProduct);
        } else {
            showProductNotFound();
        }
    }
}

// Display product details
function displayProductDetails(product) {
    const productDetail = document.getElementById('product-detail');

    productDetail.innerHTML = `
        <div class="product-images">
            <div class="main-image">
                <img src="${product.imageUrl || 'https://via.placeholder.com/500x400?text=No+Image'}" alt="${product.name}" onerror="this.src='https://via.placeholder.com/500x400?text=No+Image'">
            </div>
        </div>
        <div class="product-info">
            <h1>${product.name}</h1>
            <div class="product-price">$${product.price.toFixed(2)}</div>
            <div class="product-description">${product.description}</div>

            <div class="product-meta">
                <div class="meta-item">
                    <span>Category:</span>
                    <span>${product.category || 'General'}</span>
                </div>
                <div class="meta-item">
                    <span>Stock:</span>
                    <span>${product.stockQuantity || 'Available'}</span>
                </div>
            </div>

            <div class="add-to-cart-section">
                <div class="quantity-selector">
                    <button class="quantity-btn" onclick="updateQuantity(-1)">-</button>
                    <input type="number" class="quantity-input" id="quantity" value="1" min="1" max="${product.stockQuantity || 99}">
                    <button class="quantity-btn" onclick="updateQuantity(1)">+</button>
                </div>
                <button class="btn-primary" onclick="addToCart('${product.id}', parseInt(document.getElementById('quantity').value))">
                    <i class="fas fa-cart-plus"></i> Add to Cart
                </button>
            </div>

            <div class="product-actions">
                <button class="btn-secondary" onclick="window.history.back()">
                    <i class="fas fa-arrow-left"></i> Back to Products
                </button>
            </div>
        </div>
    `;
}

// Update quantity
function updateQuantity(change) {
    const quantityInput = document.getElementById('quantity');
    const currentValue = parseInt(quantityInput.value);
    const newValue = currentValue + change;

    if (newValue >= 1 && newValue <= parseInt(quantityInput.max)) {
        quantityInput.value = newValue;
    }
}

// Load related products
async function loadRelatedProducts() {
    const relatedProductsGrid = document.getElementById('related-products');

    try {
        const result = await apiRequest('/products');

        if (result.ok) {
            const allProducts = result.data;
            const relatedProducts = allProducts
                .filter(p => p.id !== currentProduct?.id)
                .slice(0, 4);

            displayRelatedProducts(relatedProducts);
        } else {
            // Load sample related products
            const sampleProducts = getSampleProducts().filter(p => p.id !== currentProduct?.id).slice(0, 4);
            displayRelatedProducts(sampleProducts);
        }
    } catch (error) {
        console.error('Error loading related products:', error);
        const sampleProducts = getSampleProducts().filter(p => p.id !== currentProduct?.id).slice(0, 4);
        displayRelatedProducts(sampleProducts);
    }
}

// Display related products
function displayRelatedProducts(products) {
    const relatedProductsGrid = document.getElementById('related-products');
    relatedProductsGrid.innerHTML = '';

    products.forEach(product => {
        const productCard = document.createElement('div');
        productCard.className = 'product-card';
        productCard.innerHTML = `
            <div class="product-image">
                <img src="${product.imageUrl || 'https://via.placeholder.com/300x200?text=No+Image'}" alt="${product.name}" onerror="this.src='https://via.placeholder.com/300x200?text=No+Image'">
            </div>
            <div class="product-info">
                <div class="product-name">${product.name}</div>
                <div class="product-price">$${product.price.toFixed(2)}</div>
                <a href="product-details.html?id=${product.id}" class="btn-secondary" style="display: block; text-align: center; margin-top: 10px; text-decoration: none;">View Details</a>
            </div>
        `;
        relatedProductsGrid.appendChild(productCard);
    });
}

// Show product not found
function showProductNotFound() {
    const productDetail = document.getElementById('product-detail');
    productDetail.innerHTML = `
        <div style="text-align: center; padding: 60px 20px;">
            <i class="fas fa-exclamation-triangle fa-3x" style="color: #e74c3c; margin-bottom: 20px;"></i>
            <h2>Product Not Found</h2>
            <p>The product you're looking for doesn't exist or has been removed.</p>
            <a href="products.html" class="btn-primary" style="margin-top: 20px;">Browse Products</a>
        </div>
    `;
}

// Get sample product by ID
function getSampleProduct(id) {
    return getSampleProducts().find(p => p.id === id);
}

// Get sample products
function getSampleProducts() {
    return [
        { id: '1', name: 'Wireless Headphones', price: 99.99, description: 'High-quality wireless headphones with noise cancellation and premium sound quality.', category: 'Electronics', stockQuantity: 50, imageUrl: 'https://via.placeholder.com/500x400?text=Headphones' },
        { id: '2', name: 'Smart Watch', price: 199.99, description: 'Feature-rich smartwatch with health tracking, notifications, and long battery life.', category: 'Electronics', stockQuantity: 30, imageUrl: 'https://via.placeholder.com/500x400?text=Smart+Watch' },
        { id: '3', name: 'Laptop', price: 899.99, description: 'Powerful laptop for work and entertainment with high-performance specifications.', category: 'Computers', stockQuantity: 15, imageUrl: 'https://via.placeholder.com/500x400?text=Laptop' },
        { id: '4', name: 'Phone Case', price: 19.99, description: 'Protective case for your smartphone with stylish design and shock absorption.', category: 'Accessories', stockQuantity: 100, imageUrl: 'https://via.placeholder.com/500x400?text=Phone+Case' },
        { id: '5', name: 'Tablet', price: 299.99, description: 'Portable tablet perfect for work, entertainment, and creativity on the go.', category: 'Electronics', stockQuantity: 25, imageUrl: 'https://via.placeholder.com/500x400?text=Tablet' },
        { id: '6', name: 'Keyboard', price: 49.99, description: 'Mechanical keyboard with RGB lighting and responsive switches for gaming and typing.', category: 'Computers', stockQuantity: 40, imageUrl: 'https://via.placeholder.com/500x400?text=Keyboard' }
    ];
}
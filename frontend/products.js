// Products page specific functionality
let currentPage = 1;
let currentCategory = '';
let currentSearch = '';

document.addEventListener('DOMContentLoaded', function() {
    loadProducts();
    setupFilters();
    setupPagination();
});

// Setup filters
function setupFilters() {
    const categoryFilter = document.getElementById('category-filter');
    const searchInput = document.getElementById('search-input');
    const searchBtn = document.getElementById('search-btn');

    categoryFilter.addEventListener('change', function() {
        currentCategory = this.value;
        currentPage = 1;
        loadProducts();
    });

    searchBtn.addEventListener('click', function() {
        currentSearch = searchInput.value.trim();
        currentPage = 1;
        loadProducts();
    });

    searchInput.addEventListener('keypress', function(e) {
        if (e.key === 'Enter') {
            currentSearch = this.value.trim();
            currentPage = 1;
            loadProducts();
        }
    });
}

// Setup pagination
function setupPagination() {
    document.getElementById('prev-page').addEventListener('click', function() {
        if (currentPage > 1) {
            currentPage--;
            loadProducts();
        }
    });

    document.getElementById('next-page').addEventListener('click', function() {
        currentPage++;
        loadProducts();
    });
}

// Load products with filters
async function loadProducts() {
    const loading = document.getElementById('loading');
    const productsGrid = document.getElementById('products-grid');

    loading.style.display = 'block';
    productsGrid.innerHTML = '';

    try {
        let url = '/products?';
        const params = [];

        if (currentCategory) params.push(`category=${encodeURIComponent(currentCategory)}`);
        if (currentSearch) params.push(`search=${encodeURIComponent(currentSearch)}`);
        params.push(`page=${currentPage}`);
        params.push('pageSize=12');

        url += params.join('&');

        const result = await apiRequest(url);

        if (result.ok) {
            const products = result.data;
            displayProducts(products);

            // Update pagination
            updatePagination(products.length === 12); // Assuming 12 is page size
        } else {
            loadSampleProducts();
        }
    } catch (error) {
        console.error('Error loading products:', error);
        loadSampleProducts();
    }

    loading.style.display = 'none';
}

// Update pagination display
function updatePagination(hasNextPage) {
    const prevBtn = document.getElementById('prev-page');
    const nextBtn = document.getElementById('next-page');
    const pageInfo = document.getElementById('page-info');

    prevBtn.disabled = currentPage === 1;
    nextBtn.disabled = !hasNextPage;
    pageInfo.textContent = `Page ${currentPage}`;
}

// Display products (override the one in app.js for this page)
function displayProducts(products) {
    const productsGrid = document.getElementById('products-grid');
    productsGrid.innerHTML = '';

    if (products.length === 0) {
        productsGrid.innerHTML = '<p>No products found matching your criteria.</p>';
        return;
    }

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
                <div class="product-description">${product.description}</div>
                <div class="product-category">${product.category || 'General'}</div>
                <button class="add-to-cart-btn" onclick="addToCart('${product.id}')">
                    <i class="fas fa-cart-plus"></i> Add to Cart
                </button>
                <a href="product-details.html?id=${product.id}" class="btn-secondary" style="display: block; text-align: center; margin-top: 10px; text-decoration: none;">View Details</a>
            </div>
        `;
        productsGrid.appendChild(productCard);
    });
}

// Load sample products as fallback
function loadSampleProducts() {
    const sampleProducts = [
        { id: '1', name: 'Wireless Headphones', price: 99.99, description: 'High-quality wireless headphones', category: 'Electronics', imageUrl: 'https://via.placeholder.com/300x200?text=Headphones' },
        { id: '2', name: 'Smart Watch', price: 199.99, description: 'Feature-rich smartwatch', category: 'Electronics', imageUrl: 'https://via.placeholder.com/300x200?text=Smart+Watch' },
        { id: '3', name: 'Laptop', price: 899.99, description: 'Powerful laptop', category: 'Computers', imageUrl: 'https://via.placeholder.com/300x200?text=Laptop' },
        { id: '4', name: 'Phone Case', price: 19.99, description: 'Protective case', category: 'Accessories', imageUrl: 'https://via.placeholder.com/300x200?text=Phone+Case' },
        { id: '5', name: 'Tablet', price: 299.99, description: 'Portable tablet', category: 'Electronics', imageUrl: 'https://via.placeholder.com/300x200?text=Tablet' },
        { id: '6', name: 'Keyboard', price: 49.99, description: 'Mechanical keyboard', category: 'Computers', imageUrl: 'https://via.placeholder.com/300x200?text=Keyboard' }
    ];

    // Filter products based on current filters
    let filteredProducts = sampleProducts;

    if (currentCategory) {
        filteredProducts = filteredProducts.filter(p => p.category === currentCategory);
    }

    if (currentSearch) {
        const searchLower = currentSearch.toLowerCase();
        filteredProducts = filteredProducts.filter(p =>
            p.name.toLowerCase().includes(searchLower) ||
            p.description.toLowerCase().includes(searchLower)
        );
    }

    displayProducts(filteredProducts);
    updatePagination(filteredProducts.length > 4); // Simple pagination logic
}
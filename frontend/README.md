# E-Commerce Front-End

A modern, responsive e-commerce website built with HTML5, CSS3, and JavaScript.

## Features

- **Responsive Design**: Works perfectly on desktop, tablet, and mobile devices
- **Product Catalog**: Browse products with images, descriptions, and prices
- **Shopping Cart**: Add/remove items, update quantities, persistent cart storage
- **User Authentication**: Register and login functionality
- **Modern UI**: Clean, professional design with smooth animations
- **API Integration**: Connects to the E-Commerce API backend

## Technologies Used

- **HTML5**: Semantic markup and structure
- **CSS3**: Modern styling with Flexbox and Grid
- **JavaScript (ES6+)**: Dynamic functionality and API integration
- **Font Awesome**: Icons for better visual appeal

## Getting Started

1. **Prerequisites**:
   - A web server (Apache, Nginx, or even Python's `http.server`)
   - The E-Commerce API running (default: `https://localhost:5001`)

2. **Setup**:
   - Clone or download the front-end files
   - Open `index.html` in your web browser, or serve via a web server

3. **API Configuration**:
   - Update the `API_BASE_URL` in `app.js` to match your API endpoint
   - Default: `https://localhost:5001/api`

## File Structure

```
frontend/
├── index.html      # Main HTML file
├── styles.css      # CSS styles
├── app.js         # JavaScript functionality
└── README.md      # This file
```

## Features Overview

### Navigation
- Fixed header with logo and navigation menu
- Cart icon with item count
- Login/Register buttons

### Product Display
- Grid layout for products
- Product cards with images, names, prices, and descriptions
- Add to cart functionality

### Shopping Cart
- Modal popup for cart management
- Quantity controls for each item
- Total price calculation
- Persistent storage (survives page refresh)

### Authentication
- Login and registration forms
- Tabbed interface for switching between forms
- JWT token storage for authenticated requests

### Responsive Design
- Mobile-first approach
- Flexible grid layouts
- Touch-friendly buttons and controls

## API Endpoints Used

- `GET /api/products` - Get all products
- `GET /api/products/{id}` - Get product by ID
- `POST /api/user/register` - User registration
- `POST /api/user/login` - User login
- `POST /api/cart/add` - Add item to cart
- `GET /api/cart` - Get cart contents
- `PUT /api/cart/items/{id}` - Update cart item
- `DELETE /api/cart` - Clear cart
- `POST /api/orders` - Create order

## Browser Support

- Chrome (latest)
- Firefox (latest)
- Safari (latest)
- Edge (latest)

## Development

To modify the front-end:

1. Edit `index.html` for structure changes
2. Modify `styles.css` for styling updates
3. Update `app.js` for functionality changes

The code is well-commented and organized for easy maintenance.

## Demo

For demonstration purposes, the app includes sample product data. To use real data, ensure your API is running and update the API URL in `app.js`.

## License

This project is part of the E-Commerce application suite.
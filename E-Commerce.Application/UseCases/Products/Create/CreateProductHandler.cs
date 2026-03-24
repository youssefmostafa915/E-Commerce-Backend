using E_Commerce.Domain.Shared; 
using E_Commerce.Application.Interfaces.Repositories;
using E_Commerce.Domain.Entities;
using E_Commerce.Domain.ValueObjects; 
using E_Commerce.Application.DTOs; 
using E_Commerce.Domain.Enums;
using E_Commerce.Application.UseCases.Products.Create;
using System.Net;

namespace E_Commerce.Application.UseCases.Products
{
    public class CreateProductHandler
    {
        private readonly IProductRepository _productRepository;

        public CreateProductHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<BaseResponse<object>> HandleAsync(CreateProductCommand command, CancellationToken cancellationToken)
        {
            try
            {
                // Logic: Switch acts as the Traffic Controller for the E-Commerce operations
                switch (command.OperationType)
                {
                    case ProductOperationType.Create:
                        return await HandleCreateAsync(command);
                    case ProductOperationType.Update:
                        return await HandleUpdateAsync(command);
                    case ProductOperationType.Delete:
                        return await HandleDeleteAsync(command);
                    case ProductOperationType.GetById:
                        return await HandleGetByIdAsync(command);
                    case ProductOperationType.GetAll:
                        return await HandleGetAllAsync();
                    default:
                        return BaseResponse<object>.Fail("PRODUCT_INVALID_OPERATION", "Invalid operation type.");
                }
            }
            catch (Exception ex)
            {
                return BaseResponse<object>.Fail("PRODUCT_ERROR", ex.Message, HttpStatusCode.InternalServerError);
            }
        }

        private async Task<BaseResponse<object>> HandleCreateAsync(CreateProductCommand command)
        {
            // Logic: Validation (Footsteps)
            if (string.IsNullOrWhiteSpace(command.Name) || command.PriceAmount <= 0)
            {
                return BaseResponse<object>.Fail("PRODUCT_VALIDATION", "Name and Price are required.");
            }

            var existing = await _productRepository.GetByProductNameAsync(command.Name);
            if (existing != null)
            {
                return BaseResponse<object>.Fail("PRODUCT_CONFLICT", "Product already exists.", HttpStatusCode.Conflict);
            }

            var product = new Product
            {
                Name = command.Name.Trim(), // Logic: Trimming
                Description = command.Description,
                Price = new Money(command.PriceAmount, command.Currency ?? "USD"),
                StockQuantity = command.StockQuantity
            };

            await _productRepository.AddAsync(product);
            return BaseResponse<object>.Ok(product, "Product created successfully");
        }

        private async Task<BaseResponse<object>> HandleUpdateAsync(CreateProductCommand command)
        {
            if (string.IsNullOrWhiteSpace(command.Id))
                return BaseResponse<object>.Fail("PRODUCT_VALIDATION", "Id is required.");

            var product = await _productRepository.GetByIdAsync(command.Id);
            if (product == null)
                return BaseResponse<object>.Fail("PRODUCT_NOT_FOUND", "Product not found.", HttpStatusCode.NotFound);

            if (!string.IsNullOrWhiteSpace(command.Name)) product.Name = command.Name.Trim();
            if (!string.IsNullOrWhiteSpace(command.Description)) product.Description = command.Description;
            if (command.PriceAmount > 0) product.Price = new Money(command.PriceAmount, command.Currency ?? product.Price.Currency);
            if (command.StockQuantity >= 0) product.StockQuantity = command.StockQuantity;

            await _productRepository.UpdateAsync(product);
            return BaseResponse<object>.Ok(product, "Product updated successfully");
        }

        private async Task<BaseResponse<object>> HandleDeleteAsync(CreateProductCommand command)
        {
            if (string.IsNullOrWhiteSpace(command.Id))
                return BaseResponse<object>.Fail("PRODUCT_VALIDATION", "Id is required.");

            var existing = await _productRepository.GetByIdAsync(command.Id);
            if (existing == null)
                return BaseResponse<object>.Fail("PRODUCT_NOT_FOUND", "Product not found.", HttpStatusCode.NotFound);

            await _productRepository.DeleteAsync(command.Id);
            return BaseResponse<object>.Ok(true, "Product deleted successfully");
        }

        private async Task<BaseResponse<object>> HandleGetByIdAsync(CreateProductCommand command)
        {
            if (string.IsNullOrWhiteSpace(command.Id))
                return BaseResponse<object>.Fail("PRODUCT_VALIDATION", "Id is required.");

            var product = await _productRepository.GetByIdAsync(command.Id);
            return product == null 
                ? BaseResponse<object>.Fail("PRODUCT_NOT_FOUND", "Product not found.", HttpStatusCode.NotFound) 
                : BaseResponse<object>.Ok(product, "Product retrieved successfully");
        }

        private async Task<BaseResponse<object>> HandleGetAllAsync()
        {
            var products = await _productRepository.GetAllAsync();
            return BaseResponse<object>.Ok(products, "Products retrieved successfully");
        }
    }
}
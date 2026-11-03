using KodaStore.Api.DTOs;
using KodaStore.Api.Models;
using KodaStore.Api.Repositories;

namespace KodaStore.Api.Services;

public sealed class ProductService : IProductService
{
    private readonly IProductRepository _repository;

    public ProductService(IProductRepository repository)
    {
        _repository = repository;
    }

    public ProductResponse? Create(ProductRequest request)
    {
        var product = new Products(request.ProductCode, request.Name, request.Price, request.Stock, request.Category);

        return _repository.AddProduct(product) ? ProductResponse.FromModel(product) : null;

    }

    public IReadOnlyList<ProductResponse> GetAll()
    {
        return _repository.GetAll().Select(ProductResponse.FromModel).ToArray();
    }

    public ProductResponse? GetByCode(string productCode)
    {
        var product = _repository.GetByCode(productCode);
        return product is null ? null : ProductResponse.FromModel(product);
    }

    public ProductResponse? Update(string productCode, UpdateProductRequest request)
    {
        var product = new Products(productCode, request.Name, request.Price, request.Stock, request.Category);

        return _repository.UpdateProduct(product) ? ProductResponse.FromModel(product) : null;
    }

    public bool Delete(string productCode)
    {
        return _repository.DeleteProduct(productCode);
    }
}
using KodaStore.Api.Models;

namespace KodaStore.Api.Repositories;

public interface IProductRepository
{
    IReadOnlyList<Products> GetAll();
    Products? GetByCode(string productCode);
    bool AddProduct(Products products);
    bool UpdateProduct(Products products);
    bool DeleteProduct(string productCode);
}
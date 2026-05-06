
using api.models.Entities;
using api.models.Responses;

namespace api.services.Repositories
{
    public interface IProductRepository
    {
        Task<string> GetProducts();
        Task<string> GetProductById(int id);

        Task<GeneralResponse> CreateProduct(Product product);

        Task<GeneralResponse> UpdateProduct(Product product);
        Task<GeneralResponse> DeleteProduct(int id);
    }
}

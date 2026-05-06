using api.models.Entities;
using api.models.Responses;
using api.services.Handlers;
using api.services.Repositories;
using System.Collections.Generic;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace api.services.v1
{
    public class ProductService : IProductRepository
    {
        //Normal User
        public async Task<string> GetProducts()
        {
            string query = "select * from products";
            return await Task.FromResult(SqliteHandler.GetJson(query));
        }

        public async Task<string> GetProductById(int id)
        {
            string query = $"select * from products where id = {id}";
            return await Task.FromResult(SqliteHandler.GetJson(query));
        }

        //Super User
        public async Task<GeneralResponse> CreateProduct(Product product)
        {
            GeneralResponse result = new GeneralResponse();

            string query = $"insert into products(product_name, description, price, stock) " +
                           $"values('{ product.Product_Name}','{ product.Description}', '{ product.Price.ToString(System.Globalization.CultureInfo.InvariantCulture)}', '{ product.Stock}')";

            bool success = SqliteHandler.Exec(query);
            result.Estado = success;
            result.Codigo = success ? 1 : 0;
            result.Mensaje = success ? "Producto Creado" : "Error al crear producto";

            return await Task.FromResult(result);
        }
        public async Task<GeneralResponse> UpdateProduct(Product product)
        {
            GeneralResponse result = new GeneralResponse();
            string query = $"update products set " +
                        $"product_name = '{product.Product_Name}', " +
                        $"description = '{product.Description}', " +
                        $"price = {product.Price.ToString(System.Globalization.CultureInfo.InvariantCulture)}, " +
                        $"stock = {product.Stock} " +
                        $"where id = {product.Id}";
            bool success = SqliteHandler.Exec(query);
            result.Estado = success;
            result.Codigo = success ? 1 : 0;
            result.Mensaje = success ? "Producto actualizado" : "Error al actualizar";

            return await Task.FromResult(result);
        }

        public async Task<GeneralResponse> DeleteProduct(int id)
        {
            GeneralResponse result = new GeneralResponse();
            string query = $"delete from products where id = {id}";

            bool success = SqliteHandler.Exec(query);
            result.Estado = success;
            result.Codigo = success ? 1 : 0;
            result.Mensaje = success ? "Producto eliminado" : "Error al eliminar";
            return await Task.FromResult(result);
        }
    }
}

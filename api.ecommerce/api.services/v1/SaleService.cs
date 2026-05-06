
using api.models.Responses;
using api.services.Handlers;
using api.services.Repositories;
using Newtonsoft.Json;

namespace api.services.v1
{
    public class SaleService : ISaleRepository
    {
        public async Task<GeneralResponse> ConfirmSale(int userId)
        {
            GeneralResponse result = new GeneralResponse();

            //traigo el carrito del user con precios
            string cartQuery = $"select c.product_id, c.quantity, p.price " +
                $"from cart c inner join products p on c.product_id = p.id " +
                $"where c.user_id = {userId}";

            string cartJson = SqliteHandler.GetJson(cartQuery);

            if (cartJson == "[]")
            {
                result.Estado = false;
                result.Codigo = 0;
                result.Mensaje = "El carrito esta vacio";
                return await Task.FromResult(result);
            }

            //Calculo el total, pasando el cart a una lista
            var items = JsonConvert.DeserializeObject<List<dynamic>>(cartJson);
            decimal total = 0;
            foreach (var item in items)
            {
                total += (decimal)item.price * (int)item.quantity;
            }

            //Inserto la compra
            string fecha = DateTime.Now.ToString();
            string saleQuery = $"insert into sale(user_id, total, fecha) " +
                $"values({userId}, {total.ToString(System.Globalization.CultureInfo.InvariantCulture)}, '{fecha}')";
            SqliteHandler.Exec(saleQuery);

            //Obtengo id de la compra recien insertada
            string lastIdJson = SqliteHandler.GetJson("select last_insert_rowid() as id");
            var lastId = JsonConvert.DeserializeObject<List<dynamic>>(lastIdJson);
            int saleId = (int)lastId[0].id;

            // Inserto el detalle
            foreach (var item in items)
            {
                string detalleQuery = $"insert into compra_detalle(compra_id, product_id, cantidad, precio_unitario) " +
                $"values ({saleId}, {item.product_id}, {item.quantity},{((decimal)item.price).ToString(System.Globalization.CultureInfo.InvariantCulture)})";
                SqliteHandler.Exec(detalleQuery);

                // Descuento el stock
                string stockQuery = $"update products " +
                    $"set stock = stock - {item.quantity} " +
                    $"where id ={item.product_id}";
                SqliteHandler.Exec(stockQuery);
            }//fin foreach

            // Limpio el carrito
            SqliteHandler.Exec($"delete from cart where user_id = {userId}");

            result.Estado = true;
            result.Codigo = 1;
            result.Mensaje = $"Compra confirmada. Total: ${total}";

            return await Task.FromResult(result);
        }

        public async Task<string> GetSaleByUser(int userId)
        {
            string query = $"select * from sale where user_id = {userId}";
            return await Task.FromResult(SqliteHandler.GetJson(query));
        }

        public async Task<string> GetAllSales()
        {
            string query = "select s.*, u.username from sale s inner join users u on s.user_id = u.id";
            return await Task.FromResult(SqliteHandler.GetJson(query));
        }

    }
}

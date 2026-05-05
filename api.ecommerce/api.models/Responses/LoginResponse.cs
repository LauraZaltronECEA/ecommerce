
namespace api.models.Responses
{
    public class LoginResponse
    {
        public int Codigo { get; set; }

        public required string Mensaje { get; set; }
        public bool Estado { get; set; }

        public required string Token { get; set; }

        public required string FechaLogin { get; set; }
    }
}

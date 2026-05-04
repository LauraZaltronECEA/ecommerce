 
namespace api.models.DTO
{
    public class UserDTO
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
    }
}

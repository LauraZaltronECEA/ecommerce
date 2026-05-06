using api.models.DTO;
using api.models.Entities;
using api.models.Responses;
using api.services.Handlers;
using api.services.Repositories;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;


namespace api.services.v1
{
    public class UserService : IUserRepository
    {

        private readonly IConfiguration _configuration;

        public UserService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<LoginResponse> Login(UserDTO user)
        { 
            string query = $"SELECT * FROM Users WHERE " +
                $"username = '{user.Username}' " +
                $"AND password = '{user.Password}'";

            string json = SqliteHandler.GetJson(query);

            // Simplificado con 'new()'
            LoginResponse result = new()
            {
                Mensaje = string.Empty,
                Token = string.Empty,
                FechaLogin = string.Empty,
                Estado = false,
                Codigo = 0
            };

            if (json == "[]")
            { 
                result.Estado = false;
                result.Codigo = 0;
                result.Mensaje = "Credenciales Invalidas";
                result.Token = "";
                result.FechaLogin = "";

                return await Task.FromResult(result);
            }

            var userList = JsonConvert.DeserializeObject<List<User>>(json);
            var userDb = userList?.FirstOrDefault();

            result.Estado = true;
            result.Codigo = 1;
            result.Mensaje = "Login Exitoso";
            result.FechaLogin = DateTime.Now.ToString();

            // Generar token JWT
            JwtHandler jwt = new(_configuration);
            result.Token = jwt.CrearJWT(userDb.Username,userDb.Id,userDb.Name);

            return await Task.FromResult(result);
        }//fin login

        public async Task<GeneralResponse> Register(UserRegisterDTO user) 
        {
            // Simplificado con 'new()'
            GeneralResponse result = new();

            string checkQuery = $"SELECT * FROM Users WHERE username = '{user.Username}'";
            string checkJson = SqliteHandler.GetJson(checkQuery);

            if (checkJson != "[]")
            {
                result.Estado = false;
                result.Codigo = 0;
                result.Mensaje = "El nombre de usuario ya existe";
                return await Task.FromResult(result);
            }

            string query = $"INSERT INTO Users (username, password, name, email) VALUES " +
                $"('{user.Username}', '{user.Password}', '{user.Name}', '{user.Email}')";

            bool success = SqliteHandler.Exec(query);

            result.Estado = success;
            result.Codigo = success ? 1 : 0;
            result.Mensaje = success ? "Registro exitoso" : "Error al registrar el usuario";

            return await Task.FromResult(result);
        }//fin register

        //Super User Methods
        public async Task<string> GetAllUsers()
        {
            string query = "select id, username, name, email from users";
            return await Task.FromResult(SqliteHandler.GetJson(query));
        }

        public async Task<GeneralResponse> DeleteUser(int id)
        {
            GeneralResponse result = new GeneralResponse();
            string query = $"delete from users where id = {id}";
            bool success = SqliteHandler.Exec(query);
            result.Estado = success;
            result.Codigo = success ? 1 : 0;
            result.Mensaje = success ? "Usuario eliminado" : "Error al eliminar";
            return await Task.FromResult(result);
        }



    }
}

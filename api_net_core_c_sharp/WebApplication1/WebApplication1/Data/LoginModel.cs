namespace WebApplication1.Data
{
    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public LoginModel()
        {
            Username = string.Empty; // Se asigna un valor por defecto, por que el campo es requerido
            Password = string.Empty; // Se asigna un valor por defecto, por que el campo es requerido
        }
    }
    
}

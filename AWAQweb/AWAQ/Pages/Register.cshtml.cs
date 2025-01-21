using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AWAQ.Pages
{
    public class RegisterModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public RegisterModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [BindProperty]
        [Required(ErrorMessage = "Registra un correo*")]
        public string correo { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Registra un nombre*")]
        public string nombre { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Registra una contraseña*")]
        public string password { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Registra tu edad*")]
        public int? edad { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Selecciona tu país*")]
        public string pais { get; set; }

        public bool userExists { get; set; }

        public IActionResult OnGet()
        {
            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            string connectionString = _configuration.GetConnectionString("database1");
            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                conexion.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandText = "SELECT COUNT(*) FROM usuario WHERE correo = @mail";
                cmd.Parameters.AddWithValue("@mail", correo);
                int count_user = Convert.ToInt32(cmd.ExecuteScalar());

                MySqlCommand cmd3 = new MySqlCommand();
                cmd3.Connection = conexion;
                cmd3.CommandText = "SELECT COUNT(*) FROM nuevo_usuario WHERE correo = @mail";
                cmd3.Parameters.AddWithValue("@mail", correo);
                int count_newUser = Convert.ToInt32(cmd3.ExecuteScalar());

                if (count_user > 0 || count_newUser > 0)
                {
                    userExists = true;
                    return Page(); 
                }

                MySqlCommand cmd2 = new MySqlCommand();
                cmd2.Connection = conexion;
                cmd2.CommandText = "INSERT INTO nuevo_usuario (nombre, correo, contrasena, pais, edad) VALUES (@name, @mail, @password, @country, @age)";
                cmd2.Parameters.AddWithValue("@name", nombre);
                cmd2.Parameters.AddWithValue("@mail", correo);
                cmd2.Parameters.AddWithValue("@password", password);
                cmd2.Parameters.AddWithValue("@country", pais);
                cmd2.Parameters.AddWithValue("@age", edad);
                cmd2.ExecuteNonQuery();

                return RedirectToPage("/Index"); // Redirect despues de registrarse al login
            }
        }
    }
}

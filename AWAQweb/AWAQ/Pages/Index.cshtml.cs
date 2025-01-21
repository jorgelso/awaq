using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using Microsoft.AspNetCore.Session;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;
using MySqlX.XDevAPI;

namespace AWAQ.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public IndexModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        [BindProperty]
        [Required(ErrorMessage = "Correo requerido*")]
        public string correo { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Contraseña requerida*")]
        public string password { get; set; }

        public bool is_admin { get; set; }

        public bool loginIncorrecto { get; set; }

        public string msgLogin { get; set; }


        public void OnGet(bool? fallo)
        {
            HttpContext.Session.Clear();
            if (fallo == true)
            {
                loginIncorrecto = true;
            }
        }

        public IActionResult OnPost()
        {
            string connectionString = _configuration.GetConnectionString("database1");
            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                conexion.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandText = "SELECT contraseña FROM admin WHERE correo = @correo";
                cmd.Parameters.AddWithValue("@correo", correo);

                MySqlCommand cmd2 = new MySqlCommand();
                cmd2.Connection = conexion;
                cmd2.CommandText = "SELECT contrasena FROM usuario WHERE correo = @correo";
                cmd2.Parameters.AddWithValue("@correo", correo);


                object result = cmd.ExecuteScalar();
                object result2 = cmd2.ExecuteScalar();



                if (result != null && password == result.ToString())
                {
                    HttpContext.Session.SetString("correo", correo);
                    HttpContext.Session.SetString("password", password);

                    MySqlCommand cmd5 = new MySqlCommand();
                    cmd5.Connection = conexion;
                    cmd5.CommandText = "SELECT id_admin FROM admin WHERE correo = @correo";
                    cmd5.Parameters.AddWithValue("@correo", correo);
                    object result5 = cmd5.ExecuteScalar();

                    conexion.Dispose();
                    HttpContext.Session.SetString("id", result5.ToString());
                    return RedirectToPage("/Dashboard");
                }
                else if (result2 != null && password == result2.ToString())
                {
                    MySqlCommand cmd3 = new MySqlCommand();
                    cmd3.Connection = conexion;
                    cmd3.CommandText = "SELECT id_usuario FROM usuario WHERE correo = @correo";
                    cmd3.Parameters.AddWithValue("@correo", correo);
                    object result3 = cmd3.ExecuteScalar();

                    HttpContext.Session.SetString("correo", correo);
                    HttpContext.Session.SetString("password", password);
                    HttpContext.Session.SetString("id", result3.ToString());

                    conexion.Dispose();
                    return RedirectToPage("/DashboardUsuario");
                }
                else if (correo == null || password == null)
                {
                    conexion.Dispose();
                    return Page();
                }
                else
                {
                    conexion.Dispose();
                    return RedirectToPage("/Index", new { fallo = true });
                }
            }
        }
    }
}
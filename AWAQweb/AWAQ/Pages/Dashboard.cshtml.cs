using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Session;
using Microsoft.AspNetCore.Http;

namespace AWAQ.Pages
{
    public class DashboardModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public DashboardModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [BindProperty]
        public int id_admin { get; set; }

        public int id_usuario { get; set; }

        [BindProperty]
        public string mail { get; set; }

        [BindProperty]
        public int? id_usuarioSeleccionado { get; set; }

        public IList<Usuario> ListaUsuarios { get; set; }

        public IActionResult OnGet()
        {
            mail = HttpContext.Session.GetString("correo");

            if (string.IsNullOrEmpty(mail))
            {
                return RedirectToPage("/Index");
            }

            string connectionString = _configuration.GetConnectionString("database1");
            MySqlConnection conexion = new MySqlConnection(connectionString);
            conexion.Open();

            MySqlCommand cmd2 = new MySqlCommand();
            cmd2.Connection = conexion;
            cmd2.CommandText = "SELECT id_admin from admin where correo = @correo;";
            cmd2.Parameters.AddWithValue("@correo", mail);

            object result = cmd2.ExecuteScalar();

            id_admin = Convert.ToInt32(result);

            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conexion;
            cmd.CommandText = "SELECT * FROM usuario ORDER BY ultima_conexion LIMIT 5";

            ListaUsuarios = new List<Usuario>();

            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    Usuario usr = new Usuario();
                    usr.id_usuario = Convert.ToInt32(reader["id_usuario"]);
                    usr.Nombre = reader["nombre"].ToString();
                    usr.correo = reader["correo"].ToString();
                    usr.pais = reader["pais"].ToString();
                    usr.ultima_conexion = reader["ultima_conexion"].ToString();
                    ListaUsuarios.Add(usr);
                }
            }

            MySqlCommand cmd4 = new MySqlCommand();
            cmd4.Connection = conexion;
            cmd4.CommandText = "SELECT COUNT(*) FROM animal;";

            object result2 = cmd4.ExecuteScalar();

            int result3 = Convert.ToInt32(result2);

            foreach (var usuario in ListaUsuarios)
            {
                int id_usuario = usuario.id_usuario;
                int progreso = 0;

                using (MySqlCommand cmd3 = new MySqlCommand())
                {
                    cmd3.Connection = conexion;
                    cmd3.CommandText = "SELECT COUNT(id_animal) AS num_animals FROM usuario_has_animal WHERE id_usuario = @id_usuario";
                    cmd3.Parameters.AddWithValue("@id_usuario", id_usuario);

                    using (MySqlDataReader reader = cmd3.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            if (!reader.IsDBNull(0))
                            {
                                progreso = reader.GetInt32(0);
                                progreso = progreso * 100;
                            }
                        }
                    }
                }

                usuario.progreso = progreso / result3;
                if (usuario.progreso == 100)
                {
                    usuario.estatus = "Completado";
                    usuario.css = "aprobado";
                }
                else
                {
                    usuario.estatus = "Pendiente";
                    usuario.css = "pendiente";
                }
            }

            conexion.Close();
            return(Page());
        }

        public IActionResult OnPost()
        {
            return RedirectToPage("/List");
        }

        public IActionResult OnPostBusqueda()
        {
            return RedirectToPage("/Biomonitor", new { id_usuarioSeleccionado = id_usuarioSeleccionado });
        }

        
    }
}
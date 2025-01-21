using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Session;
using Microsoft.AspNetCore.Http;

namespace AWAQ.Pages
{
    public class BiomonitorModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public BiomonitorModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [BindProperty]
        public int id_admin { get; set; }

        [BindProperty]
        public string nombre { get; set; }

        [BindProperty]
        public string pais { get; set; }

        [BindProperty]
        public string bandera { get; set; }

        [BindProperty]
        public string exp { get; set; }

        [BindProperty]
        public string mail { get; set; }

        [BindProperty]
        public int id_usuarioSeleccionado { get; set; }

        [BindProperty]
        public string? ultima_conexion { get; set; }

        [BindProperty]
        public string username { get; set; }

        [BindProperty]
        public int edad { get; set; }

        [BindProperty]
        public int tiempo_jugado { get; set; }

        [BindProperty]
        public int metodos_completados { get; set; }

        [BindProperty]
        public int progresoP { get; set; }

        [BindProperty]
        public string ultimoAnimal { get; set; }

        [BindProperty]
        public float promedio { get; set; }

        [BindProperty]
        public int ranking { get; set; }


        public IList<Usuario> ListaUsuarios { get; set; }


        [BindProperty]
        public int animalesEncontrados { get; set; }

        

        public IActionResult OnGet(int id_usuarioSeleccionado)
        {
            try
            {

                mail = HttpContext.Session.GetString("correo");
                id_admin = Convert.ToInt32(HttpContext.Session.GetString("id"));

                if (string.IsNullOrEmpty(mail))
                {
                    return RedirectToPage("/Index");
                }

                this.id_usuarioSeleccionado = id_usuarioSeleccionado;
                string connectionString = _configuration.GetConnectionString("database1");
                MySqlConnection conexion = new MySqlConnection(connectionString);
                conexion.Open();

                MySqlCommand cmd5 = new MySqlCommand();
                cmd5.Connection = conexion;
                cmd5.CommandText = "SELECT a.nombre AS animal_name\nFROM usuario_has_animal uha\nJOIN animal a ON uha.id_animal = a.id_animal\nWHERE uha.id_usuario = @id_usuario \nORDER BY uha.id_usuario_animal DESC\nLIMIT 1";
                cmd5.Parameters.AddWithValue("@id_usuario", id_usuarioSeleccionado);
                object result5 = cmd5.ExecuteScalar();
                ultimoAnimal = result5 != null ? (string)result5 : null;

                MySqlCommand cmd6 = new MySqlCommand();
                cmd6.Connection = conexion;
                cmd6.CommandText = "SELECT SUM(tiempo) FROM sesiones WHERE id_usuario = @id_usuario";
                cmd6.Parameters.AddWithValue("@id_usuario", id_usuarioSeleccionado);
                object result6 = cmd6.ExecuteScalar();
                tiempo_jugado = Convert.ToInt32(result6);

                MySqlCommand cmd7 = new MySqlCommand();
                cmd7.Connection = conexion;
                cmd7.CommandText = "SELECT AVG(tiempo) AS total_tiempo FROM sesiones WHERE id_usuario = @id_usuario";
                cmd7.Parameters.AddWithValue("@id_usuario", id_usuarioSeleccionado);
                object result7 = cmd7.ExecuteScalar();
                promedio = Convert.ToSingle(result7);

                MySqlCommand cmd8 = new MySqlCommand();
                cmd8.Connection = conexion;
                cmd8.CommandText = "SELECT \n    (SELECT COUNT(*) + 1 FROM usuario u2 WHERE u2.tiempo_jugado > u1.tiempo_jugado) AS ranking\nFROM\n    usuario u1\nWHERE\n    id_usuario = @id_usuario";
                cmd8.Parameters.AddWithValue("@id_usuario", id_usuarioSeleccionado);
                object result8 = cmd8.ExecuteScalar();
                ranking = Convert.ToInt32(result8);

                MySqlCommand cmd9 = new MySqlCommand();
                cmd9.Connection = conexion;
                cmd9.CommandText = "SELECT nombre from usuario WHERE id_usuario = @id_usuario";
                cmd9.Parameters.AddWithValue("@id_usuario", id_usuarioSeleccionado);
                object result9 = cmd9.ExecuteScalar();
                nombre = result9.ToString();






                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandText = "SELECT * from usuario WHERE id_usuario = @id_usuario";
                cmd.Parameters.AddWithValue("@id_usuario", id_usuarioSeleccionado);

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
                        usr.ultima_conexion = reader["ultima_conexion"] != DBNull.Value ? Convert.ToDateTime(reader["ultima_conexion"]).ToString() : null;
                        ultima_conexion = usr.ultima_conexion;
                        usr.username = reader["username"].ToString();
                        username = usr.username;
                        usr.edad = Convert.ToInt32(reader["edad"]);
                        edad = usr.edad;
                        usr.tiempo_jugado = Convert.ToInt32(reader["tiempo_jugado"]);
                        usr.metodos_completados = Convert.ToInt32(reader["metodos_completados"]);
                        metodos_completados = usr.metodos_completados;

                        ListaUsuarios.Add(usr);

                        if (usr.pais == "México")
                        {
                            bandera = "Mexico.jpg";
                        }
                        else if (usr.pais == "España")
                        {
                            bandera = "Spain.jpg";
                        }
                        else if (usr.pais == "Colombia")
                        {
                            bandera = "Colombia.png";
                        }
                    }
                }


                MySqlCommand cmd4 = new MySqlCommand();
                cmd4.Connection = conexion;
                cmd4.CommandText = "SELECT COUNT(id_animal) AS num_animals FROM usuario_has_animal WHERE id_usuario = @id_usuario";

                cmd4.Parameters.AddWithValue("@id_usuario", id_usuarioSeleccionado);

                object result2 = cmd4.ExecuteScalar();

                int result3 = Convert.ToInt32(result2);
                animalesEncontrados = result3;

                MySqlCommand cmd10 = new MySqlCommand();
                cmd10.Connection = conexion;
                cmd10.CommandText = "select count(*) from animal;";
                object result4 = cmd10.ExecuteScalar();
                int numAnimales = Convert.ToInt32(result4);

                if (result3 > 0)
                {
                    progresoP = (int)((animalesEncontrados * 100) / numAnimales);
                }
                else
                {
                    progresoP = 0;
                }
                if (progresoP <= 25)
                {
                    exp = "Nula";
                }
                else if (progresoP <= 50)
                {
                    exp = "Mínima";
                }
                else if (progresoP <= 75)
                {
                    exp = "Intermedia";
                }
                else if (progresoP <= 100)
                {
                    exp = "Avanzada";
                }



                conexion.Close();
                return (Page());
            }
            catch (Exception ex)
            {
                
                nombre = "Usuario invalido";
                exp = "Usuario invalido";
                Console.WriteLine("Error " + ex.Message);
                return (Page());
            }
        }

        

        public IActionResult OnPostBusqueda()
        {
            return RedirectToPage("/Biomonitor", new { id_usuarioSeleccionado = id_usuarioSeleccionado });
        }

        public IActionResult OnPostBlur()
        {
            return RedirectToPage("/Popup");
        }
        public IActionResult OnPostDashboard()
        {
            return RedirectToPage("/Dashboard");
        }
    }
}
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Session;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AWAQ.Pages
{
    public class PopupModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public PopupModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [BindProperty]
        public int id_admin { get; set; }

        public int id_usuario { get; set; }

        [BindProperty]
        public int? id_usuarioSeleccionado { get; set; }

        [BindProperty]
        public int id_usuarioSeleccionadoPrev { get; set; }

        [BindProperty]
        public string mail { get; set; }
        

        [BindProperty]
        public string userMail { get; set; }

        public IList<Usuario> ListaUsuarios { get; set; }
        public IList<Usuario> ListaUsuarios2 { get; set; }

      
        [BindProperty]
        public int? seleccion { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Registra un correo*")]
        public string? correo { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Registra un nombre*")]
        public string? nombre { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Registra una contraseña*")]
        public string? password { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Registra tu edad*")]
        public int? edad { get; set; }

        [BindProperty]
        [Required(ErrorMessage = "Selecciona tu país*")]
        public string pais { get; set; }
        [BindProperty]
        public string? usuarioRegistro { get; set; }

        [BindProperty]
        public bool? userExists { get; set; }

        [BindProperty]
        public int idSeleccionada { get; set; }
        [BindProperty]
        public string correoSeleccionado { get; set; }
        [BindProperty]
        public string usernameSeleccionado { get; set; }
        [BindProperty]
        public string nombreSeleccionado { get; set; }
        [BindProperty]
        public string contrasenaSeleccionada { get; set; }
        [BindProperty]
        public string paisSeleccionado { get; set; }
        [BindProperty]
        public int edadSeleccionada { get; set; }

        [BindProperty]
        public bool? error1 { get; set; }
        [BindProperty]
        public bool? error2 { get; set; }
        [BindProperty]
        public bool? error3 { get; set; }
        [BindProperty]
        public bool? error4 { get; set; }
        [BindProperty]
        public bool? error5 { get; set; }
        [BindProperty]
        public bool textError1 { get; set; }
        [BindProperty]
        public bool textError2 { get; set; } 
        [BindProperty]
        public bool textError3 { get; set; } 
        [BindProperty]
        public bool textError4 { get; set; }
        [BindProperty]
        public bool textError5 { get; set; }


        public IActionResult OnGet(int seleccion, bool? error1, bool? error2, bool? error3, bool? error4, bool? error5, string? nombre, string? correo, string? password, int? edad, bool? userExists, string usuarioRegistro )
        {
            this.nombre = nombre;
            this.correo = correo;
            this.edad = edad;
            this.seleccion = seleccion;
            this.usuarioRegistro = usuarioRegistro;
            this.error1 = error1;
            this.error2 = error2;
            this.error3 = error3;
            this.error4 = error4;
            this.error5 = error5;
            this.userExists = userExists;
            
            mail = HttpContext.Session.GetString("correo");
            id_admin = Convert.ToInt32(HttpContext.Session.GetString("id"));

            if (string.IsNullOrEmpty(mail))
            {
                return RedirectToPage("/Index");
            }

            if (seleccion == 1)
            {
                return (Page());
            }
            else
            {
                

                string connectionString = _configuration.GetConnectionString("database1");
                MySqlConnection conexion = new MySqlConnection(connectionString);
                conexion.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandText = "SELECT * FROM nuevo_usuario";

                ListaUsuarios = new List<Usuario>();

                MySqlCommand cmd2 = new MySqlCommand();
                cmd2.Connection = conexion;
                cmd2.CommandText = "SELECT * FROM usuario";

                ListaUsuarios2 = new List<Usuario>();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Usuario usr = new Usuario();
                        usr.id_usuario = Convert.ToInt32(reader["id_usuario"]);
                        usr.Nombre = reader["nombre"].ToString();
                        usr.correo = reader["correo"].ToString();
                        usr.contrasena = reader["contrasena"].ToString();
                        usr.pais = reader["pais"].ToString();
                        usr.edad = Convert.ToInt32(reader["edad"]);
                        ListaUsuarios.Add(usr);
                    }
                }

                using (var reader2 = cmd2.ExecuteReader())
                {
                    while (reader2.Read())
                    {
                        Usuario usr = new Usuario();
                        usr.id_usuario = Convert.ToInt32(reader2["id_usuario"]);
                        usr.Nombre = reader2["nombre"].ToString();
                        usr.correo = reader2["correo"].ToString();
                        usr.contrasena = reader2["contrasena"].ToString();
                        usr.edad = Convert.ToInt32(reader2["edad"]);
                        usr.pais = reader2["pais"].ToString();
                        ListaUsuarios2.Add(usr);
                    }
                }

                conexion.Close();
                return (Page());
            }
        }

        public IActionResult OnPost()
        {
            return RedirectToPage("/Dashboard");
        }
        public IActionResult OnPostVolver()
        {
            return RedirectToPage("/Biomonitor", new { id_usuarioSeleccionado = id_usuarioSeleccionadoPrev });
        }
        public IActionResult OnPostAcceso()
        {
            string connectionString = _configuration.GetConnectionString("database1");
            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                conexion.Open();

                MySqlCommand cmd5 = new MySqlCommand();
                cmd5.Connection = conexion;
                cmd5.CommandText = "delete from nuevo_usuario where correo = @mail";
                cmd5.Parameters.AddWithValue("@mail", correoSeleccionado);
                cmd5.ExecuteNonQuery();

                MySqlCommand cmd6 = new MySqlCommand();
                cmd6.Connection = conexion;
                cmd6.CommandText = "INSERT INTO usuario (nombre, correo, contrasena, pais, username, edad, tiempo_jugado, metodos_completados)\nVALUES \n(@nuevoNombre,@nuevoCorreo,@nuevaContrasena,@nuevoPais, @nuevoUsername, @nuevaEdad , 0, 0 );";
                cmd6.Parameters.AddWithValue("@nuevoNombre", nombreSeleccionado);
                cmd6.Parameters.AddWithValue("@nuevoCorreo", correoSeleccionado);
                cmd6.Parameters.AddWithValue("@nuevaContrasena", contrasenaSeleccionada);
                cmd6.Parameters.AddWithValue("@nuevoPais", paisSeleccionado);
                cmd6.Parameters.AddWithValue("@nuevoUsername", usernameSeleccionado);
                cmd6.Parameters.AddWithValue("@nuevaEdad", edadSeleccionada);
                cmd6.ExecuteNonQuery();

                MySqlCommand cmd10 = new MySqlCommand();
                cmd10.Connection = conexion;
                cmd10.CommandText = "select id_usuario from usuario where nombre = @name;";
                cmd10.Parameters.AddWithValue("@name", nombreSeleccionado);
                object result10 = cmd10.ExecuteScalar();
                int id = Convert.ToInt32(result10);

                MySqlCommand cmd11 = new MySqlCommand();
                cmd11.Connection = conexion;
                cmd11.CommandText = "INSERT INTO sesiones (id_usuario, tiempo) VALUES (@idUs, 0);";
                cmd11.Parameters.AddWithValue("@idUs", id);
                cmd11.ExecuteNonQuery();


                conexion.Close();
            }
            return RedirectToPage();
        }
        public IActionResult OnPostRechazo()
        {
            string connectionString = _configuration.GetConnectionString("database1");
            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                conexion.Open();

                MySqlCommand cmd4 = new MySqlCommand();
                cmd4.Connection = conexion;
                cmd4.CommandText = "delete from nuevo_usuario where correo = @mail";
                cmd4.Parameters.AddWithValue("@mail", correoSeleccionado);
                cmd4.ExecuteNonQuery();
                

                conexion.Close();
            }
                return RedirectToPage();
        }
        public IActionResult OnPostAgregar()
        {
            return RedirectToPage("/Popup", new { seleccion = 1 });
        }
        public IActionResult OnPostEliminacion()
        {
            string connectionString = _configuration.GetConnectionString("database1");
            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                conexion.Open();

                MySqlCommand cmdDeleteSessions = new MySqlCommand();
                cmdDeleteSessions.Connection = conexion;
                cmdDeleteSessions.CommandText = "DELETE FROM sesiones WHERE id_usuario = @userId";
                cmdDeleteSessions.Parameters.AddWithValue("@userId", idSeleccionada);
                cmdDeleteSessions.ExecuteNonQuery();

                MySqlCommand cmdDeleteAnimals = new MySqlCommand();
                cmdDeleteAnimals.Connection = conexion;
                cmdDeleteAnimals.CommandText = "DELETE FROM usuario_has_animal WHERE id_usuario = @userId";
                cmdDeleteAnimals.Parameters.AddWithValue("@userId", idSeleccionada);
                cmdDeleteAnimals.ExecuteNonQuery();

                MySqlCommand cmd4 = new MySqlCommand();
                cmd4.Connection = conexion;
                cmd4.CommandText = "delete from usuario where correo = @mail";
                cmd4.Parameters.AddWithValue("@mail", correoSeleccionado);
                cmd4.ExecuteNonQuery();


                conexion.Close();
            }
            return RedirectToPage();
        }
        public IActionResult OnPostRegistrar()
        {
            if (string.IsNullOrEmpty(nombre))
            {
                textError1 = true;
                //return RedirectToPage("/Popup", new { seleccion = 1, error1 = true });
            }
            if (string.IsNullOrEmpty(correo))
            {
                textError2 = true;
                //return RedirectToPage("/Popup", new { seleccion = 1, error2 = true });
            }
            if (string.IsNullOrEmpty(password))
            {
                textError3 = true;
            }
            if (!edad.HasValue)
            {
                textError4 = true;
            }
            if (string.IsNullOrEmpty(usuarioRegistro))
            {
                textError5 = true;
            }
            if (textError1 == true || textError2 == true || textError3 == true || textError4 == true || textError5 == true)
            {
                return RedirectToPage("/Popup", new { seleccion = 1, error1 = textError1, error2 = textError2, error3 = textError3 , error4 = textError4, error5 = textError5, nombre = nombre, correo = correo, edad = edad, usuarioRegistro = usuarioRegistro });
            }

            string connectionString = _configuration.GetConnectionString("database1");
            using (MySqlConnection conexion = new MySqlConnection(connectionString))
            {
                conexion.Open();

                MySqlCommand cmd7 = new MySqlCommand();
                cmd7.Connection = conexion;
                cmd7.CommandText = "SELECT COUNT(*) FROM usuario WHERE correo = @mail";
                cmd7.Parameters.AddWithValue("@mail", correo);
                int count_user = Convert.ToInt32(cmd7.ExecuteScalar());

                MySqlCommand cmd8 = new MySqlCommand();
                cmd8.Connection = conexion;
                cmd8.CommandText = "SELECT COUNT(*) FROM nuevo_usuario WHERE correo = @mail";
                cmd8.Parameters.AddWithValue("@mail", correo);
                int count_newUser = Convert.ToInt32(cmd8.ExecuteScalar());

                if (count_user > 0 || count_newUser > 0)
                {
                    userExists = true;
                    return RedirectToPage("/Popup", new { seleccion = 1, userExists = userExists });
                }

                MySqlCommand cmd9 = new MySqlCommand();
                cmd9.Connection = conexion;
                cmd9.CommandText = "INSERT INTO usuario (nombre, correo, contrasena, pais, username, edad, tiempo_jugado, metodos_completados) VALUES (@name, @mail, @password, @country, @nUsername, @age, 0, 0);";
                cmd9.Parameters.AddWithValue("@name", nombre);
                cmd9.Parameters.AddWithValue("@mail", correo);
                cmd9.Parameters.AddWithValue("@password", password);
                cmd9.Parameters.AddWithValue("@country", pais);
                cmd9.Parameters.AddWithValue("@nUsername", usuarioRegistro);
                cmd9.Parameters.AddWithValue("@age", edad);
                cmd9.ExecuteNonQuery();

                MySqlCommand cmd12 = new MySqlCommand();
                cmd12.Connection = conexion;
                cmd12.CommandText = "select id_usuario from usuario where nombre = @name;";
                cmd12.Parameters.AddWithValue("@name", nombre);
                object result12 = cmd12.ExecuteScalar();
                int id = Convert.ToInt32(result12);

                MySqlCommand cmd13 = new MySqlCommand();
                cmd13.Connection = conexion;
                cmd13.CommandText = "INSERT INTO sesiones (id_usuario, tiempo) VALUES (@idUs, 0);";
                cmd13.Parameters.AddWithValue("@idUs", id);
                cmd13.ExecuteNonQuery();



                conexion.Close();
                return RedirectToPage("/Popup"); // Redirect despues de registrarse al login
            }
        }
    }
}
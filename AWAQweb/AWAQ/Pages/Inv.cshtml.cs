using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Session;

namespace AWAQ.Pages
{
    public class InvModel : PageModel
    {

        [BindProperty]
        public int? seleccion { get; set; }

        private readonly IConfiguration _configuration;

        public InvModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IList<Animal> ListaAnimales { get; set; }

        [BindProperty]
        public string testt { get; set; }
        [BindProperty]
        public bool hasSession { get; set; }

        public void OnGet(int seleccion)
        {
            testt = HttpContext.Session.GetString("correo");
            if (!string.IsNullOrEmpty(testt))
            {
                hasSession = true;
            }
            this.seleccion = seleccion;

            if (seleccion == 1)
            {
                string connectionString = _configuration.GetConnectionString("database1");
                MySqlConnection conexion = new MySqlConnection(connectionString);
                conexion.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandText = "SELECT * FROM animal where id_metodo_muestreo = @seleccionado";
                cmd.Parameters.AddWithValue("@seleccionado", seleccion);

                ListaAnimales = new List<Animal>();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Animal animal = new Animal();
                        animal.idAnimal = Convert.ToInt32(reader["id_animal"]);
                        animal.nombre = reader["nombre"].ToString();
                        animal.idMetodoMuestreo = Convert.ToInt32(reader["id_metodo_muestreo"]);
                        animal.nombreCientifico = reader["nombre_cientifico"].ToString();
                        animal.altura = reader["lugares_comunes"].ToString();
                        animal.peso = reader["advertencias"].ToString();
                        animal.imagen = reader["imagen"].ToString();
                        animal.active = true;
                        ListaAnimales.Add(animal);
                        animal.clase = "Pájaros";
                        animal.sonido = animal.idAnimal+".mp3";
                    }
                }
                conexion.Close();
            }
            else if (seleccion == 2)
            {
                string connectionString = _configuration.GetConnectionString("database1");
                MySqlConnection conexion = new MySqlConnection(connectionString);
                conexion.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandText = "SELECT * FROM animal where id_metodo_muestreo = @seleccionado";
                cmd.Parameters.AddWithValue("@seleccionado", seleccion);

                ListaAnimales = new List<Animal>();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Animal animal = new Animal();
                        animal.idAnimal = Convert.ToInt32(reader["id_animal"]);
                        animal.nombre = reader["nombre"].ToString();
                        animal.idMetodoMuestreo = Convert.ToInt32(reader["id_metodo_muestreo"]);
                        animal.nombreCientifico = reader["nombre_cientifico"].ToString();
                        animal.altura = reader["lugares_comunes"].ToString();
                        animal.peso = reader["advertencias"].ToString();
                        animal.imagen = reader["imagen"].ToString();
                        animal.active = true;
                        ListaAnimales.Add(animal);
                        animal.clase = "Mamíferos";
                    }
                }
                conexion.Close();
            }
            else if (seleccion == 3)
            {
                string connectionString = _configuration.GetConnectionString("database1");
                MySqlConnection conexion = new MySqlConnection(connectionString);
                conexion.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandText = "SELECT * FROM animal where id_metodo_muestreo = @seleccionado";
                cmd.Parameters.AddWithValue("@seleccionado", seleccion);

                ListaAnimales = new List<Animal>();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Animal animal = new Animal();
                        animal.idAnimal = Convert.ToInt32(reader["id_animal"]);
                        animal.nombre = reader["nombre"].ToString();
                        animal.idMetodoMuestreo = Convert.ToInt32(reader["id_metodo_muestreo"]);
                        animal.nombreCientifico = reader["nombre_cientifico"].ToString();
                        animal.altura = reader["lugares_comunes"].ToString();
                        animal.peso = reader["advertencias"].ToString();
                        animal.imagen = reader["imagen"].ToString();
                        animal.active = true;
                        ListaAnimales.Add(animal);
                        animal.clase = "Plantas";
                    }
                }
                conexion.Close();
            }
            else if (seleccion == 4)
            {
                string connectionString = _configuration.GetConnectionString("database1");
                MySqlConnection conexion = new MySqlConnection(connectionString);
                conexion.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandText = "SELECT * FROM animal where id_metodo_muestreo = @seleccionado";
                cmd.Parameters.AddWithValue("@seleccionado", seleccion);

                ListaAnimales = new List<Animal>();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Animal animal = new Animal();
                        animal.idAnimal = Convert.ToInt32(reader["id_animal"]);
                        animal.nombre = reader["nombre"].ToString();
                        animal.idMetodoMuestreo = Convert.ToInt32(reader["id_metodo_muestreo"]);
                        animal.nombreCientifico = reader["nombre_cientifico"].ToString();
                        animal.altura = reader["lugares_comunes"].ToString();
                        animal.peso = reader["advertencias"].ToString();
                        animal.imagen = reader["imagen"].ToString();
                        animal.active = true;
                        ListaAnimales.Add(animal);
                        animal.clase = "Insectos";
                    }
                }
                conexion.Close();
            }
            else if (seleccion == 5)
            {
                string connectionString = _configuration.GetConnectionString("database1");
                MySqlConnection conexion = new MySqlConnection(connectionString);
                conexion.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandText = "SELECT * FROM animal where id_metodo_muestreo = @seleccionado";
                cmd.Parameters.AddWithValue("@seleccionado", seleccion);

                ListaAnimales = new List<Animal>();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Animal animal = new Animal();
                        animal.idAnimal = Convert.ToInt32(reader["id_animal"]);
                        animal.nombre = reader["nombre"].ToString();
                        animal.idMetodoMuestreo = Convert.ToInt32(reader["id_metodo_muestreo"]);
                        animal.nombreCientifico = reader["nombre_cientifico"].ToString();
                        animal.altura = reader["lugares_comunes"].ToString();
                        animal.peso = reader["advertencias"].ToString();
                        animal.imagen = reader["imagen"].ToString();
                        animal.active = true;
                        ListaAnimales.Add(animal);
                        animal.clase = "Anfibios";

                       
                    }
                }
                conexion.Close();
            }
            else
            {
                string connectionString = _configuration.GetConnectionString("database1");
                MySqlConnection conexion = new MySqlConnection(connectionString);
                conexion.Open();

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = conexion;
                cmd.CommandText = "SELECT * FROM animal";

                ListaAnimales = new List<Animal>();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Animal animal = new Animal();
                        animal.idAnimal = Convert.ToInt32(reader["id_animal"]);
                        animal.nombre = reader["nombre"].ToString();
                        animal.idMetodoMuestreo = Convert.ToInt32(reader["id_metodo_muestreo"]);
                        animal.nombreCientifico = reader["nombre_cientifico"].ToString();
                        animal.altura = reader["lugares_comunes"].ToString();
                        animal.peso = reader["advertencias"].ToString();
                        animal.imagen = reader["imagen"].ToString();
                        animal.active = true;
                        ListaAnimales.Add(animal);
                        if(animal.idMetodoMuestreo == 1) {
                            animal.clase = "Pájaros";
                            animal.sonido = animal.idAnimal + ".mp3";
                        }
                        else if (animal.idMetodoMuestreo == 2) { animal.clase = "Mamíferos"; }
                        else if (animal.idMetodoMuestreo == 3) { animal.clase = "Plantas"; }
                        else if (animal.idMetodoMuestreo == 4) { animal.clase = "Insectos"; }
                        else if (animal.idMetodoMuestreo == 5) { animal.clase = "Anfibios"; }
                    }
                }
                conexion.Close();
            }
        }

        public IActionResult OnPostBusquedaPajaros()
        {
            return RedirectToPage("/Inv", new { seleccion = 1 });
        }
        public IActionResult OnPostBusquedaMamiferos()
        {
            return RedirectToPage("/Inv", new { seleccion = 2 });
        }
        public IActionResult OnPostBusquedaPlantas()
        {
            return RedirectToPage("/Inv", new { seleccion = 3 });
        }
        public IActionResult OnPostBusquedaInsectos()
        {
            return RedirectToPage("/Inv", new { seleccion = 4 });
        }
        public IActionResult OnPostBusquedaAnfibios()
        {
            return RedirectToPage("/Inv", new { seleccion = 5 });
        }

    }
}



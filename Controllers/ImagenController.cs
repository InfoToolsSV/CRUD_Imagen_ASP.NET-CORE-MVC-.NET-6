using System.Data.SqlClient;
using CRUD_Imagen.Data;
using CRUD_Imagen.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace CRUD_Imagen.Controllers
{
    public class ImagenController : Controller
    {
        public readonly Contexto _contexto;

        public ImagenController(Contexto contexto)
        {
            _contexto = contexto;
        }
        public IActionResult Index()
        {
            using (SqlConnection con = new(_contexto.Conexion))
            {
                List<Imagen> lista_imagenes = new();
                using (SqlCommand cmd = new("sp_listar_imagenes", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();

                    var rd = cmd.ExecuteReader();
                    while (rd.Read())
                    {
                        lista_imagenes.Add(new Imagen
                        {
                            Id_Imagen = (int)rd["Id_Imagen"],
                            Nombre = rd["Nombre"].ToString(),
                            Image = rd["Imagen"].ToString()
                        });
                    }
                }
                ViewBag.listado = lista_imagenes;
                return View();
            }
        }

        public IActionResult Crear()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Crear(Imagen imagen)
        {
            try
            {
                byte[] bytes;
                if (imagen.File != null && imagen.Nombre != null)
                {
                    using (Stream fs = imagen.File.OpenReadStream())
                    {
                        using (BinaryReader br = new(fs))
                        {
                            bytes = br.ReadBytes((int)fs.Length);
                            imagen.Image = Convert.ToBase64String(bytes, 0, bytes.Length);

                            using (SqlConnection con = new(_contexto.Conexion))
                            {
                                using (SqlCommand cmd = new("sp_insertar_imagen", con))
                                {
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.Add("@Nombre", SqlDbType.VarChar).Value = imagen.Nombre;
                                    cmd.Parameters.Add("@Imagen", SqlDbType.VarChar).Value = imagen.Image;
                                    con.Open();
                                    cmd.ExecuteNonQuery();
                                    con.Close();
                                }
                            }
                        }
                    }
                }
            }
            catch (System.Exception e)
            {
                ViewBag.error = e.Message;
                return View();
            }
            return RedirectToAction("Index");
        }

        public IActionResult Editar(int id)
        {
            using (SqlConnection con = new(_contexto.Conexion))
            {
                Imagen registro = new();
                using (SqlCommand cmd = new("sp_buscar_imagen", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;
                    con.Open();

                    SqlDataAdapter da = new(cmd);
                    DataTable dt = new();
                    da.Fill(dt);
                    registro.Id_Imagen = (int)dt.Rows[0][0];
                    registro.Nombre = dt.Rows[0][1].ToString();
                    registro.Image = dt.Rows[0][2].ToString();
                }
                return View(registro);
            }
        }

        [HttpPost]
        public IActionResult Editar(Imagen imagen)
        {
            try
            {
                using (SqlConnection con = new(_contexto.Conexion))
                {
                    string i;
                    if (imagen.File == null)
                    {
                        i = "null";
                    }
                    else
                    {
                        byte[] bytes;
                        using (Stream fs = imagen.File.OpenReadStream())
                        {
                            using (BinaryReader br = new(fs))
                            {
                                bytes = br.ReadBytes((int)fs.Length);
                                i = Convert.ToBase64String(bytes, 0, bytes.Length);
                            }
                        }
                    }
                    using (SqlCommand cmd = new("sp_actualizar_imagen", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@Id", SqlDbType.Int).Value = imagen.Id_Imagen;
                        cmd.Parameters.Add("@Nombre", SqlDbType.VarChar).Value = imagen.Nombre;
                        cmd.Parameters.Add("@Imagen", SqlDbType.VarChar).Value = i;
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
            }
            catch (System.Exception e)
            {
                ViewBag.error = e.Message;
                return View();
            }
            return RedirectToAction("Index");
        }

        public IActionResult Eliminar(int id)
        {
            using (SqlConnection con = new(_contexto.Conexion))
            {
                Imagen registro = new();
                using (SqlCommand cmd = new("sp_buscar_imagen", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Id", SqlDbType.Int).Value = id;
                    con.Open();

                    SqlDataAdapter da = new(cmd);
                    DataTable dt = new();
                    da.Fill(dt);
                    registro.Id_Imagen = (int)dt.Rows[0][0];
                    registro.Nombre = dt.Rows[0][1].ToString();
                    registro.Image = dt.Rows[0][2].ToString();
                }
                return View(registro);
            }
        }

        [HttpPost]
        public IActionResult Eliminar(Imagen img)
        {
            using (SqlConnection con = new(_contexto.Conexion))
            {
                using (SqlCommand cmd = new("sp_eliminar_imagen", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@Id", SqlDbType.Int).Value = img.Id_Imagen;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                return RedirectToAction("Index");
            }
        }
    }
}
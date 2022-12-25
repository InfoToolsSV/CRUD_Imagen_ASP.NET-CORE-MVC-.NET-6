using System.ComponentModel.DataAnnotations.Schema;

namespace CRUD_Imagen.Models
{
    public class Imagen
    {
        public int Id_Imagen { get; set; }
        public string? Nombre { get; set; }
        public string? Image { get; set; }

        [NotMapped]
        public IFormFile? File { get; set; }
    }
}
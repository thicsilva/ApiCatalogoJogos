using System.ComponentModel.DataAnnotations;

namespace ApiCatalogoJogo.InputModel
{
    public class GameInputModel
    {
        [Required]
        [StringLength(100, MinimumLength =3, ErrorMessage = "Name must be have between 3 and 100 characters")]
        public string Name { get; set; }
        [Required]
        [StringLength(100, MinimumLength =1, ErrorMessage ="Producer must be have between 1 and 100 characters")]
        public string Producer { get; set; }
        [Required]
        [Range(1,1000, ErrorMessage ="Price must be between 1 and 1000 dollars")]
        public double Price { get; set; }
    }
}

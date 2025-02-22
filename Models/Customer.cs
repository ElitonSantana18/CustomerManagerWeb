using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net;

namespace CustomerManagerWeb.Models
{
    public class Customer
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo Nome é obrigatório.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "O campo Email é obrigatório.")]
        public string Email { get; set; }

        /// <summary>
        /// Imagem da companhia
        /// </summary>
        public string? CompanyLogo { get; set; }

        public List<Address>? Addresses { get; set; } 

        public IFormFile? ImageFile { get; set; }
    }
}

using CustomerManagerWeb.Models.Helpers;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace CustomerManagerWeb.Models
{
    public class Address
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }

        [Required(ErrorMessage = "O campo Nome é obrigatório.")]
        public string Name { get; set; }
    }
}

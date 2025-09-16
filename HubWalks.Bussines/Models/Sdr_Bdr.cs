using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HubWalks.Bussines.Models
{
    public class Sdr_Bdr
    {
        [Key]
        public Guid IdSdr_Bdr { get; set; }

        [Required(ErrorMessage = "O campo Nome é obrigatório")]
        [StringLength(200, ErrorMessage = "O Nome pode ter no máximo 200 caracteres")]
        public string Nome { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DataCadastro { get; set; }

        public ICollection<JobOrder> JobOrders { get; set; } = new List<JobOrder>();

        public Sdr_Bdr()
        {
            IdSdr_Bdr = Guid.NewGuid();
            DataCadastro = DateTime.Now;
        }
    }
}

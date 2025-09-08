using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HubWalks.Bussines.Models
{
    public class NotaFiscal
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("JobOrder")]
        public int IdJobOrder { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DataEmissao { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal ValorTotal { get; set; }

        [StringLength(20)]
        public string NumeroNota { get; set; }

        [StringLength(20)]
        public string Serie { get; set; }

        [StringLength(100)]
        public string ChaveAcesso { get; set; }

    }
}

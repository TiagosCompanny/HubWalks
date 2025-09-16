using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace HubWalks.Bussines.Models
{
    public class JobOrder
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(120)]
        [Display(Name = "Nome do Projeto")]
        public string NomeProjeto { get; set; }

        [Required]
        [StringLength(2000)]
        [Display(Name = "Descrição")]
        public string Descricao { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Data de Cadastro")]
        public DateTime DataSolicitacao { get; set; }

        [Required]
        [Display(Name = "Cliente")]
        public Guid IdClient { get; set; }

        [ValidateNever]
        [ForeignKey(nameof(IdClient))]
        public Cliente Cliente { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Closer")]
        public string Closer { get; set; }


        [Required]
        [Display(Name = "SDR/BDR")]
        public Guid SdrBdrId { get; set; }


        [ValidateNever]
        [ForeignKey(nameof(SdrBdrId))]
        public Sdr_Bdr SdrBdr { get; set; }

        [Range(0, 100)]
        [Display(Name = "% Comissão Comercial")]
        public double PercentualComissaoComercial { get; set; }

        [StringLength(2000)]
        [Display(Name = "Observação")]
        public string Observacao { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Prazo")]
        public DateTime Prazo { get; set; }

        [StringLength(2000)]
        [Display(Name = "Promessas")]
        public string Promessas { get; set; }

        [Range(0, double.MaxValue)]
        [DataType(DataType.Currency)]
        [Display(Name = "Valor")]
        public double Valor { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Forma de Pagamento")]
        public string FormaPagamento { get; set; }


        public JobOrder()
        {
            DataSolicitacao = DateTime.Now;
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HubWalks.Bussines.Models
{
    public class Cliente
    {
        [Key]
        public Guid IdCliente { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório")]
        [StringLength(150, ErrorMessage = "O nome pode ter no máximo 150 caracteres")]
        public string NomeCliente { get; set; }

        [Required(ErrorMessage = "O endereço é obrigatório")]
        [StringLength(250)]
        public string Endereco { get; set; }

        [EmailAddress(ErrorMessage = "E-mail inválido")]
        [StringLength(100)]
        public string? Email { get; set; }

        [Required(ErrorMessage = "O número de telefone é obrigatório")]
        [Phone(ErrorMessage = "Telefone inválido")]
        [StringLength(20)]
        public string NumeroTelefone { get; set; }

        [Required(ErrorMessage = "CPF/CNPJ é obrigatório")]
        [StringLength(18, ErrorMessage = "CPF ou CNPJ inválido")]
        public string CpfCnpj { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DataNascimento { get; set; }

        [Url(ErrorMessage = "URL inválida")]
        public string? SiteOficial { get; set; }

        [Url(ErrorMessage = "URL inválida")]
        public string? Instagram { get; set; }

        [Url(ErrorMessage = "URL inválida")]
        public string? RedeSocial_1 { get; set; }

        [Url(ErrorMessage = "URL inválida")]
        public string? RedeSocial_2 { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime DataCadastro { get; set; }

        public Cliente()
        {
            IdCliente = Guid.NewGuid();
            DataCadastro = DateTime.Now;
        }
    }
}

using System;
using System.ComponentModel.DataAnnotations;

namespace ListagemdeCompradores.Models
{
    public class ClienteModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "*O Campo Telefone é obrigatório")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "*O Campo Email é obrigatório")]
        [EmailAddress(ErrorMessage ="Email informado não é válido")]
        public string Email { get; set; }

        [Required(ErrorMessage ="*O Campo Telefone é obrigatório")]
        [Phone(ErrorMessage ="*O Telefone informado não é válido")]
        public string Telefone { get; set; }
        public DateTime DtaCadastro { get; set; }
        public int TipoPessoa { get; set; }

        [Required(ErrorMessage = "*O Campo Documento é obrigatório")]
        public string Documento { get; set; }

        public string InscricaoEstadual { get; set; }

        public bool Isento { get; set; }
        public string Genero { get; set;}
        public bool Bloqueio { get; set;}
        public string Senha { get; set; }
    }
}

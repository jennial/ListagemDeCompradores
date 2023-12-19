using Microsoft.AspNetCore.Mvc;
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
        [Remote("VerificaEmail", "Cliente", ErrorMessage = "Email já cadastrado.")]

        public string Email { get; set; }

        [Required(ErrorMessage ="*O Campo Telefone é obrigatório")]
        [Phone(ErrorMessage ="*O Telefone informado não é válido")]

        public string Telefone { get; set; }
        public DateTime DtaCadastro { get; set; } 
    
        public int TipoPessoa { get; set; }

        [Required(ErrorMessage = "*O Campo Documento é obrigatório")]
        [Remote("VerificarExistenciaCPF", "Cliente", ErrorMessage = "CPF já cadastrado.")]

        public string Documento { get; set; }

        public string InscricaoEstadual { get; set; }

        public bool Isento { get; set; }
        public string Genero { get; set;}
        public bool Bloqueio { get; set;}
        [Required(ErrorMessage = "*O Campo Senha é obrigatório")]
        public string Senha { get; set; }
        [Required(ErrorMessage = "*O Campo Confirmar Senha é obrigatório")]

        [Compare("Senha", ErrorMessage = "*A senha e a confirmação de senha não coincidem.")]
        public string confirmarSenha { get; set; }

        public DateTime DtaNascimento { get; set; } 
    }

}

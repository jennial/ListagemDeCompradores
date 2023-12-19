using ListagemdeCompradores.Data;
using ListagemdeCompradores.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ListagemdeCompradores.Repositorios
{
    public class ClienteRepositorio : IClienteRepositorio
    {
        private readonly BancoContext _bancoContext;
        public ClienteRepositorio(BancoContext bancoContext)
        {
            _bancoContext = bancoContext;
        }
        public ClienteModel Adicionar(ClienteModel cliente)
        {
            _bancoContext.Cliente.Add(cliente);
            _bancoContext.SaveChanges();
            return cliente;
        }

        public ClienteModel Atualizar(ClienteModel cliente)
        {
            ClienteModel clienteDB = listarId(cliente.Id);
            if (clienteDB == null) throw new System.Exception("Erro ");

            clienteDB.Nome = cliente.Nome;
            clienteDB.Email = cliente.Email;
            clienteDB.Telefone = cliente.Telefone;
            clienteDB.Documento = cliente.Documento;
            clienteDB.Senha = cliente.Senha;
            clienteDB.Bloqueio = cliente.Bloqueio;
            clienteDB.Genero = cliente.Genero;
            clienteDB.InscricaoEstadual = cliente.InscricaoEstadual;
            clienteDB.TipoPessoa = cliente.TipoPessoa;
            clienteDB.Isento = cliente.Isento;
            clienteDB.Senha = cliente.Senha;
            clienteDB.confirmarSenha = cliente.confirmarSenha;
            clienteDB.DtaNascimento = cliente.DtaNascimento;
            clienteDB.DtaCadastro = cliente.DtaNascimento;

            _bancoContext.Cliente.Update(clienteDB);
            _bancoContext.SaveChanges();
            return clienteDB;
        }
        public List<ClienteModel> VerificaEmailDB(string email, string documento, string inscricaoEstadual)
        {
            List<ClienteModel> clientesExistentes = _bancoContext.Cliente
            .Where(c => c.Email == email || c.Documento == documento || c.InscricaoEstadual == inscricaoEstadual)
            .ToList();
            return clientesExistentes;
        }


        public List<ClienteModel> FiltrarClientes(string Nome, string Email, string Telefone, DateTime DtaCadastro, bool Bloqueado)
        {
            IQueryable<ClienteModel> query = _bancoContext.Cliente;

            if (!string.IsNullOrEmpty(Nome))
            {
                query = query.Where(item => item.Nome.Contains(Nome));
            }

            if (!string.IsNullOrEmpty(Email))
            {
                query = query.Where(item => item.Email.Contains(Email));
            }

            if (!string.IsNullOrEmpty(Telefone))
            {
                query = query.Where(item => item.Telefone.Contains(Telefone));
            }

            if (DtaCadastro != default(DateTime))
            {
                query = query.Where(item => item.DtaCadastro == DtaCadastro);
            }

            query = query.Where(item => item.Bloqueio == Bloqueado);

            return query.ToList();
        }


        public ClienteModel listarId(int id)
        {
            return _bancoContext.Cliente.FirstOrDefault(item => item.Id == id);
        }

        public List<ClienteModel> TodosRegistros()
        {
            return _bancoContext.Cliente.ToList();
        }
    }
}

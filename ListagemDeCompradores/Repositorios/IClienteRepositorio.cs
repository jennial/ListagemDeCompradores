using ListagemdeCompradores.Models;
using System;
using System.Collections.Generic;

namespace ListagemdeCompradores.Repositorios
{
    public interface IClienteRepositorio
    {
        ClienteModel listarId(int Id);
        List<ClienteModel> TodosRegistros();

        ClienteModel Adicionar(ClienteModel cliente);

        ClienteModel Atualizar(ClienteModel cliente);
        List<ClienteModel> VerificaEmailDB(string email, string documento, string inscricaoEstadual);
        List<ClienteModel> FiltrarClientes(string Nome, string Email, string Telefone, DateTime dtaCadastro, bool Bloqueado);
    }
}

using AspNetCoreHero.ToastNotification.Abstractions;
using ListagemdeCompradores.Data;
using ListagemdeCompradores.Models;
using ListagemdeCompradores.Repositorios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ListagemdeCompradores.Controllers
{
    public class ClienteController : Controller
    {
        private readonly IClienteRepositorio _clienteRepositorio;
        private readonly INotyfService _notyf;
        public ClienteController(IClienteRepositorio clienteRepositorio, INotyfService notyf)
        {
            _clienteRepositorio = clienteRepositorio;
            _notyf = notyf;
        }
        public IActionResult Index(int? page)
        {
            List<ClienteModel> clientes;
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            if (HttpContext.Request.Method == "POST")
            {
                string Nome = Request.Form["Nome"];
                string Email = Request.Form["Email"];
                string Telefone = Request.Form["Telefone"];
                DateTime.TryParse(Request.Form["DtaCadastro"], out DateTime DtaCadastro);
                DtaCadastro = DtaCadastro.Date;
                bool.TryParse(Request.Form["Bloqueio"], out bool Bloqueio);
                if (string.IsNullOrEmpty(Nome) && string.IsNullOrEmpty(Email) && string.IsNullOrEmpty(Telefone) && Bloqueio == false)
                {
                    clientes = _clienteRepositorio.TodosRegistros();
                    return View(clientes);
                }
         
               
                return FiltrarClientes(Nome, Email, Telefone, DtaCadastro, Bloqueio) as ViewResult;
            }          
            else
            {
                clientes = _clienteRepositorio.TodosRegistros();
                return View(clientes.ToPagedList(pageNumber, pageSize));
            }
        }


        [HttpPost]
        public IActionResult FiltrarClientes(string Nome, string Email, string Telefone, DateTime DtaCadastro, bool Bloqueio)
        {
            List<ClienteModel> clientesFiltrados = _clienteRepositorio.FiltrarClientes(Nome, Email, Telefone, DtaCadastro, Bloqueio);

            return View("Index", clientesFiltrados);
        }

        public IActionResult Cadastrar()
        {
            return View();
        }
        public IActionResult Editar(int id)
        {
            ClienteModel contatoId = _clienteRepositorio.listarId(id);
            return View(contatoId);
        }

        [HttpPost]
        public IActionResult Cadastrar(ClienteModel cliente) 
        {
            if(ModelState.IsValid)
            {
                cliente.DtaCadastro = DateTime.Now;
                _clienteRepositorio.Adicionar(cliente);
                _notyf.Success("Cliente Cadastrado com sucesso!");

                return RedirectToAction("Index");
            }
            _notyf.Error("Não foi possível realizar o cadastro.");
            _notyf.Information("Verifique se todos os campos estão preenchidos corretamente.");
            return View(cliente);
        } 
        
        [HttpPost]
        public IActionResult Editar(ClienteModel cliente) 
        {
            if (ModelState.IsValid)
            {

                cliente.DtaCadastro = DateTime.Now;
                _clienteRepositorio.Atualizar(cliente);
                return RedirectToAction("Index");
            }
            return View(cliente);
        }
    }
}

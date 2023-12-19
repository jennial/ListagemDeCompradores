using AspNetCoreHero.ToastNotification.Abstractions;
using ListagemdeCompradores.Data;
using ListagemdeCompradores.Models;
using ListagemdeCompradores.Repositorios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PagedList;
using System.Globalization;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ListagemdeCompradores.Controllers
{
    public class ClienteController : Controller
    {
        private readonly IClienteRepositorio _clienteRepositorio;
        private readonly INotyfService _notyf;
        private bool formOK = true;
        public ClienteController(IClienteRepositorio clienteRepositorio, INotyfService notyf)
        {
            _clienteRepositorio = clienteRepositorio;
            _notyf = notyf;
        }
        public IActionResult Index(int? page)
        {
            const int pageSize = 5;
            List<ClienteModel> clientes;

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
                }
                else
                {
                    var resultadoFiltrado = FiltrarClientes(Nome, Email, Telefone, DtaCadastro, Bloqueio) as ViewResult;
                    if (resultadoFiltrado != null)
                    {
                        var clientesFiltrados = resultadoFiltrado.Model as List<ClienteModel>;
                        clientes = clientesFiltrados?.ToList() ?? new List<ClienteModel>();
                    }
                    else
                    {
                        clientes = new List<ClienteModel>();
                    }
                }
            }
            else
            {
                clientes = _clienteRepositorio.TodosRegistros();
            }

            var pageNumber = page ?? 1;
            var pagedClientes = clientes.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            ViewData["PageNumber"] = pageNumber;
            ViewData["PageSize"] = pageSize;
            ViewData["TotalPages"] = (int)Math.Ceiling((double)clientes.Count / pageSize);

            return View(pagedClientes);
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
            if (ModelState.IsValid)
            {
                List<ClienteModel> clientesExistentes = _clienteRepositorio.VerificaEmailDB(cliente.Email, cliente.Documento, cliente.InscricaoEstadual);
               
                if (clientesExistentes.Count > 0)
                {

                    if (clientesExistentes.Any(c => c.Email == cliente.Email))
                    {
                        formOK = false;
                        _notyf.Error("Não foi possível realizar o cadastro. O email já está cadastrado.");
                    }

                    if (clientesExistentes.Any(c => c.Documento == cliente.Documento))
                    {
                        formOK = false;
                        _notyf.Error("Não foi possível realizar o cadastro. O documento já está cadastrado.");
                    }

                    if (clientesExistentes.Any(c => c.InscricaoEstadual == cliente.InscricaoEstadual && c.InscricaoEstadual != null))
                    {
                        formOK = false;
                        _notyf.Error("Não foi possível realizar o cadastro. A inscrição estadual já está cadastrada.");
                    }

                }
                if (formOK)
                {
                    cliente.DtaCadastro = DateTime.Now;
                    _clienteRepositorio.Adicionar(cliente);
                    _notyf.Success("Cliente cadastrado com sucesso!");

                    return RedirectToAction("Index");

                }


            }

            _notyf.Error("Não foi possível realizar oewewe cadastro.");
            _notyf.Information("Verifique se todos os campos estão preenchidos corretamente.");

            return View(cliente);

        }

        [HttpPost]
        public IActionResult Editar(ClienteModel cliente)
        {
            if (ModelState.IsValid)
            {
                if (formOK == false)
                {
                    _notyf.Error("Não foi possível realizar a alteração. Verifique os campos alterados");
                }

                cliente.DtaCadastro = DateTime.Now;
                _clienteRepositorio.Atualizar(cliente);
                return RedirectToAction("Index");
            }
            return View(cliente);
        }
        [HttpPost]
        public JsonResult VerificaEmail(string email, string documento, string inscricaoEstadual)
        {
            List<ClienteModel> clientesExistentes = _clienteRepositorio.VerificaEmailDB(email, documento, inscricaoEstadual);
            return Json(new { EmailExiste = clientesExistentes.Count > 0, Clientes = clientesExistentes });
        }



    }


}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HubWalks.Bussines.Interfaces;
using HubWalks.Bussines.Models;

namespace HubWalks.Controllers
{
    public class ClientesController : Controller
    {
        private readonly IService<Cliente> _clienteService;

        public ClientesController(IService<Cliente> clienteService)
        {
            _clienteService = clienteService;
        }

        // GET: Clientes
        public async Task<IActionResult> Index()
        {
            var clientes = await _clienteService.GetAllAsync();
            return View(clientes.OrderBy(c => c.NomeCliente));
        }

        // GET: Clientes/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null) return NotFound();

            var cliente = await _clienteService.GetByIdAsync(id.Value);

            if (cliente == null) return NotFound();

            return View(cliente);
        }

        // GET: Clientes/Create
        public IActionResult Create() => View();

        // POST: Clientes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NomeCliente,Endereco,Email,NumeroTelefone,CpfCnpj,DataNascimento,SiteOficial,Instagram,RedeSocial_1,RedeSocial_2")] Cliente cliente)
        {
            if (!ModelState.IsValid) return View(cliente);

            cliente.IdCliente = Guid.NewGuid();
            cliente.DataCadastro = DateTime.UtcNow; // defina no servidor

            await _clienteService.AddAsync(cliente);
            return RedirectToAction(nameof(Index));
        }

        // GET: Clientes/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null) return NotFound();

            var cliente = await _clienteService.GetByIdAsync(id.Value);
            if (cliente == null) return NotFound();

            return View(cliente);
        }

        // POST: Clientes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("NomeCliente,Endereco,Email,NumeroTelefone,CpfCnpj,DataNascimento,SiteOficial,Instagram,RedeSocial_1,RedeSocial_2")] Cliente form)
        {
            // garante o Id do registro
            form.IdCliente = id;

            if (!ModelState.IsValid) return View(form);

            // preserva DataCadastro do banco
            var original = await _clienteService.GetByIdAsync(id);

            if (original == null) return NotFound();

            form.DataCadastro = original.DataCadastro;

            try
            {
                await _clienteService.UpdateAsync(form);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await ClienteExists(id)) return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Clientes/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null) return NotFound();

            var cliente = await _clienteService.GetByIdAsync(id.Value);

            if (cliente == null) return NotFound();

            return View(cliente);
        }

        // POST: Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _clienteService.RemoveAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> ClienteExists(Guid id) =>
            await _clienteService.GetByIdAsync(id) != null;
    }
}

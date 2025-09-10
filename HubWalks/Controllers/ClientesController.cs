using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HubWalks.Bussines.Models;
using HubWalks.Data;
using HubWalks.Data.Context;

namespace HubWalks.Controllers
{
    public class ClientesController : Controller
    {
        private readonly HubWalksDbContext _context;

        public ClientesController(HubWalksDbContext context)
        {
            _context = context;
        }

        // GET: Clientes
        public async Task<IActionResult> Index()
        {
            var clientes = await _context.Clientes
                .OrderBy(c => c.NomeCliente)
                .AsNoTracking()
                .ToListAsync();

            return View(clientes);
        }

        // GET: Clientes/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null) return NotFound();

            var cliente = await _context.Clientes
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.IdCliente == id);

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

            _context.Add(cliente);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Clientes/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null) return NotFound();

            var cliente = await _context.Clientes.FindAsync(id);
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
            var original = await _context.Clientes
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.IdCliente == id);

            if (original == null) return NotFound();

            form.DataCadastro = original.DataCadastro;

            try
            {
                _context.Update(form);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClienteExists(id)) return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: Clientes/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null) return NotFound();

            var cliente = await _context.Clientes
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.IdCliente == id);

            if (cliente == null) return NotFound();

            return View(cliente);
        }

        // POST: Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente != null)
            {
                _context.Clientes.Remove(cliente);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool ClienteExists(Guid id) =>
            _context.Clientes.Any(e => e.IdCliente == id);
    }
}

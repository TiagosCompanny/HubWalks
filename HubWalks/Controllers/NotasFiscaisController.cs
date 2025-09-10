using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HubWalks.Bussines.Models;
using HubWalks.Data.Context;

namespace HubWalks.Controllers
{
    public class NotasFiscaisController : Controller
    {
        private readonly HubWalksDbContext _context;

        public NotasFiscaisController(HubWalksDbContext context)
        {
            _context = context;
        }

        // GET: NotasFiscais
        public async Task<IActionResult> Index()
        {
            var list = await _context.NotasFicais
                .AsNoTracking()
                .OrderByDescending(n => n.DataEmissao)
                .ToListAsync();

            return View(list);
        }

        // GET: NotasFiscais/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var notaFiscal = await _context.NotasFicais
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);

            if (notaFiscal == null) return NotFound();

            return View(notaFiscal);
        }

        // GET: NotasFiscais/Create
        public IActionResult Create()
        {
            var orders = _context.OrdensDeServico
                .AsNoTracking()
                .OrderBy(o => o.NomeProjeto)
                .Select(o => new { o.Id, Texto = $"{o.Id} - {o.NomeProjeto}" })
                .ToList();

            ViewBag.SemJobOrders = !orders.Any();
            ViewBag.JobOrders = new SelectList(orders, "Id", "Texto");

            return View();
        }

        // POST: NotasFiscais/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdJobOrder,ValorTotal,NumeroNota,Serie,ChaveAcesso")] NotaFiscal notaFiscal)
        {
            // Define a data no servidor
            notaFiscal.DataEmissao = DateTime.UtcNow;

            if (!ModelState.IsValid)
            {
                await CarregarJobOrders(notaFiscal.IdJobOrder);
                return View(notaFiscal);
            }

            _context.Add(notaFiscal);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: NotasFiscais/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var notaFiscal = await _context.NotasFicais.FindAsync(id);
            if (notaFiscal == null) return NotFound();

            await CarregarJobOrders(notaFiscal.IdJobOrder);
            return View(notaFiscal);
        }

        // POST: NotasFiscais/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdJobOrder,ValorTotal,NumeroNota,Serie,ChaveAcesso")] NotaFiscal form)
        {
            if (id != form.Id) return NotFound();

            if (!ModelState.IsValid)
            {
                await CarregarJobOrders(form.IdJobOrder);
                return View(form);
            }

            // preserva DataEmissao do banco
            var original = await _context.NotasFicais
                .AsNoTracking()
                .FirstOrDefaultAsync(n => n.Id == id);
            if (original == null) return NotFound();

            form.DataEmissao = original.DataEmissao;

            try
            {
                _context.Update(form);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NotaFiscalExists(form.Id)) return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: NotasFiscais/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var notaFiscal = await _context.NotasFicais
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);

            if (notaFiscal == null) return NotFound();

            return View(notaFiscal);
        }

        // POST: NotasFiscais/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var notaFiscal = await _context.NotasFicais.FindAsync(id);
            if (notaFiscal != null)
            {
                _context.NotasFicais.Remove(notaFiscal);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool NotaFiscalExists(int id) =>
            _context.NotasFicais.Any(e => e.Id == id);

        private async Task CarregarJobOrders(int? selectedId)
        {
            var orders = await _context.OrdensDeServico
                .AsNoTracking()
                .OrderBy(o => o.NomeProjeto)
                .Select(o => new { o.Id, Texto = $"{o.Id} - {o.NomeProjeto}" })
                .ToListAsync();

            ViewBag.SemJobOrders = !orders.Any();
            ViewBag.JobOrders = new SelectList(orders, "Id", "Texto", selectedId);
        }
    }
}

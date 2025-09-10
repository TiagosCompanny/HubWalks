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
    public class JobOrdersController : Controller
    {
        private readonly HubWalksDbContext _context;

        public JobOrdersController(HubWalksDbContext context)
        {
            _context = context;
        }

        // GET: JobOrders
        public async Task<IActionResult> Index()
        {
            var list = await _context.OrdensDeServico
                .AsNoTracking()
                .OrderBy(o => o.NomeProjeto)
                .ToListAsync();

            return View(list);
        }

        // GET: JobOrders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var jobOrder = await _context.OrdensDeServico
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);

            if (jobOrder == null) return NotFound();

            return View(jobOrder);
        }

        // GET: JobOrders/Create
        public IActionResult Create()
        {
            var clientes = _context.Clientes.AsNoTracking().OrderBy(c => c.NomeCliente).ToList();
            var sdrBdrs = _context.Sdr_Bdrs.AsNoTracking().OrderBy(s => s.Nome).ToList();

            ViewBag.SemClientesOuBdr = !clientes.Any() || !sdrBdrs.Any();
            ViewBag.Clientes = new SelectList(clientes, "IdCliente", "NomeCliente");
            // value = IdSdr_Bdr (GUID) e texto = Nome; irá para string Sdr_Bdr
            ViewBag.SdrBdrs = new SelectList(sdrBdrs, "IdSdr_Bdr", "Nome");

            return View();
        }

        // POST: JobOrders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NomeProjeto,Descricao,IdClient,Closer,Sdr_Bdr,PercentualComissaoComercial,Observacao,Prazo,Promessas,Valor,FormaPagamento")] JobOrder jobOrder)
        {
            // seta data no servidor
            jobOrder.DataSolicitacao = DateTime.UtcNow;

            if (!ModelState.IsValid)
            {
                await CarregarCombos(jobOrder.IdClient, jobOrder.Sdr_Bdr);
                return View(jobOrder);
            }

            _context.Add(jobOrder);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: JobOrders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var jobOrder = await _context.OrdensDeServico.FindAsync(id);
            if (jobOrder == null) return NotFound();

            await CarregarCombos(jobOrder.IdClient, jobOrder.Sdr_Bdr);
            return View(jobOrder);
        }

        // POST: JobOrders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NomeProjeto,Descricao,IdClient,Closer,Sdr_Bdr,PercentualComissaoComercial,Observacao,Prazo,Promessas,Valor,FormaPagamento")] JobOrder form)
        {
            if (id != form.Id) return NotFound();

            if (!ModelState.IsValid)
            {
                await CarregarCombos(form.IdClient, form.Sdr_Bdr);
                return View(form);
            }

            // preserva DataSolicitacao do banco
            var original = await _context.OrdensDeServico
                .AsNoTracking()
                .FirstOrDefaultAsync(o => o.Id == id);

            if (original == null) return NotFound();

            form.DataSolicitacao = original.DataSolicitacao;

            try
            {
                _context.Update(form);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!JobOrderExists(form.Id)) return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: JobOrders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var jobOrder = await _context.OrdensDeServico
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);

            if (jobOrder == null) return NotFound();

            return View(jobOrder);
        }

        // POST: JobOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var jobOrder = await _context.OrdensDeServico.FindAsync(id);
            if (jobOrder != null)
            {
                _context.OrdensDeServico.Remove(jobOrder);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private bool JobOrderExists(int id) =>
            _context.OrdensDeServico.Any(e => e.Id == id);

        private async Task CarregarCombos(Guid? selectedCliente, string selectedSdrBdr)
        {
            var clientes = await _context.Clientes.AsNoTracking().OrderBy(c => c.NomeCliente).ToListAsync();
            var sdrBdrs = await _context.Sdr_Bdrs.AsNoTracking().OrderBy(s => s.Nome).ToListAsync();

            ViewBag.SemClientesOuBdr = !clientes.Any() || !sdrBdrs.Any();
            ViewBag.Clientes = new SelectList(clientes, "IdCliente", "NomeCliente", selectedCliente);
            ViewBag.SdrBdrs = new SelectList(sdrBdrs, "IdSdr_Bdr", "Nome", selectedSdrBdr);
        }
    }
}

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
            return View(await _context.OrdensDeServico.ToListAsync());
        }

        // GET: JobOrders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var jobOrder = await _context.OrdensDeServico
                .FirstOrDefaultAsync(m => m.Id == id);

            if (jobOrder == null) return NotFound();

            return View(jobOrder);
        }

        // GET: JobOrders/Create
        public IActionResult Create()
        {
            var clientes = _context.Clientes.ToList();
            var sdrBdrs = _context.Sdr_Bdrs.ToList();

            if (!clientes.Any() || !sdrBdrs.Any())
            {
                ViewBag.SemClientesOuBdr = true;
            }

            ViewBag.Clientes = new SelectList(clientes, "IdCliente", "NomeCliente");
            ViewBag.SdrBdrs = new SelectList(sdrBdrs, "IdSdr_Bdr", "Nome");

            return View();
        }

        // POST: JobOrders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,NomeProjeto,Descricao,DataSolicitacao,IdClient,Closer,Sdr_Bdr,PercentualComissaoComercial,Observacao,Prazo,Promessas,Valor,FormaPagamento")] JobOrder jobOrder)
        {
            if (ModelState.IsValid)
            {
                _context.Add(jobOrder);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Recarregar selects em caso de erro de validação
            ViewBag.Clientes = new SelectList(_context.Clientes, "IdCliente", "NomeCliente", jobOrder.IdClient);
            ViewBag.SdrBdrs = new SelectList(_context.Sdr_Bdrs, "IdSdr_Bdr", "Nome", jobOrder.Sdr_Bdr);

            return View(jobOrder);
        }

        // GET: JobOrders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var jobOrder = await _context.OrdensDeServico.FindAsync(id);
            if (jobOrder == null) return NotFound();

            ViewBag.Clientes = new SelectList(_context.Clientes, "IdCliente", "NomeCliente", jobOrder.IdClient);
            ViewBag.SdrBdrs = new SelectList(_context.Sdr_Bdrs, "IdSdr_Bdr", "Nome", jobOrder.Sdr_Bdr);

            return View(jobOrder);
        }

        // POST: JobOrders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NomeProjeto,Descricao,DataSolicitacao,IdClient,Closer,Sdr_Bdr,PercentualComissaoComercial,Observacao,Prazo,Promessas,Valor,FormaPagamento")] JobOrder jobOrder)
        {
            if (id != jobOrder.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(jobOrder);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JobOrderExists(jobOrder.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Clientes = new SelectList(_context.Clientes, "IdCliente", "NomeCliente", jobOrder.IdClient);
            ViewBag.SdrBdrs = new SelectList(_context.Sdr_Bdrs, "IdSdr_Bdr", "Nome", jobOrder.Sdr_Bdr);

            return View(jobOrder);
        }

        // GET: JobOrders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var jobOrder = await _context.OrdensDeServico
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
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool JobOrderExists(int id)
        {
            return _context.OrdensDeServico.Any(e => e.Id == id);
        }
    }
}

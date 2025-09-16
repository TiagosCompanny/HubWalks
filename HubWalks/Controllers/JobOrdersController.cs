using System;
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
                .Include(j => j.Cliente)
                .Include(j => j.SdrBdr)
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
                .Include(j => j.Cliente)
                .Include(j => j.SdrBdr)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (jobOrder == null) return NotFound();

            return View(jobOrder);
        }

        // GET: JobOrders/Create
        public async Task<IActionResult> CreateAsync()
        {
            await CarregarCombos(null, null);
            return View();
        }

        // POST: JobOrders/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("NomeProjeto,Descricao,IdClient,Closer,SdrBdrId,PercentualComissaoComercial,Observacao,Prazo,Promessas,Valor,FormaPagamento")] JobOrder jobOrder)
        {
            // seta data no servidor
            jobOrder.DataSolicitacao = DateTime.UtcNow;

            // Não validar objetos de navegação (não vêm no POST)
            ModelState.Remove(nameof(JobOrder.Cliente));
            ModelState.Remove(nameof(JobOrder.SdrBdr));

            // Garantir seleção válida de FK (Guid vazio não dispara [Required])
            if (jobOrder.IdClient == Guid.Empty)
                ModelState.AddModelError(nameof(JobOrder.IdClient), "Selecione um cliente.");
            if (jobOrder.SdrBdrId == Guid.Empty)
                ModelState.AddModelError(nameof(JobOrder.SdrBdrId), "Selecione um SDR/BDR.");

            if (!ModelState.IsValid)
            {
                await CarregarCombos(jobOrder.IdClient, jobOrder.SdrBdrId);
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

            await CarregarCombos(jobOrder.IdClient, jobOrder.SdrBdrId);
            return View(jobOrder);
        }

        // POST: JobOrders/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,NomeProjeto,Descricao,IdClient,Closer,SdrBdrId,PercentualComissaoComercial,Observacao,Prazo,Promessas,Valor,FormaPagamento")] JobOrder form)
        {
            if (id != form.Id) return NotFound();

            // Não validar navegação
            ModelState.Remove(nameof(JobOrder.Cliente));
            ModelState.Remove(nameof(JobOrder.SdrBdr));

            if (form.IdClient == Guid.Empty)
                ModelState.AddModelError(nameof(JobOrder.IdClient), "Selecione um cliente.");
            if (form.SdrBdrId == Guid.Empty)
                ModelState.AddModelError(nameof(JobOrder.SdrBdrId), "Selecione um SDR/BDR.");

            if (!ModelState.IsValid)
            {
                await CarregarCombos(form.IdClient, form.SdrBdrId);
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
                .Include(j => j.Cliente)
                .Include(j => j.SdrBdr)
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

        private async Task CarregarCombos(Guid? selectedCliente, Guid? selectedSdrBdr)
        {
            var clientes = await _context.Clientes
                .AsNoTracking()
                .OrderBy(c => c.NomeCliente)
                .ToListAsync();

            var sdrBdrs = await _context.Sdr_Bdrs
                .AsNoTracking()
                .OrderBy(s => s.Nome)
                .ToListAsync();

            ViewBag.SemClientesOuBdr = !clientes.Any() || !sdrBdrs.Any();
            ViewBag.Clientes = new SelectList(clientes, "IdCliente", "NomeCliente", selectedCliente);
            ViewBag.SdrBdrs = new SelectList(sdrBdrs, "IdSdr_Bdr", "Nome", selectedSdrBdr);
        }
    }
}

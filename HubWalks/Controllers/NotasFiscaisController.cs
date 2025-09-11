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
    public class NotasFiscaisController : Controller
    {
        private readonly IService<NotaFiscal> _notaFiscalService;
        private readonly IService<JobOrder> _jobOrderService;

        public NotasFiscaisController(IService<NotaFiscal> notaFiscalService, IService<JobOrder> jobOrderService)
        {
            _notaFiscalService = notaFiscalService;
            _jobOrderService = jobOrderService;
        }

        // GET: NotasFiscais
        public async Task<IActionResult> Index()
        {
            var list = await _notaFiscalService.GetAllAsync();
            return View(list.OrderByDescending(n => n.DataEmissao));
        }

        // GET: NotasFiscais/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var notaFiscal = await _notaFiscalService.GetByIdAsync(id.Value);

            if (notaFiscal == null) return NotFound();

            return View(notaFiscal);
        }

        // GET: NotasFiscais/Create
        public IActionResult Create()
        {
            var orders = (await _jobOrderService.GetAllAsync())
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

            await _notaFiscalService.AddAsync(notaFiscal);
            return RedirectToAction(nameof(Index));
        }

        // GET: NotasFiscais/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var notaFiscal = await _notaFiscalService.GetByIdAsync(id.Value);
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
            var original = await _notaFiscalService.GetByIdAsync(id);
            if (original == null) return NotFound();

            form.DataEmissao = original.DataEmissao;

            try
            {
                await _notaFiscalService.UpdateAsync(form);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await NotaFiscalExists(form.Id)) return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: NotasFiscais/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var notaFiscal = await _notaFiscalService.GetByIdAsync(id.Value);

            if (notaFiscal == null) return NotFound();

            return View(notaFiscal);
        }

        // POST: NotasFiscais/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _notaFiscalService.RemoveAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> NotaFiscalExists(int id) =>
            await _notaFiscalService.GetByIdAsync(id) != null;

        private async Task CarregarJobOrders(int? selectedId)
        {
            var orders = (await _jobOrderService.GetAllAsync())
                .OrderBy(o => o.NomeProjeto)
                .Select(o => new { o.Id, Texto = $"{o.Id} - {o.NomeProjeto}" })
                .ToList();

            ViewBag.SemJobOrders = !orders.Any();
            ViewBag.JobOrders = new SelectList(orders, "Id", "Texto", selectedId);
        }
    }
}

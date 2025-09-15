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
    public class JobOrdersController : Controller
    {
        private readonly IService<JobOrder> _jobOrderService;
        private readonly IService<Cliente> _clienteService;
        private readonly IService<Sdr_Bdr> _sdrBdrService;

        public JobOrdersController(IService<JobOrder> jobOrderService, IService<Cliente> clienteService, IService<Sdr_Bdr> sdrBdrService)
        {
            _jobOrderService = jobOrderService;
            _clienteService = clienteService;
            _sdrBdrService = sdrBdrService;
        }

        // GET: JobOrders
        public async Task<IActionResult> Index()
        {
            var list = await _jobOrderService.GetAllAsync();
            return View(list.OrderBy(o => o.NomeProjeto));
        }

        // GET: JobOrders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var jobOrder = await _jobOrderService.GetByIdAsync(id.Value);

            if (jobOrder == null) return NotFound();

            return View(jobOrder);
        }

        // GET: JobOrders/Create
        public async Task<IActionResult> CreateAsync()
        {
            var clientes = (await _clienteService.GetAllAsync()).OrderBy(c => c.NomeCliente).ToList();
            var sdrBdrs = (await _sdrBdrService.GetAllAsync()).OrderBy(s => s.Nome).ToList();

            ViewBag.SemClientesOuBdr = !clientes.Any() || !sdrBdrs.Any();
            ViewBag.Clientes = new SelectList(clientes, "IdCliente", "NomeCliente");
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

            await _jobOrderService.AddAsync(jobOrder);
            return RedirectToAction(nameof(Index));
        }

        // GET: JobOrders/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var jobOrder = await _jobOrderService.GetByIdAsync(id.Value);
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
            var original = await _jobOrderService.GetByIdAsync(id);

            if (original == null) return NotFound();

            form.DataSolicitacao = original.DataSolicitacao;

            try
            {
                await _jobOrderService.UpdateAsync(form);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await JobOrderExists(form.Id)) return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: JobOrders/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var jobOrder = await _jobOrderService.GetByIdAsync(id.Value);

            if (jobOrder == null) return NotFound();

            return View(jobOrder);
        }

        // POST: JobOrders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _jobOrderService.RemoveAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> JobOrderExists(int id) =>
            await _jobOrderService.GetByIdAsync(id) != null;

        private async Task CarregarCombos(Guid? selectedCliente, string selectedSdrBdr)
        {
            var clientes = (await _clienteService.GetAllAsync()).OrderBy(c => c.NomeCliente).ToList();
            var sdrBdrs = (await _sdrBdrService.GetAllAsync()).OrderBy(s => s.Nome).ToList();

            ViewBag.SemClientesOuBdr = !clientes.Any() || !sdrBdrs.Any();
            ViewBag.Clientes = new SelectList(clientes, "IdCliente", "NomeCliente", selectedCliente);
            ViewBag.SdrBdrs = new SelectList(sdrBdrs, "IdSdr_Bdr", "Nome", selectedSdrBdr);
        }
    }
}

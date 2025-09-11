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
    public class SdrBdrController : Controller
    {
        private readonly IService<Sdr_Bdr> _sdrBdrService;

        public SdrBdrController(IService<Sdr_Bdr> sdrBdrService)
        {
            _sdrBdrService = sdrBdrService;
        }

        // GET: SdrBdr
        public async Task<IActionResult> Index()
        {
            var list = await _sdrBdrService.GetAllAsync();
            return View(list);
        }

        // GET: SdrBdr/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sdr_Bdr = await _sdrBdrService.GetByIdAsync(id.Value);
            if (sdr_Bdr == null)
            {
                return NotFound();
            }

            return View(sdr_Bdr);
        }

        // GET: SdrBdr/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SdrBdr/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdSdr_Bdr,Nome,DataCadastro")] Sdr_Bdr sdr_Bdr)
        {
            if (ModelState.IsValid)
            {
                sdr_Bdr.IdSdr_Bdr = Guid.NewGuid();
                await _sdrBdrService.AddAsync(sdr_Bdr);
                return RedirectToAction(nameof(Index));
            }
            return View(sdr_Bdr);
        }

        // GET: SdrBdr/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sdr_Bdr = await _sdrBdrService.GetByIdAsync(id.Value);
            if (sdr_Bdr == null)
            {
                return NotFound();
            }
            return View(sdr_Bdr);
        }

        // POST: SdrBdr/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("IdSdr_Bdr,Nome,DataCadastro")] Sdr_Bdr sdr_Bdr)
        {
            if (id != sdr_Bdr.IdSdr_Bdr)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _sdrBdrService.UpdateAsync(sdr_Bdr);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await Sdr_BdrExists(sdr_Bdr.IdSdr_Bdr))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(sdr_Bdr);
        }

        // GET: SdrBdr/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sdr_Bdr = await _sdrBdrService.GetByIdAsync(id.Value);
            if (sdr_Bdr == null)
            {
                return NotFound();
            }

            return View(sdr_Bdr);
        }

        // POST: SdrBdr/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            await _sdrBdrService.RemoveAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> Sdr_BdrExists(Guid id) =>
            await _sdrBdrService.GetByIdAsync(id) != null;
    }
}

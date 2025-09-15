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
    public class SdrBdrController : Controller
    {
        private readonly HubWalksDbContext _context;

        public SdrBdrController(HubWalksDbContext context)
        {
            _context = context;
        }

        // GET: SdrBdr
        public async Task<IActionResult> Index()
        {
            return View(await _context.Sdr_Bdrs.ToListAsync());
        }

        // GET: SdrBdr/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sdr_Bdr = await _context.Sdr_Bdrs
                .FirstOrDefaultAsync(m => m.IdSdr_Bdr == id);
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
                _context.Add(sdr_Bdr);
                await _context.SaveChangesAsync();
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

            var sdr_Bdr = await _context.Sdr_Bdrs.FindAsync(id);
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
                    _context.Update(sdr_Bdr);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!Sdr_BdrExists(sdr_Bdr.IdSdr_Bdr))
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

            var sdr_Bdr = await _context.Sdr_Bdrs
                .FirstOrDefaultAsync(m => m.IdSdr_Bdr == id);
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
            var sdr_Bdr = await _context.Sdr_Bdrs.FindAsync(id);
            if (sdr_Bdr != null)
            {
                _context.Sdr_Bdrs.Remove(sdr_Bdr);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool Sdr_BdrExists(Guid id)
        {
            return _context.Sdr_Bdrs.Any(e => e.IdSdr_Bdr == id);
        }
    }
}

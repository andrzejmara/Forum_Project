using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Snake.Context;
using Snake.Models;

namespace Snake.Controllers
{
    public class ChannelController : Controller
    {
        private readonly EFContext _context;

        public ChannelController(EFContext context)
        {
            _context = context;
        }

        // GET: Channel
        public async Task<IActionResult> Index()
        {
            return View(await _context.Channels.ToListAsync());
        }

        // GET: Channel/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var channelModel = await _context.Channels.Include(c => c.Messages)
                .SingleOrDefaultAsync(m => m.ID == id);

            if (channelModel == null)
            {
                return NotFound();
            }

            return View(channelModel);
        }

        // GET: Channel/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Channel/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,Name,Color")] ChannelModel channelModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(channelModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(channelModel);
        }

        // GET: Channel/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var channelModel = await _context.Channels.SingleOrDefaultAsync(m => m.ID == id);
            if (channelModel == null)
            {
                return NotFound();
            }
            return View(channelModel);
        }

        // POST: Channel/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,Name,Color")] ChannelModel channelModel)
        {
            if (id != channelModel.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(channelModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChannelModelExists(channelModel.ID))
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
            return View(channelModel);
        }

        // GET: Channel/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var channelModel = await _context.Channels
                .SingleOrDefaultAsync(m => m.ID == id);
            if (channelModel == null)
            {
                return NotFound();
            }

            return View(channelModel);
        }

        // POST: Channel/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var channelModel = await _context.Channels.SingleOrDefaultAsync(m => m.ID == id);
            _context.Channels.Remove(channelModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ChannelModelExists(int id)
        {
            return _context.Channels.Any(e => e.ID == id);
        }
    }
}

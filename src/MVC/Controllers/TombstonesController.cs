using System.Collections.Concurrent;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using MVC.Models;

namespace MVC.Controllers
{
    public class TombstonesController : Controller
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IServiceProvider _serviceProvider;
        private readonly PitchforkContext _context;

        private static int MaxDegreeOfParallelism = 4;

        public TombstonesController(PitchforkContext context)
        {
            _context = context;
        }

        //// GET: Tombstones
        [Authorize (Roles = "Admin, Moderator")]
        public IActionResult Index()
        {
            using (var context = new PitchforkContext())
            {
                var watch = new System.Diagnostics.Stopwatch();
                watch.Start();
                var list = context.Tombstones.FromSql($"SELECT * FROM dbo.tombstones").ToList();
                watch.Stop();
                ViewBag.Raw = watch.ElapsedMilliseconds;
                ViewBag.RawLength = list.Count;
            }

            using (var context = new PitchforkContext())
            {
                var watch = new System.Diagnostics.Stopwatch();
                watch.Start();
                List<Tombstone> list = context.Tombstones.Where(p => p.Id > -1).ToList();
                watch.Stop();
                ViewBag.Linq = watch.ElapsedMilliseconds;
                ViewBag.LinqLength = list.Count;
            }

            using (var context = new PitchforkContext())
            {
                var watch = new System.Diagnostics.Stopwatch();
                watch.Start();
                List<Tombstone> list = context.Tombstones.AsParallel()
                   .Where(p => p.Id > -1).ToList();
                watch.Stop();
                ViewBag.Plinq = watch.ElapsedMilliseconds;
                ViewBag.PlinqLength = list.Count;
            }

            using (var context = new PitchforkContext())
            {
                List<int> ids = context.Tombstones.Where(p => p.Id > -1).Select(item => item.Id).ToList();
                var watch = new System.Diagnostics.Stopwatch();
                watch.Start();
                Parallel.ForEach(ids, new ParallelOptions { MaxDegreeOfParallelism = 8 },
                           id => context.Tombstones.Where(p => p.Id == id));
                watch.Stop();
                ViewBag.Parallel = watch.ElapsedMilliseconds;
                ViewBag.ParallelLength = ids.Count;
            }
            Random rnd = new Random();
            int limit = rnd.Next(1, 10);
            ViewBag.Limit = limit;

            using (var context = new PitchforkContext())
            {
                var watch = new System.Diagnostics.Stopwatch();

                watch.Start();
                var list = context.Tombstones.FromSql($"SELECT * FROM dbo.tombstones where [score] > {limit}").ToList();
                watch.Stop();
                ViewBag.ScoreRaw = watch.ElapsedMilliseconds;
                ViewBag.ScoreRawLength = list.Count;
            }
            using (var context = new PitchforkContext())
            {
                var watch = new System.Diagnostics.Stopwatch();
                watch.Start();
                var list = context.Tombstones.Where(p => p.Score > limit).ToList();
                watch.Stop();
                //context.Database = Console.Write;
                ViewBag.ScoreLinq = watch.ElapsedMilliseconds;
                ViewBag.ScoreLinqLength = list.Count;
            }
            using (var context = new PitchforkContext())
            {
                var watch = new System.Diagnostics.Stopwatch();
                watch.Start();
                var list = context.Tombstones.AsParallel().Where(p => p.Score > limit).ToList();
                watch.Stop();
                ViewBag.ScorePlinq = watch.ElapsedMilliseconds;
                ViewBag.ScorePlinqLength = list.Count;
            }
            using (var context = new PitchforkContext())
            {
                var watch = new System.Diagnostics.Stopwatch();
                watch.Start();
                List<int> ids = context.Tombstones.Where(p => p.Score > limit).Select(item => item.Id).ToList();
                Parallel.ForEach(ids, new ParallelOptions { MaxDegreeOfParallelism = 8 },
                           id => context.Tombstones.Where(p => p.Id == id));
                watch.Stop();
                ViewBag.ScoreParallel = watch.ElapsedMilliseconds;
                ViewBag.ScoreParallelLength = ids.Count;
            }
            
            List<double> scores = _context.Tombstones.Select(item => item.Score ?? -1).ToList();
            
            using (var context = new PitchforkContext())
            {
                var tasks = new List<Task<string>>();
                List<double> mean = scores.ToList();

                var watch = Stopwatch.StartNew();

                var rangeSize = scores.Count / MaxDegreeOfParallelism;
                if (scores.Count % MaxDegreeOfParallelism != 0)
                {
                    rangeSize++;
                }

                var partitions = Partitioner.Create(0, scores.Count, rangeSize);
                var options = new ParallelOptions() { MaxDegreeOfParallelism = MaxDegreeOfParallelism };

                Parallel.ForEach(partitions, options, x =>
                {
                    for (var i = x.Item1; i < x.Item2; i++)
                    {
                        mean[i] = FindPrimeNumber(scores[i] * 10);
                    }
                });

                watch.Stop();

                ViewBag.ParallelMean = watch.ElapsedMilliseconds;
            }

            using (var context = new PitchforkContext())
            {
                List<double> meansync = scores.ToList();
                var watch = new Stopwatch();
                watch.Start();
                for (int i = 0; i < scores.Count; i++)
                {
                    meansync[i] = FindPrimeNumber(scores[i]*10);
                }
                watch.Stop();

                ViewBag.SyncMean = watch.ElapsedMilliseconds;
            }

                return View();
        }

        public long FindPrimeNumber(double n)
        {
            int count = 0;
            long a = 2;
            while (count < n)
            {
                long b = 2;
                int prime = 1;// to check if found a prime
                while (b * b <= a)
                {
                    if (a % b == 0)
                    {
                        prime = 0;
                        break;
                    }
                    b++;
                }
                if (prime > 0)
                {
                    count++;
                }
                a++;
            }
            return (--a);
        }

        // GET: Tombstones/Details/5
        [Authorize]
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null || _context.Tombstones == null)
            {
                return NotFound();
            }

            var tombstone = await _context.Tombstones
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tombstone == null)
            {
                return NotFound();
            }

            return View(tombstone);
        }

        // GET: Tombstones/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Tombstones/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("ReviewTombstoneId,ReviewUrl,PickerIndex,Title,Score,BestNewMusic,BestNewReissue,Id")] Tombstone tombstone)
        {
            if (ModelState.IsValid)
            {
                _context.Add(tombstone);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(tombstone);
        }

        // GET: Tombstones/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(long? id)
        {
            if (id == null || _context.Tombstones == null)
            {
                return NotFound();
            }

            var tombstone = await _context.Tombstones.FindAsync(id);
            if (tombstone == null)
            {
                return NotFound();
            }
            return View(tombstone);
        }

        // POST: Tombstones/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Edit(long id, [Bind("ReviewTombstoneId,ReviewUrl,PickerIndex,Title,Score,BestNewMusic,BestNewReissue,Id")] Tombstone tombstone)
        {
            if (id != tombstone.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(tombstone);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TombstoneExists(tombstone.Id))
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
            return View(tombstone);
        }

        // GET: Tombstones/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(long? id)
        {
            if (id == null || _context.Tombstones == null)
            {
                return NotFound();
            }

            var tombstone = await _context.Tombstones
                .FirstOrDefaultAsync(m => m.Id == id);
            if (tombstone == null)
            {
                return NotFound();
            }

            return View(tombstone);
        }

        // POST: Tombstones/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            if (_context.Tombstones == null)
            {
                return Problem("Entity set 'PitchforkContext.Tombstones'  is null.");
            }
            var tombstone = await _context.Tombstones.FindAsync(id);
            if (tombstone != null)
            {
                _context.Tombstones.Remove(tombstone);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [Authorize]
        private bool TombstoneExists(long id)
        {
          return (_context.Tombstones?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}

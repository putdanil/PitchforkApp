using System.Collections.Concurrent;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using MVC.Models;
using NLayerApp.BLL.DTO;
using NLayerApp.BLL.Interfaces;
using NLayerApp.BLL.Services;
using NuGet.Packaging.Signing;

namespace MVC.Controllers
{
    public class TombstonesController : Controller
    {
        private readonly ITombstoneService _tombstoneService;
        public TombstonesController(ITombstoneService tombstoneService)
        {
            _tombstoneService = tombstoneService;
        }

        //// GET: Tombstones
        [Authorize(Roles = "Admin, Moderator")]
        public IActionResult Index()
        {
            var watch = new Stopwatch();
            watch.Start();
            _tombstoneService.RawAll();
            watch.Stop();
            ViewBag.Raw = watch.ElapsedMilliseconds;

            watch.Restart();
            _tombstoneService.LinqAll();
            watch.Stop();
            ViewBag.Linq = watch.ElapsedMilliseconds;

            watch.Restart();
            _tombstoneService.PlinqAll();
            watch.Stop();
            ViewBag.Plinq = watch.ElapsedMilliseconds;

            watch.Restart();
            _tombstoneService.ParallelAll();
            watch.Stop();
            ViewBag.Parallel = watch.ElapsedMilliseconds;

            ViewBag.Limit = 4;

            watch.Restart();
            var count = _tombstoneService.ParallelScore();
            watch.Stop();
            ViewBag.ScoreParallel = watch.ElapsedMilliseconds;
            ViewBag.ScoreParallelLength = count;

            watch.Restart();
            _tombstoneService.RawScore();
            watch.Stop();
            ViewBag.ScoreRaw = watch.ElapsedMilliseconds;
            ViewBag.ScoreRawLength = count;


            watch.Restart();
            _tombstoneService.LinqScore();
            watch.Stop();
            ViewBag.ScoreLinq = watch.ElapsedMilliseconds;
            ViewBag.ScoreLinqLength = count;

            watch.Restart();
            _tombstoneService.PlinqScore();
            watch.Stop();
            ViewBag.ScorePlinq = watch.ElapsedMilliseconds;
            ViewBag.ScorePlinqLength = count;


            watch.Restart();
            _tombstoneService.ParallelCalculation();
            watch.Stop();
            ViewBag.ParallelMean = watch.ElapsedMilliseconds;

            watch.Restart();
            _tombstoneService.SyncCalculation();
            watch.Stop();
            ViewBag.SyncMean = watch.ElapsedMilliseconds;

            return View();
        }


        // GET: Tombstones/Details/5
        [Authorize]
        public IActionResult Details(long? id)
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<TombstoneDTO, Tombstone>()).CreateMapper();
            var tombstones = mapper.Map<IEnumerable<TombstoneDTO>, List<Tombstone>>(_tombstoneService.GetTombstones());
            if (id == null || tombstones == null)
            {
                return NotFound();
            }

            var tombstone = tombstones.FirstOrDefault(m => m.Id == id);
            if (tombstone == null)
            {
                return NotFound();
            }

            return View(tombstone);
        }
    }
}

using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Security.Principal;
using Microsoft.EntityFrameworkCore;
using NLayerApp.DAL.EF;
using NLayerApp.DAL.Entities;
using NLayerApp.DAL.Interfaces;

namespace NLayerApp.DAL.Repositories
{
    public class TombstoneRepository : IRepository<Tombstone>
    {
        private PitchforkContext db;

        public TombstoneRepository(PitchforkContext context)
        {
            this.db = context;
        }

        public IEnumerable<Tombstone> GetAll()
        {
            return db.Tombstones;
        }

        public Tombstone Get(int id)
        {
            return db.Tombstones.Find(id);
        }

        public void Create(Tombstone tombstone)
        {
            db.Tombstones.Add(tombstone);
        }

        public void Update(Tombstone tombstone)
        {
            db.Entry(tombstone).State = EntityState.Modified;
        }

        public IEnumerable<Tombstone> Find(Func<Tombstone, Boolean> predicate)
        {
            return db.Tombstones.Where(predicate).ToList();
        }

        public void Delete(int id)
        {
            Tombstone tombstone = db.Tombstones.Find(id);
            if (tombstone != null)
                db.Tombstones.Remove(tombstone);
        }

        public void ParallelAll()
        {
            List<int> ids = db.Tombstones.Where(p => p.Id > -1).Select(item => item.Id).ToList();
            Parallel.ForEach(ids, new ParallelOptions { MaxDegreeOfParallelism = 8 },
                       id => db.Tombstones.Where(p => p.Id == id));
        }
        public void RawAll()
        {
            _ = db.Tombstones.FromSql($"SELECT * FROM dbo.tombstones").ToList();
        }
        public void LinqAll()
        {
            List<Tombstone> list = db.Tombstones.Where(p => p.Id > -1).ToList();
        }
        public void PlinqAll()
        {
            List<Tombstone> list = db.Tombstones.AsParallel()
                   .Where(p => p.Id > -1).ToList();
        }

        public int ParallelScore()
        {
            List<int> ids = db.Tombstones.Where(p => p.Score > 4).Select(item => item.Id).ToList();
            Parallel.ForEach(ids, new ParallelOptions { MaxDegreeOfParallelism = 8 },
                       id => db.Tombstones.Where(p => p.Id == id));
            return ids.Count;
        }
        public void RawScore()
        {
            _ = db.Tombstones.FromSql($"SELECT * FROM dbo.tombstones where [score] > 4").ToList();

        }
        public void LinqScore()
        {
            var list = db.Tombstones.Where(p => p.Score > 4).ToList();
        }
        public void PlinqScore()
        {
            var list = db.Tombstones.AsParallel().Where(p => p.Score > 4).ToList();
        }

    }
}


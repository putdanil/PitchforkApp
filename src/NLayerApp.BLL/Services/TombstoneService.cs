using AutoMapper;
using NLayerApp.BLL.DTO;
using NLayerApp.BLL.Infrastructure;
using NLayerApp.BLL.Interfaces;
using NLayerApp.DAL.Entities;
using NLayerApp.DAL.Interfaces;
using System.Collections.Concurrent;
using static System.Formats.Asn1.AsnWriter;

namespace NLayerApp.BLL.Services
{
    public class TombstoneService : ITombstoneService
    {
        IUnitOfWork Database { get; set; }

        public TombstoneService(IUnitOfWork uow)
        {
            Database = uow;
        }

        public IEnumerable<TombstoneDTO> GetTombstones()
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Tombstone, TombstoneDTO>()).CreateMapper();
            return mapper.Map<IEnumerable<Tombstone>, List<TombstoneDTO>>(Database.Tombstones.GetAll());
        }

        public IEnumerable<Tombstone> FilterTombstonesByScore(int? score)
        {
            var tombstones = Database.Tombstones.Find(p => p.Score > score).ToList();
            return tombstones;
        }
        public void RawAll()
        {
            Database.Tombstones.RawAll();
        }
        public void LinqAll()
        {
            Database.Tombstones.LinqAll();
        }
        public void PlinqAll()
        {
            Database.Tombstones.PlinqAll();
        }
        public void ParallelAll()
        {
            Database.Tombstones.ParallelAll();
        }
        public void RawScore()
        {
            Database.Tombstones.RawScore();
        }
        public void LinqScore()
        {
            Database.Tombstones.LinqScore();
        }
        public void PlinqScore()
        {
            Database.Tombstones.PlinqScore();
        }
        public int ParallelScore()
        {
            return Database.Tombstones.ParallelScore();
        }
        public void ParallelCalculation()
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Tombstone, TombstoneDTO>()).CreateMapper();
            var list = mapper.Map<IEnumerable<Tombstone>, List<TombstoneDTO>>(Database.Tombstones.GetAll());
            List<double> scores = list.Select(x => x.Score ?? -1).ToList();

            var tasks = new List<Task<string>>();
            List<double> mean = scores.ToList();

            var rangeSize = scores.Count / 8;
            if (scores.Count % 8 != 0)
            {
                rangeSize++;
            }

            var partitions = Partitioner.Create(0, scores.Count, rangeSize);
            var options = new ParallelOptions() { MaxDegreeOfParallelism = 8 };

            Parallel.ForEach(partitions, options, x =>
            {
                for (var i = x.Item1; i < x.Item2; i++)
                {
                    mean[i] = FindPrimeNumber(scores[i] * 10);
                }
            });
        }

        public void SyncCalculation()
        {
            var mapper = new MapperConfiguration(cfg => cfg.CreateMap<Tombstone, TombstoneDTO>()).CreateMapper();
            var list = mapper.Map<IEnumerable<Tombstone>, List<TombstoneDTO>>(Database.Tombstones.GetAll());
            List<double> scores = list.Select(x => x.Score ?? -1).ToList();
            List<double> meansync = scores.ToList();

            for (int i = 0; i < scores.Count; i++)
            {
                meansync[i] = FindPrimeNumber(scores[i] * 10);
            }
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
        public void Dispose()
        {
            Database.Dispose();
        }
    }
}

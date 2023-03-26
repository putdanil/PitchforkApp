using NLayerApp.BLL.DTO;
using NLayerApp.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayerApp.BLL.Interfaces
{
    public interface ITombstoneService
    {
        IEnumerable<Tombstone> FilterTombstonesByScore(int? score);
        void RawAll();
        void LinqAll();
        void PlinqAll();
        void ParallelAll();
        void RawScore();
        void LinqScore();
        void PlinqScore();
        int ParallelScore();
        void ParallelCalculation();
        void SyncCalculation();
        IEnumerable<TombstoneDTO> GetTombstones();
        void Dispose();
    }
}

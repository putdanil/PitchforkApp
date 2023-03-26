using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NLayerApp.BLL.DTO
{
    public class TombstoneDTO
    {
        public string? ReviewTombstoneId { get; set; }
        public string? ReviewUrl { get; set; }
        public byte? PickerIndex { get; set; }
        public string? Title { get; set; }
        public double? Score { get; set; }
        public bool? BestNewMusic { get; set; }
        public bool? BestNewReissue { get; set; }
        public int Id { get; set; }
    }
}


using System.ComponentModel.DataAnnotations;

namespace Domain
{
    public class ShipConfig
    {
        [Key]
        public int ShipId { get; set; }
        
        [MinLength(2)] [MaxLength(128)]
        public string ShipName { get; set; } = default!;
        public int ShipQuantity { get; set; }
        public int ShipSizeX { get; set; }
        public int ShipSizeY { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ArcadisWeb.Models
{
    public class Equipment
    {
        [Key]
        public int EquipmentId { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string Title { get; set; }
        public int Cost { get; set; }
        public int Quantity { get; set; }
        [NotMapped]
        public bool editable { get; set; } = false;
    }
}

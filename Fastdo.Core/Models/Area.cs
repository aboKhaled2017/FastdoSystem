using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Fastdo.Core.Models
{
    public partial class Area
    {
        public Area()
        {
            SubAreas = new HashSet<Area>();
        }
        [Key]
        public byte Id { get; set; }
        [Required]
        public string Name { get; set; }
        public byte? SuperAreaId { get; set; } = null;
        [ForeignKey("SuperAreaId")]
        public virtual Area SuperArea { get; set;}
        public virtual ICollection<Area> SubAreas { get; set; }
    }
}

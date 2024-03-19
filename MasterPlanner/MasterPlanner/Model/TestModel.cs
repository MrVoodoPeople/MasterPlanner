using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MasterPlanner.Model 
{
    [Table ("datenotes")]
    class TestModel
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("date")]
        public DateTime? Date { get; set; }
        [Column("date_end")]
        public DateTime? DateEnd { get; set; }
        [Column("notes")]
        public string? Notes { get; set; }
    }
}

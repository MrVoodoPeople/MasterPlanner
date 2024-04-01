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
    class PlannerNote
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }
        [Column("date")]
        public DateTime Date { get; set; }
        [Column("date_end")]
        public DateTime DateEnd { get; set; }
        [Column("notes")]
        public string? Notes { get; set; }

        [Column("reminder_date")]
        public DateTime? ReminderDate { get; set; }

        [Column("should_remind")]
        public bool ShouldRemind { get; set; }
        [Column("reminder_shown")]
        public bool ReminderShown { get; set; }
        public string DateEndDisplay => ConvertUtcToMsk(this.DateEnd);
        public string DateDisplay => ConvertUtcToMsk(this.Date);

        private string ConvertUtcToMsk(DateTime? utcDateTime)
        {
            if (!utcDateTime.HasValue)
                return string.Empty;

            // Предполагаем, что MSK = UTC+3.
            //return utcDateTime.Value.AddHours(3).ToString("g");
            return utcDateTime.Value.ToString("g");
        }
    }
}

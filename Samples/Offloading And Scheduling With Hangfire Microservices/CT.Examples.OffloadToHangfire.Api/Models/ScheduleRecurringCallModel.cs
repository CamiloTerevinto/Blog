using System.ComponentModel.DataAnnotations;

namespace CT.Examples.OffloadToHangfire.Api.Models
{
    public class ScheduleRecurringCallModel
    {
        [Required]
        public string SomeImportantData { get; set; } = default!;

        [Required]
        public string CronExpression { get; set; } = default!;
    }
}

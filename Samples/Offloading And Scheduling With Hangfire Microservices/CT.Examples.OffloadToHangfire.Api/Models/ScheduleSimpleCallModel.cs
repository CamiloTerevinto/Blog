using System.ComponentModel.DataAnnotations;

namespace CT.Examples.OffloadToHangfire.Api.Models
{
    public class ScheduleSimpleCallModel
    {
        [Required]
        public string SomeImportantData { get; set; }
    }
}

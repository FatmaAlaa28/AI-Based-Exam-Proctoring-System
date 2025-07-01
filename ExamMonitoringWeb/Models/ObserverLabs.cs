using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExamMonitoringWeb.Models
{
    public class ObserverLabs
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int UserId { get; set; }
        public ApplicationUser User { get; set; }

        public int LabId { get; set; }
        public Lab Lab { get; set; }

        public DateTime DateTime { get; set; }

        
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ExamMonitoringWeb.Models
{
    public class Exam : IValidatableObject
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Exam name is required.")]
        [MaxLength(50, ErrorMessage = "Exam name can't be longer than 50 characters.")]
        public string ExamName { get; set; }

        [Required(ErrorMessage = "Exam link is required.")]
        [Url(ErrorMessage = "Please enter a valid URL.")]
        public string ExamLink { get; set; }

        [Required(ErrorMessage = "Student Excel sheet is required.")]
        public byte[] Sheet { get; set; }

        [Required(ErrorMessage = "Start time is required.")]
        public TimeSpan StartTime { get; set; }

        [Required(ErrorMessage = "End time is required.")]
        public TimeSpan EndTime { get; set; }

        [Required(ErrorMessage = "Exam date is required.")]
        public DateOnly Date { get; set; }

        [Range(1, 180, ErrorMessage = "Duration must be between 1 and 180 minutes (maximum 3 hours.")]
        public int Duration { get; set; }

        // Navigation Properties
        public ICollection<ExamLab> ExamLabs { get; set; }
        public ICollection<StudentExam> StudentExams { get; set; }
        public ICollection<Violation> Violations { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (EndTime <= StartTime)
            {
                yield return new ValidationResult("End time must be after start time.", new[] { nameof(EndTime) });
                yield break; // Skip further checks if time is invalid
            }

            var expectedDuration = (EndTime - StartTime).TotalMinutes;
            if (Duration != (int)expectedDuration)
            {
                yield return new ValidationResult(
                    $"Duration should match the difference between start and end time ({(int)expectedDuration} minutes).",
                    new[] { nameof(Duration) });
            }
        }

    }
}

//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;

//namespace ExamMonitoringWeb.Models
//{
//    public class Exam
//    {
//        [Key]
//        public int Id { get; set; }

//        [Required]
//        public string ExamName { get; set; }

//        [Required]
//        public string ExamLink { get; set; }

//        [Required]
//        public byte[] Sheet { get; set; }

//        //public string ExamPklPath { get; set; }

//        [Required]

//        public TimeSpan StartTime { get; set; }

//        [Required]

//        public TimeSpan EndTime { get; set; }


//        public DateOnly Date { get; set; }


//        public int Duration { get; set; }

//        // Navigation
//        public ICollection<ExamLab> ExamLabs { get; set; }
//        public ICollection<StudentExam> StudentExams { get; set; }
//        public ICollection<Violation> Violations { get; set; }
//    }
//}
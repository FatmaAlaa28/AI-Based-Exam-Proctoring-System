using ExamMonitoringWeb.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace ExamMonitoringWeb.Controllers
{
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AuthController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost]
        [Route("api/auth/login")]
        public async Task<IActionResult> Login([FromBody] LoginModel login)
        {
            if (string.IsNullOrEmpty(login.Email) || string.IsNullOrEmpty(login.Password))
            {
                return BadRequest("Email and password are required");
            }

            var user = await _userManager.FindByEmailAsync(login.Email);
            if (user == null)
            {
                return Unauthorized("Invalid email or password");
            }

            var passwordValid = await _userManager.CheckPasswordAsync(user, login.Password);
            if (!passwordValid)
            {
                return Unauthorized("Invalid email or password");
            }

            var today = DateTime.Today;
            var exams = await _context.StudentExams
                .Where(se => se.UserId == user.Id 
                && se.Exam.Date == DateOnly.FromDateTime(DateTime.Now))
                .Include(se => se.Exam)
                .Select(se => new
                {
                    name = se.Exam.ExamName,
                    url = se.Exam.ExamLink,
                    student_id = user.Id.ToString(),
                    exam_id = se.ExamId.ToString()
                })
                .ToListAsync();
            Console.WriteLine("Raw exams data: " + JsonSerializer.Serialize(exams));

            return Ok(new { links = exams });
        }

        
        [HttpPost]
        [Route("api/detection/cheating")]
        public async Task<IActionResult> ReceiveCheatingData([FromBody] CheatingData data)
        {
            try
            {
                // Log the CheatingData object directly instead of re-reading the body
                Console.WriteLine($"Received CheatingData: {JsonSerializer.Serialize(data)}");

                if (data == null || string.IsNullOrEmpty(data.student_id) || string.IsNullOrEmpty(data.exam_id) || string.IsNullOrEmpty(data.cheating_type))
                {
                    Console.WriteLine("Invalid cheating data received.");
                    return BadRequest(new { error = "Invalid cheating data", details = "Required fields (student_id, exam_id, cheating_type) are missing or empty" });
                }

                if (!int.TryParse(data.student_id, out int userId) || !int.TryParse(data.exam_id, out int examId))
                {
                    Console.WriteLine("Invalid Student ID or Exam ID format.");
                    return BadRequest(new { error = "Invalid Student ID or Exam ID format", details = "Student ID and Exam ID must be valid integers" });
                }

                // Retrieve the Exam record with the stored Excel sheet
                var exam = await _context.Exams
                    .Where(e => e.Id == examId)
                    .FirstOrDefaultAsync();
                if (exam == null || exam.Sheet == null)
                {
                    Console.WriteLine("Exam or its Excel sheet not found.");
                    return BadRequest(new { error = "Exam not found or Excel sheet missing" });
                }

                int labId = 1; // Default value if not found
                using (var memoryStream = new MemoryStream(exam.Sheet))
                {
                    ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
                    using (var package = new ExcelPackage(memoryStream))
                    {
                        var worksheet = package.Workbook.Worksheets.FirstOrDefault();
                        if (worksheet != null && worksheet.Dimension != null)
                        {
                            // Search for the row where UserId matches
                            for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
                            {
                                var userIdText = worksheet.Cells[row, 1].Text; // Assuming ID is in column 1 (A)
                                var labIdText = worksheet.Cells[row, 3].Text; // Assuming lab is in column 3 (C)

                                if (!string.IsNullOrEmpty(userIdText) && int.TryParse(userIdText, out int currentUserId) && currentUserId == userId)
                                {
                                    if (!string.IsNullOrEmpty(labIdText) && int.TryParse(labIdText, out int currentLabId))
                                    {
                                        labId = currentLabId;
                                        break; // Exit once the matching user ID is found
                                    }
                                }
                            }
                        }
                    }
                }

                var violation = new Violation
                {
                    UserId = userId,
                    ExamId = examId,
                    LabId = labId,
                    ViolationType = data.cheating_type,
                    Timestamp = DateTime.Parse(data.timestamp),
                    Direction = string.IsNullOrEmpty(data.direction) ? "none" : data.direction
                };

                _context.Violations.Add(violation);
                await _context.SaveChangesAsync();

                Console.WriteLine($"Cheating detected at {DateTime.Now}:");
                Console.WriteLine($"Student ID: {data.student_id}");
                Console.WriteLine($"Exam ID: {data.exam_id}");
                Console.WriteLine($"Cheating Type: {data.cheating_type}");
                Console.WriteLine($"Confidence: {data.confidence}");
                Console.WriteLine($"Timestamp: {data.timestamp}");
                Console.WriteLine($"SEB Running: {data.is_seb_running}");
                if (!string.IsNullOrEmpty(data.direction))
                    Console.WriteLine($"Direction: {data.direction}");
                if (!string.IsNullOrEmpty(data.detected_id))
                    Console.WriteLine($"Detected ID: {data.detected_id}");
                if (data.objects != null && data.objects.Any())
                    Console.WriteLine($"Objects: {string.Join(", ", data.objects)}");
                Console.WriteLine("-------------------");

                return Ok("Cheating data received and stored");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing cheating data: {ex}");
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        [HttpPost]
        [Route("api/auth/check-seb")]
        public async Task<IActionResult> CheckSebStatus([FromBody] SebStatus status)
        {
            if (status == null || string.IsNullOrEmpty(status.StudentId))
            {
                Console.WriteLine("Invalid SEB status data received.");
                return BadRequest("Invalid SEB status data");
            }

            Console.WriteLine($"SEB Status at {DateTime.Now}:");
            Console.WriteLine($"Student ID: {status.StudentId}");
            Console.WriteLine($"SEB Running: {status.IsSebRunning}");
            Console.WriteLine("-------------------");

            if (status.IsSebRunning)
            {
                return Ok("SEB is running");
            }
            return BadRequest("SEB is not running");
        }
    }

    public class LoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class CheatingData
    {
        public string student_id { get; set; } = string.Empty;
        public string exam_id { get; set; } = string.Empty;
        public string cheating_type { get; set; } = string.Empty;
        public double confidence { get; set; }
        public string timestamp { get; set; } = string.Empty;
        public string direction { get; set; } = string.Empty;
        public string detected_id { get; set; } = string.Empty;
        public List<string> objects { get; set; } = new List<string>();
        public bool is_seb_running { get; set; }
    }

    public class SebStatus
    {
        public string StudentId { get; set; }
        public bool IsSebRunning { get; set; }
    }
}
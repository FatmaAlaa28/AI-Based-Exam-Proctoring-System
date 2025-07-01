using System.Linq;
using ExamMonitoringWeb.DTO;
using ExamMonitoringWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ExamMonitoringWeb.Controllers
{
    
    public class StudentController : Controller
    {
        private ApplicationUser AppUser { get; set; }
        ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public StudentController(ApplicationDbContext context , UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }
        public IActionResult Index()
        {
			var now = DateTime.Now; // EEST: 09:59 PM, June 23, 2025
			var nowDateOnly = DateOnly.FromDateTime(now); // 2025-06-23
			var nowTimeSpan = now.TimeOfDay; // 21:59:00
				

            var students = (from u in _context.Users
                            join se in _context.StudentExams on u.Id equals se.UserId
                            join e in _context.Exams on se.ExamId equals e.Id
                            where e.Date == nowDateOnly && // Include today and tomorrow
                                 e.StartTime <= nowTimeSpan && e.EndTime > nowTimeSpan // Active exams
							select new Student
                            {
                                Id = u.Id,
                                Name = u.FullName,
                                CurrentExam = e.ExamName,
                                LabName = "Lab 101" // Placeholder; replace with actual logic
                            })
                           .Distinct()
                           .ToList();

            return View(students);
        }
        // Show Student
        public IActionResult ShowStudent(int id)
        {
            var now = DateTime.Now;
            var user = _context.Users.FirstOrDefault(x => x.Id == id);

            if (user == null)
                return RedirectToAction("Index");

            // Get exam IDs the student is registered for
            var examIds = _context.StudentExams
                .Where(x => x.UserId == id)
                .Select(x => x.ExamId)
                .ToList();

            // Load exams with violations into memory
            var examsRaw = _context.Exams
                .Where(e => examIds.Contains(e.Id))
                .Include(e => e.Violations)
                .ToList();

            // Convert to ExamAnalysis with status
            var examslst = examsRaw.Select(e =>
            {
                var startTimeOnly = TimeOnly.FromTimeSpan(e.StartTime);
                var endTimeOnly = TimeOnly.FromTimeSpan(e.EndTime);

                var startDateTime = e.Date.ToDateTime(startTimeOnly);
                var endDateTime = e.Date.ToDateTime(endTimeOnly);

                string status = now < startDateTime ? "Upcoming" :
                                now >= startDateTime && now <= endDateTime ? "In Progress" :
                                "Completed";

                return new ExamAnalysis
                {
                    ExamName = e.ExamName,
                    Date = e.Date,
                    Duration = e.Duration,
                    ViolationCount = e.Violations.Count(v => v.UserId == id),
                    Status = status
                };
            }).ToArray();


            var alerts = _context.Violations.Count(x => x.UserId == id);

            var newuser = new StudentAnalysis
            {
                Id = user.Id,
                Name = user.FullName,
                Email = user.Email,
                Department = user.Department,
                NumofExams = examslst.Length,
                Alerts = alerts,
                exams = examslst
            };

            return View(newuser);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(StudentRegister model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    FullName = model.FullName,
                    PhoneNumber = model.PhoneNumber,
                    Email = model.Email,
                    Role = Role.Student,
                    RegistrationDate = DateTime.Now,
                    UserName = model.UserName,
                    NationalId = model.NationalId,
                    Department = (Department)model.Department
                    
                };
                string pass = model.FullName +"@"+ model.NationalId;  
                var result = await _userManager.CreateAsync(user, pass);
                
                if(result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                
            }

            return View(model);
        }
    }
}

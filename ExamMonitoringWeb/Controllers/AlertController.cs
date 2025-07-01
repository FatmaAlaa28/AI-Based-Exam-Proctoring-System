using ExamMonitoringWeb.DTO;
using ExamMonitoringWeb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ExamMonitoringWeb.Controllers
{

    public class AlertController : Controller
    {
        private readonly ApplicationDbContext _context;

       public AlertController(ApplicationDbContext context)
        {
            _context = context;
        }
		public IActionResult Index()
		{
			var now = DateTime.Now; // EEST: 09:59 PM, June 23, 2025
			var nowDateOnly = DateOnly.FromDateTime(now); // 2025-06-23
			var nowTimeSpan = now.TimeOfDay; // 21:59:00

			var exam = _context.Exams
				.Where(x => x.Date == nowDateOnly 
			 && x.StartTime <= nowTimeSpan && nowTimeSpan <= x.EndTime)
				.Select(x => new { x.Id, x.ExamName })
				.FirstOrDefault();

			if (exam == null)
			{
				return View(new List<ExamMonitoringWeb.DTO.Alert>());
			}

			// Get violations with user data using a join
			var alerts = (from v in _context.Violations
						  join u in _context.Users on v.UserId equals u.Id into userGroup
						  from u in userGroup.DefaultIfEmpty()
						  where v.ExamId == exam.Id
						  select new ExamMonitoringWeb.DTO.Alert
						  {
							  ExamName = exam.ExamName,
							  Name = v.ViolationType.ToString(),
							  Time = v.Timestamp,
							  StudentId = v.UserId,
							  StudentName = u.FullName
						  })
						  .ToList();

			return View(alerts);
		}



	}
}

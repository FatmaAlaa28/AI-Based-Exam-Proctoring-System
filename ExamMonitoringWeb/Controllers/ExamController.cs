//using System;
//using System.IO;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using ExamMonitoringWeb.Models;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Logging;
//using OfficeOpenXml;
//using System.Linq;

//namespace ExamMonitoringWeb.Controllers
//{
//    public class ExamController : Controller
//    {
//        private readonly ApplicationDbContext _context;
//        private readonly ILogger<ExamController> _logger;

//        public ExamController(ApplicationDbContext context, ILogger<ExamController> logger)
//        {
//            _context = context;
//            _logger = logger;
//        }

//        // GET: Exam/Create
//        public IActionResult Create()
//        {
//            return View();
//        }

//        // POST: Exam/Create
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public async Task<IActionResult> Create(Exam model, IFormFile excelUpload)
//        {
//            // Handle exam sheet upload if provided
//            if (excelUpload != null && excelUpload.Length > 0 && excelUpload.FileName.EndsWith(".xlsx"))
//            {
//                using (var memoryStream = new MemoryStream())
//                {
//                    await excelUpload.CopyToAsync(memoryStream);
//                    model.Sheet = memoryStream.ToArray();
//                }
//            }
//            else
//            {
//                ModelState.AddModelError("", "Please upload a valid Excel file (.xlsx).");
//                return View(model);
//            }

//            try
//            {
//                // Add exam to database
//                _context.Exams.Add(model);
//                await _context.SaveChangesAsync();

//                // Process Excel file if provided
//                if (excelUpload != null && excelUpload.Length > 0 && excelUpload.FileName.EndsWith(".xlsx"))
//                {
//                    ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

//                    using (var stream = new MemoryStream())
//                    {
//                        await excelUpload.CopyToAsync(stream);
//                        using (var package = new ExcelPackage(stream))
//                        {
//                            var worksheet = package.Workbook.Worksheets.FirstOrDefault();
//                            if (worksheet == null || worksheet.Dimension == null)
//                            {
//                                ModelState.AddModelError("", "The Excel sheet is empty or does not contain any data.");
//                                return View(model);
//                            }

//                            var studentExams = new List<StudentExam>();
//                            var labIds = new HashSet<int>(); // To store distinct lab IDs

//                            // Read the "Id" and "lab" columns starting from row 2 (assuming row 1 is headers)
//                            for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
//                            {
//                                var userIdText = worksheet.Cells[row, 1].Text; // Assuming ID is in column 1
//                                var labIdText = worksheet.Cells[row, 3].Text; // Assuming lab is in column 3 (C)

//                                if (!string.IsNullOrEmpty(userIdText) && int.TryParse(userIdText, out int userId))
//                                {
//                                    var user = await _context.Users.FindAsync(userId);
//                                    if (user != null)
//                                    {
//                                        studentExams.Add(new StudentExam
//                                        {
//                                            UserId = userId,
//                                            ExamId = model.Id
//                                        });
//                                    }
//                                    else
//                                    {
//                                        _logger.LogWarning($"User ID {userId} not found in database.");
//                                    }
//                                }

//                                if (!string.IsNullOrEmpty(labIdText) && int.TryParse(labIdText, out int labId))
//                                {
//                                    labIds.Add(labId); // Add distinct lab ID
//                                }
//                            }

//                            // Check if any valid records were created
//                            if (studentExams.Count == 0)
//                            {
//                                ModelState.AddModelError("", "No valid student IDs found in the Excel sheet.");
//                                return View(model);
//                            }

//                            // Add student exam records to the database
//                            _context.StudentExams.AddRange(studentExams);

//                            // Add distinct lab records to the ExamLab table
//                            var examLabs = labIds.Select(labId => new ExamLab
//                            {
//                                ExamId = model.Id,
//                                LabId = labId
//                            });
//                            _context.ExamLabs.AddRange(examLabs);

//                            await _context.SaveChangesAsync();
//                        }
//                    }
//                }
//                else
//                {
//                    ModelState.AddModelError("", "Please upload a valid Excel file (.xlsx).");
//                    return View(model);
//                }

//                return RedirectToAction("Index", "Home");
//            }
//            catch (DbUpdateException ex)
//            {
//                _logger.LogError(ex, "Error saving exam data.");
//                ModelState.AddModelError("", "An error occurred while saving the exam data.");
//                return View(model);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Error processing Excel file.");
//                ModelState.AddModelError("", "An error occurred while processing the Excel file.");
//                return View(model);
//            }
//        }
//    }
//}
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using ExamMonitoringWeb.Models;

namespace ExamMonitoringWeb.Controllers
{
    public class ExamController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ExamController> _logger;

        public ExamController(ApplicationDbContext context, ILogger<ExamController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Exam model, IFormFile excelUpload, IFormFile observerUpload)
        {
            // 1. Validate exam time
            //if (model.StartTime >= model.EndTime)
            //{
            //    ModelState.AddModelError("", "Start time must be earlier than end time.");
            //    return View(model);
            //}

            var durationMinutes = (model.EndTime - model.StartTime).TotalMinutes;

            if (model.StartTime >= model.EndTime)
            {
                ModelState.AddModelError("", "Start time must be earlier than end time.");
                return View(model);
            }

            if (durationMinutes < 1)
            {
                ModelState.AddModelError("", "Exam duration must be at least 1 minute.");
                return View(model);
            }

            if (durationMinutes > 180)
            {
                ModelState.AddModelError("", "Exam duration must not exceed 180 minutes.");
                return View(model);
            }

            // 2. Validate and store the student Excel sheet
            if (excelUpload == null || excelUpload.Length == 0 || !excelUpload.FileName.EndsWith(".xlsx"))
            {
                ModelState.AddModelError("", "Please upload a valid Student Excel file (.xlsx).");
                return View(model);
            }
            if (observerUpload == null || observerUpload.Length == 0 || !observerUpload.FileName.EndsWith(".xlsx"))
            {
                ModelState.AddModelError("", "Please upload a valid Observers Excel file (.xlsx).");
                return View(model);
            }
            // 3. Store file content in Exam model
            using (var memoryStream = new MemoryStream())
            {
                await excelUpload.CopyToAsync(memoryStream);
                model.Sheet = memoryStream.ToArray();
            }

            // 4. Add exam to database
            _context.Exams.Add(model);
            await _context.SaveChangesAsync();

            // 5. Process student file
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            using (var stream = new MemoryStream())
            {
                await excelUpload.CopyToAsync(stream);
                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets.FirstOrDefault();
                    if (worksheet == null || worksheet.Dimension == null)
                    {
                        ModelState.AddModelError("", "Student Excel sheet is empty.");
                        return View(model);
                    }

                    var studentExams = new List<StudentExam>();
                    var labIds = new HashSet<int>();

                    for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
                    {
                        var userIdText = worksheet.Cells[row, 1].Text;
                        var labIdText = worksheet.Cells[row, 3].Text;

                        if (!string.IsNullOrEmpty(userIdText) && int.TryParse(userIdText, out int userId))
                        {
                            var user = await _context.Users.FindAsync(userId);
                            if (user != null)
                            {
                                studentExams.Add(new StudentExam
                                {
                                    UserId = userId,
                                    ExamId = model.Id
                                });
                            }
                            else
                            {
                                _logger.LogWarning($"Student ID {userId} not found.");
                            }
                        }

                        if (!string.IsNullOrEmpty(labIdText) && int.TryParse(labIdText, out int labId))
                        {
                            var lab = await _context.Labs.FindAsync(labId);
                            if (lab == null)
                            {
                                _logger.LogWarning($"lab ID {labId} not found.");
                            }
                            labIds.Add(labId);
                        }
                    }

                    if (studentExams.Count == 0)
                    {
                        ModelState.AddModelError("", "No valid student records found.");
                        return View(model);
                    }

                    _context.StudentExams.AddRange(studentExams);
                    
                    var examLabs = labIds.Select(labId => new ExamLab
                    {
                        ExamId = model.Id,
                        LabId = labId
                    });
                    _context.ExamLabs.AddRange(examLabs);

                    await _context.SaveChangesAsync();
                }
            }

            // 6. Process observer file (optional)
            if (observerUpload != null && observerUpload.Length > 0 && observerUpload.FileName.EndsWith(".xlsx"))
            {
                using (var stream = new MemoryStream())
                {
                    await observerUpload.CopyToAsync(stream);
                    using (var package = new ExcelPackage(stream))
                    {
                        var worksheet = package.Workbook.Worksheets.FirstOrDefault();
                        if (worksheet != null && worksheet.Dimension != null)
                        {
                            for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
                            {
                                var userIdText = worksheet.Cells[row, 1].Text;
                                var labIdText = worksheet.Cells[row, 3].Text;

                                if (int.TryParse(userIdText, out int userId) && int.TryParse(labIdText, out int labId))
                                {
                                    var user = await _context.Users.FindAsync(userId);
                                    var lab = await _context.Labs.FindAsync(labId);
                                    if (user != null && lab!=null)
                                    {
                                        _context.ObserverLabs.Add(new ObserverLabs
                                        {
                                            UserId = userId,
                                            LabId = labId,
                                            DateTime = DateTime.Now
                                        });
                                    }
                                    else
                                    {
                                        _logger.LogWarning($"Student ID or lab ID not found.");
                                    }
                                }
                                await _context.SaveChangesAsync();
                            }
                        }
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "Please upload a valid Observer Excel file (.xlsx).");
                return View(model);
            }

                return RedirectToAction("Index", "Home");
        }
    }
}


using Scheduler.Data;
using Scheduler.Models;
using System.Text;
using ExcelDataReader;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;


namespace Scheduler.Controllers
{
    public class DataSeedController : Controller
    {
        private readonly ILogger<DataSeedController> _logger;
        private readonly DBContextSystem _context;

        public DataSeedController(ILogger<DataSeedController> logger, DBContextSystem context)
        {
            _logger = logger;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult UploadExcelStudyPlan()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadExcelStudyPlan(IFormFile file)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            if (file != null && file.Length > 0)
            {
                var uploadsFolder = $"{Directory.GetCurrentDirectory()}\\wwwroot\\Uploads\\";

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var filePath = Path.Combine(uploadsFolder, file.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                using (var stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
                {
                    // Auto-detect format, supports:
                    //  - Binary Excel files (2.0-2003 format; *.xls)
                    //  - OpenXml Excel files (2007 format; *.xlsx, *.xlsb)
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        // Choose one of either 1 or 2:

                        // 1. Use the reader methods
                        do
                        {
                            bool isHeaderSkipped = false;
                            while (reader.Read())
                            {
                                if (!isHeaderSkipped)
                                {
                                    isHeaderSkipped = true;
                                    continue;
                                }
                                // reader.GetDouble(0);
                                StudyPlan studyPlan = new StudyPlan();
                                studyPlan.Name = reader.GetValue(1).ToString();
                                studyPlan.College = reader.GetValue(2).ToString();
                                studyPlan.Major = reader.GetValue(3).ToString();
                            }
                        } while (reader.NextResult());

                        // 2. Use the AsDataSet extension method
                        //var result = reader.AsDataSet();

                        // The result of each spreadsheet is in result.Tables
                    }
                }
            }
            return View();
        }


        public IActionResult UploadExcelInstructors()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadExcelInstructors(IFormFile file)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            if (file != null && file.Length > 0)
            {
                var uploadsFolder = $"{Directory.GetCurrentDirectory()}\\wwwroot\\Uploads\\";

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var filePath = Path.Combine(uploadsFolder, file.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                using (var stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
                {
                    // Auto-detect format, supports:
                    //  - Binary Excel files (2.0-2003 format; *.xls)
                    //  - OpenXml Excel files (2007 format; *.xlsx, *.xlsb)
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        // Choose one of either 1 or 2:

                        // 1. Use the reader methods
                        do
                        {
                            bool isHeaderSkipped = false;
                            while (reader.Read())
                            {
                                if (!isHeaderSkipped)
                                {
                                    isHeaderSkipped = true;
                                    continue;
                                }

                                // reader.GetDouble(0);
                                Instructor instructors = new Instructor();
                                instructors.Job_ID = Convert.ToInt32(reader.GetValue(1).ToString());
                                instructors.Name = reader.GetValue(2).ToString();

                                _context.Add(instructors);
                                await _context.SaveChangesAsync();
                            }
                        } while (reader.NextResult());

                        // 2. Use the AsDataSet extension method
                        //var result = reader.AsDataSet();

                        // The result of each spreadsheet is in result.Tables
                    }
                }
            }
            return View();
        }

        public IActionResult UploadExcelCourse()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadExcelCourse(IFormFile file)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            if (file != null && file.Length > 0)
            {
                var uploadsFolder = $"{Directory.GetCurrentDirectory()}\\wwwroot\\Uploads\\";

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var filePath = Path.Combine(uploadsFolder, file.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                using (var stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
                {
                    // Auto-detect format, supports:
                    //  - Binary Excel files (2.0-2003 format; *.xls)
                    //  - OpenXml Excel files (2007 format; *.xlsx, *.xlsb)
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        // Choose one of either 1 or 2:

                        // 1. Use the reader methods
                        do
                        {
                            bool isHeaderSkipped = false;
                            while (reader.Read())
                            {
                                if (!isHeaderSkipped)
                                {
                                    isHeaderSkipped = true;
                                    continue;
                                }

                                // reader.GetDouble(0);
                                Course course = new Course();
                                course.CRS_NO = Convert.ToInt32(reader.GetValue(1).ToString());
                                course.CRS_CR_HOURS = Convert.ToInt32(reader.GetValue(2).ToString());
                                course.CRS_A_NAME = reader.GetValue(3).ToString();
                                course.CRS_SPEC = reader.GetValue(4).ToString();
                                course.CRS_ACTIVE = Convert.ToInt32(reader.GetValue(5).ToString());
                                course.Type = reader.GetValue(6).ToString();

                                _context.Add(course);
                                await _context.SaveChangesAsync();
                            }
                        } while (reader.NextResult());

                        // 2. Use the AsDataSet extension method
                        //var result = reader.AsDataSet();

                        // The result of each spreadsheet is in result.Tables
                    }
                }
            }
            return View();
        }


        public IActionResult UploadExcelPlanContent()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadExcelPlanContent(IFormFile file)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            if (file != null && file.Length > 0)
            {
                var uploadsFolder = $"{Directory.GetCurrentDirectory()}\\wwwroot\\Uploads\\";

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var filePath = Path.Combine(uploadsFolder, file.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                using (var stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
                {
                    // Auto-detect format, supports:
                    //  - Binary Excel files (2.0-2003 format; *.xls)
                    //  - OpenXml Excel files (2007 format; *.xlsx, *.xlsb)
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        // Choose one of either 1 or 2:

                        // 1. Use the reader methods
                        do
                        {
                            bool isHeaderSkipped = false;
                            while (reader.Read())
                            {
                                if (!isHeaderSkipped)
                                {
                                    isHeaderSkipped = true;
                                    continue;
                                }

                                // reader.GetDouble(0);
                                PlanContent planContent = new PlanContent();
                                //planContent.course = await _context.Courses.FindAsync(1);



                                int courseId = Convert.ToInt32(reader.GetValue(1).ToString());
                                planContent.course = await _context.Courses.FindAsync(courseId);
                                    
                                    //= await _context.studyPlan.FindAsync(courseId);
                                
                                int StudyPlanId = Convert.ToInt32(reader.GetValue(2).ToString());
                                planContent.StudyPlan = await _context.StudyPlans.FindAsync(StudyPlanId);

                                planContent.code = Convert.ToInt32(reader.GetValue(3).ToString());
                                planContent.prerequisite = Convert.ToInt32(reader.GetValue(4)?.ToString());

                                _context.Add(planContent);
                                await _context.SaveChangesAsync();
                            }
                        } while (reader.NextResult());

                        // 2. Use the AsDataSet extension method
                        //var result = reader.AsDataSet();

                        // The result of each spreadsheet is in result.Tables
                    }
                }
            }
            return View();
        }


        public IActionResult UploadExcelDegreeProgressPlan()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadExcelDegreeProgressPlan(IFormFile file)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            if (file != null && file.Length > 0)
            {
                var uploadsFolder = $"{Directory.GetCurrentDirectory()}\\wwwroot\\Uploads\\";

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var filePath = Path.Combine(uploadsFolder, file.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                using (var stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
                {
                    // Auto-detect format, supports:
                    //  - Binary Excel files (2.0-2003 format; *.xls)
                    //  - OpenXml Excel files (2007 format; *.xlsx, *.xlsb)
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        // Choose one of either 1 or 2:

                        // 1. Use the reader methods
                        do
                        {
                            bool isHeaderSkipped = false;
                            while (reader.Read())
                            {
                                if (!isHeaderSkipped)
                                {
                                    isHeaderSkipped = true;
                                    continue;
                                }

                                // reader.GetDouble(0);
                                DegreeProgressPlan degreeProgressPlan = new DegreeProgressPlan();
                                degreeProgressPlan.Name = reader.GetValue(1).ToString();
                                degreeProgressPlan.College = reader.GetValue(2).ToString();
                                degreeProgressPlan.Major = reader.GetValue(3).ToString();

                                _context.Add(degreeProgressPlan);
                                await _context.SaveChangesAsync();
                            }
                        } while (reader.NextResult());

                        // 2. Use the AsDataSet extension method
                        //var result = reader.AsDataSet();

                        // The result of each spreadsheet is in result.Tables
                    }
                }
            }
            return View();
        }

        public IActionResult UploadExcelDegreeProgresContent()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadExcelDegreeProgresContent(IFormFile file)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            if (file != null && file.Length > 0)
            {
                var uploadsFolder = $"{Directory.GetCurrentDirectory()}\\wwwroot\\Uploads\\";

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var filePath = Path.Combine(uploadsFolder, file.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                using (var stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
                {
                    // Auto-detect format, supports:
                    //  - Binary Excel files (2.0-2003 format; *.xls)
                    //  - OpenXml Excel files (2007 format; *.xlsx, *.xlsb)
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        // Choose one of either 1 or 2:

                        // 1. Use the reader methods
                        do
                        {
                            bool isHeaderSkipped = false;
                            while (reader.Read())
                            {
                                if (!isHeaderSkipped)
                                {
                                    isHeaderSkipped = true;
                                    continue;
                                }

                                // reader.GetDouble(0);
                                DegreeProgresContent degreeProgresContent = new DegreeProgresContent();

                                //degreeProgresContent.course = await _context.Courses.FindAsync(1);
                                int courseId=  Convert.ToInt32(reader.GetValue(1).ToString());
                                degreeProgresContent.course = await _context.Courses.FindAsync(courseId);

                                int planId = Convert.ToInt32(reader.GetValue(2).ToString());
                                degreeProgresContent.DegreeProgressPlan = await _context.degreeProgressPlans.FindAsync(planId);

                                degreeProgresContent.SPEC_CODE = Convert.ToInt32(reader.GetValue(3).ToString());
                                degreeProgresContent.SMST_NO = Convert.ToInt32(reader.GetValue(4).ToString());
                                degreeProgresContent.SPEC_YYT = Convert.ToInt32(reader.GetValue(5).ToString());
                                degreeProgresContent.SPEC_LVL = Convert.ToInt32(reader.GetValue(6).ToString());

                                _context.Add(degreeProgresContent);
                                await _context.SaveChangesAsync();
                            }
                        } while (reader.NextResult());

                        // 2. Use the AsDataSet extension method
                        //var result = reader.AsDataSet();

                        // The result of each spreadsheet is in result.Tables
                    }
                }
            }
            return View();
        }

        public IActionResult UploadExcelStudent()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadExcelStudent(IFormFile file)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            if (file != null && file.Length > 0)
            {
                var uploadsFolder = $"{Directory.GetCurrentDirectory()}\\wwwroot\\Uploads\\";

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var filePath = Path.Combine(uploadsFolder, file.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                using (var stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
                {
                    // Auto-detect format, supports:
                    //  - Binary Excel files (2.0-2003 format; *.xls)
                    //  - OpenXml Excel files (2007 format; *.xlsx, *.xlsb)
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        // Choose one of either 1 or 2:

                        // 1. Use the reader methods
                        do
                        {
                            bool isHeaderSkipped = false;
                            while (reader.Read())
                            {
                                if (!isHeaderSkipped)
                                {
                                    isHeaderSkipped = true;
                                    continue;
                                }

                                // reader.GetDouble(0);
                                Student student = new Student();
                                student.ID_Student = Convert.ToInt32(reader.GetValue(1).ToString());
                                student.password = reader.GetValue(2).ToString();
                                student.Email = reader.GetValue(3).ToString();
                                student.Name = reader.GetValue(4).ToString();
                                int studyPlanId = Convert.ToInt32(reader.GetValue(5).ToString());
                                student.studyPlan = await _context.StudyPlans.FindAsync(studyPlanId);

                                int planId = Convert.ToInt32(reader.GetValue(6).ToString());
                                student.degreeProgressPlan = await _context.degreeProgressPlans.FindAsync(planId);

                                _context.Add(student);
                                await _context.SaveChangesAsync();
                            }
                        } while (reader.NextResult());

                        // 2. Use the AsDataSet extension method
                        //var result = reader.AsDataSet();

                        // The result of each spreadsheet is in result.Tables
                    }
                }
            }
            return View();
        }

        public IActionResult UploadExcelSection()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadExcelSection(IFormFile file)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            if (file != null && file.Length > 0)
            {
                var uploadsFolder = $"{Directory.GetCurrentDirectory()}\\wwwroot\\Uploads\\";

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var filePath = Path.Combine(uploadsFolder, file.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                using (var stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
                {
                    // Auto-detect format, supports:
                    //  - Binary Excel files (2.0-2003 format; *.xls)
                    //  - OpenXml Excel files (2007 format; *.xlsx, *.xlsb)
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        // Choose one of either 1 or 2:

                        // 1. Use the reader methods
                        do
                        {
                            bool isHeaderSkipped = false;
                            while (reader.Read())
                            {
                                if (!isHeaderSkipped)
                                {
                                    isHeaderSkipped = true;
                                    continue;
                                }

                                // reader.GetDouble(0);
                                Section section = new Section();
                                section.SectionNumber = Convert.ToInt32(reader.GetValue(1).ToString());
                                section.Hall = reader.GetValue(2).ToString();
                                section.Start_Sunday = reader.GetValue(3) as DateTime?;
                                section.End_Sunday = reader.GetValue(4) as DateTime?;
                                section.Start_Monday = reader.GetValue(5) as DateTime?;
                                section.End_Monday = reader.GetValue(6) as DateTime?;
                                section.Start_Tuesday = reader.GetValue(7) as DateTime?;
                                section.End_Tuesday = reader.GetValue(8) as DateTime?;
                                section.Start_Wednesday = reader.GetValue(9) as DateTime?;
                                section.End_Wednesday = reader.GetValue(10) as DateTime?;
                                section.Start_Thursday = reader.GetValue(11) as DateTime?;
                                section.End_Thursday = reader.GetValue(12) as DateTime?;
                                section.Status = reader.GetString(13);


                                int courseId = Convert.ToInt32(reader.GetValue(14).ToString());
                                section.course = await _context.Courses.FindAsync(courseId);

                                int instructorId = Convert.ToInt32(reader.GetValue(15).ToString());
                                section.Instructors = await _context.Instructors.FindAsync(instructorId);

                                _context.Add(section);
                                await _context.SaveChangesAsync();
                            }
                        } while (reader.NextResult());

                        // 2. Use the AsDataSet extension method
                        //var result = reader.AsDataSet();

                        // The result of each spreadsheet is in result.Tables
                    }
                }
            }
            return View();
        }

        public IActionResult UploadExcelSchedule()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadExcelSchedule(IFormFile file)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            if (file != null && file.Length > 0)
            {
                var uploadsFolder = $"{Directory.GetCurrentDirectory()}\\wwwroot\\Uploads\\";

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var filePath = Path.Combine(uploadsFolder, file.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                using (var stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
                {
                    // Auto-detect format, supports:
                    //  - Binary Excel files (2.0-2003 format; *.xls)
                    //  - OpenXml Excel files (2007 format; *.xlsx, *.xlsb)
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        // Choose one of either 1 or 2:

                        // 1. Use the reader methods
                        do
                        {
                            bool isHeaderSkipped = false;
                            while (reader.Read())
                            {
                                if (!isHeaderSkipped)
                                {
                                    isHeaderSkipped = true;
                                    continue;
                                }

                                // reader.GetDouble(0);
                                Schedule schedule = new Schedule();
                                int studentseId = Convert.ToInt32(reader.GetValue(1).ToString()); // Assuming column index 6 contains degreeProgressPlan ID
                                schedule.students = await _context.Students.FindAsync(studentseId);
                                schedule.Approv_Schedule = Convert.ToInt32(reader.GetValue(2).ToString());

                                _context.Add(schedule);
                                await _context.SaveChangesAsync();
                            }
                        } while (reader.NextResult());

                        // 2. Use the AsDataSet extension method
                        //var result = reader.AsDataSet();

                        // The result of each spreadsheet is in result.Tables
                    }
                }
            }
            return View();
        }


        public IActionResult UploadExcelSectionSchedule()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadExcelSectionSchedule(IFormFile file)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            if (file != null && file.Length > 0)
            {
                var uploadsFolder = $"{Directory.GetCurrentDirectory()}\\wwwroot\\Uploads\\";

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var filePath = Path.Combine(uploadsFolder, file.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                using (var stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
                {
                    // Auto-detect format, supports:
                    //  - Binary Excel files (2.0-2003 format; *.xls)
                    //  - OpenXml Excel files (2007 format; *.xlsx, *.xlsb)
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        // Choose one of either 1 or 2:

                        // 1. Use the reader methods
                        do
                        {
                            bool isHeaderSkipped = false;
                            while (reader.Read())
                            {
                                if (!isHeaderSkipped)
                                {
                                    isHeaderSkipped = true;
                                    continue;
                                }

                                // reader.GetDouble(0);
                                SectionSchedule sectionSchedule = new SectionSchedule();
                                int sectionId = Convert.ToInt32(reader.GetValue(1).ToString()); // Assuming column index 5 contains studyPlan ID
                                sectionSchedule.section = await _context.Sections.FindAsync(sectionId);

                                int scheduleId = Convert.ToInt32(reader.GetValue(2).ToString()); // Assuming column index 6 contains degreeProgressPlan ID
                                sectionSchedule.schedule = await _context.Schedules.FindAsync(scheduleId);



                                _context.Add(sectionSchedule);
                                await _context.SaveChangesAsync();
                            }
                        } while (reader.NextResult());

                        // 2. Use the AsDataSet extension method
                        //var result = reader.AsDataSet();

                        // The result of each spreadsheet is in result.Tables
                    }
                }
            }
            return View();
        }


        public IActionResult UploadExcelProgress()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> UploadExcelProgress(IFormFile file)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            if (file != null && file.Length > 0)
            {
                var uploadsFolder = $"{Directory.GetCurrentDirectory()}\\wwwroot\\Uploads\\";

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var filePath = Path.Combine(uploadsFolder, file.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                using (var stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.Read))
                {
                    // Auto-detect format, supports:
                    //  - Binary Excel files (2.0-2003 format; *.xls)
                    //  - OpenXml Excel files (2007 format; *.xlsx, *.xlsb)
                    using (var reader = ExcelReaderFactory.CreateReader(stream))
                    {
                        // Choose one of either 1 or 2:

                        // 1. Use the reader methods
                        do
                        {
                            bool isHeaderSkipped = false;
                            while (reader.Read())
                            {
                                if (!isHeaderSkipped)
                                {
                                    isHeaderSkipped = true;
                                    continue;
                                }

                                // reader.GetDouble(0);
                                Progress progress = new Progress();
                                progress.Mark = Convert.ToSingle(reader.GetValue(1));
                                progress.Student = await _context.Students.FindAsync(Convert.ToInt32(reader.GetValue(2)));
                                progress.course = await _context.Courses.FindAsync(Convert.ToInt32(reader.GetValue(3)));

                                _context.Add(progress);
                                await _context.SaveChangesAsync();
                            }
                        } while (reader.NextResult());

                        // 2. Use the AsDataSet extension method
                        //var result = reader.AsDataSet();

                        // The result of each spreadsheet is in result.Tables
                    }
                }
            }
            return View();
        }
    }
}

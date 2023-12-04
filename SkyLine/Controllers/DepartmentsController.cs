using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkyLine.Data;
using SkyLine.Models;

// Any action method must have HttpVerb -> HttpGet / HttpPost



// routing controller / + class name without the suffix(controller) / method name
// URL: case in-sensetive
namespace SkyLine.Controllers
{
    public class DepartmentsController : Controller
    {
        ApplicationDbContext _context;
        IWebHostEnvironment _webHostEnvironment;
        public DepartmentsController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;

        }


        // Action method (Action)
        // https://localhost:7282/departments/getindexview
        [HttpGet]
        public IActionResult GetIndexView(string? search)
        {
            ViewBag.Search = search;
            if (string.IsNullOrEmpty(search))
            {
                return View("Index", _context.Departments.ToList());

            }
            return View("Index", _context.Departments.Where(d => d.Name.Contains(search) || 
                                                                                         d.Description.Contains(search)).ToList());
        }

        [HttpGet]
        public IActionResult GetDetailsView(int id)
        {
            Department dept = _context.Departments.Include(d => d.Employees).FirstOrDefault(dept => dept.Id == id);
            ViewBag.CurrentDept = dept;
            if (dept == null)
                return NotFound();
            return View("Details", dept);
        }

        [HttpGet]
        public IActionResult GetCreateView() {
            return View("Create");
        }

        [HttpPost] // anything that sends data
        public IActionResult AddNew(Department dept, IFormFile? imageFormFile)
        {

            if (dept.StartDate < new DateTime(2014,2,1))
            {
                ModelState.AddModelError(string.Empty, "Start Date must be after 31 January 2014");
            }
            // GUID (Globally unique identifier) 
            if (ModelState.IsValid)
            {
                if (imageFormFile != null)
                {
                    string imgExtension = Path.GetExtension(imageFormFile.FileName);
                    Guid guid = Guid.NewGuid();
                    string imgPath = "\\images\\" + guid + imgExtension;
                    dept.ImagePath = imgPath;
                    string imgFullPath = _webHostEnvironment.WebRootPath + imgPath;
                    FileStream imgFileStream = new FileStream(imgFullPath, FileMode.Create);
                    imageFormFile.CopyTo(imgFileStream);
                    imgFileStream.Dispose();
                }
                else
                {
                    dept.ImagePath = "\\images\\No_Image.png";
                }
                _context.Departments.Add(dept);
                _context.SaveChanges();
                return RedirectToAction("GetIndexView"); // returns New empty view without errors
            }
            return View("Create", dept); // return view after errors
            // same as return View("Create"); // asp.net puts the dept
        }

        [HttpGet]
        public IActionResult GetEditView(int id)
        {
           Department department = _context.Departments.FirstOrDefault(department => department.Id == id);
            if (department == null)
                return NotFound();
            return View("Edit", department);
        }
        [HttpPost]
        public IActionResult EditCurrent(Department dept, IFormFile? imageFormFile)
        {
            if (dept.StartDate < new DateTime(2014, 2, 1))
            {
                ModelState.AddModelError(string.Empty, "Start Date must be after 31 January 2014");
            }
            if (ModelState.IsValid)
            {
                if (imageFormFile != null)
                {
                    if(dept.ImagePath != "\\images\\No_Image.png")
                    {
                        string oldImgFullPath = _webHostEnvironment.WebRootPath + dept.ImagePath;
                        System.IO.File.Delete(oldImgFullPath);
                    }
                    string imgExtension = Path.GetExtension(imageFormFile.FileName);
                    Guid guid = Guid.NewGuid();
                    string imgPath = "\\images\\" + guid + imgExtension;
                    dept.ImagePath = imgPath;
                    string imgFullPath = _webHostEnvironment.WebRootPath + imgPath;
                    FileStream imgFileStream = new FileStream(imgFullPath, FileMode.Create);
                    imageFormFile.CopyTo(imgFileStream);
                    imgFileStream.Dispose();
                }

                _context.Departments.Update(dept);
                _context.SaveChanges();
                return RedirectToAction("GetIndexView"); // returns New empty view without errors
            }
            return View("Edit", dept);
        }

        [HttpGet]
        public IActionResult GetDeleteView(int id)
        {
            Department department = _context.Departments.Include(d => d.Employees).FirstOrDefault(dept => dept.Id == id);
            ViewBag.CurrentDept = department;

            if (department == null)
                return NotFound();
            return View("Delete", department);

        }


        [HttpPost]
        public IActionResult DeleteCurrent(int id)
        {
            Department dept = _context.Departments.Find(id);
            if(dept != null && dept.ImagePath != "\\images\\No_Image.png")
            {
                string imgFullPath = _webHostEnvironment.WebRootPath + dept.ImagePath;
                System.IO.File.Delete(imgFullPath);
            } 
            _context.Departments.Remove(dept);
            _context.SaveChanges();

            return RedirectToAction("GetIndexView");
        }

       public string GreetVisitor()
        {
            return "Welcome to SkyLine for Software Solutions";
        }

        //https://localhost:7282/departments/greetuser?firstname=folan&lastName=EL%20Ellany
        public string GreetUser(String firstName, string lastName) {

            return $"Hi {firstName} {lastName}!";
        }
    }
}




//List<Department> departments = new List<Department>
//{
//    new Department { Id = 101, Name = "Web Dev",Description = "Web Development",AnnualBudget = 250000m,StartDate = new DateTime(2015, 5, 15),IsActive = true},
//    new Department{ Id = 102, Name = "Mob Dev",Description = "Mobile App Development",AnnualBudget = 150000m,StartDate = new DateTime(2016, 6, 16),IsActive = true},
//    new Department{ Id = 103, Name = "Desk Dev",Description = "Desktop App Development",AnnualBudget = 275000m,StartDate = new DateTime(2014, 4, 14),IsActive = true},
//    new Department{ Id = 104, Name = "QC",Description = "Quality Control",AnnualBudget = 135000m,StartDate = new DateTime(2017, 7, 17),IsActive = true},
//    new Department{ Id = 105, Name = "PM",Description = "Project Management",AnnualBudget = 0m,StartDate = new DateTime(2018, 8, 18),IsActive = false}
//};
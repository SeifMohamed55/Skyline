using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SkyLine.Data;
using SkyLine.Models;

namespace SkyLine.Controllers
{
    public class EmployeesController : Controller
    {

        ApplicationDbContext _context;
        IWebHostEnvironment _webHostEnvironment;
        public EmployeesController(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;

        }
        public IActionResult GetIndexView(string? search)
        {
            if (string.IsNullOrEmpty(search))
            {
                return View("Index", _context.Employees.Include(e => e.Department).ToList());
            }

            ViewBag.SearchVal = search;
            return View("Index", _context.Employees.Include(e => e.Department).Where(e => (e.FullName.Contains(search)) ||                                                                                     e.Position.Contains(search)).ToList());
        }

        public IActionResult GetDetailsView(int id)
        {
            Employee employee = _context.Employees.Include(e => e.Department).FirstOrDefault(emp => emp.Id == id);
            if (employee == null)
                return NotFound();
            ViewBag.Employee = employee;
            return View("Details", employee);
        }

        public IActionResult GetCreateView()
        {
            //ViewBag.AllDepartments = _context.Departments.ToList();
            //ViewData["AllDepartments"] = _context.Departments.ToList();
            //TempData["AllDepartments"] = _context.Departments.ToList();
            ViewBag.AllDepartments = _context.Departments.ToList();
            return View("Create");
        }

        [HttpPost]
        public IActionResult AddNew(Employee employee, IFormFile? formFile)
        {
            if (employee.JoinDate < employee.BirthDate)
            {
                ModelState.AddModelError(String.Empty, "JoinDate Can't be before BirthDate");
            }
            if (ModelState.IsValid)
            {
                if(formFile != null)
                {
                    string formFileExtension = Path.GetExtension(formFile.FileName);
                    Guid guid = Guid.NewGuid();
                    string newName = guid + formFileExtension;
                    string relPath = "\\Images\\" + newName;
                    employee.ImagePath = relPath;
                    string fullPath = _webHostEnvironment.WebRootPath + relPath;
                    FileStream fileStream = new FileStream(fullPath, FileMode.Create);
                    formFile.CopyTo(fileStream);
                    fileStream.Dispose();
                }
                else
                {
                    employee.ImagePath = "\\images\\No_Image.png";
                }

                _context.Employees.Add(employee);
                _context.SaveChanges();
                return RedirectToAction("GetIndexView");
            }
            else
            {
                ViewBag.AllDepartments = _context.Departments.ToList();
                return View("Create", employee);
            }
        }

        public IActionResult GetEditView(int id)
        {
            Employee emp = _context.Employees.FirstOrDefault(emp => emp.Id == id);
            if (emp == null)
                return NotFound();
            else
            {
                ViewBag.AllDepartments = _context.Departments.ToList();
                return View("Edit", emp);
            }
        }
        [HttpPost]
        public IActionResult EditCurrent(Employee employee, IFormFile? formFile)
        {
            if (employee.JoinDate < employee.BirthDate)
            {
                ModelState.AddModelError(String.Empty, "JoinDate Can't be before BirthDate");
            }
            if (ModelState.IsValid)
            {
                if (formFile != null)
                {
                    if(employee.ImagePath != "\\images\\No_Image.png")
                    {
                        System.IO.File.Delete(_webHostEnvironment.WebRootPath + employee.ImagePath);
                    }
                    string formFileExtension = Path.GetExtension(formFile.FileName);
                    Guid guid = Guid.NewGuid();
                    string newName = guid + formFileExtension;
                    string relPath = "\\Images\\" + newName;
                    employee.ImagePath = relPath;
                    string fullPath = _webHostEnvironment.WebRootPath + relPath;
                    FileStream fileStream = new FileStream(fullPath, FileMode.Create);
                    formFile.CopyTo(fileStream);
                    fileStream.Dispose();
                }
                


                _context.Employees.Update(employee);
                _context.SaveChanges();
                return RedirectToAction("GetIndexView");
            }
            else
            {
                ViewBag.AllDepartments = _context.Departments.ToList();
                return View("Edit", employee);
            }
        }

        public IActionResult GetDeleteView(int id)
        {
            Employee emp = _context.Employees.Include(e => e.Department).FirstOrDefault(emp => emp.Id == id);
            if (emp == null)
                return NotFound();
            ViewBag.Employee = emp;

            return View("Delete", emp);
        }

        public IActionResult DeleteCurrent(int id)
        {
            Employee emp = _context.Employees.Find(id);
             if(emp == null)
                return NotFound();
             if(emp.ImagePath != "\\images\\No_Image.png")
            {
                System.IO.File.Delete(_webHostEnvironment.WebRootPath + emp.ImagePath);
            }
            _context.Employees.Remove(emp);
            _context.SaveChanges();
            return RedirectToAction("GetIndexView");
        }

    }
}


/*
          List<Employee> employees = new List<Employee>()
        {
            new Employee{Id = 1,FullName = "Bahaa Ali", JoinDate = new DateTime(2021,10,11), Salary = 10250m, IsActive = true, BirthDate = new DateTime(1999,3,27) },

            new Employee {Id = 2, FullName = "Wael Ahmed",JoinDate = new DateTime(2019,9,8), Salary = 18500.25m,IsActive = false, BirthDate = new DateTime(1995, 11, 29) },

            new Employee{Id = 3, FullName = "Ayman Ashraf", JoinDate = new DateTime(2020,6,7), Salary = 14500.5m, IsActive = true, BirthDate = new DateTime(1998,12,24) },

            new Employee{Id = 4, FullName = "Hossam Kamal", JoinDate = new DateTime(2022,5,23), Salary = 21500m, IsActive = true, BirthDate = new DateTime(1993,4,14) },

        };
*/

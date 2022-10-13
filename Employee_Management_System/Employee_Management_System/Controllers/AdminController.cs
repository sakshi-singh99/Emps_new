using Employee_Management_System.Data;
using Employee_Management_System.Models;
using Microsoft.AspNetCore.Mvc;
using System.Dynamic;

namespace Employee_Management_System.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;
        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            IEnumerable<Employee> objCatlist = _context.Employees;
            return View(objCatlist);
        }

        public IActionResult Create()
        {
         
            List<Role> RoleDD = new List<Role>();
            RoleDD = (from c in _context.Roles select c).ToList();
            RoleDD.Insert(0, new Role { RoleId = 0, RoleName = "---Select Role---" });
            ViewBag.msg = RoleDD;
            return View();
        }

        [HttpPost]
        public IActionResult Create(Employee empobj)
        {
            var get_user = _context.Employees.FirstOrDefault(email => email.EmpEmail == empobj.EmpEmail);
            //HttpContext.Session.SetString("FirstName", get_user.EmpFirstName.ToString());
            if (get_user == null)
            {
                _context.Employees.Add(empobj);
                _context.SaveChanges();
                TempData["ResultOk"] = "Registered Successfully!";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["ResultOk"] = "Employee allready exist!";
            }

            return View(empobj);
        }

        public IActionResult Edit(int? id)
        {

            List<Role> RoleDD = new List<Role>();
            RoleDD = (from c in _context.Roles select c).ToList();
            RoleDD.Insert(0, new Role { RoleId = 0, RoleName = "---Select Role---" });
            ViewBag.msg = RoleDD;

            if (id == null || id == 0)
            {
                return NotFound();
            }
            var empfromdb = _context.Employees.Find(id);

            if (empfromdb == null)
            {
                return NotFound();
            }
            return View(empfromdb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Employee employee)
        {
            var empfromdb = _context.Employees.Find(id);
            if (empfromdb == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid==false)
            {
                empfromdb.EmpFirstName =employee.EmpFirstName;
                empfromdb.EmpLastName =employee.EmpLastName;
                empfromdb.EmpEmail=employee.EmpEmail;
                empfromdb.EmpAddress=employee.EmpAddress;
                empfromdb.EmpPhoneNumber=employee.EmpPhoneNumber;
                empfromdb.RoleId=employee.RoleId;
                empfromdb.EmpRegisteredDate=employee.EmpRegisteredDate;
                _context.Employees.Update(empfromdb);
                _context.SaveChanges();
                TempData["ResultOk"] = "Data Updated Successfully !";
                return RedirectToAction("Index");
            }

            return View(employee);
        }


        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var empfromdb = _context.Employees.Find(id);

            if (empfromdb == null)
            {
                return NotFound();
            }
            return View(empfromdb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteEmp(int EmpId)
        {
            var deleterecord = _context.Employees.Find(EmpId);
            if (deleterecord == null)
            {
                return NotFound();
            }
            _context.Employees.Remove(deleterecord);
            _context.SaveChanges();
            TempData["ResultOk"] = "Data Deleted Successfully !";
            return RedirectToAction("Index");
        }

        public IActionResult Dashboard()
        {

            if (HttpContext.Session.GetString("Email") != null)
            {
                return View();

            }
            else
            {
                return RedirectToAction("Login","Employee");
            }
        }


    }
}

using Employee_Management_System.Data;
using Employee_Management_System.Models;
using Microsoft.AspNetCore.Mvc;

namespace Employee_Management_System.Controllers
{
    public class ProjectController : Controller
    {
        private readonly AppDbContext _context;
        public ProjectController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            IEnumerable<Project> objCatlist = _context.Projects;
            return View(objCatlist);
        }

        public IActionResult Create()
        {
            
            return View();
        }

        [HttpPost]
        public IActionResult Create(Project projectobj)
        {
            var get_project = _context.Projects.FirstOrDefault(id => id.ProjectId == projectobj.ProjectId);
            if (get_project == null)
            {
                _context.Projects.Add(projectobj);
                _context.SaveChanges();
                TempData["ResultOk"] = "Project Registered Successfully!";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["ResultOk"] = "Project allready exist!";
            }

            return View(projectobj);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var projectfromdb = _context.Projects.Find(id);

            if (projectfromdb == null)
            {
                return NotFound();
            }
            return View(projectfromdb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id,Project project)
        {
            var projectfromdb = _context.Projects.Find(id);
            if (projectfromdb == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                projectfromdb.ProjectName = project.ProjectName;
                _context.Projects.Update(projectfromdb);
                _context.SaveChanges();
                TempData["ResultOk"] = "Data Updated Successfully !";
                return RedirectToAction("Index");
            }

            return View(project);
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var projectfromdb = _context.Projects.FirstOrDefault(m => m.ProjectId == id);

            if (projectfromdb == null)
            {
                return NotFound();
            }
            return View(projectfromdb);
        }

        [HttpPost]
        public IActionResult DeleteProject(int ProjectId)
        {
            var projectfromdb = _context.Projects.FirstOrDefault(m => m.ProjectId == ProjectId);
            _context.Projects.Remove(projectfromdb);
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
                return RedirectToAction("Login", "Employee");
            }
        }

    }
}

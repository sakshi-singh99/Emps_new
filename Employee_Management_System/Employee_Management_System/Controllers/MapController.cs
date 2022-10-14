using Employee_Management_System.Data;
using Employee_Management_System.Models;
using Microsoft.AspNetCore.Mvc;

namespace Employee_Management_System.Controllers
{
    public class MapController : Controller
    {
        private readonly AppDbContext _context;
        public MapController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            List<Employee> employee = _context.Employees.ToList();
            List<Project> project = _context.Projects.ToList();
            List<Map> map = _context.Maps.ToList();
            var objCatlist = from m in map
                              join e in employee on m.EmpId equals e.EmpId into table1
                              from e in table1.ToList()
                              join p in project on m.ProjectId equals p.ProjectId into table2
                              from p in table2.ToList()
                              select new UiModel
                              {
                                  Mapui = m,
                                  EmployeeUi = e,
                                  ProjectUi = p
                              };
            //List <Map> map =  new List<Map>();
            //var get_user = objCatlist.Single(p => p.ProjectId == map.Any(m=>m.ProjectId)
            //           && p.Password == empobj.EmpPassword);

            return View(objCatlist);
        }

        public IActionResult Create()
        {
            List<Employee> EmpDD = new List<Employee>();
            List<Project> ProjectDD = new List<Project>();
            EmpDD = (from e in _context.Employees select e).ToList();
            ProjectDD = (from c in _context.Projects select c).ToList();
            EmpDD.Insert(0, new Employee{ EmpId = 0, EmpEmail = "---Select Employee---" });
            ProjectDD.Insert(0, new Project{ ProjectId = 0, ProjectName = "---Select Project---" });
            ViewBag.empDropDown = EmpDD;
            ViewBag.projectDropDown = ProjectDD;
            return View();
        }

        [HttpPost]
        public IActionResult Create(Map mapobj)
        {
            try
            {
                var get_user = _context.Maps.FirstOrDefault(Id => Id.EmpId == mapobj.EmpId && Id.ProjectId == mapobj.ProjectId);
                ////HttpContext.Session.SetString("FirstName", get_user.EmpFirstName.ToString());
                if (get_user == null)
                {
                    _context.Maps.Add(mapobj);
                    _context.SaveChanges();
                    TempData["ResultOk"] = "Maped Successfully!";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["ResultOk"] = "Employee to this project allready exist!";
                }

            }
            catch
            {
                TempData["ResultOk"] = "Employee to this project allready exist!";
            }


            return View(mapobj);
        }

        public IActionResult Edit(int? id)
        {

            List<Employee> EmpDD = new List<Employee>();
            List<Project> ProjectDD = new List<Project>();
            EmpDD = (from e in _context.Employees select e).ToList();
            ProjectDD = (from c in _context.Projects select c).ToList();
            EmpDD.Insert(0, new Employee { EmpId = 0, EmpEmail = "---Select Employee---" });
            ProjectDD.Insert(0, new Project { ProjectId = 0, ProjectName = "---Select Project---" });
            ViewBag.empDropDown = EmpDD;
            ViewBag.projectDropDown = ProjectDD;

            if (id == null || id == 0)
            {
                return NotFound();
            }
            var mapfromdb = _context.Maps.Find(id);

            if (mapfromdb == null)
            {
                return NotFound();
            }
            return View(mapfromdb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Map map)
        {
            var mapfromdb = _context.Maps.Find(id);
            if (mapfromdb == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid == false)
            {
                mapfromdb.EmpId = map.EmpId;
                mapfromdb.ProjectId = map.ProjectId;
                _context.Maps.Update(mapfromdb);
                _context.SaveChanges();
                TempData["ResultOk"] = "Data Updated Successfully !";
                return RedirectToAction("Index");
            }

            return View(map);
        }


        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var mapfromdb = _context.Maps.Find(id);

            if (mapfromdb == null)
            {
                return NotFound();
            }
            return View(mapfromdb);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteMap(int MapId)
        {
            var deleterecord = _context.Maps.Find(MapId);
            if (deleterecord == null)
            {
                return NotFound();
            }
            _context.Maps.Remove(deleterecord);
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

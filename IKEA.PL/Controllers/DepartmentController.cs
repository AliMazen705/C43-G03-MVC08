using System.Linq.Expressions;
using System.Security.Policy;
using IKEA.BLL.Models;
using IKEA.BLL.Services;
using IKEA.PL.Models.Departments;
using Microsoft.AspNetCore.Mvc;

namespace IKEA.PL.Controllers
{
    public class DepartmentController : Controller
    {
        #region Services
        private readonly IDepartmentService _departmentService;
        private readonly ILogger<DepartmentController> _logger;
        private readonly IWebHostEnvironment _environment;
        public DepartmentController(IDepartmentService departmentService, ILogger<DepartmentController> logger, IWebHostEnvironment environment)
        {
            _departmentService = departmentService;
            _logger = logger;
            _environment = environment;
            _environment = environment;
        }
        #endregion
        #region Index
        [HttpGet]
        //BaseUrl/Department/Index
        public IActionResult Index()
        {
            //view's dictionary: pass data from controller [action]
            //to view [from this view => partial view layout]
            //-:view data :>is a dictionary type property (introduced in asp.net freamwork)
            //3.5=>it helps us to transfer the data from controller[action] to view
            ViewData["message"] = "Welcome To IKEA";
            ViewBag.Message = "Welcome To IKEA";
            var departments = _departmentService.GetAllDepartments();
            return View(departments);
        }
        #endregion
        #region Create
        #region Get
        [HttpGet]
         public IActionResult Create()
        {
            return View();
        }
        #endregion
        #region Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(CreatedDepartmentDto department)
        {
            if (!ModelState.IsValid)
                return View(department);
            var message = string.Empty;
            try
            {
                var result = _departmentService.CreateDerpartmenet(department);
                if (result > 0)
                {
                    TempData["Message"] = "The Department Has Been Created Successfully";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["Message"] = "Sorry The Department Has Not Been Created";
                    message = "Sorry The Department Has Not Been Created";
                    ModelState.AddModelError(string.Empty, message);
                    return View(department);
                }
                 
            }
            
            catch(Exception ex)
            {
                //1- Log Exception
                _logger.LogError(ex, ex.Message);
                //2- Set Frindly Message
                if(_environment.IsDevelopment())
                {
                    message = ex.Message;
                    return View(department);
                }
                else
                {
                    message = "Sorry The Department Has Not Been Created";
                    return View("Error", message);
                }

            }
        }
        #endregion
        #endregion
        #region Details
        [HttpGet]
         public IActionResult Details(int? id)
        {
            if(id is null)
            {
                return BadRequest();
            }
            var department = _departmentService.GetDepartmentById(id.Value);
            if(department is null)
            {
                return NotFound();
            }
            return View(department);

        }
        #endregion
        #region  Edit
        #region Get
        [HttpGet]
         public IActionResult Edit(int? id)
        {
            if (id is null)
            {
                return BadRequest();//400

            }
            var department = _departmentService.GetDepartmentById(id.Value);
            if(department is null)
            {
                return NotFound();//404
            }
            var viewModel = new DepartmentEditViewModel()
            {
                Id = department.Id,
                Code = department.Code,
                Name = department.Name,
                Description = department.Description,
                CreationDate = department.CreationDate
            };
            return View(viewModel);

        }
        #endregion
        #region  Post
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit (int id, DepartmentEditViewModel departmentVM)
        {
            if (!ModelState.IsValid)
                return View(departmentVM);
            var message = string.Empty;
            try
            {
                var updatedDepartment = new UpdateDepartmentDto()
                {
                    Id = id,
                    Code = departmentVM.Code,
                    Name = departmentVM.Name,
                    Description = departmentVM.Description,
                    CreationDate = departmentVM.CreationDate


                };
                var updated = _departmentService.UpdateDepartment(updatedDepartment)>0;
                if(updated)
                {
                    return RedirectToAction(nameof(Index));
                }
                message = "Sorry , An Error Occured While Updating The Department";

            }
             catch(Exception ex)
            {
                //1-
                _logger.LogError(ex, ex.Message);
                message = _environment.IsDevelopment() ? ex.Message : "Sorry , An Error Occured While Updating The Department";

            }
            ModelState.AddModelError(string.Empty, message);
            return View(departmentVM);


        }
        #endregion
        #endregion
        #region Delete
        [HttpGet]
         public IActionResult Delete (int? id)
        {
            if(id is null)
            {
                return BadRequest();
            }
            var department = _departmentService.GetDepartmentById(id.Value);
            if(department is null)
            {
                return NotFound();
            }
            return View(department);
        }
       
        [HttpPost]
        [ValidateAntiForgeryToken] 
        public  IActionResult Delete(int id)
        {
            var message = string.Empty;
            try
            {
                var deleted = _departmentService.DeletedDepartment(id);
                if (deleted)
                {
                    return RedirectToAction(nameof(Index));

                }
                message = "An Error Ocurred During Deleting The Department";
            }
             catch(Exception ex)
            {
                //1-
                _logger.LogError(ex, ex.Message);
                //2- 
                message = _environment.IsDevelopment() ? ex.Message : "An Error Ocurred During Deleting The Department";


            }
            return RedirectToAction(nameof(Index));
        }
        #endregion


    } 
}

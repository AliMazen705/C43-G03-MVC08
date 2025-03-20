using IKEA.BLL.Models;
using IKEA.BLL.Models.Employees;
using IKEA.BLL.Services;
using IKEA.BLL.Services.Employees;
using IKEA.PL.Models.Departments;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace IKEA.PL.Controllers
{
    public class EmployeeController : Controller
    {
        #region Service
        private readonly IEmployeeService _employeeService;
        private readonly ILogger<EmployeeController> _logger;
        private readonly IWebHostEnvironment _environment;

        public EmployeeController(
            IEmployeeService employeeService,
            ILogger<EmployeeController> logger,
            IWebHostEnvironment environment)
        {
            _employeeService = employeeService;
            _logger = logger;
            _environment = environment;
        }
        //public EmployeeController(IEmployeeService employeeService, ILogger<EmployeeController> logger, IWebHostEnvironment environment)
        //{
        //    _employeeService = employeeService;
        //    _logger = logger;
        //    _environment = environment;
        //}
        #endregion
        #region Index
        [HttpGet]
        //BaseUrl/Employee/Index
        public IActionResult Index()
        {
            var employees = _employeeService.GetAllEmployees();
            return View(employees);
        }
        #endregion  
        #region Create
        #region Get
        [HttpGet]
        public IActionResult Create([FromServices] IDepartmentService departmentService)
        {
            ViewData["Departments"] = departmentService.GetAllDepartments();
            return View();
            #endregion
            #region Post
            [HttpPost]
            [ValidateAntiForgeryToken] 
             IActionResult Create(CreatedEmployeeDto employee)
            {
                if (!ModelState.IsValid)
                    return View(employee);
                var message = string.Empty;
                try
                {
                    var result = _employeeService.CreateEmployee(employee);
                    if (result > 0)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        message = "Sorry The Employee Has Not Been Created";
                        ModelState.AddModelError(string.Empty, message);
                        return View(employee);
                    }

                }

                catch (Exception ex)
                {
                    //1- Log Exception
                    _logger.LogError(ex, ex.Message);
                    //2- Set Frindly Message
                    if (_environment.IsDevelopment())
                    {
                        message = ex.Message;
                        return View(employee);
                    }
                    else
                    {
                        message = "Sorry The Employee Has Not Been Created";
                        return View("Error", message);
                    }

                }
            }
            #endregion
            #endregion
            #region Details
            [HttpGet]
             IActionResult Details(int? id)
            {
                if (id is null)
                {
                    return BadRequest();
                }
                var employee = _employeeService.GetEmployeeById(id.Value);
                if (employee is null)
                {
                    return NotFound();
                }
                return View(employee);

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
                var employee = _employeeService.GetEmployeeById(id.Value);
                if (employee is null)
                {
                    return NotFound();//404
                }
                ViewData["Departments"] = departmentService.GetAllDepartments();
                var viewModel = new UpdatedEmployeeDto()
                {
                    Name = employee.Name,
                    Address = employee.Address,
                    Email = employee.Email,
                    Age = employee.Age,
                    Salary = employee.Salary,
                    PhoneNumber = employee.PhoneNumber,
                    IsActive = employee.IsActive,
                    EmployeeType = employee.EmployeeType,
                    Gender = employee.Gender,
                    HiringDate = employee.HiringDate
                };
                return View(viewModel);

            }
            #endregion
            #region  Post
            [HttpPost]
            [ValidateAntiForgeryToken]
             IActionResult Edit([FromRoute] int id, UpdatedEmployeeDto employee)
            {
                if (!ModelState.IsValid)
                    return View(employee);
                var message = string.Empty;
                try
                {
                    var updated = -employeeService.UpdateEmployee(employee) > 0;
                    if (updated)
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    message = "Employee Is Not Updated";

                }
                catch (Exception ex)
                {
                    //1-
                    _logger.LogError(ex, ex.Message);
                    message = _environment.IsDevelopment() ? ex.Message : "The Employee Is Not Created";

                }
                ModelState.AddModelError(string.Empty, message);
                return View(employee);


            }
            #endregion
            #endregion
            #region Delete
            [HttpPost]
            [ValidateAntiForgeryToken] 
               IActionResult Delete(int id)
            {
                var message = string.Empty;
                try
                {
                    var deleted = _employeeService.DeleteEmployee(id);
                    if (deleted)
                    {
                        return RedirectToAction(nameof(Index));

                    }
                    message = "An Error Ocurred During Deleting The Employee";
                }
                catch (Exception ex)
                {
                    //1-
                    _logger.LogError(ex, ex.Message);
                    //2- 
                    message = _environment.IsDevelopment() ? ex.Message : "An Error Ocurred During Deleting The Employee";


                }
                return RedirectToAction(nameof(Index));
            }
            #endregion
        }
    }
}

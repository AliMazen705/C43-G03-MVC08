using IKEA.BLL.Models.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKEA.BLL.Services.Employees
{
    public interface IEmployeeService
    {
        IEnumerable<EmployeeDto> GetAllEmployees();
        EmployeeDetailsDto? GetEmployeeById(int id);
        int CreateEmployee (CreatedEmployeeDto employeeDto);
        int UpdateEmployee (UpdatedEmployeeDto employeeDto);
        bool DeleteEmployee (int id);
    }
}

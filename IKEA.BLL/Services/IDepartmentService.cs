using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IKEA.BLL.Models;
using IKEA.DAL.Models.Departments;

namespace IKEA.BLL.Services
{
    public interface IDepartmentService
    {
        IEnumerable<DepartmentToReturnDto> GetAllDepartments();
       
        DepartmentDetailsToReturnDto? GetDepartmentById(int id);
        int CreateDerpartmenet(CreatedDepartmentDto departmentDto);
        int UpdateDepartment(UpdateDepartmentDto department);
        bool DeletedDepartment(int id);


    }
}

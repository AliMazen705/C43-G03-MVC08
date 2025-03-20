using IKEA.DAL.Models.Employees;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKEA.DAL.Models.Departments
{
    public class Department:ModelBase
    {
        public string Name { get; set; } = null!;
        public string Code { get; set; } = null!;
         public string? Description { get; set; }
         public DateOnly CreationDate { get; set; }

        //#region 
        //[InverseProperty(nameof(Models.Employees.Employee.ManageDepartment))]
        //public virtual Employee Manager { get; set; }
        //[ForeignKey(nameof(Manager))]
        //public int? ManagerId { get; set; }
        //#endregion
        //#region 
        ////Navigational Property [Many]
        //[InverseProperty(nameof(Models.Employees.Employee.Department))]
        //public virtual ICollection<Employee> Employee { get; set; } = new HashSet<Employee>();
        //#endregion
    }
}

using POS_2._0.Web.Enums;
using POS_2._0.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POS_2._0.Web.Services.Employees
{
    public interface IEmployeeService
    {
        void Create(Employee model);
        void Update(Employee model);
        Employee Get(int id);
        void Delete(int id);
    }
}

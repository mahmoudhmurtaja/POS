using POS_2._0.Web.Data;
using POS_2._0.Web.Enums;
using POS_2._0.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace POS_2._0.Web.Services.Employees
{
    public class EmployeeService : IEmployeeService
    {
        private readonly ApplicationDbContext _DB;

        public EmployeeService(ApplicationDbContext DB)
        {
            _DB = DB;
        }

        public void Create(Employee model)
        {
            _DB.Employees.Add(model);
            _DB.SaveChanges();

        }
        public void Update(Employee model)
        {
            _DB.Employees.Update(model);
            _DB.SaveChanges();
        }
        public Employee Get(int id)
        {
            var employee = _DB.Employees.SingleOrDefault(x=> !x.IsDelete && x.Id == id);
            return employee;
        }

        public void Delete(int id)
        {
            var emp = Get(id);
            emp.IsDelete = true;
            _DB.Employees.Update(emp);
            _DB.SaveChanges();
        }

    }
}
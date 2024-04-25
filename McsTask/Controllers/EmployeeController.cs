using McsTask.Core.Enums;
using McsTask.Data;
using McsTask.Models;
using McsTask.Models.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PagedList.Core;

namespace McsTask.Controllers
{
    public class EmployeeController : Controller
    {
        private static List<Employee> employees = new List<Employee>();
        private readonly DataProvider _dataProvider;

        public EmployeeController(DataProvider dataProvider)
        {
            employees = dataProvider.employees;
        }

        // Action to read employee data from a text file and populate the drop-down lists
        public IActionResult Index(int? page , int? employmentType)
        {
            var filteredEmployees = employees;
            if (employmentType != null) {
                filteredEmployees = employees.Where(e => (int)e.EmploymentType == employmentType).ToList();
            }
            
            int pageSize = 10; // Number of employees per page
            int pageNumber = (page ?? 1); // If no page is specified, use the first page

            var pagedEmployees = filteredEmployees.AsQueryable().ToPagedList(pageNumber, pageSize);

            return View(pagedEmployees);
        }
        [HttpGet]
        public IActionResult Create() {
            return View();
        }

        // Action to add a new employee
        [HttpPost]
        public ActionResult Create(Employee employee)
        {
            try
            {
                employee.Id = employees.Count + 1;
                employees.Add(employee);
                return RedirectToAction("Index");
            }
           
            catch (Exception ex)
            {
                // Handle the exception
                return View("Error", new ErrorViewModel() { RequestId = ex.Message });
            }
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var employee = employees.FirstOrDefault(e => e.Id == id);
            return View(employee);
        }


        // Action to edit an existing employee
        [HttpPost]
        public ActionResult Edit(Employee employee)
        {
            try
            {
                var existingEmployee = employees.FirstOrDefault(e => e.Id == employee.Id);
                if (existingEmployee != null)
                {
                    existingEmployee.Name = employee.Name;
                    existingEmployee.Address = employee.Address;
                    existingEmployee.BirthDate = employee.BirthDate;
                    existingEmployee.Graduation = employee.Graduation;
                    existingEmployee.EmploymentType = employee.EmploymentType;
                    existingEmployee.Assurance = employee.Assurance;
                    existingEmployee.Salary = employee.Salary;
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Handle the exception
                return View("Error", new ErrorViewModel() { RequestId = ex.Message });
            }
        }



        // Action to delete an employee
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                var employeeToDelete = employees.FirstOrDefault(e => e.Id == id);
                if (employeeToDelete != null)
                {
                    employees.Remove(employeeToDelete);
                }
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Handle the exception
                return View("Error", new ErrorViewModel (){ RequestId=ex.Message });
            }
        }

        // Function to calculate the overtime hour rate
        public decimal GetOvertimeHourRate(Employee employee)
        {
            switch (employee.EmploymentType)
            {
                case EmploymentType.Monthly:
                    return employee.Salary / 160m;
                case EmploymentType.Hourly:
                    return (employee.Salary) * 3 / 16;
                case EmploymentType.Freelancer:
                    return employee.Salary * 1.5m;
                default:
                    return 0;
            }
        }

       
        private void PopulateDropDownLists()
        {
            ViewBag.MonthlyEmployees = new SelectList(employees.Where(e => e.EmploymentType == EmploymentType.Monthly), "Id", "Name");
            ViewBag.HourlyEmployees = new SelectList(employees.Where(e => e.EmploymentType == EmploymentType.Hourly), "Id", "Name");
            ViewBag.FreelancerEmployees = new SelectList(employees.Where(e => e.EmploymentType == EmploymentType.Freelancer), "Id", "Name");
        }

    }
}

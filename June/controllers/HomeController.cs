using June.models;
using June.viewmodels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace June.controllers
{
    public class HomeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;

        public HomeController(IEmployeeRepository employeeRepository)
        {
             _employeeRepository = employeeRepository;
        }
        public ViewResult Index()
        {
            var model = _employeeRepository.GetAllEmployee();
            return View(model);

        }

public ViewResult Details(int? Id)
            {

            HomeDetailsviewmodels homeDetailsviewmodels = new HomeDetailsviewmodels()
            {
                Employee = _employeeRepository.GetEmployee(Id ?? 1),
                PageTitle = "Employee Details"
            };
            
            
            return View(homeDetailsviewmodels);//view is resolved at runtime.error at browseer but not when debugging
            }
        [HttpGet]
        public ViewResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Employee employee)
        {


            if (ModelState.IsValid)
            {
                Employee newEmployee = _employeeRepository.Add(employee);
                return RedirectToAction("details", new { id = newEmployee.Id });

            }
            return View();

        }
    }
}

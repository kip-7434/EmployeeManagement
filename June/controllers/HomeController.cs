using June.models;
using June.viewmodels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace June.controllers
{

    [Authorize]
    public class HomeController : Controller
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IHostingEnvironment hostingEnvironment;

        public HomeController(IEmployeeRepository employeeRepository,
             IHostingEnvironment hostingEnvironment)
        {
             _employeeRepository = employeeRepository;
            this.hostingEnvironment = hostingEnvironment;
        }


        [AllowAnonymous]
        public ViewResult Index()
        {
            var model = _employeeRepository.GetAllEmployee();
            return View(model);

        }

        [AllowAnonymous]
public ViewResult Details(int? Id)
            
        {


            Employee employee = _employeeRepository.GetEmployee(Id.Value);
            if(employee ==null)
            {
                Response.StatusCode = 404;
                return View("EmployeeNotFound", Id.Value);
            }

            HomeDetailsviewmodels homeDetailsviewmodels = new HomeDetailsviewmodels()
            {
                Employee = employee,
                PageTitle = "Employee Details"
            };
            
            
            return View(homeDetailsviewmodels);//view is resolved at runtime.error at browseer but not when debugging
            }
        [HttpGet]
        
        public ViewResult Create()
        {
            return View();
        }

        [HttpGet]
        
        public ViewResult Edit(int id)
        {
            Employee employee = _employeeRepository.GetEmployee(id);
            EmployeeEditViewModel employeeEditViewModel = new EmployeeEditViewModel
            {
                Id = employee.Id,
                Name = employee.Name,
                Email = employee.Email,
                Department = employee.Department,
                ExistingPhotopath = employee.PhotoPath

            };
            return View(employeeEditViewModel); 
        }


        [HttpPost]
        
        public IActionResult Edit(EmployeeEditViewModel model)
        {


            if (ModelState.IsValid)

            {

                Employee employee = _employeeRepository.GetEmployee(model.Id);
                employee.Name = model.Name;
                employee.Email = model.Email;
                employee.Department = model.Department;
                if (model.Photo != null)
                {   
                    if(model.ExistingPhotopath !=null)
                    {
                        string filepath= Path.Combine(hostingEnvironment.WebRootPath, 
                            "images", model.ExistingPhotopath);
                        System.IO.File.Delete(filepath);
                    }
                    employee.PhotoPath = ProcessUploadeFile(model);

                }

          
                _employeeRepository.Update(employee);
                return RedirectToAction("index");

            }
            return View();

        }

        private string ProcessUploadeFile(EmployeeCreateViewModel model)
        {
            string uniqueFileName = null;
            if (model.Photo != null)
            {
                string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                
                using(var filestream= new FileStream(filePath, FileMode.Create))
                {
                    model.Photo.CopyTo(filestream);
                }
                

            }

            return uniqueFileName;
        }

        [HttpPost]
        public IActionResult Create(EmployeeCreateViewModel model)
        {


            if (ModelState.IsValid)
            {
                string uniqueFileName = ProcessUploadeFile(model);


                Employee newEmployee = new Employee
                {
                    Name = model.Name,
                    Email = model.Email,
                    Department = model.Department,
                    PhotoPath = uniqueFileName
                };
                _employeeRepository.Add(newEmployee);
                return RedirectToAction("details", new { id = newEmployee.Id });

            }
            return View();

        }
    }
}

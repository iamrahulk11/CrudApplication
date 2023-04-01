using CrudApplicationTest.Data;
using CrudApplicationTest.Models;
using Microsoft.AspNetCore.Mvc;

namespace CrudApplicationTest.Controllers
{
    public class StudentController : Controller
    {
        private readonly ApplicationContext context;

        public StudentController(ApplicationContext context)
        {
            this.context = context;
        }
        public IActionResult Index(int id)
        {
            List<Student> result; 
            if (id!=0 && id != null)
            {
                result = context.Students.Where(s => s.StudentId == id).ToList();
                if(result.Count == 0)
                {
                    TempData["error"] = "No Student Found";
                }
            }
            else
            {
                result = context.Students.ToList();
            }
            return View(result);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create(Student model)
        {
            if(ModelState.IsValid)
            {
                var std = new Student()
                {
                    Name = model.Name,
                    Batch = model.Batch,
                    PhoneNo = model.PhoneNo,
                    State = model.State,
                    City = model.City,
                    Pincode = model.Pincode
                };
                
                context.Students.Add(std);
                context.SaveChanges();
                TempData["error"] = "Record Saved Successfully ";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["message"] = "Fill All The Fields";
                return View(model);
            }
        }
        public IActionResult Delete(int id)
        {
            var std = context.Students.SingleOrDefault(e => e.StudentId == id);
            var studentdata = new Student()
            {
                StudentId=std.StudentId,
                Name = std.Name,
                Batch = std.Batch,
                PhoneNo = std.PhoneNo,
                State = std.State,
                City = std.City,
                Pincode = std.Pincode

            };

            return View(studentdata);
        }
        [HttpPost]
        public IActionResult Delete(int id,Student model)
        {
            var std = context.Students.SingleOrDefault(e => e.StudentId == id);
            context.Students.Remove(std);
            context.SaveChanges();
            TempData["error"] = "Record Deleted ";
            return RedirectToAction("Index");
        }

        public IActionResult Edit(int id)
        {
            var std = context.Students.SingleOrDefault(e => e.StudentId == id);
            var studentdata = new Student()
            {
                Name = std.Name,
                Batch = std.Batch,
                PhoneNo= std.PhoneNo,
                State = std.State,
                City = std.City,
                Pincode = std.Pincode

            };

            return View(studentdata);
        }
        [HttpPost]
        public IActionResult Edit(int id,Student model)
        {
            var s = context.Students.SingleOrDefault(e => e.StudentId == id);

            if (s != null)
            {
               // StudentId = model.StudentId;
                s.Name = model.Name;
                s.Batch = model.Batch;
                s.PhoneNo = model.PhoneNo;
                s.State = model.State;
                s.City = model.City;
                s.Pincode = model.Pincode;

                
                context.SaveChanges();
                TempData["error"] = "Record Updated Successfully ";
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }
        
        public IActionResult SearchStudent(int id)
        {
            List<Student> result;
            if (id != 0 && id != null)
            {
                result = context.Students.Where(s => s.StudentId == id).ToList();
                if (result.Count == 0)
                {
                    TempData["error"] = "No Student Found";
                }
                
            }
            else
            {
                result = context.Students.Where(s => s.StudentId == id).ToList();
                
            }
           
            return View(result);
            
        }
    }
}

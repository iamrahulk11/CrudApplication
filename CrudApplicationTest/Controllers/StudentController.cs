using CrudApplicationTest.Data;
using CrudApplicationTest.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using System.Configuration;
using Microsoft.Extensions.Configuration;


namespace CrudApplicationTest.Controllers
{
    public class StudentController : Controller
    {
        public IConfiguration Configuration;
        private readonly ApplicationContext context;
        private readonly IHttpContextAccessor _httpContextAccessor;
       

        public StudentController(ApplicationContext context, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            Configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            this.context = context;
            Configuration = configuration;
        }
        public IActionResult Index(int id)
        {
            List<Student> result; 
            if (id!=0)
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
                Student std = new Student()
                {
                    Name = model.Name,
                    Batch = model.Batch,
                    PhoneNo = model.PhoneNo,
                    State = model.State,
                    City = model.City,
                    Pincode = model.Pincode
                };
               /* Random generator = new Random();
                String r = generator.Next(0, 1000000).ToString("D6");*/
                
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
            Student? std = context.Students.SingleOrDefault(e => e.StudentId == id);
            Student? studentdata = new Student()
            {
                StudentId = std.StudentId,
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
            Student? std = context.Students.SingleOrDefault(e => e.StudentId == id);
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
            
            List<Student> studentsList = new List<Student>();
            if (id!=0)
            {
                /*result = context.Students.Where(s => s.StudentId == id).ToList();*/
                string xmlp = Configuration.GetValue<string>("xmlPath");//Path of the xml script  

               DataSet ds = new DataSet();//Using dataset to read xml file  
               ds?.ReadXml(xmlp);
                if(ds == null)
                {
                    return RedirectToAction("SearchStudent");
                }
                for (int i = 0; i < ds?.Tables[0].Rows.Count - 1; i++)
                {
                    if (ds?.Tables[0]?.Rows[i]?.ItemArray[0]?.ToString() == id.ToString() )
                    {
                       
                        var std = new Student()
                        {
                            StudentId = Convert.ToInt32(ds.Tables[0].Rows[i].ItemArray[0]), //Convert row to int  
                            Name = ds?.Tables[0]?.Rows[i]?.ItemArray[1]?.ToString(),
                            Batch = ds?.Tables[0]?.Rows[i]?.ItemArray[2]?.ToString(),
                            PhoneNo = ds?.Tables[0]?.Rows[i]?.ItemArray[3]?.ToString(),
                            State = ds?.Tables[0]?.Rows[i]?.ItemArray[4]?.ToString(),
                            City = ds?.Tables[0]?.Rows[i]?.ItemArray[5]?.ToString(),
                            Pincode = Convert.ToInt32(ds?.Tables[0].Rows[i].ItemArray[6])
                        };

                        studentsList.Add(std);
                        break;

                    }
                }
                return View(studentsList);
            }
            else
            {
                studentsList.DefaultIfEmpty();
                return View(studentsList);
            }
        }
       
        public IActionResult XmlExport() {
            
                List<Student> stlist = context.Students.ToList();
            /* if (stlist.Count == 0 && stlist != null)
             {*/

                XDocument xdoc = new XDocument();

            XDeclaration doc = new XDeclaration("1.0", "Utf-0", null);
               
                xdoc.Add(new XElement("Student",
                    from std in stlist
                    select new XElement("Student_Data",
                    new XElement("StudentId", std.StudentId),
                    new XElement("Name", std.Name),
                    new XElement("Batch", std.Batch),
                    new XElement("PhoneNo", std.PhoneNo),
                    new XElement("State", std.State),
                    new XElement("City", std.City),
                    new XElement("Pincode", std.Pincode)
                    )));
           
                var sw = new StringWriter();
                xdoc.Save(sw);


            string filename = "C:\\Users\\Admin\\Desktop\\C#\\xmll.xml";
            XmlTextWriter xmlwriter = new XmlTextWriter(filename, System.Text.ASCIIEncoding.UTF8);

            xmlwriter.Formatting = Formatting.Indented;
            xmlwriter.WriteStartDocument();
            xmlwriter.WriteStartElement("StudentData");
            foreach(var item in stlist)
            {
                xmlwriter.WriteStartElement("StudentData");
                xmlwriter.WriteElementString("StudentId", item.StudentId.ToString());
                xmlwriter.WriteElementString("Name", item?.Name?.ToString());
                xmlwriter.WriteElementString("Batch", item?.Batch?.ToString());
                xmlwriter.WriteElementString("PhoneNo", item?.PhoneNo?.ToString());
                xmlwriter.WriteElementString("State", item?.State?.ToString());
                xmlwriter.WriteElementString("City", item?.City?.ToString());
                xmlwriter.WriteElementString("Pincode", item?.Pincode.ToString());
                xmlwriter.WriteEndElement();
            }
            xmlwriter.WriteEndElement();
            xmlwriter.WriteEndDocument();
            xmlwriter.Flush();
            xmlwriter.Close();
             return Content(sw.ToString(), "text/xml");

            }
            }
            
        }
    

using login.Models;

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;


namespace login.Controllers
{
    public class AddEmplyeeDetailsController : Controller
    {
        // GET: AddEmplyeeDetails
        public Employee_ProfileEntities1 db = new Employee_ProfileEntities1();
        public ActionResult Index()
        {
            List<AddEmployeeDetails> details = new List<AddEmployeeDetails>();

            details = db.Employee_details.Join(db.Client_Details,
                x => x.Emp_Id,
                y => y.Emp_id_fk,
                (x, y) => new
                {
                    x.Emp_Id,
                    x.Emp_Name,
                    x.Joining_Date,
                    x.Practice,
                    x.Dob,
                    x.Designation_Lancesoft,
                    x.Salary,

                    y.Client_Id,
                    y.Client_Name,
                    y.Desination_at_client,
                    y.Po_end_date,
                    y.Po_start_Date,
                    y.Billing


                })
                .Join(db.Bills, x => x.Emp_Id, z => z.Emp_Id_FK, (x, z) => new
                {
                    x.Emp_Id,
                    x.Emp_Name,
                    x.Joining_Date,
                    x.Practice,
                    x.Dob,
                    x.Designation_Lancesoft,
                    x.Client_Id,
                    x.Client_Name,
                    x.Desination_at_client,
                    x.Po_end_date,
                    x.Po_start_Date,
                    x.Salary,
                    x.Billing,
                    z.Cubical_cost,
                    z.Food_cost,
                    z.Transport_cost

                })
                .Select(p => new AddEmployeeDetails
                {
                    Client_Name = p.Client_Name,
                    Transport_cost = p.Transport_cost,
                    Food_cost = p.Food_cost,
                    Cubical_cost = p.Cubical_cost,
                    Designation_Lancesoft = p.Designation_Lancesoft,
                    Desination_at_client = p.Desination_at_client,
                    Dob = p.Dob,
                    Emp_Id = p.Emp_Id,
                    Emp_Name = p.Emp_Name,
                    Joining_Date = p.Joining_Date,
                    Po_end_date = p.Po_end_date,
                    Po_start_Date = p.Po_start_Date,
                    Practice = p.Practice,
                    Salary = p.Salary,
                    Billing = p.Billing

                }).ToList();

            //return View(db.Employee_details.ToList());
            return View(details);

        }
        public ActionResult Details(int id, AddEmployeeDetails addEmployeeDetails)
        {
            
            addEmployeeDetails = db.Employee_details.Join(db.Client_Details,
                x => x.Emp_Id,
                y => y.Emp_id_fk,
                (x, y) => new
                {
                    x.Emp_Id,
                    x.Emp_Name,
                    x.Joining_Date,
                    x.Practice,
                    x.Dob,
                    x.Designation_Lancesoft,
                    x.Salary,

                    y.Client_Id,
                    y.Client_Name,
                    y.Desination_at_client,
                    y.Po_end_date,
                    y.Po_start_Date,
                    y.Billing


                })
                .Join(db.Bills, x => x.Emp_Id, z => z.Emp_Id_FK, (x, z) => new
                {
                    x.Emp_Id,
                    x.Emp_Name,
                    x.Joining_Date,
                    x.Practice,
                    x.Dob,
                    x.Designation_Lancesoft,
                    x.Client_Id,
                    x.Client_Name,
                    x.Desination_at_client,
                    x.Po_end_date,
                    x.Po_start_Date,
                    x.Salary,
                    x.Billing,
                    z.Cubical_cost,
                    z.Food_cost,
                    z.Transport_cost

                })
                .Select(p => new AddEmployeeDetails
                {
                    Client_Name = p.Client_Name,
                    Transport_cost = p.Transport_cost,
                    Food_cost = p.Food_cost,
                    Cubical_cost = p.Cubical_cost,
                    Designation_Lancesoft = p.Designation_Lancesoft,
                    Desination_at_client = p.Desination_at_client,
                    Dob = p.Dob,
                    Emp_Id = p.Emp_Id,
                    Emp_Name = p.Emp_Name,
                    Joining_Date = p.Joining_Date,
                    Po_end_date = p.Po_end_date,
                    Po_start_Date = p.Po_start_Date,
                    Practice = p.Practice,
                    Salary = p.Salary,
                    Billing = p.Billing

                }).FirstOrDefault(x => x.Emp_Id == id);
            Employee_details employee_details = db.Employee_details.Where(x => x.Emp_Id == id).FirstOrDefault();
            Bill _empbills = employee_details.Bills.FirstOrDefault();
            Client_Details c = employee_details.Client_Details.Where(x => x.Emp_id_fk == id).FirstOrDefault();

            DateTime Jd = DateTime.Parse(employee_details.Joining_Date.Value.ToString());
            decimal Salary1 = employee_details.Salary.Value;

            int tenure = (DateTime.Parse(DateTime.Now.ToString()) - Jd).Days / 30;
            decimal paidtillnow = (Salary1 * tenure);

            DateTime p1 = (c != null ? c.Po_start_Date.Value : DateTime.Now);
            DateTime p2 = (c != null ? c.Po_end_date.Value : DateTime.Now);
            int Ctenure = ((p2.Year - p1.Year) * 12) + p2.Month - p1.Month;
            int btenure = tenure - Ctenure;
            decimal bench_exp = Salary1;
            decimal bench_expenes = 0;
            if (_empbills != null)
            {
                bench_expenes = btenure * (_empbills.Food_cost.Value + _empbills.Transport_cost.Value + _empbills.Cubical_cost.Value);
            }

            decimal Clientsal = Ctenure * (c != null ? c.Billing.Value : 0);
            if (employee_details.Salary != null && employee_details.Salary.Value > 0 && tenure > 0)
            {
                employee_details.PaidTillNow = paidtillnow;
            }
            decimal Profit_loss = Clientsal - (paidtillnow + bench_expenes);
            addEmployeeDetails.PaidTillNow = paidtillnow;
            addEmployeeDetails.Profit_OR_loss = Profit_loss;
            addEmployeeDetails.Tenure = tenure;
            addEmployeeDetails.Bench_Tenure = btenure;
            addEmployeeDetails.Bench_expences = bench_expenes;


            return View(addEmployeeDetails);

        }

        public ActionResult Create()
        {
            
            return View();
        }

       // POST: Employee_details/Create

       [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AddEmployeeDetails addEmployeeDetails)
        {



            if (ModelState.IsValid)
            {
                Employee_details obj = new Employee_details();
                
                obj.Emp_Name = addEmployeeDetails.Emp_Name;
                obj.Dob = addEmployeeDetails.Dob;
                obj.Joining_Date = addEmployeeDetails.Joining_Date;
                obj.Salary = addEmployeeDetails.Salary;
                obj.Practice = addEmployeeDetails.Practice;
                obj.Designation_Lancesoft = addEmployeeDetails.Designation_Lancesoft;
                db.Employee_details.Add(obj);
                db.SaveChanges();
                Client_Details obj1 = new Client_Details();
                obj1.Client_Name = addEmployeeDetails.Client_Name;
                obj1.Billing = addEmployeeDetails.Billing;
                obj1.Po_start_Date = addEmployeeDetails.Po_start_Date;
                obj1.Po_end_date = addEmployeeDetails.Po_end_date;
                obj1.Desination_at_client = addEmployeeDetails.Desination_at_client;
                obj1.Emp_id_fk = obj.Emp_Id;
                db.Client_Details.Add(obj1);
                db.SaveChanges();
                Bill obj2 = new Bill();
                
                obj2.Cubical_cost = addEmployeeDetails.Cubical_cost;
                obj2.Food_cost = addEmployeeDetails.Food_cost;
                obj2.Transport_cost = addEmployeeDetails.Transport_cost;
                obj2.Emp_Id_FK = obj.Emp_Id;
                db.Bills.Add(obj2);
                db.SaveChanges();

                return RedirectToAction("Index");

            }

            return View(addEmployeeDetails);

        }
        public ActionResult Edit(int id)
        {

            var data = GetEmplyeeDetails(id);



            return View(data);

        }

        public AddEmployeeDetails GetEmplyeeDetails(int id)
        {
            AddEmployeeDetails obg = new AddEmployeeDetails();
            obg = db.Employee_details.Join(db.Client_Details,
                x => x.Emp_Id,
                y => y.Emp_id_fk,
                (x, y) => new
                {
                    x.Emp_Id,
                    x.Emp_Name,
                    x.Joining_Date,
                    x.Practice,
                    x.Dob,
                    x.Designation_Lancesoft,
                    x.Salary,

                    y.Client_Id,
                    y.Client_Name,
                    y.Desination_at_client,
                    y.Po_end_date,
                    y.Po_start_Date,
                    y.Billing


                })
                .Join(db.Bills, x => x.Emp_Id, z => z.Emp_Id_FK, (x, z) => new
                {
                    x.Emp_Id,
                    x.Emp_Name,
                    x.Joining_Date,
                    x.Practice,
                    x.Dob,
                    x.Designation_Lancesoft,
                    x.Client_Id,
                    x.Client_Name,
                    x.Desination_at_client,
                    x.Po_end_date,
                    x.Po_start_Date,
                    x.Salary,
                    x.Billing,
                    z.Cubical_cost,
                    z.Food_cost,
                    z.Transport_cost

                })
                .Select(p => new AddEmployeeDetails
                {
                    Client_Name = p.Client_Name,
                    Transport_cost = p.Transport_cost,
                    Food_cost = p.Food_cost,
                    Cubical_cost = p.Cubical_cost,
                    Designation_Lancesoft = p.Designation_Lancesoft,
                    Desination_at_client = p.Desination_at_client,
                    Dob = p.Dob,
                    Emp_Id = p.Emp_Id,
                    Emp_Name = p.Emp_Name,
                    Joining_Date = p.Joining_Date,
                    Po_end_date = p.Po_end_date,
                    Po_start_Date = p.Po_start_Date,
                    Practice = p.Practice,
                    Salary = p.Salary,
                    Billing = p.Billing

                }).FirstOrDefault(x => x.Emp_Id == id);
            return obg;


        }
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Edit(AddEmployeeDetails addEmployeeDetails)
        {
            Employee_ProfileEntities1 db = new Employee_ProfileEntities1();

            var emp = db.Employee_details.Where(x => x.Emp_Id == addEmployeeDetails.Emp_Id).FirstOrDefault();

            if (emp != null)
            {
                emp.Emp_Name = addEmployeeDetails.Emp_Name;
                emp.Dob = addEmployeeDetails.Dob;
                emp.Joining_Date = addEmployeeDetails.Joining_Date;
                emp.Salary = addEmployeeDetails.Salary;
                emp.Practice = addEmployeeDetails.Practice;
                emp.Designation_Lancesoft = addEmployeeDetails.Designation_Lancesoft;

                db.Entry(emp).State = EntityState.Modified;
                db.SaveChanges();

            }
            var empclient = db.Client_Details.Where(x => x.Emp_id_fk == addEmployeeDetails.Emp_Id).FirstOrDefault();
            if (empclient != null)
            {
                empclient.Client_Name = addEmployeeDetails.Client_Name;
                empclient.Billing = addEmployeeDetails.Billing;
                empclient.Po_start_Date = addEmployeeDetails.Po_start_Date;
                empclient.Po_end_date = addEmployeeDetails.Po_end_date;
                empclient.Desination_at_client = addEmployeeDetails.Desination_at_client;
                db.Entry(empclient).State = EntityState.Modified;
                db.SaveChanges();
            }
            var empbill = db.Bills.Where(x => x.Emp_Id_FK == addEmployeeDetails.Emp_Id).FirstOrDefault();
            if (empbill != null)
            {
                empbill.Cubical_cost = addEmployeeDetails.Cubical_cost;
                empbill.Food_cost = addEmployeeDetails.Food_cost;
                empbill.Transport_cost = addEmployeeDetails.Transport_cost;
                db.Entry(empbill).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
           
            return View(addEmployeeDetails);
        }



        public ActionResult Delete(int? id, AddEmployeeDetails addEmployeedetails)
        {
            addEmployeedetails = db.Employee_details.Join(db.Client_Details,
                x => x.Emp_Id,
                y => y.Emp_id_fk,
                (x, y) => new
                {
                    x.Emp_Id,
                    x.Emp_Name,
                    x.Joining_Date,
                    x.Practice,
                    x.Dob,
                    x.Designation_Lancesoft,
                    x.Salary,

                    y.Client_Id,
                    y.Client_Name,
                    y.Desination_at_client,
                    y.Po_end_date,
                    y.Po_start_Date,
                    y.Billing


                })
                .Join(db.Bills, x => x.Emp_Id, z => z.Emp_Id_FK, (x, z) => new
                {
                    x.Emp_Id,
                    x.Emp_Name,
                    x.Joining_Date,
                    x.Practice,
                    x.Dob,
                    x.Designation_Lancesoft,
                    x.Client_Id,
                    x.Client_Name,
                    x.Desination_at_client,
                    x.Po_end_date,
                    x.Po_start_Date,
                    x.Salary,
                    x.Billing,
                    z.Cubical_cost,
                    z.Food_cost,
                    z.Transport_cost

                })
                .Select(p => new AddEmployeeDetails
                {
                    Client_Name = p.Client_Name,
                    Transport_cost = p.Transport_cost,
                    Food_cost = p.Food_cost,
                    Cubical_cost = p.Cubical_cost,
                    Designation_Lancesoft = p.Designation_Lancesoft,
                    Desination_at_client = p.Desination_at_client,
                    Dob = p.Dob,
                    Emp_Id = p.Emp_Id,
                    Emp_Name = p.Emp_Name,
                    Joining_Date = p.Joining_Date,
                    Po_end_date = p.Po_end_date,
                    Po_start_Date = p.Po_start_Date,
                    Practice = p.Practice,
                    Salary = p.Salary,
                    Billing = p.Billing

                }).FirstOrDefault(x => x.Emp_Id == id);



            return View(addEmployeedetails);


        }

        // POST: Employee_details/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete( int id)
        {
            AddEmployeeDetails addEmployeeDetails = new AddEmployeeDetails();

            
            var empclient = db.Client_Details.Where(x => x.Emp_id_fk == id).FirstOrDefault();
            if (empclient != null)
            {
                
                db.Client_Details.Remove(empclient); 
                db.SaveChanges();
            }
            var empbill = db.Bills.Where(x => x.Emp_Id_FK ==id).FirstOrDefault();
            if (empbill != null)
            {
                
                db.Bills.Remove(empbill);
                db.SaveChanges();
            }
            var emp = db.Employee_details.Where(x => x.Emp_Id == id).FirstOrDefault();

            if (emp != null)
            {

                db.Employee_details.Remove(emp);
                db.SaveChanges();

            }
            return RedirectToAction("Index");
        }
    }
}

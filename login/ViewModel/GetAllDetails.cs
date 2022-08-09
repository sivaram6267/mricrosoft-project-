using login.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace login.ViewModel
{
    public class GetAllDetails
    {
       public List<Employee_details> employe { get; set; }
        public List<Client_Details> clientDetails { get; set; }
        public List<Bill> billdetails { get; set; }
    }
}
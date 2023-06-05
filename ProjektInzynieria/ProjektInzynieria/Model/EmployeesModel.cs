using ProjektInzynieria.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ProjektInzynieria.Model
{
    public class EmployeesModel:Employees
    {
        public EmployeesModel(Employees Copyemployees)
        {
            employeeID = Copyemployees.employeeID;
            EFirstName = Copyemployees.EFirstName;
            ELastName = Copyemployees.ELastName;
            Phone = Copyemployees.Phone;
            Mail = Copyemployees.Mail;
            Passsword = Copyemployees.Passsword;
            IsAdmin = Copyemployees.IsAdmin;
        }
    }
}

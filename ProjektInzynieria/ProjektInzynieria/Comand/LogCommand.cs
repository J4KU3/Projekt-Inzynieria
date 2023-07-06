using ProjektInzynieria.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ProjektInzynieria.Comand
{
  public class LogCommand:BaseCommand
    {
        private readonly MainViewModel _mainViewModel;

      public LogCommand(MainViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;
        }

        public override void Execute(object parameter)
        {
            Login();
        }

        public override bool CanExecute(object parameter)
        {
            bool isEmployeeExists = CheckEmployeeExists(_mainViewModel.Employee.Mail, _mainViewModel.Employee.Passsword);
            if (isEmployeeExists)
            {
                return true;
            }
            return true;
        }


        private void Login()
        {
            bool IsAdmin = true;
            bool isEmployeeExists = CheckEmployeeExists(_mainViewModel.Employee.Mail, _mainViewModel.Employee.Passsword);
            bool haveAnAdminFunction = CheckAdmin(_mainViewModel.Employee.Mail, _mainViewModel.Employee.Passsword, IsAdmin);
            _mainViewModel.GetEmployeesFromDatabase();
            _mainViewModel.GetOrders();

            if (isEmployeeExists && !haveAnAdminFunction)
            {
                // Przejście do innej zakładki
                _mainViewModel.SelectedTabIndex = "1";
                MessageBox.Show("Panel Zwykly");
                _mainViewModel.IsLogin = true;
            }
            if (isEmployeeExists && haveAnAdminFunction)
            {
                MessageBox.Show("Panel Zwykly");
                _mainViewModel.SelectedTabIndex = "2";
                _mainViewModel.IsLogin = true;
            }
            if(!isEmployeeExists)
            {

                // Wyczyść pola logowania
                MessageBox.Show("Nie zalogowano");
                _mainViewModel.Employee.Mail = null;
                _mainViewModel.Employee.Passsword = null;
                _mainViewModel.IsLogin = false;

            }
        }

        private bool CheckEmployeeExists(string username, string password)
        {
            using (var context = new Data.ZarzadzanieFirmaDBEntities())
            {
                var employee = context.Employees.FirstOrDefault(e => e.Mail == username && e.Passsword == password);
                return employee != null;
            }

        }
        private bool CheckAdmin(string username, string password, bool isAdmin)
        {
            using (var context = new Data.ZarzadzanieFirmaDBEntities())
            {
                var employee = context.Employees.FirstOrDefault(e => e.Mail == username && e.Passsword == password && e.IsAdmin == isAdmin);
                return employee != null;
            }
        }


    }
}

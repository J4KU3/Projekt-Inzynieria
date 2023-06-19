
using GalaSoft.MvvmLight.Command;
using ProjektInzynieria.Comand;
using ProjektInzynieria.Data;
using ProjektInzynieria.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;


namespace ProjektInzynieria.ViewModel
{
    public class MainViewModel : BaseViewModel
    {


        private EmployeesModel _employee;
        private string _selectedTabIndex;
        private bool IsLogin =false;
        public EmployeesModel Employee
        {
            get { return _employee; }
            set
            {
                _employee = value;
                OnPropertyChanged(nameof(Employee));
            }
        }

        public string SelectedTabIndex
        {
            get { return _selectedTabIndex; }
            set
            {
                _selectedTabIndex = value;
                OnPropertyChanged(nameof(SelectedTabIndex));
            }
        }

        public ICommand ChangeTabCommand { get; }
        public ICommand LoginCommand { get; }
        public ICommand LogoutCommand { get; }

        //konstruktor
        public MainViewModel()
        {
            Employee = new EmployeesModel(new Employees());
            ChangeTabCommand = new RelayCommand<string>(ChangeTab);
            LoginCommand = new RelayCommand(Login);
            LogoutCommand = new RelayCommand(Logout);
        }
        //

        //zmiana zakładki 
        private void ChangeTab(string TabIndex)
        {

            SelectedTabIndex = TabIndex;
            
        }

        #region logowanie
        private void Login()
        {
            bool IsAdmin=true;
            bool isEmployeeExists = CheckEmployeeExists(Employee.Mail, Employee.Passsword);
            bool haveAnAdminFunction = CheckAdmin(Employee.Mail, Employee.Passsword, IsAdmin);

            if (isEmployeeExists && !haveAnAdminFunction)
            {
                // Przejście do innej zakładki
                SelectedTabIndex = "1";
                IsLogin = true;
            }
            if (isEmployeeExists && haveAnAdminFunction)
            {
                SelectedTabIndex = "2";
                IsLogin = true;
            }
            else
            {

                // Wyczyść pola logowania
                Employee.Mail = string.Empty;
                Employee.Passsword = string.Empty;
                IsLogin = false;
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
                var employee = context.Employees.FirstOrDefault(e => e.Mail == username && e.Passsword == password && e.IsAdmin==isAdmin);
                return employee != null;
            }
        }
        #endregion

        #region LogOUt
       

        private void Logout()
        {
            // Wykonaj logikę wylogowania, np. czyść dane logowania
            Employee.Mail = string.Empty;
            Employee.Passsword = string.Empty;
            IsLogin = false;

            // Przejście do ekranu logowania
            SelectedTabIndex = "0";
        }

        #endregion

    }
}


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
using System.Windows;
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
        public ICommand CloseProgram { get; }

        //konstruktor
        public MainViewModel()
        {
            Employee = new EmployeesModel(new Employees());
            ChangeTabCommand = new RelayCommand<string>(ChangeTab);
            LoginCommand = new RelayCommand(Login);
            LogoutCommand = new RelayCommand(Logout);
            CloseProgram = new RelayCommand(Cl);
            GetEmployeesFromDatabase();
        }
        //

        //zmiana zakładki 
        private void ChangeTab(string TabIndex)
        {

            SelectedTabIndex = TabIndex;
            
        }

        #region PanelLogowania


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
            
            Employee.Mail = string.Empty;
            Employee.Passsword = string.Empty;
            IsLogin = false;

          
            SelectedTabIndex = "0";
        }

        #endregion
        private void Cl()
        {

            MainWindow mainWindow = new MainWindow();
            mainWindow.Close();
            Application.Current.Shutdown();


        }

        #endregion

        #region Panel PRacownicy

        private ObservableCollection<EmployeesModel> _listofEmployees;

        public ObservableCollection<EmployeesModel> ListofEmployees
        {
            get { return _listofEmployees; }
            set
            {
                _listofEmployees = value;
                OnPropertyChanged(nameof(ListofEmployees));
            }
        }

        private void GetEmployeesFromDatabase()
        {
            using (var context = new ZarzadzanieFirmaDBEntities())
            {
                List<Employees> employeesList = context.Employees.ToList();

                // Dokonaj jawnej konwersji na listę obiektów EmployeesModel
                List<EmployeesModel> convertedList = employeesList.Select(e => new EmployeesModel(e)).ToList();

                ListofEmployees = new ObservableCollection<EmployeesModel>(convertedList);
            }
        }

        #endregion
    }
}

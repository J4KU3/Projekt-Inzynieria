
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
        public bool IsLogin =false;
        public EmployeesModel Employee
        {
            get { return _employee; }
            set
            {
                _employee = value;
                OnPropertyChanged(nameof(Employee));
            }
        }

        #region ListOFModel

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

        private ObservableCollection<ClientModel> _listofClient;

        public ObservableCollection<ClientModel> ListofClient
        {
            get { return _listofClient; }
            set
            {
                _listofClient = value;
                OnPropertyChanged(nameof(ListofClient));
            }
        }

        private ObservableCollection<ComplaintsModel> _listofComplaints;

        public ObservableCollection<ComplaintsModel> ListofComplaints
        {
            get { return _listofComplaints; }
            set
            {
                _listofComplaints = value;
                OnPropertyChanged(nameof(ListofComplaints));
            }
        }

        private ObservableCollection<OrderModel> _listofOrders;

        public ObservableCollection<OrderModel> ListofOrders
        {
            get { return _listofOrders; }
            set
            {
                _listofOrders = value;
                OnPropertyChanged(nameof(ListofOrders));
            }
        }
#endregion
        public string SelectedTabIndex
        {
            get { return _selectedTabIndex; }
            set
            {
                _selectedTabIndex = value;
                LoCommand.OnCanExecuteChanged();
                OnPropertyChanged(nameof(SelectedTabIndex));
            }
        }

        public ICommand ChangeTabCommand { get; }
       
        public ICommand LogoutCommand { get; }
        public ICommand CloseProgram { get; }
        public ICommand AddUserCommand{ get; }
        public ICommand DeleteUserCommand { get; }
        public ICommand EditUserCommand { get; }

        public LogCommand LoCommand { get; }

        //konstruktor
        public MainViewModel()
        {
            Employee = new EmployeesModel(new Employees());
            ChangeTabCommand = new RelayCommand<string>(ChangeTab);
         
            LogoutCommand = new RelayCommand(Logout);
            CloseProgram = new RelayCommand(Cl);
            AddUserCommand = new RelayCommand(AddEmployee);
            DeleteUserCommand = new RelayCommand(DeleteEmployee);
            EditUserCommand = new RelayCommand(EditEmployee);
            LoCommand = new LogCommand(this);
           
        }
        //

        //zmiana zakładki 
        private void ChangeTab(string TabIndex)
        {

            SelectedTabIndex = TabIndex;
            bool IsAdmin = true;
            bool haveAnAdminFunction = CheckAdmin(Employee.Mail, Employee.Passsword, IsAdmin);
            if (!haveAnAdminFunction)
            {
                if (SelectedTabIndex=="3" || SelectedTabIndex=="5" || SelectedTabIndex=="8")
                {
                    SelectedTabIndex = "1";
                    MessageBox.Show("nie masz dostepu do funkcji admina");
                    
                }


            }
            
        }

        #region PanelLogowania


        #region logowanie
        

       
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
            
            Employee.Mail = null;
            Employee.Passsword = null;
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

        

        private EmployeesModel _selectedUser;

        public EmployeesModel SelectedUser
        {
            get { return _selectedUser; }
            set
            {
                _selectedUser = value;
                OnPropertyChanged(nameof(SelectedUser));
            }
        }


        public void GetEmployeesFromDatabase()
        {
            using (var context = new ZarzadzanieFirmaDBEntities())
            {
                List<Employees> employeesList = context.Employees.ToList();

                // Dokonaj jawnej konwersji na listę obiektów EmployeesModel
                List<EmployeesModel> convertedList = employeesList.Select(e => new EmployeesModel(e)).ToList();

                ListofEmployees = new ObservableCollection<EmployeesModel>(convertedList);
            }
        }

        public void AddEmployee()
        {
            using (var context = new ZarzadzanieFirmaDBEntities())
            {
                // Utwórz nowy obiekt pracownika na podstawie wprowadzonych danych
                var newEmployee = new Employees
                {
                    employeeID = Employee.employeeID,
                    EFirstName = Employee.EFirstName,
                    ELastName = Employee.ELastName,
                    Phone = Employee.Phone,
                    Mail = Employee.Mail,
                    Passsword = Employee.Passsword,
                    IsAdmin = Employee.IsAdmin
                };

                // Dodaj nowego pracownika do kontekstu bazy danych
                context.Employees.Add(newEmployee);
                context.SaveChanges();

                // Wyczyść wprowadzone dane
                Employee.EFirstName = string.Empty;
                Employee.ELastName = string.Empty;
                Employee.Phone = string.Empty;
                Employee.Mail = string.Empty;
                Employee.Passsword = string.Empty;
                Employee.IsAdmin = false;

                // Odśwież listę pracowników z bazy danych i cofnij na zarzadzanie pracownikami
                SelectedTabIndex = "3";
                GetEmployeesFromDatabase();
                
            }

        }
       

        public void DeleteEmployee()
        {
            if (SelectedUser != null)
            {
                using (var context = new ZarzadzanieFirmaDBEntities())
                {
                    var employeeToDelete = context.Employees.FirstOrDefault(e => e.employeeID == SelectedUser.employeeID);
                    if (employeeToDelete != null)
                    {
                        context.Employees.Remove(employeeToDelete);
                        context.SaveChanges();
                        GetEmployeesFromDatabase(); // Aktualizuj listę pracowników
                        SelectedUser = null; // Czyść wybranego pracownika
                    }
                }
            }
        }

        public void EditEmployee()
        {
            if (SelectedUser != null)
            {
                using (var context = new ZarzadzanieFirmaDBEntities())
                {
                    var employeeToEdit = context.Employees.FirstOrDefault(e => e.employeeID == SelectedUser.employeeID);
                    if (employeeToEdit != null)
                    {
                        employeeToEdit.employeeID = SelectedUser.employeeID;
                        employeeToEdit.EFirstName = SelectedUser.EFirstName;
                        employeeToEdit.ELastName = SelectedUser.ELastName;
                        employeeToEdit.Mail = SelectedUser.Mail;
                        employeeToEdit.Phone = SelectedUser.Phone;
                        employeeToEdit.Passsword = SelectedUser.Passsword;
                        employeeToEdit.IsAdmin = SelectedUser.IsAdmin;


                        context.SaveChanges();
                        GetEmployeesFromDatabase(); // Aktualizuj listę pracowników
                        SelectedUser = null; // Czyść wybranego pracownika
                    }
                }
            }
        }


        #endregion

        #region panel Zamowienia 

        public void GetOrders()
        {
            using (var context = new ZarzadzanieFirmaDBEntities())
            {
                List<Orders> orderList = context.Orders.ToList();

                // Dokonaj jawnej konwersji na listę obiektów EmployeesModel
                List<OrderModel> convertedList = orderList.Select(e => new OrderModel(e)).ToList();

                ListofOrders = new ObservableCollection<OrderModel>(convertedList);
            }
        }
        #endregion


        #region panel Reklamacje

        private void GetComplaints()
        {
            using (var context = new ZarzadzanieFirmaDBEntities())
            {
                List<Complaints> ComplaintsList = context.Complaints.ToList();

                // Dokonaj jawnej konwersji na listę obiektów EmployeesModel
                List<ComplaintsModel> convertedList = ComplaintsList.Select(e => new ComplaintsModel(e)).ToList();

                ListofComplaints = new ObservableCollection<ComplaintsModel>(convertedList);
            }
        }
        #endregion
    }
}

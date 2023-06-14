
using GalaSoft.MvvmLight.Command;
using ProjektInzynieria.Comand;
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

        private ObservableCollection<ClientModel> _listOfClients = new ObservableCollection<ClientModel>();
        private ObservableCollection<ComplaintsModel> _listOfComplaints = new ObservableCollection<ComplaintsModel>();
        private ObservableCollection<EmployeesModel> _listOfEmployees = new ObservableCollection<EmployeesModel>();
        private ObservableCollection<OrderModel> _listOfOrders = new ObservableCollection<OrderModel>();
        private string _username;
       
        private string _password;
        
        private string _selectedTabIndex;

        public ObservableCollection<ClientModel> ListOfClients
        {
            get { return _listOfClients; }
            set { _listOfClients = value; }
        }
        public ObservableCollection<ComplaintsModel> ListOfComplaints
        {
            get { return _listOfComplaints; }
            set { _listOfComplaints = value; }
        }
        public ObservableCollection<EmployeesModel> ListOfEmployees
        {
            get { return _listOfEmployees; }
            set { _listOfEmployees = value; }
        }
        public ObservableCollection<OrderModel> ListOfOrders
        {
            get { return _listOfOrders; }
            set { _listOfOrders = value; }
        }

        public string Username
        {
            get { return _username; }
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
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

        //konstruktor
        public MainViewModel()
        {
            ChangeTabCommand = new RelayCommand<string>(ChangeTab);
            LoginCommand = new RelayCommand(Login);
        }
        //

        //zmiana zakładki 
        private void ChangeTab(string TabIndex)
        {

            SelectedTabIndex = TabIndex;
            
        }


        private void Login()
        {
            bool isEmployeeExists = CheckEmployeeExists(Username, Password);

            if (isEmployeeExists)
            {
                // Przejście do innej zakładki
                SelectedTabIndex = "1"; // Indeks zakładki do przejścia
            }
            else
            {
               
                // Wyczyść pola logowania
                Username = string.Empty;
                Password = string.Empty;
            }
        }

        private bool CheckEmployeeExists(string username, string password)
        {
            using (var context = new Data.ZarzadzanieFirmaDBEntities()) // Zastąp YourDbContext nazwą swojego kontekstu bazy danych
            {
                var employee = context.Employees.FirstOrDefault(e => e.Mail == username && e.Passsword == password);
                return employee != null;
            }
        }


    }
}

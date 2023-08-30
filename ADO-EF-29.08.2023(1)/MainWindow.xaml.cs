using ADO_EF_29._08._2023_1_.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ADO_EF_29._08._2023_1_
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DataContext dataContext;
        public MainWindow()
        {
            InitializeComponent();
            dataContext = new();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            DepartmentsCountLabel.Content = dataContext.Departments.Count().ToString();
            ManagersCountLabel.Content = dataContext.Managers.Count().ToString();
            TopChiefsCountLabel.Content = dataContext.Managers.Where(manager => manager.IdChief == null).Count().ToString();
            SubordinatesCountLabel.Content = dataContext.Managers.Where(manager => manager.IdChief != null).Count().ToString();
            Guid itGuid = Guid.Parse(dataContext.Departments.Where(department => department.Name == "IT відділ").Select(department => department.Id).First().ToString());
            ItDepartmentCountLabel.Content = dataContext.Managers.Where(manager => manager.IdMainDep == itGuid || manager.IdSecDep == itGuid).Count().ToString();
            EmpTwoDepCountLabel.Content = dataContext.Managers.Where(manager => manager.IdMainDep != null && manager.IdSecDep != null).Count().ToString();
        }
    }
}




﻿using ADO_EF_29._08._2023_1_.Data;
using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using static ADO_EF_29._08._2023_1_.MainWindow;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using ADO_EF_29._08._2023_1_.Data.Entity;
using System.ComponentModel;
using ADO_EF_29._08._2023_1_.Migrations;

namespace ADO_EF_29._08._2023_1_
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DataContext dataContext;
        public ObservableCollection<Pair> Pairs { get; set; }
        public ObservableCollection<Data.Entity.Department>? DepartmentsView { get; set; }
        private ICollectionView departmentsListView;

        public MainWindow()
        {
            InitializeComponent();
            dataContext = new();
            Pairs = new();
            this.DataContext = this;
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

            dataContext.Departments.Load();
            DepartmentsView = dataContext.Departments.Local.ToObservableCollection();
            departmentsList.ItemsSource = DepartmentsView;
            // departmentsList.ItemsSource = dataContext.Departments.Local.ToObservableCollection();
            departmentsListView = CollectionViewSource.GetDefaultView(DepartmentsView);
            departmentsListView.Filter = item => (item as Data.Entity.Department)?.DeleteDt == null;
        }

        private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (sender is ListViewItem item)
            {
                if (item.Content is Data.Entity.Department department)
                {
                    // MessageBox.Show(department.Name);
                    CrudDepartmentWindow dialog = new()
                    {
                        Department = department
                    };
                    if (dialog.ShowDialog() ?? false)
                    {
                        if(dialog.Department != null)
                        {
                            if(dialog.IsDelete)
                            {
                                department.DeleteDt = DateTime.Now;
                                dataContext.SaveChanges();
                                departmentsListView.Refresh();
                            }
                            else
                            {
                                var dep = dataContext.Departments.Find(department.Id);
                                if (dep != null)
                                {
                                    dep.Name = department.Name;
                                }
                                dataContext.SaveChanges();
                                // dataContext.Departments.Local.Reset();

                                departmentsListView.Refresh();

                                /*dataContext.Departments.Load();
                                departmentsList.Items.Clear();

                                // departmentsListView.Filter = item => true;
                                departmentsList.ItemsSource = dataContext.Departments.Local.ToObservableCollection();
                                departmentsListView.Refresh();*/


                                /*int index = DepartmentsView.IndexOf(department);
                                DepartmentsView.Remove(department);
                                DepartmentsView.Insert(index, department);*/
                            }
                            
                        }
                        else
                        {
                            dataContext.Departments.Remove(department);
                            dataContext.SaveChanges();
                        }
                    }
                    
                }
            }
        }

        private void AdddepartmentButton_Click(object sender, RoutedEventArgs e)
        {
            Data.Entity.Department newDepartment = new()
            {
                Id = Guid.NewGuid(),
                Name = null!
            };

            CrudDepartmentWindow dialog = new()
            {
                Department = newDepartment
            };
            if (dialog.ShowDialog() ?? false)
            {
                dataContext.Departments.Add(newDepartment);
                dataContext.SaveChanges();
            }
          
        }


        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            var quary = dataContext.Managers.Where(m => m.IdMainDep == Guid.Parse("131ef84b-f06e-494b-848f-bb4bc0604266"))
                .Select(m =>
                    new Pair
                    {
                        Key = m.Surname,
                        Value = $"{m.Name[0]}. {m.Secname[0]}."
                    });
            Pairs.Clear();
            foreach (var pair in quary)
            {
                Pairs.Add(pair);
            }
            // Pairs.Add(new() { Key = "1", Value = "11" });
        }

        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            var quary = dataContext
                .Managers
                .Join(
                    dataContext.Departments,
                    m => m.IdMainDep,
                    d => d.Id,
                    (m, d) =>
                     new Pair
                     {
                         Key = $"{m.Surname} {m.Name[0]}. {m.Secname[0]}.",
                         Value = d.Name
                     }
                )
                .Skip(3)
                .Take(10);
            Pairs.Clear();
            foreach (var pair in quary)
            {
                Pairs.Add(pair);
            }
        }
        private void Button3_Click(object sender, RoutedEventArgs e)
        {
            var quary = dataContext
                .Managers
                .Join(
                    dataContext.Managers,
                    m => m.IdChief,
                    chief => chief.Id,
                    (m, chief) => new Pair
                    {
                        Key = $"{m.Surname} {m.Name[0]}. {m.Secname[0]}.",
                        Value = chief.Surname
                    }
                )
                // .OrderBy(pair => pair.Key)
                .ToList()
                .OrderBy(pair => pair.Key);

            Pairs.Clear();
            foreach (var pair in quary)
            {
                Pairs.Add(pair);
            }
        }
        private void Button4_Click(object sender, RoutedEventArgs e)
        {
            var quary = dataContext.Managers
                .OrderByDescending(m => m.CreateDt)
                .Select(m => new Pair { Key = $"{m.CreateDt}", Value = $"{m.Surname} {m.Name[0]}.{m.Secname[0]}." })
                .Take(7);
            Pairs.Clear();
            foreach (var pair in quary)
            {
                Pairs.Add(pair);
            }
        }

        private void Button5_Click(object sender, RoutedEventArgs e)
        {
            var quary = dataContext.Managers.Join(
                    dataContext.Departments,
                    m => m.IdSecDep,
                    d => d.Id,
                    (m, d) => new Pair() { Key = $"{m.Surname} {m.Name[0]}.{m.Secname[0]}.", Value = d.Name }
                ).OrderBy(pair => pair.Value);
            Pairs.Clear();
            foreach (var pair in quary)
            {
                Pairs.Add(pair);
            }
        }


        private int _N;
        public int N { get => _N++; set => _N = value; }
        private void Button6_Click(object sender, RoutedEventArgs e)
        {
            N = 1;
            var quary = dataContext
                .Departments
                .Select(d => new Pair()
                {
                    Key = (N).ToString(),
                    Value = d.Name
                });

            Pairs.Clear();
            foreach (var pair in quary)
            {
                Pairs.Add(pair);
            }
        }

        private void Button7_Click(object sender, RoutedEventArgs e)
        {
            N = 1;
            var quary = dataContext
                .Departments
                .OrderBy(d => d.Name)
                .AsEnumerable()
                .Select(d => new Pair()
                {
                    Key = (N).ToString(),
                    Value = d.Name
                });


            Pairs.Clear();
            foreach (var pair in quary)
            {
                Pairs.Add(pair);
            }
        }

        private void Button8_Click(object sender, RoutedEventArgs e)
        {
            var quary = dataContext
                .Departments
                .GroupJoin(
                    dataContext.Managers,
                    d => d.Id,
                    m => m.IdMainDep,
                    (dep, mans) => new Pair
                    {
                        Key = dep.Name,
                        Value = mans.Count().ToString()
                    });
            Pairs.Clear();
            foreach (var pair in quary)
            {
                Pairs.Add(pair);
            }
        }

        private void Button9_Click(object sender, RoutedEventArgs e)
        {
            var quary = dataContext.Managers
                .GroupJoin(
                dataContext.Managers,
                chef => chef.Id,
                sub => sub.IdChief,
                (chef, subs) => new Pair
                {
                    Key = $"{chef.Surname} {chef.Name[0]}. {chef.Secname[0]}.",
                    Value = subs.Count().ToString()
                })
                .Where(pair => !string.IsNullOrEmpty(pair.Value) && pair.Value != "0");


            Pairs.Clear();
            foreach (var pair in quary)
            {
                Pairs.Add(pair);
            }
        }

        private void Button10_Click(object sender, RoutedEventArgs e)
        {
            var quary = dataContext.Managers
                .GroupBy(m => m.Surname)
                .Select(group => new Pair
                {
                    Key = group.Key,
                    Value = group.Count().ToString()
                })
                .Where(p => Convert.ToInt32(p.Value) > 1);

            Pairs.Clear();
            foreach (var pair in quary)
            {
                Pairs.Add(pair);
            }
        }

        private void Button11_Click(object sender, RoutedEventArgs e)
        {
            var quary = dataContext.Departments.GroupJoin(
                    dataContext.Managers,
                    d => d.Id,
                    m => m.IdSecDep,
                    (d, m) => new Pair { Key = d.Name, Value = m.Count().ToString() }
                )
                .OrderByDescending(pair => Convert.ToInt32(pair.Value));
            Pairs.Clear();
            foreach (var pair in quary)
            {
                Pairs.Add(pair);
            }
        }

        private void Button12_Click(object sender, RoutedEventArgs e)
        {
            N = 1;
            var quary = dataContext.Managers
                .GroupBy(m => m.Surname)
                .AsEnumerable()
                .Where(m => m.Count() > 1)
                .Select(group => new Pair { Key = N.ToString(), Value = group.Key });
            Pairs.Clear();
            foreach (var pair in quary)
            {
                Pairs.Add(pair);
            }
        }

        private void Button13_Click(object sender, RoutedEventArgs e)
        {
            var quary = dataContext.Managers.GroupJoin(
                    dataContext.Managers,
                    chief => chief.Id,
                    m => m.IdChief,
                    (chief, m) => new Pair { Key = m.Count().ToString(), Value = $"{chief.Surname} {chief.Name[0]}.{chief.Secname[0]}." }
                )
                .OrderByDescending(pair => Convert.ToInt32(pair.Key))
                .Take(5);
            Pairs.Clear();
            foreach (var pair in quary)
            {
                Pairs.Add(pair);
            }
        }

        private void ButtonNav1_Click(object sender, RoutedEventArgs e)
        {
            var quary = dataContext
                .Managers
                .Include(m => m.MainDep)
                .Select(m => new Pair
                {
                    Key = $"{m.Surname} {m.Name[0]}. {m.Secname[0]}.",
                    Value = m.MainDep.Name
                });

            Pairs.Clear();
            foreach (var pair in quary)
            {
                Pairs.Add(pair);
            }
        }

        private void ButtonNav2_Click(object sender, RoutedEventArgs e)
        {
            var quary = dataContext
                .Managers
                .Include(m => m.SecDep)
                .Select(m => new Pair
                {
                    Key = $"{m.Surname} {m.Name[0]}. {m.Secname[0]}.",
                    Value = m.SecDep.Name == null ? "--" : m.SecDep.Name
                });

            Pairs.Clear();
            foreach (var pair in quary)
            {
                Pairs.Add(pair);
            }
        }

        private void ButtonNav3_Click(object sender, RoutedEventArgs e)
        {

            var quary = dataContext
                .Departments
                .Include(d => d.MainManagers)
                .Where(d => d.DeleteDt == null)
                .Select(d => new Pair
                {
                    Key = d.Name,
                    Value = (d.MainManagers.Count() != 0) ? d.MainManagers.Count().ToString() : "Закритий"
                });

            Pairs.Clear();
            foreach (var pair in quary)
            {
                Pairs.Add(pair);
            }
        }

        private void ButtonNav4_Click(object sender, RoutedEventArgs e)
        {
            var quary = dataContext
                            .Departments
                            .Include(d => d.SecManagers)
                            .Where(d => d.DeleteDt == null)
                            .Select(d => new Pair
                            {
                                Key = d.Name,
                                Value = (d.SecManagers.Count() != 0) ? d.SecManagers.Count().ToString() : "Закритий"
                            });

            Pairs.Clear();
            foreach (var pair in quary)
            {
                Pairs.Add(pair);
            }
        }

        private void ButtonNav5_Click(object sender, RoutedEventArgs e)
        {

            var quary = dataContext.Managers
                .Include(m => m.SubManagers)
                .Where(m => m.SubManagers.Count() > 0)
                .Select(m => new Pair
                {
                    Key = m.Surname,
                    Value = m.SubManagers.Count().ToString()
                });
            foreach (var pair in quary)
            {
                Pairs.Add(pair);
            }
        }
    }
    public class Pair
    {
        public String Key { get; set; } = null!;
        public String? Value { get; set; }
    }
}




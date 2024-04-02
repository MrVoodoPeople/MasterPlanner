using System;
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
using System.Windows.Shapes;
using MasterPlanner.Controller;
using MasterPlanner.Model;

namespace MasterPlanner.View
{
    /// <summary>
    /// Логика взаимодействия для Conflict.xaml
    /// </summary>
    public partial class Conflict : Window
    {
        private NotesController controller;
        public Conflict(ObservableCollection<PlannerNote> ConflictItems)
        {
            InitializeComponent();
            controller = new NotesController(this.Dispatcher);
            listView.ItemsSource = controller.CurrentPageItems;
            controller.Items = ConflictItems;
            controller.CalculateTotalPages();
            controller.UpdateCurrentPageItems();
            CurPage.Text = controller.CurrentPage.ToString();
            TotalPage.Text = controller.TotalPages.ToString();
        }

        private void Button_Force_Add_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void Button_Exit_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void GoToNextPage_Click(object sender, RoutedEventArgs e)
        {
            controller.GoToNextPage();
        }

        private void GoToPreviousPage_Click(object sender, RoutedEventArgs e)
        {
            controller.GoToPreviousPage();
        }
    }
}

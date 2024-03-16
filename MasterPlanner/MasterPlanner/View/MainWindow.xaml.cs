using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MasterPlanner.Controller;
using MasterPlanner.Model;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Microsoft.EntityFrameworkCore;

namespace MasterPlanner.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new TestC();    
            dateLabel.Content = DateTime.Now.ToString("dd/MM/yyyy") ;

        }

        private void calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime? selectedDate = calendar1.SelectedDate;
            dateLabel.Content = selectedDate?.ToShortDateString();
        }

        private void Button_Add_Click(object sender, RoutedEventArgs e)
        {
            var context = new TestDbContext();
            var test = DateTime.Parse(dateLabel.Content.ToString());
            TestModel model = new TestModel()
            {
                Date = test.ToUniversalTime(),
                Notes = "TEST"
            };

            context.Notes.Add(model);
            context.SaveChanges();
        }

        private void Button_Delete_Click(object sender, RoutedEventArgs e)
{
  
    var selectedOrder = listView.SelectedItem as TestModel; 

    if (selectedOrder != null) 
    {
        using (var context = new TestDbContext()) 
        {
            context.Entry(selectedOrder).State = EntityState.Deleted; 
            context.SaveChanges(); 
        }

                
                listView.Items.Refresh();
    }
    else
    {
        MessageBox.Show("Выберите элемент для удаления.");
    }
}

     /*   private void Button_Update_Click(object sender, RoutedEventArgs e)
        {
            var context = new TestDbContext();
            TestModel order = new TestModel
            {
                Id = 2 
            };
            context.Notes.Attach(order);
            context.Notes.Remove(order);

            context.SaveChanges();
        }*/
    }
}
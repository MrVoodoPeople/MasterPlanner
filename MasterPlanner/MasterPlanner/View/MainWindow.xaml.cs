using MasterPlanner.Controller;
using MasterPlanner.Model;
using Microsoft.EntityFrameworkCore;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace MasterPlanner.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TestC controller;
        public MainWindow()
        {
            InitializeComponent();
            controller = new TestC();
            this.DataContext = controller;
            dateLabel.Content = DateTime.Now.ToString("dd/MM/yyyy");

        }

        private void calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime? selectedDate = calendar1.SelectedDate;
            dateLabel.Content = selectedDate?.ToShortDateString();
        }

        private void Button_Add_Click(object sender, RoutedEventArgs e)
        {
            //var context = new TestDbContext();
            var text = "Привет";
            var date = calendar1.DisplayDate;
            var dateEnd = calendar1.DisplayDate;
            var textAndDate = new AddTextAndDate(text, date);
            if (textAndDate.ShowDialog() == true)
            {
                text = textAndDate.noteTextBox.Text;
                if (textAndDate.datePicker1.SelectedDates == null)
                {
                    date = textAndDate.datePicker1.DisplayDate;
                    dateEnd = textAndDate.datePicker1.DisplayDate;
                }
                else
                {
                    date = textAndDate.datePicker1.SelectedDates.First();
                    dateEnd = textAndDate.datePicker1.SelectedDates.Last();
                }
                controller.AddItem(date, dateEnd, text);
            }
            //var test = DateTime.Parse(dateLabel.Content.ToString());
        }

        private void Button_Delete_Click(object sender, RoutedEventArgs e)
        {

            var selected = listView.SelectedItem as TestModel;

            if (selected != null)
            {
                using (var context = new TestDbContext())
                {
                    controller.DeleteItem(selected);
                }
                listView.Items.Refresh();
            }
            else
            {
                MessageBox.Show("Выберите элемент для удаления.");
            }
        }

        private void Button_Edit_Click(object sender, RoutedEventArgs e)
        {
            var selected = listView.SelectedItem as TestModel;
            if (selected != null)
            {
                var editNoteDialog = new EditNoteDialog(selected.Notes);
                if (editNoteDialog.ShowDialog() == true)
                {
                    selected.Notes = editNoteDialog.Notes;
                    controller.UpdateItem(selected);
                }
                listView.Items.Refresh();
            }
            else
            {
                MessageBox.Show("Выберите заметку для редактирования.");
            }
        }
    }
}
using MasterPlanner.Controller;
using MasterPlanner.Model;
using System.Security.Cryptography.Xml;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MasterPlanner.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private TestC controller;
        private bool isReminderCheckInProgress;

        public MainWindow()
        {
            InitializeComponent();
            controller = new TestC(this.Dispatcher);
            this.DataContext = controller;
            dateLabel.Content = DateTime.Now.ToString("dd/MM/yyyy");
            controller.InitializeReminder();

        }


        private void calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime? selectedDate = calendar1.SelectedDate;
            dateLabel.Content = selectedDate?.ToShortDateString();
            if (selectedDate is not null)
            {   
                var filtredDate = controller.GetItemsByDate(selectedDate);
                listView.ItemsSource = filtredDate;
            }
        }

        private void Button_Add_Click(object sender, RoutedEventArgs e)
        {
            var textAndDate = new AddTextAndDate("Привет", DateTime.UtcNow);
            if (textAndDate.ShowDialog() == true)
            {
                DateTime date;
                if (textAndDate.datePicker1.SelectedDate is not null)
                {
                    date = (DateTime)textAndDate.datePicker1.SelectedDate;
                }
                else
                {
                    date = DateTime.UtcNow;
                }
                    // Создание новой модели с текстом из диалогового окна
                    controller.AddNewNote(
                        textAndDate.noteTextBox.Text,
                        date.ToUniversalTime(),
                        textAndDate.ShouldAddReminder);
               
            }
            else
            {
                // Обработка случая, когда диалоговое окно закрыто без подтверждения
                MessageBox.Show("Добавление отменено пользователем.");
            }
        }

        //var test = DateTime.Parse(dateLabel.Content.ToString());

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
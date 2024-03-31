using MasterPlanner.Controller;
using MasterPlanner.Model;
using System.Security.Cryptography.Xml;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Windows.Input;


namespace MasterPlanner.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private NotesController controller;
        private bool isReminderCheckInProgress;

        public MainWindow()
        {
            InitializeComponent();
            controller = new NotesController(this.Dispatcher);
            this.DataContext = controller;
            dateLabel.Content = DateTime.Now.ToString("dd/MM/yyyy");
            controller.InitializeReminder();
            item_Count();
        }

        public void item_Count()
        {
            int count = controller.ItemCount();
            noteCount.Content = count + " notes";
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
            var textAndDate = new AddTextAndDate("", DateTime.UtcNow);
            if (textAndDate.ShowDialog() == true)
            {
                DateTime date;
                DateTime date_end;
                if (textAndDate.datePicker1.SelectedDates.Count() != 0)
                {
                    date = (DateTime)textAndDate.datePicker1.SelectedDates.First();
                    date_end = (DateTime)textAndDate.datePicker1.SelectedDates.Last();
                }
                else
                {
                    date = DateTime.Now;
                    date_end = DateTime.Now;
                }
                if (textAndDate.hourComboBox.SelectedItem != null && textAndDate.minuteComboBox.SelectedItem != null)
                {
                    int hour = int.Parse(textAndDate.hourComboBox.SelectedItem.ToString());
                    int minute = int.Parse(textAndDate.minuteComboBox.SelectedItem.ToString());

                    // Установка времени с учетом локального часового пояса
                    date = new DateTime(date.Year, date.Month, date.Day, hour, minute, 0);
                    date_end = new DateTime(date_end.Year, date_end.Month, date_end.Day, hour, minute, 0);
                }

                DateTime utcDate = date.ToUniversalTime();
                DateTime utcDateEnd = date_end.ToUniversalTime();
                // Создание новой модели с текстом из диалогового окна
                controller.AddNewNote(
                        textAndDate.noteTextBox.Text,
                        date.ToUniversalTime(),
                        date_end.ToUniversalTime(),
                        textAndDate.ShouldAddReminder);
                item_Count();
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

            var selected = listView.SelectedItem as PlannerNote;

            if (selected != null)
            {
                using (var context = new PlannerDbContext())
                {
                    controller.DeleteItem(selected);
                }
                listView.Items.Refresh();
                item_Count();
            }
            else
            {
                MessageBox.Show("Выберите элемент для удаления.");
            }
        }

        private void EditNoteDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var selected = listView.SelectedItem as PlannerNote;
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

        private void GoToPreviousPage_Click(object sender, RoutedEventArgs e)
        {
            controller.GoToPreviousPage();
        }

        private void GoToNextPage_Click(object sender, RoutedEventArgs e)
        {
            controller.GoToNextPage();
        }

    }


}
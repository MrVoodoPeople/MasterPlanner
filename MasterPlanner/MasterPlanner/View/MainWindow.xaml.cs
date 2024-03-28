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
                    date = DateTime.UtcNow;
                    date_end = DateTime.UtcNow;
                }
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
    }
}
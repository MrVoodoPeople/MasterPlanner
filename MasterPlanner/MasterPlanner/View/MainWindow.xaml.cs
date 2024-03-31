using MasterPlanner.Controller;
using MasterPlanner.Model;
using System.Security.Cryptography.Xml;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Windows.Input;
using System.ComponentModel;
using System.Windows.Data;
using System.Globalization;


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

        }


        private void calendar_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime? selectedDate = calendar1.SelectedDate;
            dateLabel.Content = selectedDate?.ToShortDateString();
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
            var selected = listView.SelectedItems;
            PlannerNote[] selectedPlanner = new PlannerNote[selected.Count];
            for (int i = 0; i < selected.Count; i++)
            {
                selectedPlanner[i] = selected[i] as PlannerNote;
            }

            if (selectedPlanner != null)
            {
                using (var context = new PlannerDbContext())
                {
                    controller.DeleteItem(selectedPlanner);
                }
                listView.Items.Refresh();
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

        private void Sort(string sortBy, ListSortDirection direction)
        {
            controller.ClearData();
            controller.LoadData(sortBy, direction);
        }

        GridViewColumnHeader _lastHeaderClicked = null;
        ListSortDirection _lastDirection = ListSortDirection.Ascending;
        private void GridViewColumnHeaderClick(object sender, RoutedEventArgs e)
        {
            var headerClicked = e.OriginalSource as GridViewColumnHeader;
            ListSortDirection direction;

            if (headerClicked != null)
            {
                if (headerClicked.Role != GridViewColumnHeaderRole.Padding)
                {
                    if (headerClicked != _lastHeaderClicked)
                    {
                        direction = ListSortDirection.Ascending;
                    }
                    else
                    {
                        if (_lastDirection == ListSortDirection.Ascending)
                        {
                            direction = ListSortDirection.Descending;
                        }
                        else
                        {
                            direction = ListSortDirection.Ascending;
                        }
                    }

                    var columnBinding = headerClicked.Column.DisplayMemberBinding as Binding;
                    var sortBy = columnBinding?.Path.Path ?? headerClicked.Column.Header as string;

                    Sort(sortBy, direction);

                    if (direction == ListSortDirection.Ascending)
                    {
                        headerClicked.Column.HeaderTemplate =
                          Resources["HeaderTemplateArrowUp"] as DataTemplate;
                    }
                    else
                    {
                        headerClicked.Column.HeaderTemplate =
                          Resources["HeaderTemplateArrowDown"] as DataTemplate;
                    }

                    if (_lastHeaderClicked != null && _lastHeaderClicked != headerClicked)
                    {
                        _lastHeaderClicked.Column.HeaderTemplate = null;
                    }

                    _lastHeaderClicked = headerClicked;
                    _lastDirection = direction;
                }
            }
        }

        private void Button_ShowSelectedDates(object sender, RoutedEventArgs e)
        {
            DateTime? selectedDate = calendar1.SelectedDate;
            if (selectedDate is not null)
            {
                controller.Items = controller.GetItemsByDate(selectedDate);
                listView.ItemsSource = controller.CurrentPageItems;
                controller.CalculateTotalPages();
                controller.UpdateCurrentPageItems();
            }
            else
            {
                MessageBox.Show("Не выбраны даты.");
            }
        }

        private void Button_ShowAllDates(object sender, RoutedEventArgs e)
        {
            controller.ClearData();
            controller.LoadData();
            controller.CalculateTotalPages();
            controller.UpdateCurrentPageItems();
            listView.ItemsSource = controller.CurrentPageItems;
        }
    }


}
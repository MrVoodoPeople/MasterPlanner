using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
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

namespace MasterPlanner.View
{
    /// <summary>
    /// Логика взаимодействия для AddTextAndDate.xaml
    /// </summary>
    public partial class AddTextAndDate : Window
    {
        public bool ShouldAddReminder
        {
            get { return addReminderCheckBox.IsChecked ?? false; }
        }

        public string Notes
        {
            get { return noteTextBox.Text; }
            set { noteTextBox.Text = value; }
        }
        public DateTime Date
        {
            get
            {
                var date = datePicker1.SelectedDate.HasValue ? datePicker1.SelectedDate.Value : DateTime.Now;
                var hour = hourComboBox.SelectedItem != null ? int.Parse(hourComboBox.SelectedItem.ToString()) : DateTime.Now.Hour;
                var minute = minuteComboBox.SelectedItem != null ? int.Parse(minuteComboBox.SelectedItem.ToString()) : DateTime.Now.Minute;
                return new DateTime(date.Year, date.Month, date.Day, hour, minute, 0);
            }
            set
            {
                datePicker1.SelectedDate = value;
                hourComboBox.SelectedItem = value.Hour.ToString("00");
                minuteComboBox.SelectedItem = value.Minute.ToString("00");
            }
        }

        public AddTextAndDate(string note, DateTime date)
        {
            InitializeComponent();
            Notes = note;
            Date = date;

            for (int i = 0; i < 24; i++)
            {
                hourComboBox.Items.Add(i.ToString("00"));
            }
            for (int i = 0; i < 60; i += 5) // Минуты можно установить с шагом в 5 минут
            {
                minuteComboBox.Items.Add(i.ToString("00"));
            }

            // Установка текущего времени
            hourComboBox.SelectedItem = date.Hour.ToString("00");
            minuteComboBox.SelectedItem = date.Minute.ToString("00");

        }


        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if(ShouldAddReminder)
            {
                this.Date = datePicker1.SelectedDate ?? DateTime.Now;
            }
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}

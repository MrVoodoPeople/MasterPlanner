﻿using System;
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
            get { return datePicker1.DisplayDate.ToUniversalTime(); }
            set { datePicker1.DisplayDate = value; }
        }
        public AddTextAndDate(string note, DateTime date)
        {
            InitializeComponent();
            Notes = note;
            Date = date.ToUniversalTime();

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

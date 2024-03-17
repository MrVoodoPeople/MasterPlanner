using System;
using System.Collections.Generic;
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

namespace MasterPlanner.View
{
    /// <summary>
    /// Логика взаимодействия для EditNoteDialog.xaml
    /// </summary>
    public partial class EditNoteDialog : Window
    {
        public string Notes
        {
            get { return noteTextBox.Text; }
            set { noteTextBox.Text = value; }
        }
        public EditNoteDialog(string notes)
        {
            InitializeComponent();
            Notes = notes;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}

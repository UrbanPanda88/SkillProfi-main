using SkillProfiWPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SkillProfiWPF
{
    public partial class StatusSelectionDialog : Window
    {
        public RequestStatus SelectedStatus { get; private set; }

        public StatusSelectionDialog()
        {
            InitializeComponent();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            if (statusComboBox.SelectedItem != null)
            {
                SelectedStatus = (RequestStatus)Enum.Parse(typeof(RequestStatus), (string)((ComboBoxItem)statusComboBox.SelectedItem).Content);
                DialogResult = true;
            }
            else
            {
                MessageBox.Show("Please select a status.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
    }
}

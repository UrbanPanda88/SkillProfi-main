using SkillProfiWPF.Models;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace SkillProfiWPF
{
    public partial class DashBoard : Window
    {
        private readonly DashboardViewModel viewModel;

        public DashBoard()
        {
            InitializeComponent();
            viewModel = new DashboardViewModel();
            DataContext = viewModel;

            LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            await viewModel.LoadDataAsync();
            dataGrid.ItemsSource = viewModel.Requests;
        }

        private async void ChangeStatusButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedRequest = dataGrid.SelectedItem as RequestInfo;
            if (selectedRequest == null)
            {
                MessageBox.Show("Please select a request to change the status.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var statusDialog = new StatusSelectionDialog();
            if (statusDialog.ShowDialog() == true)
            {
                try
                {
                    await viewModel.UpdateStatusAsync(selectedRequest.Id, statusDialog.SelectedStatus);
                    await LoadDataAsync();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error updating status: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}

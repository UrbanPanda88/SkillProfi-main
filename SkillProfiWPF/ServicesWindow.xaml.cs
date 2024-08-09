using SkillProfiWPF.ViewModels;
using System.Windows;

namespace SkillProfiWPF
{
    public partial class ServicesWindow : Window
    {
        private ServicesViewModel viewModel;

        public ServicesWindow()
        {
            InitializeComponent();
            viewModel = new ServicesViewModel();
            DataContext = viewModel;

            LoadServicesAsync();
        }

        private async void LoadServicesAsync()
        {
            await viewModel.LoadServicesAsync();
        }
    }
}
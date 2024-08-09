using System.Windows;
using System.ComponentModel;
using SkillProfiWPF.ViewModels;

namespace SkillProfiWPF
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private MainViewModel viewModel;

        public MainWindow()
        {
            InitializeComponent();
            viewModel = new MainViewModel();
            DataContext = viewModel;
            Loaded += MainWindow_Loaded;
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            await viewModel.InitializeAsync();
        }

        private void DashBoard_Click(object sender, RoutedEventArgs e)
        {
            DashBoard dashBoard = new DashBoard();
            dashBoard.ShowDialog();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.ShowDialog();
            viewModel.IsUserLoggedIn = true;
        }

        private async void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            await viewModel.LogoutAsync();
        }
    }
}

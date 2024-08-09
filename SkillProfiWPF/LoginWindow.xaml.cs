using System.Windows;
using SkillProfiWPF.ViewModels;

namespace SkillProfiWPF
{
    public partial class LoginWindow : Window
    {
        private LoginViewModel viewModel;

        public LoginWindow()
        {
            InitializeComponent();
            viewModel = new LoginViewModel(this);
            DataContext = viewModel;
        }
    }
}

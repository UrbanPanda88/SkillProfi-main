using SkillProfiWPF.ViewModels;
using System.Windows;

namespace SkillProfiWPF
{
    public partial class ContactsWindow : Window
    {
        public ContactsWindow()
        {
            InitializeComponent();

            ContactsViewModel viewModel = new ContactsViewModel();
            DataContext = viewModel;
        }
    }
}

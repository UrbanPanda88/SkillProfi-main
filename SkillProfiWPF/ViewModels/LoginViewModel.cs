using System.Net.Http;
using System.Text;
using System.Windows;
using System.Windows.Input;

namespace SkillProfiWPF.ViewModels
{
    public class LoginViewModel : ViewModelBase
    {
        private string username;
        public string Username
        {
            get { return username; }
            set { username = value; NotifyPropertyChanged(); }
        }

        private string password;
        public string Password
        {
            get { return password; }
            set { password = value; NotifyPropertyChanged(); }
        }

        public ICommand LoginCommand { get; private set; }

        private Window mainWindow;

        public LoginViewModel(Window mainWindow)
        {
            this.mainWindow = mainWindow;
            LoginCommand = new RelayCommand(Login);
        }

        public void Login()
        {
            var httpClient = new HttpClient();
            var content = new StringContent($"{{\"Username\":\"{Username}\",\"Password\":\"{Password}\"}}", Encoding.UTF8, "application/json");
            var response = httpClient.PostAsync("https://localhost:44392/api/Account/Login", content).Result;

            if (response.IsSuccessStatusCode)
            {
                mainWindow.Close();
            }
            else
            {
                var errorMessage = response.Content.ReadAsStringAsync().Result;
                MessageBox.Show(errorMessage);
            }
        }
    }
}

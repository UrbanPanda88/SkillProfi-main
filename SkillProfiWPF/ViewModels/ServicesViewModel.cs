using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SkillProfiWPF.Models;

namespace SkillProfiWPF.ViewModels
{
    public class ServicesViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<Service> services;
        public ObservableCollection<Service> Services
        {
            get { return services; }
            set { services = value; NotifyPropertyChanged(); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void NotifyPropertyChanged(string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task LoadServicesAsync()
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync("https://localhost:44392/api/services");
                if (response.IsSuccessStatusCode)
                {
                    var servicesJson = await response.Content.ReadAsStringAsync();
                    Services = JsonConvert.DeserializeObject<ObservableCollection<Service>>(servicesJson);
                }
                else
                {

                }
            }
        }
    }
}

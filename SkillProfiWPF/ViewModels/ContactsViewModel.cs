using SkillProfiWPF.Models;
using System.Collections.ObjectModel;
using System.Net.Http;
using Newtonsoft.Json;

namespace SkillProfiWPF.ViewModels
{
    public class ContactsViewModel : ViewModelBase
    {
        private ObservableCollection<Contact> contacts;
        public ObservableCollection<Contact> Contacts
        {
            get { return contacts; }
            set { contacts = value; NotifyPropertyChanged(); }
        }

        public ContactsViewModel()
        {
            LoadContactsAsync();
        }

        private async void LoadContactsAsync()
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync("https://localhost:44392/api/Contacts");
                if (response.IsSuccessStatusCode)
                {
                    var contactsJson = await response.Content.ReadAsStringAsync();
                    Contacts = JsonConvert.DeserializeObject<ObservableCollection<Contact>>(contactsJson);
                }
                else
                {

                }
            }
        }
    }
}

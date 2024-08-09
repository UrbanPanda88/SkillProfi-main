using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;
using System.Text;
using SkillProfiWPF.Models;
using System;

namespace SkillProfiWPF
{
    public class DashboardViewModel : ViewModelBase
    {
        private List<RequestInfo> requests;
        public List<RequestInfo> Requests
        {
            get { return requests; }
            set
            {
                requests = value;
                NotifyPropertyChanged();
            }
        }

        public async Task LoadDataAsync()
        {
            try
            {
                var apiData = await FetchDataFromApi();

                Requests = apiData
                    .SelectMany(statusInfo => statusInfo.Requests.Select(request => new RequestInfo
                    {
                        Id = request.Id,
                        FullName = request.FullName,
                        Email = request.Email,
                        Message = request.Message,
                        Status = request.Status,
                        RequestDate = request.RequestDate
                    }))
                    .ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task<List<StatusInfo>> FetchDataFromApi()
        {
            List<StatusInfo> apiData = null;

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync("https://localhost:44392/api/DashBoard");
                if (response.IsSuccessStatusCode)
                {
                    apiData = await response.Content.ReadAsAsync<List<StatusInfo>>();
                }
            }

            return apiData;
        }

        public async Task UpdateStatusAsync(int requestId, RequestStatus newStatus)
        {
            using (var client = new HttpClient())
            {
                var response = await client.PostAsync($"https://localhost:44392/api/DashBoard/ChangeStatus/{requestId}",
                     new StringContent(JsonConvert.SerializeObject(newStatus), Encoding.UTF8, "application/json"));

                response.EnsureSuccessStatusCode();
            }
        }
    }
}

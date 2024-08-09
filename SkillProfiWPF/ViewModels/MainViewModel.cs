using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Newtonsoft.Json;
using SkillProfiWPF.Models;

namespace SkillProfiWPF.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        private ObservableCollection<MenuTitles> menuTitles;
        public ObservableCollection<MenuTitles> MenuTitles
        {
            get { return menuTitles; }
            set { menuTitles = value; NotifyPropertyChanged(); }
        }

        private MenuTitles selectedMenuTitle;
        public MenuTitles SelectedMenuTitle
        {
            get { return selectedMenuTitle; }
            set { selectedMenuTitle = value; NotifyPropertyChanged(); }
        }

        private Request request;
        public Request Request
        {
            get { return request; }
            set { request = value; NotifyPropertyChanged(); }
        }

        private string welcomeMessage;
        public string WelcomeMessage
        {
            get { return welcomeMessage; }
            set { welcomeMessage = value; NotifyPropertyChanged(); }
        }

        private string actionCallText;
        public string ActionCallText
        {
            get { return actionCallText; }
            set { actionCallText = value; NotifyPropertyChanged(); }
        }

        private bool isUserLoggedIn;
        public bool IsUserLoggedIn
        {
            get { return isUserLoggedIn; }
            set { isUserLoggedIn = value; NotifyPropertyChanged(); }
        }

        private ObservableCollection<Project> projects;
        public ObservableCollection<Project> Projects
        {
            get { return projects; }
            set { projects = value; NotifyPropertyChanged(); }
        }

        private ObservableCollection<BlogPost> blogPosts;
        public ObservableCollection<BlogPost> BlogPosts
        {
            get { return blogPosts; }
            set { blogPosts = value; NotifyPropertyChanged(); }
        }

        private Dictionary<int, ICommand> menuCommands;
        public Dictionary<int, ICommand> MenuCommands
        {
            get { return menuCommands; }
            set { menuCommands = value; NotifyPropertyChanged(); }
        }

        public ICommand ProjectsButtonCommand { get; private set; }
        public ICommand ServicesButtonCommand { get; private set; }
        public ICommand BlogPostButtonCommand { get; private set; }
        public ICommand ContactsButtonCommand { get; private set; }
        public ICommand SubmitFormCommand { get; private set; }

        public MainViewModel()
        {
            ProjectsButtonCommand = new RelayCommand(OpenProjectsWindow);
            ServicesButtonCommand = new RelayCommand(ServicesButton_Click);
            BlogPostButtonCommand = new RelayCommand(OpenBlogPostWindow);
            ContactsButtonCommand = new RelayCommand(ContactsButton_Click);
            SubmitFormCommand = new RelayCommand(SubmitForm);

            Request = new Request();
        }

        public async Task InitializeAsync()
        {
            await GetMenuTitlesAsync();
        }

        public async void SubmitForm()
        {
            if (string.IsNullOrEmpty(Request.FullName) || string.IsNullOrEmpty(Request.Email) || string.IsNullOrEmpty(Request.Message))
            {
                MessageBox.Show("Please fill in all required fields.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            using (var client = new HttpClient())
            {
                var requestJson = JsonConvert.SerializeObject(Request);
                var content = new StringContent(requestJson, Encoding.UTF8, "application/json");

                var response = await client.PostAsync("https://localhost:44317/api/Home/SubmitForm", content);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine("Form submitted successfully!");

                    MessageBox.Show("Form submitted successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    Console.WriteLine("Error submitting form. Status code: " + response.StatusCode);

                    MessageBox.Show("Error submitting form. Status code: " + response.StatusCode, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private async Task GetMenuTitlesAsync()
        {
            using (var client = new HttpClient())
            {
                var response = await client.GetAsync("https://localhost:44317/api/Home/MenuTitles");
                if (response.IsSuccessStatusCode)
                {
                    var menuTitlesJson = await response.Content.ReadAsStringAsync();
                    ObservableCollection<MenuTitles> menuTitles = JsonConvert.DeserializeObject<ObservableCollection<MenuTitles>>(menuTitlesJson);
                    MenuTitles = menuTitles;

                    Projects = await GetProjectsAsync();

                    BlogPosts = await GetBlogPostsAsync();

                    foreach (var menuTitle in MenuTitles)
                    {
                        Console.WriteLine(menuTitle.Name);
                        Console.WriteLine(menuTitle.ActionCallText);
                        Console.WriteLine(menuTitle.LinkAction);
                        Console.WriteLine(menuTitle.LinkController);
                        Console.WriteLine(menuTitle.WelcomeMessage);

                        if (!string.IsNullOrEmpty(menuTitle.ActionCallText))
                        {
                            WelcomeMessage = menuTitle.WelcomeMessage;
                            ActionCallText = menuTitle.ActionCallText;
                            SelectedMenuTitle = menuTitle;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Error loading menu titles.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            MenuCommands = new Dictionary<int, ICommand>();
            foreach (var menuTitle in MenuTitles)
            {
                MenuCommands[menuTitle.Id] = new RelayCommand<int>(MenuItemClicked);
            }
        }

        private async Task<ObservableCollection<Project>> GetProjectsAsync()
        {
            using (var client = new HttpClient())
            {
                var projectsResponse = await client.GetAsync("https://localhost:44392/api/Projects");
                if (projectsResponse.IsSuccessStatusCode)
                {
                    var projectsJson = await projectsResponse.Content.ReadAsStringAsync();
                    var projects = JsonConvert.DeserializeObject<ObservableCollection<Project>>(projectsJson);

                    foreach (var project in projects)
                    {
                        if (project.ImageFile != null)
                        {
                            var imagePath = Path.Combine("images", Path.GetFileName(project.ImageFile.FileName));
                            var destinationPath = Path.Combine("wwwroot", "images", imagePath);

                            File.Copy(project.ImageFile.FileName, destinationPath, true);

                            project.ImageUrl = imagePath;
                        }
                    }

                    return projects;
                }
                else
                {
                    MessageBox.Show("Error loading projects.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return new ObservableCollection<Project>();
                }
            }
        }

        private async Task<ObservableCollection<BlogPost>> GetBlogPostsAsync()
        {
            using (var client = new HttpClient())
            {
                var blogpostsResponse = await client.GetAsync("https://localhost:44392/api/Blog");
                if (blogpostsResponse.IsSuccessStatusCode)
                {
                    var blogPostsJson = await blogpostsResponse.Content.ReadAsStringAsync();
                    var blogPosts = JsonConvert.DeserializeObject<ObservableCollection<BlogPost>>(blogPostsJson);

                    foreach (var blogPost in blogPosts)
                    {
                        if (blogPost.ImageFile != null)
                        {
                            var imagePath = Path.Combine("images", Path.GetFileName(blogPost.ImageFile.FileName));
                            var destinationPath = Path.Combine("wwwroot", "images", imagePath);

                            File.Copy(blogPost.ImageFile.FileName, destinationPath, true);

                            blogPost.ImageUrl = imagePath;
                        }
                    }

                    return blogPosts;
                }
                else
                {
                    MessageBox.Show("Error loading blog posts.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return new ObservableCollection<BlogPost>();
                }
            }
        }

        public async Task LogoutAsync()
        {
            using (HttpClient client = new HttpClient())
            {
                var response = await client.PostAsync("https://localhost:44392/api/Account/Logout", null);
                if (response.IsSuccessStatusCode)
                {
                    IsUserLoggedIn = false;
                }
                else
                {
                    MessageBox.Show($"Error logging out. Status code: {response.StatusCode}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }


        private void MenuItemClicked(int menuTitleId)
        {
            using (var client = new HttpClient())
            {
                var response = client.GetAsync($"https://localhost:44392/api/menucontent/{menuTitleId}").Result;
                if (response.IsSuccessStatusCode)
                {
                    var menuTitleJson = response.Content.ReadAsStringAsync().Result;
                    SelectedMenuTitle = JsonConvert.DeserializeObject<MenuTitles>(menuTitleJson);
                }
                else
                {
                    MessageBox.Show("Error loading menu content.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            if (MenuCommands.TryGetValue(menuTitleId, out var command))
            {
                command.Execute(menuTitleId);
            }
        }

        public void OpenProjectsWindow()
        {
            ProjectsWindow projectsWindow = new ProjectsWindow();
            projectsWindow.DataContext = this;
            projectsWindow.ShowDialog();
        }

        private void ServicesButton_Click()
        {
            ServicesWindow servicesWindow = new ServicesWindow();
            servicesWindow.ShowDialog();
        }

        private void ContactsButton_Click()
        {
            ContactsWindow contactsWindow = new ContactsWindow();
            contactsWindow.ShowDialog();
        }

        public void OpenBlogPostWindow()
        {
            BlogPostWindow blogPostWindow = new BlogPostWindow();
            blogPostWindow.DataContext = this;
            blogPostWindow.ShowDialog();
        }
    }
}

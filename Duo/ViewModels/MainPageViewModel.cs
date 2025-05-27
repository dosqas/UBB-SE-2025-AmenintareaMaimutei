using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Duo.ViewModels.Base;
using DuoClassLibrary.Services;
using DuoClassLibrary.Services.Interfaces;
using Microsoft.UI.Xaml.Controls;

namespace Duo.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        public event EventHandler<Type> NavigationRequested;

        public MainPageViewModel()
        {
        }

        public void HandleNavigationSelectionChanged(NavigationViewSelectionChangedEventArgs args)
        {
            try
            {
                if (args.SelectedItem is NavigationViewItem selectedItem)
                {
                    var tag = selectedItem.Tag?.ToString();

                    if (string.IsNullOrEmpty(tag))
                    {
                        return;
                    }

                    if (selectedItem.MenuItems.Count > 0)
                    {
                        return;
                    }

                    Type? pageType = null;

                    switch (tag)
                    {
                        case "QuizParent":
                            pageType = typeof(Views.Pages.RoadmapMainPage);
                            break;
                        case "QuizAdminParent":
                            pageType = typeof(Views.Pages.AdminMainPage);
                            break;
                        case "CoursesParent":
                            pageType = typeof(Duo.Views.MainPage);
                            break;
                        default:
                            RaiseErrorMessage("Navigation Error", $"Unknown page tag: {tag}");
                            return;
                    }

                    if (pageType != null)
                    {
                        NavigationRequested?.Invoke(this, pageType);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in navigation selection: {ex.Message}");
                RaiseErrorMessage("Navigation Failed", ex.Message);
            }
        }

        public async Task HandleLogoutClick()
        {
            try
            {
                // Logic to handle logout, e.g., clearing user session, navigating to login page, etc.
                var CurrentUser = App.CurrentUser;
                if (CurrentUser == null)
                {
                    RaiseErrorMessage("Logout Error", "No user is currently logged in.");
                    return;
                }
                await ((ILoginService)App.ServiceProvider.GetService(typeof(ILoginService))).UpdateUserStatusOnLogout(CurrentUser);
                App.userService.ClearCurrentUser();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error during logout: {ex.Message}");
                RaiseErrorMessage("Logout Failed", ex.Message);
            }
        }
    }
}

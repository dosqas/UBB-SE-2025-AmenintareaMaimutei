using System;
using System.Net.Http;
using System.Diagnostics.CodeAnalysis;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Duo.Models;
using Duo.ViewModels;
using Duo.Services;

namespace Duo.Views
{
    [ExcludeFromCodeCoverage]
    public sealed partial class ModulePage : Page
    {
        private IModuleViewModel viewModel = null!;

        private int CurrentUserId { get; init; } = 1;

        public ModulePage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(Microsoft.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            if (e.Parameter is ValueTuple<Module, CourseViewModel> tuple)
            {
                var (module, courseVM) = tuple;

                var httpClient = new HttpClient
                {
                    BaseAddress = new Uri("https://localhost:7174")
                };

                viewModel = new ModuleViewModel(
                    module,
                    courseVM,
                    CurrentUserId,
                    new CourseService(new CourseServiceProxy(httpClient)),
                    new CoinsService(new CoinsServiceProxy(httpClient)));

                this.DataContext = viewModel;

                DispatcherQueue.TryEnqueue(async () =>
                {
                    try
                    {
                        await viewModel.InitializeAsync();
                    }
                    catch (Exception ex)
                    {
                        var dialog = new ContentDialog
                        {
                            Title = "Error loading module",
                            Content = $"Failed to initialize module: {ex.Message}",
                            CloseButtonText = "OK",
                            XamlRoot = this.XamlRoot
                        };
                        await dialog.ShowAsync();
                    }
                });
            }
        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.Frame.CanGoBack)
            {
                this.Frame.GoBack();
            }
        }
    }
}
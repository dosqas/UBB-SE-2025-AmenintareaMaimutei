using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using DuoClassLibrary.Models;
using Duo.ViewModels;
using Microsoft.UI.Xaml.Navigation;
using System.Runtime.InteropServices;
using System.Collections.Generic;

#pragma warning disable CS8602
#pragma warning disable IDE0059

namespace Duo.Views
{
    [ExcludeFromCodeCoverage]
    public sealed partial class CoursePage : Page
    {
        private CourseViewModel? viewModel;
        private readonly Queue<string> _notificationQueue = new();
        private bool _isNotificationDialogOpen = false;

        private int CurrentUserId { get; set; }

        public CoursePage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            switch (e.Parameter)
            {
                case CourseViewModel vm:
                    viewModel = vm;
                    break;

                case ValueTuple<Module, CourseViewModel> tuple:
                    viewModel = tuple.Item2;
                    break;

                default:
                    return;
            }

            this.CurrentUserId = viewModel.CurrentUserId;

            this.DataContext = viewModel;

            viewModel.PropertyChanged += async (s, args) =>
            {
                if (args.PropertyName == nameof(viewModel.ShowNotification)
                 && viewModel.ShowNotification)
                {
                    viewModel.ShowNotification = false;
                    EnqueueNotification(viewModel.NotificationMessage);

                    await SafeShowNotificationDialog(viewModel.NotificationMessage);
                }
            };

            ModulesListView.ItemClick += ModulesListView_ItemClick;

            DispatcherQueue.TryEnqueue(async () =>
            {
                try
                {
                    Console.WriteLine("Starting InitializeAsync");
                    await viewModel.InitializeAsync(CurrentUserId);
                    Console.WriteLine("Finished InitializeAsync");
                    if (viewModel.IsEnrolled && !viewModel.IsCourseCompleted)
                        viewModel.StartCourseProgressTimer();
                }
                catch (Exception ex)
                {
                    var dialog = new ContentDialog
                    {
                        Title = "Initialization Error",
                        Content = $"Failed to initialize course: {ex.Message}",
                        CloseButtonText = "OK",
                        XamlRoot = this.XamlRoot
                    };
                    await dialog.ShowAsync();
                }
            });
        }

        /// <summary>
        /// Displays an error message from the ViewModel.
        /// </summary>
        private async void ViewModel_ShowErrorMessageRequested(object? sender, (string Title, string Message) e)
        {
            await ShowErrorMessage(e.Title, e.Message);
        }


        private void EnqueueNotification(string message)
        {
            _notificationQueue.Enqueue(message);

            if (!_isNotificationDialogOpen)
                _ = ProcessNextNotificationAsync();
        }

        /// <summary>
        /// Shows a ContentDialog with an error message.
        /// </summary>
        private async Task ShowErrorMessage(string title, string message)
        {
            var dialog = new ContentDialog
            {
                Title = title,
                Content = message,
                CloseButtonText = "OK",
                XamlRoot = this.XamlRoot
            };

            await dialog.ShowAsync();
        }

        private async Task ProcessNextNotificationAsync()
        {
            if (_notificationQueue.Count == 0)
                return;

            _isNotificationDialogOpen = true;
            string msg = _notificationQueue.Dequeue();

            try
            {
                var dlg = new ContentDialog
                {
                    XamlRoot = this.XamlRoot,
                    Title = "Notification",
                    Content = msg,
                    CloseButtonText = "OK"
                };
                await dlg.ShowAsync();
            }
            catch (COMException)
            {
            }
            finally
            {
                _isNotificationDialogOpen = false;

                if (_notificationQueue.Count > 0)
                    await ProcessNextNotificationAsync();
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.Frame.CanGoBack)
            {
                viewModel.PauseCourseProgressTimer(CurrentUserId);
                this.Frame.GoBack();
            }
        }

        private async void ModulesListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (e.ClickedItem is CourseViewModel.ModuleProgressStatus moduleDisplay && viewModel!.IsEnrolled)
            {
                if (moduleDisplay.IsUnlocked)
                {
                    this.Frame.Navigate(typeof(ModulePage), (moduleDisplay.Module, viewModel));
                    return;
                }
                try
                    {
                    if (moduleDisplay.Module!.IsBonus)
                    {
                        if (moduleDisplay.Module!.IsBonus)
                        {
                            await viewModel.AttemptBonusModulePurchaseAsync(moduleDisplay.Module, CurrentUserId);
                        }
                    }
                }
                catch (Exception ex)
                {
                    var dialog = new ContentDialog
                    {
                        Title = "Error",
                        Content = $"An error occurred while attempting to unlock the module: {ex.Message}",
                        CloseButtonText = "OK",
                        XamlRoot = this.XamlRoot
                    };

                    await dialog.ShowAsync();
                }

                viewModel.RaiseErrorMessage("Module Locked", "You need to complete the previous modules to unlock this one.");
            }
        }

        private async Task SafeShowNotificationDialog(string message)
        {
            if (_isNotificationDialogOpen)
                return;

            _isNotificationDialogOpen = true;
            try
            {
                var dlg = new ContentDialog
                {
                    XamlRoot = this.XamlRoot,
                    Title = "Notification",
                    Content = message,
                    CloseButtonText = "OK"
                };
                await dlg.ShowAsync();
            }
            catch (COMException)
            {
            }
            finally
            {
                _isNotificationDialogOpen = false;
            }
        }

    }
}

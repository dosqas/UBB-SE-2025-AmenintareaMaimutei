using System;
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
        public ModulePage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(Microsoft.UI.Xaml.Navigation.NavigationEventArgs e)
        {
            if (e.Parameter is ValueTuple<Module, CourseViewModel> tuple)
            {
                var (module, courseVM) = tuple;
                viewModel = new ModuleViewModel(
                            module,
                            courseVM,
                            new CourseService(),          // as IModuleCompletionService
                            new CoinsService());         // as ICoinsService
                this.DataContext = viewModel;
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
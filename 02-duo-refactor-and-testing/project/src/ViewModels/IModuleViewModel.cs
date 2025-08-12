using System.Windows.Input;
using CourseApp.Models;

#pragma warning disable IDE0079
#pragma warning disable CA1050

/// <summary>
/// Interface for the view model that handles module-related operations and state.
/// </summary>
public interface IModuleViewModel : IBaseViewModel
{
    /// <summary>
    /// Gets the current module being displayed or worked on.
    /// </summary>
    Module CurrentModule { get; }

    /// <summary>
    /// Indicates whether the current module has been completed.
    /// </summary>
    bool IsCompleted { get; }

    /// <summary>
    /// Command that triggers the completion of the current module.
    /// </summary>
    ICommand CompleteModuleCommand { get; }

    /// <summary>
    /// Command for handling actions related to the module's image click.
    /// </summary>
    ICommand ModuleImageClickCommand { get; set; }

    /// <summary>
    /// Gets the formatted time spent on the current module.
    /// </summary>
    string TimeSpent { get; }

    /// <summary>
    /// Gets the current coin balance related to the module.
    /// </summary>
    int CoinBalance { get; }

    /// <summary>
    /// Handles the image click event for the current module, performing necessary actions.
    /// </summary>
    void HandleModuleImageClick(object? obj);

    /// <summary>
    /// Executes the module image click logic, triggering associated actions and UI updates.
    /// </summary>
    void ExecuteModuleImageClick(object? obj);
}
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using DuoClassLibrary.Models;

#pragma warning disable IDE0079
#pragma warning disable CA1050

/// <summary>
/// Represents the main view model for managing course display, filters, and user interactions.
/// </summary>
public interface IMainViewModel : IBaseViewModel
{
    /// <summary>
    /// Gets the list of courses to be displayed.
    /// </summary>
    ObservableCollection<Course> DisplayedCourses { get; }

    /// <summary>
    /// Gets the list of all available tags for filtering.
    /// </summary>
    ObservableCollection<Tag> AvailableTags { get; }

    /// <summary>
    /// Gets the current coin balance of the user.
    /// </summary>
    int UserCoinBalance { get; }

    /// <summary>
    /// Gets or sets the search query used to filter courses.
    /// </summary>
    string SearchQuery { get; set; }

    /// <summary>
    /// Gets or sets the filter to show only premium courses.
    /// </summary>
    bool FilterByPremium { get; set; }

    /// <summary>
    /// Gets or sets the filter to show only free courses.
    /// </summary>
    bool FilterByFree { get; set; }

    /// <summary>
    /// Gets or sets the filter to show only enrolled courses.
    /// </summary>
    bool FilterByEnrolled { get; set; }

    /// <summary>
    /// Gets or sets the filter to show only courses the user is not enrolled in.
    /// </summary>
    bool FilterByNotEnrolled { get; set; }

    /// <summary>
    /// Gets the command to reset all filters to their default state.
    /// </summary>
    ICommand ResetAllFiltersCommand { get; }

    /// <summary>
    /// Tries to award the user a daily login reward.
    /// </summary>
    Task<bool> TryDailyLoginReward();
}

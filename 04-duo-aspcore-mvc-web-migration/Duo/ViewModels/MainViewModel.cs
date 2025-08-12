using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Duo.Commands;
using DuoClassLibrary.Models;
using DuoClassLibrary.Services;
using Windows.System.Threading;

#pragma warning disable IDE0028, CS8618, CS8602, CS8601, IDE0060

namespace Duo.ViewModels
{
    /// <summary>
    /// ViewModel responsible for managing the main application logic, including course display, filtering, and user coin balance.
    /// </summary>
    public partial class MainViewModel : BaseViewModel, IMainViewModel
    {
        private int CurrentUserId { get; init; } = 1;

        private readonly ICourseService courseService;
        private readonly ICoinsService coinsService;

        private string searchQuery = string.Empty;
        private bool filterByPremium;
        private bool filterByFree;
        private bool filterByEnrolled;
        private bool filterByNotEnrolled;

        /// <summary>
        /// Observable collection of courses to be displayed.
        /// </summary>
        private ObservableCollection<Course> displayedCourses;
        public ObservableCollection<Course> DisplayedCourses
        {
            get => displayedCourses;
            set
            {
                if (displayedCourses != value)
                {
                    displayedCourses = value;
                    OnPropertyChanged(nameof(DisplayedCourses));
                }
            }
        }

        /// <summary>
        /// Observable collection of available tags.
        /// </summary>
        public ObservableCollection<Tag> AvailableTags { get; private set; }

        /// <summary>
        /// User's current coin balance.
        /// </summary>
        private int userCoinBalance;
        public int UserCoinBalance
        {
            get => userCoinBalance;
            private set
            {
                userCoinBalance = value;
                OnPropertyChanged(nameof(UserCoinBalance));
            }
        }

        public async Task RefreshUserCoinBalanceAsync()
        {
            try
            {
                UserCoinBalance = await coinsService.GetCoinBalanceAsync(CurrentUserId);
            }
            catch (Exception ex)
            {
                RaiseErrorMessage("Failed to refresh coin balance", ex.Message);
            }
        }

        /// <summary>
        /// The search query used to filter courses.
        /// </summary>
        public string SearchQuery
        {
            get => searchQuery;
            set
            {
                if (value.Length <= 100 && searchQuery != value)
                {
                    searchQuery = value;
                    OnPropertyChanged();
                    ApplyAllFilters();
                }
            }
        }

        /// <summary>
        /// Filter flag for premium courses.
        /// </summary>
        public bool FilterByPremium
        {
            get => filterByPremium;
            set
            {
                if (filterByPremium != value)
                {
                    filterByPremium = value;
                    OnPropertyChanged();
                    ApplyAllFilters();
                }
            }
        }

        /// <summary>
        /// Filter flag for free courses.
        /// </summary>
        public bool FilterByFree
        {
            get => filterByFree;
            set
            {
                if (filterByFree != value)
                {
                    filterByFree = value;
                    OnPropertyChanged();
                    ApplyAllFilters();
                }
            }
        }

        /// <summary>
        /// Filter flag for enrolled courses.
        /// </summary>
        public bool FilterByEnrolled
        {
            get => filterByEnrolled;
            set
            {
                if (filterByEnrolled != value)
                {
                    filterByEnrolled = value;
                    OnPropertyChanged();
                    ApplyAllFilters();
                }
            }
        }

        /// <summary>
        /// Filter flag for not enrolled courses.
        /// </summary>
        public bool FilterByNotEnrolled
        {
            get => filterByNotEnrolled;
            set
            {
                if (filterByNotEnrolled != value)
                {
                    filterByNotEnrolled = value;
                    OnPropertyChanged();
                    ApplyAllFilters();
                }
            }
        }

        /// <summary>
        /// Command to reset all filters.
        /// </summary>
        public ICommand ResetAllFiltersCommand { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel"/> class.
        /// </summary>
        public MainViewModel(CoinsServiceProxy serviceProxy, CourseServiceProxy courseServiceProxy, int currentUserId = 1,
            ICourseService? courseService = null, ICoinsService? coinsService = null)
        {
            this.CurrentUserId = currentUserId;
            this.courseService = courseService ?? new CourseService(courseServiceProxy);
            this.coinsService = coinsService ?? new CoinsService(serviceProxy);

            InitializeAsync();
        }

        private async void InitializeAsync()
        {
            try
            {
                var courseList = await this.courseService.GetCoursesAsync();
                DisplayedCourses = new ObservableCollection<Course>(courseList);
                AvailableTags = new ObservableCollection<Tag>(await this.courseService.GetTagsAsync());
                foreach (var tag in AvailableTags)
                {
                    tag.PropertyChanged += OnTagSelectionChanged;
                }

                ResetAllFiltersCommand = new RelayCommand(ResetAllFilters);

                await RefreshUserCoinBalanceAsync();
            }
            catch (Exception e)
            {
                RaiseErrorMessage("Failed to load courses", e.Message);
            }
        }

        /// <summary>
        /// Attempts to grant a daily login reward to the user.
        /// </summary>
        public async Task<bool> TryDailyLoginReward()
        {
            try
            {
                bool loginRewardGranted = await coinsService.ApplyDailyLoginBonusAsync(CurrentUserId);
                await RefreshUserCoinBalanceAsync();
                return loginRewardGranted;
            }
            catch (Exception ex)
            {
                RaiseErrorMessage("Daily login reward failed", ex.Message);
                return false;
            }
        }

        private void OnTagSelectionChanged(object? sender, PropertyChangedEventArgs eventArgs)
        {
            if (eventArgs.PropertyName == nameof(Tag.IsSelected))
            {
                ApplyAllFilters();
            }
        }

        /// <summary>
        /// Resets all the filters and clears the search query.
        /// </summary>
        private void ResetAllFilters(object? parameter)
        {
            try
            {
                SearchQuery = string.Empty;
                FilterByPremium = false;
                FilterByFree = false;
                FilterByEnrolled = false;
                FilterByNotEnrolled = false;

                foreach (var tag in AvailableTags)
                {
                    tag.IsSelected = false;
                }

                ApplyAllFilters();
            }
            catch (Exception ex)
            {
                RaiseErrorMessage("Reset filters failed", ex.Message);
            }
        }

        /// <summary>
        /// Applies all filters based on search query, selected tags, and filter flags.
        /// </summary>
        private async void ApplyAllFilters()
        {
            try
            {
                if (AvailableTags == null)
                {
                    return;
                }

                DisplayedCourses.Clear();

                var selectedTagIds = AvailableTags
                    .Where(tag => tag.IsSelected)
                    .Select(tag => tag.TagId)
                    .ToList();

                var filteredCourses = await courseService.GetFilteredCoursesAsync(
                    searchQuery,
                    filterByPremium,
                    filterByFree,
                    filterByEnrolled,
                    filterByNotEnrolled,
                    selectedTagIds,
                    CurrentUserId);

                foreach (var course in filteredCourses)
                {
                    DisplayedCourses.Add(course);
                }
            }
            catch (Exception e)
            {
                RaiseErrorMessage("Failed to apply filters", e.Message);
            }
        }
    }
}
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Collections.Generic;
using Duo.Commands;
using DuoClassLibrary.Models;
using DuoClassLibrary.Services;
using Windows.System.Threading;
using DuoClassLibrary.Services.Interfaces;

#pragma warning disable IDE0028, CS8618, CS8602, CS8601, IDE0060

namespace Duo.ViewModels
{
    /// <summary>
    /// ViewModel responsible for managing the main application logic, including course display, filtering, and user coin balance.
    /// </summary>
    public partial class MainViewModel : BaseViewModel, IMainViewModel
    {
        private int CurrentUserId { get; init; }

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
        private ObservableCollection<Tag> availableTags;
        public ObservableCollection<Tag> AvailableTags
        {
            get => availableTags;
            set
            {
                if (availableTags != value)
                {
                    // Unsubscribe from old tags
                    if (availableTags != null)
                    {
                        foreach (var tag in availableTags)
                        {
                            tag.PropertyChanged -= Tag_PropertyChanged;
                        }
                    }

                    availableTags = value;
                    OnPropertyChanged(nameof(AvailableTags));

                    // Subscribe to new tags
                    if (availableTags != null)
                    {
                        foreach (var tag in availableTags)
                        {
                            tag.PropertyChanged += Tag_PropertyChanged;
                        }
                    }
                }
            }
        }

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
        /// Flag to indicate if filters are being updated in a batch process to prevent unnecessary re-filtering during updates.
        /// </summary>
        private bool isUpdatingFiltersBatch = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="MainViewModel"/> class.
        /// </summary>
        public MainViewModel(CoinsServiceProxy serviceProxy, CourseServiceProxy courseServiceProxy, int currentUserId,
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
                // Initialize the command first to ensure it's available
                ResetAllFiltersCommand = new RelayCommand(ResetAllFilters);

                var courseList = await this.courseService.GetCoursesAsync();
                // Load tags for each course
                foreach (var course in courseList)
                {
                    course.Tags = await this.courseService.GetCourseTagsAsync(course.CourseId);
                }
                DisplayedCourses = new ObservableCollection<Course>(courseList);
                AvailableTags = new ObservableCollection<Tag>(await this.courseService.GetTagsAsync());

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

        /// <summary>
        /// Resets all the filters and clears the search query.
        /// </summary>
        private void ResetAllFilters(object? parameter)
        {
            try
            {
                this.isUpdatingFiltersBatch = true;
                SearchQuery = string.Empty;
                FilterByPremium = false;
                FilterByFree = false;
                FilterByEnrolled = false;
                FilterByNotEnrolled = false;

                foreach (var tag in AvailableTags)
                {
                    tag.IsSelected = false;
                }
                this.isUpdatingFiltersBatch = false;
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

                var existingCourseTags = CacheExistingCourseTags();
                DisplayedCourses.Clear();
                await LoadFilteredCoursesWithTags(existingCourseTags);
            }
            catch (Exception e)
            {
                RaiseErrorMessage("Failed to apply filters", e.Message);
            }
        }

        /// <summary>
        /// Saves the tags of currently displayed courses for reuse.
        /// </summary>
        private Dictionary<int, List<Tag>> CacheExistingCourseTags()
        {
            var tagCache = new Dictionary<int, List<Tag>>();
            foreach (var course in DisplayedCourses)
            {
                if (course.Tags != null && course.Tags.Any())
                {
                    tagCache[course.CourseId] = course.Tags.ToList();
                }
            }
            return tagCache;
        }

        /// <summary>
        /// Loads filtered courses with their tags, using cached tags when available.
        /// </summary>
        private async Task LoadFilteredCoursesWithTags(Dictionary<int, List<Tag>> tagCache)
        {
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
                if (tagCache.ContainsKey(course.CourseId))
                {
                    course.Tags = tagCache[course.CourseId];
                }
                else if (course.Tags == null || !course.Tags.Any())
                {
                    course.Tags = await courseService.GetCourseTagsAsync(course.CourseId);
                }

                DisplayedCourses.Add(course);
            }
        }

        /// <summary>
        /// Handles property changes for tags to reapply filters when a tag's selection state changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Tag_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(Tag.IsSelected))
            {
                if(!isUpdatingFiltersBatch)
                {
                    // If not in batch update mode, apply filters immediately
                    ApplyAllFilters();
                }
            }
        }

        /// <summary>
        /// </summary>
        public void ResetAllFiltersPublic()
        {
            ResetAllFilters(null);
        }
    }
}
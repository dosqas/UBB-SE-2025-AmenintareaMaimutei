using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System;

using DuoClassLibrary.Models;
using static Duo.App;
using System.Collections.Generic;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI;
using Microsoft.UI.Xaml.Data;
using Duo.ViewModels;
using DuoClassLibrary.Helpers;

namespace Duo.Views.Components
{
    // Community item class with selection state
    public class CommunityItem : INotifyPropertyChanged
    {
        private int _id;
        private string _name;
        private bool _isSelected;

        public int Id 
        { 
            get => _id; 
            set 
            { 
                if (_id != value)
                {
                    _id = value;
                    OnPropertyChanged();
                }
            } 
        }
        
        public string Name 
        { 
            get => _name; 
            set 
            { 
                if (_name != value)
                {
                    _name = value;
                    OnPropertyChanged();
                }
            } 
        }
        
        public bool IsSelected 
        { 
            get => _isSelected; 
            set 
            { 
                if (_isSelected != value)
                {
                    _isSelected = value;
                    OnPropertyChanged();
                }
            } 
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    // Bool to background color converter
    public class BoolToBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is bool isSelected && isSelected)
            {
                return new SolidColorBrush(Colors.DodgerBlue);
            }
            return new SolidColorBrush(Colors.LightGray);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    // Integer to visibility converter
    public class IntToVisConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is int count)
            {
                return count > 0 ? Visibility.Visible : Visibility.Collapsed;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public sealed partial class PostDialogContent : UserControl
    {
        // Constants for validation and defaults
        private const int MAX_HASHTAGS = 5;
        
        // Validation TextBlocks
        private bool _isTitleValid = true;
        private bool _isContentValid = true;
        private bool _isHashtagValid = true;

        // ViewModel
        private PostCreationViewModel _viewModel;
        
        public PostCreationViewModel ViewModel
        {
            get { return _viewModel; }
            set
            {
                if (_viewModel != null)
                {
                    _viewModel.PropertyChanged -= ViewModel_PropertyChanged;
                }
                
                _viewModel = value;
                DataContext = _viewModel;
                
                if (_viewModel != null)
                {
                    _viewModel.PropertyChanged += ViewModel_PropertyChanged;
                }
                
                UpdateUIVisibility();
            }
        }

        public PostDialogContent()
        {
            this.InitializeComponent();
            
            // Initialize ViewModel
            ViewModel = new PostCreationViewModel();
            
            // Subscribe to the post creation success event
            ViewModel.PostCreationSuccessful += ViewModel_PostCreationSuccessful;
            
            // Ensure UI is updated
            UpdateUIVisibility();
        }

        private void ViewModel_PostCreationSuccessful(object sender, EventArgs e)
        {
            // This method can be used to handle successful post creation
            // For example, close the dialog or show a success message
        }

        public void SetSelectedCommunity(int communityId)
        {
            ViewModel.SelectedCategoryId = communityId;
        }

        public void DisableCommunitySelection()
        {
            // Make communities non-clickable
            if (CommunitiesRepeater != null)
            {
                // Disable all community buttons by finding each element in the ItemsRepeater
                for (int i = 0; i < ViewModel.Communities.Count; i++)
                {
                    if (CommunitiesRepeater.TryGetElement(i) is Button button)
                    {
                        button.IsEnabled = false;
                    }
                }
            }
            
            // Add visual indicator that community cannot be changed
            if (CommunitiesTitle != null)
            {
                CommunitiesTitle.Text = "Community (cannot be changed)";
            }
        }

        private void CommunityButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is int communityId)
            {
                ViewModel.SelectCommunity(communityId);
            }
        }

        #region Validation Methods
        
        private bool ValidateTitle()
        {
            HideError(TitleErrorTextBlock);
            
            if (string.IsNullOrWhiteSpace(ViewModel.Title))
                return true;
                
            var (isValid, errorMessage) = ValidationHelper.ValidatePostTitle(ViewModel.Title);
            
            if (!isValid)
            {
                ShowError(TitleErrorTextBlock, errorMessage);
                return false;
            }
            
            return true;
        }

        private bool ValidateContent()
        {
            HideError(ContentErrorTextBlock);
            
            if (string.IsNullOrWhiteSpace(ViewModel.Content))
                return true;
                
            var (isValid, errorMessage) = ValidationHelper.ValidatePostContent(ViewModel.Content);
            
            if (!isValid)
            {
                ShowError(ContentErrorTextBlock, errorMessage);
                return false;
            }
            
            return true;
        }

        private bool ValidateHashtag(string hashtag)
        {
            HideError(HashtagErrorTextBlock);
            
            // Only validate if there is content to validate
            if (string.IsNullOrWhiteSpace(hashtag))
                return true;
                
            var (isValid, errorMessage) = ValidationHelper.ValidateHashtagInput(hashtag);
            
            if (!isValid)
            {
                ShowError(HashtagErrorTextBlock, errorMessage);
                return false;
            }
            
            return true;
        }
        
        private void ShowError(TextBlock errorTextBlock, string errorMessage)
        {
            errorTextBlock.Text = errorMessage;
            errorTextBlock.Visibility = Visibility.Visible;
        }
        
        private void HideError(TextBlock errorTextBlock)
        {
            errorTextBlock.Text = string.Empty;
            errorTextBlock.Visibility = Visibility.Collapsed;
        }
        
        public bool IsFormValid()
        {
            bool isTitleValid = !string.IsNullOrWhiteSpace(ViewModel.Title) && ValidateTitle();
            bool isContentValid = !string.IsNullOrWhiteSpace(ViewModel.Content) && ValidateContent();
            bool isHashtagValid = true;
            bool isCommunitySelected = ViewModel.SelectedCategoryId > 0;
            
            if (!string.IsNullOrEmpty(HashtagTextBox.Text))
            {
                isHashtagValid = ValidateHashtag(HashtagTextBox.Text);
            }
            
            // Display error if no community is selected
            if (!isCommunitySelected)
            {
                ViewModel.LastError = "Please select a community for your post.";
            }
            else if (isTitleValid && isContentValid && isHashtagValid)
            {
                ViewModel.LastError = string.Empty;
            }
            
            return isTitleValid && isContentValid && isHashtagValid && isCommunitySelected;
        }
        #endregion

        #region Event Handlers
        private void TitleTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(ViewModel.Title))
            {
                _isTitleValid = ValidateTitle();
            }
            else
            {
                HideError(TitleErrorTextBlock);
            }
        }

        private void ContentTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(ViewModel.Content))
            {
                _isContentValid = ValidateContent();
            }
            else
            {
                HideError(ContentErrorTextBlock);
            }
        }
        
        private void HashtagTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(HashtagTextBox.Text))
            {
                _isHashtagValid = ValidateHashtag(HashtagTextBox.Text);
            }
            else
            {
                HideError(HashtagErrorTextBlock);
            }
        }
        
        private void AddHashtagButton_Click(object sender, RoutedEventArgs e)
        {
            AddHashtag();
        }

        private void HashtagTextBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (e.Key == Windows.System.VirtualKey.Enter)
            {
                AddHashtag();
                e.Handled = true;
            }
        }

        private void AddHashtag()
        {
            string hashtag = HashtagTextBox.Text.Trim();
            
            System.Diagnostics.Debug.WriteLine($"PostDialogContent.AddHashtag - Input text: '{hashtag}'");
            
            if (string.IsNullOrEmpty(hashtag))
            {
                System.Diagnostics.Debug.WriteLine("PostDialogContent.AddHashtag - Empty input, returning");
                return;
            }
                
            if (!ValidateHashtag(hashtag))
            {
                System.Diagnostics.Debug.WriteLine($"PostDialogContent.AddHashtag - Validation failed for '{hashtag}'");
                return;
            }
                
            // Check if maximum hashtag limit is reached
            if (ViewModel.Hashtags.Count >= MAX_HASHTAGS)
            {
                System.Diagnostics.Debug.WriteLine("PostDialogContent.AddHashtag - Max hashtags (5) reached");
                ShowError(HashtagErrorTextBlock, $"Maximum of {MAX_HASHTAGS} hashtags allowed per post.");
                return;
            }

            // Add to collection if not a duplicate
            if (!ViewModel.Hashtags.Contains(hashtag))
            {
                ViewModel.AddHashtag(hashtag);
                
                // Debug output
                System.Diagnostics.Debug.WriteLine($"PostDialogContent.AddHashtag - Added hashtag: {hashtag}, Count now: {ViewModel.Hashtags.Count}");
                
                // Explicitly check UI visibility
                HashtagsHeader.Visibility = ViewModel.Hashtags.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
            }
            else
            {
                System.Diagnostics.Debug.WriteLine($"PostDialogContent.AddHashtag - Hashtag '{hashtag}' already exists");
            }
                     
            // Clear the input
            HashtagTextBox.Text = string.Empty;
            HideError(HashtagErrorTextBlock);
            
            // Force UI update
            UpdateUIVisibility();
        }

        private void RemoveHashtag_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Content is string hashtag)
            {
                ViewModel.RemoveHashtag(hashtag);
                System.Diagnostics.Debug.WriteLine($"Removed hashtag: {hashtag}, Count now: {ViewModel.Hashtags.Count}");
                
                // Force UI update
                UpdateUIVisibility();
            }
        }
        #endregion

        private void UpdateUIVisibility()
        {
            // Debug output
            System.Diagnostics.Debug.WriteLine($"UpdateUIVisibility called. Hashtags count: {ViewModel?.Hashtags?.Count ?? 0}");
            
            // Explicitly update the hashtags header visibility
            if (HashtagsHeader != null)
            {
                bool hasHashtags = ViewModel != null && ViewModel.Hashtags != null && ViewModel.Hashtags.Count > 0;
                System.Diagnostics.Debug.WriteLine($"Setting HashtagsHeader visibility: {(hasHashtags ? "Visible" : "Collapsed")}");
                HashtagsHeader.Visibility = hasHashtags ? Visibility.Visible : Visibility.Collapsed;
            }
            
            // Update error TextBlock visibility
            if (ErrorTextBlock != null)
            {
                ErrorTextBlock.Visibility = (ViewModel != null && !string.IsNullOrWhiteSpace(ViewModel.LastError)) 
                    ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine($"Property changed: {e.PropertyName}");
            
            if (e.PropertyName == nameof(ViewModel.Hashtags) || 
                e.PropertyName == nameof(ViewModel.LastError) ||
                e.PropertyName == "Item[]") // This can be fired for collection changes
            {
                UpdateUIVisibility();
            }
        }
    }
}
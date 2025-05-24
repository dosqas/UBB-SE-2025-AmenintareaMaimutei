using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.WinUI.UI.Controls;

// is this the right way to access userService and its methods?
using static Duo.App;
using Duo.Views.Pages;
using Duo.ViewModels;
namespace Duo.Views.Components
{
    public sealed partial class Post : UserControl
    {
        // Constants
        private const string UNKNOWN_USER_TEXT = "Unknown User";
        
        public static readonly DependencyProperty UsernameProperty = 
            DependencyProperty.Register(nameof(Username), typeof(string), typeof(Post), new PropertyMetadata(""));
        
        public static readonly DependencyProperty DateProperty = 
            DependencyProperty.Register(nameof(Date), typeof(string), typeof(Post), new PropertyMetadata(""));
        
        public static readonly DependencyProperty TitleProperty = 
            DependencyProperty.Register(nameof(Title), typeof(string), typeof(Post), new PropertyMetadata(""));
        
        public static new readonly DependencyProperty ContentProperty = 
            DependencyProperty.Register(nameof(Content), typeof(string), typeof(Post), new PropertyMetadata(""));
        
        public static readonly DependencyProperty LikeCountProperty = 
            DependencyProperty.Register(nameof(LikeCount), typeof(int), typeof(Post), new PropertyMetadata(0));
        
        public static readonly DependencyProperty HashtagsProperty = 
            DependencyProperty.Register(nameof(Hashtags), typeof(IEnumerable<string>), typeof(Post), new PropertyMetadata(null));

        public static readonly DependencyProperty PostIdProperty = 
            DependencyProperty.Register(nameof(PostId), typeof(int), typeof(Post), new PropertyMetadata(0));

        public static readonly DependencyProperty IsAlwaysHighlightedProperty = 
            DependencyProperty.Register(nameof(IsAlwaysHighlighted), typeof(bool), typeof(Post), new PropertyMetadata(false, OnIsAlwaysHighlightedChanged));
            
        private static void OnIsAlwaysHighlightedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is Post post)
            {
                post.UpdateHighlightState();
            }
        }

        private bool _isPointerOver;
        private LikeButton? _likeButton;

        public Post()
        {
            InitializeComponent();
            
            UpdateMoreOptionsVisibility();
            UpdateHighlightState();
            
            // Subscribe to the Loaded event
            Loaded += Post_Loaded;
        }
        
        private void Post_Loaded(object sender, RoutedEventArgs e)
        {
            // Find and connect to the LikeButton
            _likeButton = FindDescendant<LikeButton>(this);
            if (_likeButton != null)
            {
                _likeButton.LikeClicked += LikeButton_LikeClicked;
            }
        }
        
        // Find a descendant control of a specific type
        private T FindDescendant<T>(DependencyObject parent) where T : DependencyObject
        {
            int childCount = VisualTreeHelper.GetChildrenCount(parent);
            
            for (int i = 0; i < childCount; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(parent, i);
                
                if (child is T result)
                {
                    return result;
                }
                
                T descendant = FindDescendant<T>(child);
                if (descendant != null)
                {
                    return descendant;
                }
            }
            
            return null;
        }
        
        private async void LikeButton_LikeClicked(object sender, LikeButtonClickedEventArgs e)
        {
            if (e.TargetType == LikeTargetType.Post && e.TargetId == PostId)
            {
                try
                {
                    if (await App._postService.LikePost(PostId))
                    {
                       // LikeCount++;
                        
                        if (_likeButton != null)
                        {
                            _likeButton.IncrementLikeCount();
                        }
                        
                        System.Diagnostics.Debug.WriteLine($"Post liked: ID {PostId}, new count: {LikeCount}");
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error liking post: {ex.Message}");
                }
            }
        }

        private void UpdateMoreOptionsVisibility()
        {
            var currentUser = userService.GetCurrentUser();
            if (currentUser != null)
            {
                MoreOptions.Visibility = (this.Username == currentUser.UserName) 
                    ? Visibility.Visible 
                    : Visibility.Collapsed;
            }
            else
            {
                MoreOptions.Visibility = Visibility.Collapsed;
            }
        }

        // Handle pointer entered event for hover effects
        private void PostBorder_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            _isPointerOver = true;
            
            if (!IsAlwaysHighlighted) 
            {
                if (sender is Border border)
                {
                    border.Background = Application.Current.Resources["SystemControlBackgroundAltHighBrush"] as Microsoft.UI.Xaml.Media.Brush;
                    border.BorderBrush = Application.Current.Resources["SystemControlBackgroundListLowBrush"] as Microsoft.UI.Xaml.Media.Brush;
                }
            }
        }

        // Handle pointer exited event for hover effects
        private void PostBorder_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            _isPointerOver = false;
            
            if (!IsAlwaysHighlighted) 
            {
                if (sender is Border border)
                {
                    border.Background = new Microsoft.UI.Xaml.Media.SolidColorBrush(
                        Microsoft.UI.Colors.Transparent);
                    border.BorderBrush = new Microsoft.UI.Xaml.Media.SolidColorBrush(
                        Microsoft.UI.Colors.Transparent);
                }
            }
        }

        // Handle tapped event for navigation
        private void PostBorder_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (IsAlwaysHighlighted)
            {
                return;
            }
            
            // Check if the tap originated from a LikeButton or its children
            if (IsLikeButtonTap(e.OriginalSource as DependencyObject))
            {
                // Skip navigation if the tap was on the like button
                return;
            }
            
            // Get the parent frame for navigation
            var frame = FindParentFrame();
            if (frame != null)
            {
                // Create a Post with the current post's data
                var post = new DuoClassLibrary.Models.Post
                {
                    Title = this.Title ?? string.Empty,
                    Description = this.Content ?? string.Empty,
                    Username = this.Username,
                    Date = this.Date,
                    LikeCount = this.LikeCount
                };

                // Copy hashtags
                if (this.Hashtags != null)
                {
                    foreach (var hashtag in this.Hashtags)
                    {
                        post.Hashtags.Add(hashtag);
                    }
                }

                // Navigate to the post detail page
                frame.Navigate(typeof(PostDetailPage), post);
            }
        }
        
        // Helper method to determine if a tap originated from the LikeButton
        private bool IsLikeButtonTap(DependencyObject element)
        {
            // If null, it can't be the LikeButton
            if (element == null)
                return false;
                
            // Check if the element is a LikeButton
            if (element is LikeButton)
                return true;
                
            // Check parent hierarchy
            DependencyObject parent = VisualTreeHelper.GetParent(element);
            while (parent != null && !(parent is Post))
            {
                if (parent is LikeButton)
                    return true;
                    
                parent = VisualTreeHelper.GetParent(parent);
            }
            
            return false;
        }

        // Helper method to find the parent Frame
        private Frame FindParentFrame()
        {
            DependencyObject parent = this;
            while (parent != null && !(parent is Frame))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }
            return parent as Frame;
        }

        // Event handlers for the MoreDropdown component
        private async void MoreOptions_EditClicked(object sender, RoutedEventArgs e)
        {
            // Verify that the current user is the owner of the post            
            if(this.Username != $"{userService.GetCurrentUser().UserName}")
            {
                // Display an error dialog if the user is not the owner
                ContentDialog errorDialog = new ContentDialog
                {
                    XamlRoot = this.XamlRoot,
                    Title = "Error",
                    Content = "You do not have permission to edit this item.",
                    CloseButtonText = "OK"
                };
                await errorDialog.ShowAsync();
                return;
            }

            // Handle the edit logic here
            // Display Edit Post dialog with prefilled data
            var dialogComponent = new DialogComponent();
            
            var post = await _postService.GetPostById(this.PostId);
            if (post == null)
            {
                var errorDialog = new ContentDialog
                {
                    XamlRoot = this.XamlRoot,
                    Title = "Error",
                    Content = "Post not found in database",
                    CloseButtonText = "OK"
                };
                await errorDialog.ShowAsync();
                return;
            }
            
            var result = await dialogComponent.ShowEditPostDialog(
                this.XamlRoot, 
                this.Title,          // Pass the current post title
                this.Content,        // Pass the current post content
                [.. this.Hashtags],  // Convert IEnumerable<string> to List<string> before passing
                post.CategoryID      // Pass the current post's category ID
            );

            // If the dialog returned successfully, update the post with the new data
            if (result.Success)
            {
                try
                {
                    // Check if category was changed (should not be possible with UI changes, but double-check)
                    if (post.CategoryID != result.CommunityId)
                    {
                        ContentDialog errorDialog = new ContentDialog
                        {
                            XamlRoot = this.XamlRoot,
                            Title = "Error",
                            Content = "Changing the post's community/category is not allowed.",
                            CloseButtonText = "OK"
                        };
                        await errorDialog.ShowAsync();
                        return;
                    }
                    
                    // Update the post properties
                    post.Title = result.Title;
                    post.Description = result.Content;
                    post.UpdatedAt = DateTime.UtcNow;
                    
                    // Call the service to update the post
                    await _postService.UpdatePost(post);
                    
                    // Update hashtags
                    try {
                        // First clear existing hashtags and then add new ones
                        var existingHashtags = await _postService.GetHashtagsByPostId(this.PostId);
                        foreach (var hashtag in existingHashtags)
                        {
                            await _postService.RemoveHashtagFromPost(this.PostId, hashtag.Id, userService.GetCurrentUser().UserId);
                        }

                        // Add new hashtags
                        foreach (var hashtag in result.Hashtags)
                        {
                            try
                            {
                                var hashtagService = App._hashtagService;
                                var userId = userService.GetCurrentUser().UserId;
                                
                                var existingHashtag = await hashtagService.GetHashtagByName(hashtag);
                                
                                if (existingHashtag == null)
                                {
                                    var newHashtag = hashtagService.CreateHashtag(hashtag);
                                    await hashtagService.AddHashtagToPost(this.PostId, newHashtag.Id);
                                }
                                else
                                {
                                    await hashtagService.AddHashtagToPost(this.PostId, existingHashtag.Id);
                                }
                            }
                            catch (Exception tagEx)
                            {
                                System.Diagnostics.Debug.WriteLine($"Error processing hashtag '{hashtag}': {tagEx.Message}");
                            }
                        }
                    
                        this.Hashtags = result.Hashtags;
                    }
                    catch (Exception hashtagEx)
                    {
                        System.Diagnostics.Debug.WriteLine($"Error updating hashtags: {hashtagEx.Message}");
                    }
                    
                    // Update the UI elements with the new data
                    this.Title = result.Title;
                    this.Content = result.Content;
                    
                    // Show success message
                    ContentDialog successDialog = new ContentDialog
                    {
                        XamlRoot = this.XamlRoot,
                        Title = "Updated",
                        Content = "The post has been successfully updated.",
                        CloseButtonText = "OK"
                    };
                    await successDialog.ShowAsync();
                }
                catch (Exception ex)
                {
                    // Handle error
                    ContentDialog errorDialog = new ContentDialog
                    {
                        XamlRoot = this.XamlRoot,
                        Title = "Error",
                        Content = "An error occurred while updating the post\n" + ex.Message,
                        CloseButtonText = "OK"
                    };
                    await errorDialog.ShowAsync();
                }
            }
        }

        private async void MoreOptions_DeleteClicked(object sender, RoutedEventArgs e)
        {
            if(this.Username != $"{userService.GetCurrentUser().UserName}")
            {
                // Display an error dialog if the user is not the owner
                ContentDialog errorDialog = new ContentDialog
                {
                    XamlRoot = this.XamlRoot,
                    Title = "Error",
                    Content = "You do not have permission to delete this item.",
                    CloseButtonText = "OK"
                };
                await errorDialog.ShowAsync();
                return;
            }

            // Instantiate a DeleteDialog
            var deleteDialog = new DialogComponent();

            // Create the deletion confirmation (add whatever text you wish to display)
            bool isConfirmed = await deleteDialog.ShowConfirmationDialog(
                "Confirm Deletion",
                "Are you sure you want to delete this item?",
                this.XamlRoot
            );
                
            if (isConfirmed)
            {
                try {
                await _postService.DeletePost(this.PostId);
                }
                catch (Exception ex)
                {
                    ContentDialog errorDialog = new ContentDialog
                    {
                        XamlRoot = this.XamlRoot,
                        Title = "Error",
                        Content = "An error occurred while deleting the item. Please try again.\n" + ex.Message,
                        CloseButtonText = "OK"
                    };
                    await errorDialog.ShowAsync();
                    return;
                }

                ContentDialog successDialog = new ContentDialog
                {
                    XamlRoot = this.XamlRoot,
                    Title = "Deleted",
                    Content = "The item has been successfully deleted.",
                    CloseButtonText = "OK"
                };
                await successDialog.ShowAsync();

                var frame = FindParentFrame();
                 if (frame != null)
                 {
                     if (frame.Content is Duo.Views.Pages.PostDetailPage)
                     {
                         if (frame.CanGoBack)
                         {
                             frame.GoBack();
                         }
                     }
                     else if (frame.Content is Duo.Views.Pages.PostListPage postListPage)
                     {
                         var viewModel = postListPage.DataContext as Duo.ViewModels.PostListViewModel;
                         if (viewModel != null)
                         {
                             await viewModel.LoadPosts();
                         }
                     }
                     else if (frame.Content is Duo.Views.Pages.CategoryPage categoryPage)
                     {
                         categoryPage.RefreshCurrentView();
                     }
                 }
            }
        }

        // Event handlers for MarkdownTextBlock
        private void MarkdownText_MarkdownRendered(object sender, CommunityToolkit.WinUI.UI.Controls.MarkdownRenderedEventArgs e)
        {
            // This event is fired when the markdown content is rendered
            // You can perform additional actions here if needed
        }

        private async void MarkdownText_LinkClicked(object sender, CommunityToolkit.WinUI.UI.Controls.LinkClickedEventArgs e)
        {
            // Handle link clicks in markdown text
            // For example, you might want to open URLs in the default browser
            if (Uri.TryCreate(e.Link, UriKind.Absolute, out Uri? uri))
            {
                // Launch the URI in the default browser
                await Windows.System.Launcher.LaunchUriAsync(uri);
            }
        }

        private void UpdateHighlightState()
        {
            if (PostBorder != null)
            {
                if (IsAlwaysHighlighted)
                {
                    PostBorder.Background = Application.Current.Resources["SystemControlBackgroundAltHighBrush"] as Microsoft.UI.Xaml.Media.Brush;
                    PostBorder.BorderBrush = Application.Current.Resources["SystemControlBackgroundListLowBrush"] as Microsoft.UI.Xaml.Media.Brush;
                }
                else if (!_isPointerOver) // Only reset if not being hovered
                {
                    PostBorder.Background = new Microsoft.UI.Xaml.Media.SolidColorBrush(
                        Microsoft.UI.Colors.Transparent);
                    PostBorder.BorderBrush = new Microsoft.UI.Xaml.Media.SolidColorBrush(
                        Microsoft.UI.Colors.Transparent);
                }
            }
        }

        public string Username
        {
            get => (string)GetValue(UsernameProperty);
            set 
            { 
                SetValue(UsernameProperty, value);
                // Update visibility when username changes
                UpdateMoreOptionsVisibility();
            }
        }

        public string Date
        {
            get => (string)GetValue(DateProperty);
            set => SetValue(DateProperty, value);
        }

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public new string Content
        {
            get => (string)GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);
        }

        public int LikeCount
        {
            get => (int)GetValue(LikeCountProperty);
            set => SetValue(LikeCountProperty, value);
        }

        public IEnumerable<string> Hashtags
        {
            get => (IEnumerable<string>)GetValue(HashtagsProperty);
            set => SetValue(HashtagsProperty, value);
        }

        public int PostId
        {
            get { return (int)GetValue(PostIdProperty); }
            set { SetValue(PostIdProperty, value); }
        }
        
        public bool IsAlwaysHighlighted
        {
            get { return (bool)GetValue(IsAlwaysHighlightedProperty); }
            set { SetValue(IsAlwaysHighlightedProperty, value); }
        }
    }
} 
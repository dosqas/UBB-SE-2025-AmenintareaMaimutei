using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Duo.Views.Components
{
    public sealed partial class DialogComponent
    {
        public async Task<bool> ShowConfirmationDialog(string title, string content, XamlRoot xamlRoot)
        {
            var dialogContent = new DialogContent
            {
                ContentText = content
            };

            ContentDialog dialog = new ContentDialog
            {
                XamlRoot = xamlRoot,
                Title = title,
                Content = dialogContent,
                PrimaryButtonText = "Yes",
                CloseButtonText = "No",
                DefaultButton = ContentDialogButton.Primary
            };

            dialog.PrimaryButtonStyle = Application.Current.Resources["AccentButtonStyle"] as Style;

            ContentDialogResult result = await dialog.ShowAsync();
            return result == ContentDialogResult.Primary;
        }

        public async Task<(bool Success, string Title, string Content, List<string> Hashtags, int CommunityId)> ShowCreatePostDialog(XamlRoot xamlRoot, int selectedCommunityId = 0)
        {
            var dialogContent = new PostDialogContent();
            
            // Set the initially selected community if provided
            if (selectedCommunityId > 0)
            {
                dialogContent.SetSelectedCommunity(selectedCommunityId);
            }

            // Add a success handler that will close the dialog
            bool succeeded = false;
            dialogContent.ViewModel.PostCreationSuccessful += (s, e) => {
                succeeded = true;
            };

            ContentDialog dialog = new ContentDialog
            {
                XamlRoot = xamlRoot,
                Title = "Create New Post",
                Content = dialogContent,
                PrimaryButtonText = "Create",
                CloseButtonText = "Cancel",
                DefaultButton = ContentDialogButton.Primary,
                MinWidth = 500,
                MinHeight = 450
            };

            // Apply accent button style to the create button
            dialog.PrimaryButtonStyle = Application.Current.Resources["AccentButtonStyle"] as Style;
            
            dialog.PrimaryButtonClick += async (s, e) => 
            {
                if (!dialogContent.IsFormValid())
                {
                    e.Cancel = true;
                    return;
                }
                
                // Get hashtags from the dialog
                var hashtagsList = new List<string>(dialogContent.ViewModel.Hashtags);
                
                // Debug output for hashtags
                System.Diagnostics.Debug.WriteLine($"DialogComponent: Creating post with {hashtagsList.Count} hashtags:");
                foreach (var tag in hashtagsList)
                {
                    System.Diagnostics.Debug.WriteLine($"  - {tag}");
                }
                
                // Prevent the dialog from closing automatically
                e.Cancel = true;
                
                // Use CreatePostAsync with the collected hashtags
                bool result = await dialogContent.ViewModel.CreatePostAsync(
                    dialogContent.ViewModel.Title,
                    dialogContent.ViewModel.Content,
                    dialogContent.ViewModel.SelectedCategoryId,
                    hashtagsList
                );
                
                if (result)
                {
                    // Successfully created - let the dialog close
                    succeeded = true;
                    
                    // Manually close the dialog
                    dialog.Hide();
                }
                else
                {
                    // If there was an error in post creation, show the error
                    if (!string.IsNullOrEmpty(dialogContent.ViewModel.LastError))
                    {
                        // Show error message but keep dialog open
                        System.Diagnostics.Debug.WriteLine($"DialogComponent: Post creation failed: {dialogContent.ViewModel.LastError}");
                    }
                }
            };
            
            ContentDialogResult result = await dialog.ShowAsync();
            
            if (result == ContentDialogResult.Primary || succeeded)
            {
                // Create a new list to return the hashtags
                var hashtagsList = new List<string>(dialogContent.ViewModel.Hashtags);

                return (true, dialogContent.ViewModel.Title, dialogContent.ViewModel.Content, hashtagsList, dialogContent.ViewModel.SelectedCategoryId);
            }

            return (false, string.Empty, string.Empty, new List<string>(), 0);
        }

        public async Task<(bool Success, string Title, string Content, List<string> Hashtags, int CommunityId)> ShowEditPostDialog(XamlRoot xamlRoot, string title = "", string content = "", List<string> hashtags = null, int communityId = 0)
        {
            var dialogContent = new PostDialogContent();
            
            // Prefill the dialog with existing post data
            dialogContent.ViewModel.Title = title;
            dialogContent.ViewModel.Content = content;
            
            // Add existing hashtags if provided
            if (hashtags != null)
            {
                foreach (var hashtag in hashtags)
                {
                    dialogContent.ViewModel.AddHashtag(hashtag);
                }
            }
            
            // Set community if provided and disable changing it
            if (communityId > 0)
            {
                dialogContent.SetSelectedCommunity(communityId);
                // Disable changing community in edit mode
                dialogContent.DisableCommunitySelection();
            }

            ContentDialog dialog = new ContentDialog
            {
                XamlRoot = xamlRoot,
                Title = "Edit Post",
                Content = dialogContent,
                PrimaryButtonText = "Save",
                CloseButtonText = "Cancel",
                DefaultButton = ContentDialogButton.Primary,
                MinWidth = 500,
                MinHeight = 450
            };

            // Apply accent button style to the save button
            dialog.PrimaryButtonStyle = Application.Current.Resources["AccentButtonStyle"] as Style;
            
            dialog.PrimaryButtonClick += (s, e) => 
            {
                if (!dialogContent.IsFormValid())
                {
                    e.Cancel = true;
                    return;
                }
                
                // Check if user tried to change the category (comparing with original communityId)
                if (communityId > 0 && dialogContent.ViewModel.SelectedCategoryId != communityId)
                {
                    dialogContent.ViewModel.LastError = "Changing community/category is not allowed when editing a post.";
                    e.Cancel = true;
                    return;
                }
                
                // For edits, just validate and let the dialog close
                // No need to execute a command, as the edited data will be returned to the caller
                // We explicitly return here and don't cancel the event, so the dialog will close
            };
            
            ContentDialogResult result = await dialog.ShowAsync();
            
            if (result == ContentDialogResult.Primary)
            {
                // Create a new list to return the hashtags
                var hashtagsList = new List<string>(dialogContent.ViewModel.Hashtags);

                return (true, dialogContent.ViewModel.Title, dialogContent.ViewModel.Content, hashtagsList, dialogContent.ViewModel.SelectedCategoryId);
            }

            return (false, string.Empty, string.Empty, new List<string>(), 0);
        }
    }
}
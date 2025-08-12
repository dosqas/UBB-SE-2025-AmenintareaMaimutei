using DuoClassLibrary.Models;
using Duo.Services;
using Duo.Views.Components;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Duo.ViewModels;
using static Duo.App;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI;
using DuoClassLibrary.Services.Interfaces;

namespace Duo.Views.Pages
{
    public sealed partial class PostDetailPage : Page
    {
        // Constants
        private const int INVALID_ID = 0;
        private const int DEFAULT_MARGIN = 16;
        
        private readonly ICommentService _commentService;

        public PostDetailPage()
        {
            this.InitializeComponent();

            _commentService = App._commentService;

            ViewModel.CommentsPanel = CommentsPanel;

            ViewModel.CommentsLoaded += ViewModel_CommentsLoaded;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is DuoClassLibrary.Models.Post post && post.Id > INVALID_ID)
            {
                ViewModel.LoadPostDetails(post.Id);
            }
            else
            {
                TextBlock errorText = new TextBlock
                {
                    Text = "Invalid post data received",
                    Foreground = new SolidColorBrush(Colors.Red),
                    Margin = new Thickness(0, 20, 0, 0)
                };
                CommentsPanel.Children.Add(errorText);
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.Frame.CanGoBack)
            {
                this.Frame.GoBack();
            }
        }

        private void CommentInputControl_CommentSubmitted(object sender, EventArgs e)
        {
            if (sender is CommentInput commentInput && !string.IsNullOrWhiteSpace(commentInput.CommentText))
            {
                if (ViewModel.AddCommentCommand.CanExecute(commentInput.CommentText))
                {
                    ViewModel.AddCommentCommand.Execute(commentInput.CommentText);
                    commentInput.ClearComment();
                }
            }
        }

        private void ViewModel_CommentsLoaded(object sender, EventArgs e)
        {
            RenderComments();
        }

        private void RenderComments()
        {
            CommentsPanel.Children.Clear();
            
            if (!ViewModel.HasComments)
            {
                TextBlock noCommentsText = new TextBlock
                {
                    Text = "No comments yet. Be the first to comment!",
                    Margin = new Thickness(0, DEFAULT_MARGIN, 0, DEFAULT_MARGIN)
                };
                CommentsPanel.Children.Add(noCommentsText);
                return;
            }
            
            if (!string.IsNullOrEmpty(ViewModel.ErrorMessage))
            {
                TextBlock errorText = new TextBlock
                {
                    Text = ViewModel.ErrorMessage,
                    Foreground = new SolidColorBrush(Colors.Red),
                    Margin = new Thickness(0, DEFAULT_MARGIN, 0, DEFAULT_MARGIN)
                };
                CommentsPanel.Children.Add(errorText);
                return;
            }
            
            foreach (var commentViewModel in ViewModel.CommentViewModels)
            {
                var commentComponent = new Views.Components.Comment();
                commentComponent.DataContext = commentViewModel;
                
                commentComponent.ReplySubmitted += CommentComponent_ReplySubmitted;
                commentComponent.CommentLiked += CommentComponent_CommentLiked;
                commentComponent.CommentDeleted += CommentComponent_CommentDeleted;
                
                CommentsPanel.Children.Add(commentComponent);
            }
        }

        private void CommentComponent_ReplySubmitted(object sender, CommentReplyEventArgs e)
        {
            var parameters = new Tuple<int, string>(e.ParentCommentId, e.ReplyText);
            ViewModel.AddReplyCommand.Execute(parameters);
        }

        private async void CommentComponent_CommentLiked(object sender, CommentLikedEventArgs e)
        {
            // Call the ViewModel method to like the comment and persist it to the database
            await ViewModel.LikeCommentById(e.CommentId);
        }

        private async void CommentComponent_CommentDeleted(object sender, CommentDeletedEventArgs e)
        {
            await ViewModel.DeleteComment(e.CommentId);
            RenderComments();
        }
    }
}
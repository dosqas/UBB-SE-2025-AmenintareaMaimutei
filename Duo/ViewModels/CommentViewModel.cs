using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Duo.Commands;

using Duo.ViewModels.Base;
using Duo.Services;
using static Duo.App;
using DuoClassLibrary.Models;
using DuoClassLibrary.Helpers;

namespace Duo.ViewModels
{
    public class CommentViewModel : ViewModelBase
    {
        private DuoClassLibrary.Models.Comment _comment;
        private ObservableCollection<CommentViewModel> _replies;
        private bool _isExpanded = true;
        private string _replyText;
        private bool _isReplyVisible;
        private int _likeCount;
        private bool _isDeleteButtonVisible;
        private bool _isReplyButtonVisible;
        private bool _isToggleButtonVisible;
        private string _toggleIconGlyph = "\uE109"; // Plus icon by default
        private const int MAX_NESTING_LEVEL = 3;

        // Events
        public event EventHandler<Tuple<int, string>> ?ReplySubmitted;

        public CommentViewModel(Comment comment, Dictionary<int, List<Comment>> repliesByParentId)
        {
            _comment = comment ?? throw new ArgumentNullException(nameof(comment));
            _replies = new ObservableCollection<CommentViewModel>();
            _likeCount = comment.LikeCount;
            _replyText = "";
            
            // Load any child comments/replies
            if (repliesByParentId != null && repliesByParentId.TryGetValue(comment.Id, out var childComments))
            {
                foreach (var reply in childComments)
                {
                    _replies.Add(new CommentViewModel(reply, repliesByParentId));
                }
            }
            
            ToggleRepliesCommand = new RelayCommand(ToggleReplies);
            ShowReplyFormCommand = new RelayCommand(ShowReplyForm);
            CancelReplyCommand = new RelayCommand(CancelReply);
            LikeCommentCommand = new RelayCommand(OnLikeComment);
        }

        public int Id => _comment.Id;
        public int UserId => _comment.UserId;
        public int? ParentCommentId => _comment.ParentCommentId;
        public string Content => _comment.Content;
        public string Username => _comment.Username;
        public string Date => DateTimeHelper.FormatDate(_comment.CreatedAt);
        public int Level => _comment.Level;
        
        public int LikeCount
        {
            get => _likeCount;
            set => SetProperty(ref _likeCount, value);
        }

        public ObservableCollection<CommentViewModel> Replies
        {
            get => _replies;
            set => SetProperty(ref _replies, value);
        }

        public bool IsExpanded
        {
            get => _isExpanded;
            set => SetProperty(ref _isExpanded, value);
        }

        public string ReplyText
        {
            get => _replyText;
            set => SetProperty(ref _replyText, value);
        }

        public bool IsReplyVisible
        {
            get => _isReplyVisible;
            set => SetProperty(ref _isReplyVisible, value);
        }

        public void LikeComment()
        {
            _comment.IncrementLikeCount();
            LikeCount = _comment.LikeCount;
        }

        public ICommand ToggleRepliesCommand { get; }
        public ICommand ShowReplyFormCommand { get; }
        public ICommand CancelReplyCommand { get; }
        public ICommand LikeCommentCommand { get; }

        private void ToggleReplies()
        {
            IsExpanded = !IsExpanded;
        }

        private void ShowReplyForm()
        {
            IsReplyVisible = true;
            ReplyText = string.Empty;
        }

        private void CancelReply()
        {
            IsReplyVisible = false;
            ReplyText = string.Empty;
        }

        private void SubmitReply()
        {
            if (!string.IsNullOrWhiteSpace(ReplyText))
            {
                ReplySubmitted?.Invoke(this, new Tuple<int, string>(Id, ReplyText));
                IsReplyVisible = false;
                ReplyText = string.Empty;
            }
        }

        private void OnLikeComment()
        {
            _comment.IncrementLikeCount();
        }
    }
}

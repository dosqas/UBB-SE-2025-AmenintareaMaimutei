using System;
using System.Windows.Input;
using Duo.Commands;
using Duo.ViewModels.Base;
using DuoClassLibrary.Helpers;


namespace Duo.ViewModels
{
    public class CommentCreationViewModel : ViewModelBase
    {
        private string _commentText;
        private string _errorMessage;
        private bool _isSubmitting;

        public event EventHandler CommentSubmitted;

        public CommentCreationViewModel()
        {
            SubmitCommentCommand = new RelayCommand(SubmitComment, CanSubmitComment);
        }

        public string CommentText
        {
            get => _commentText;
            set 
            {
                if (SetProperty(ref _commentText, value))
                {
                    try 
                    {
                        if (!string.IsNullOrWhiteSpace(value))
                        {
                            ValidationHelper.ValidateComment(value);
                            ErrorMessage = string.Empty;
                        }
                        else
                        {
                            ErrorMessage = "Comment cannot be empty.";
                        }
                    }
                    catch (ArgumentException ex)
                    {
                        ErrorMessage = ex.Message;
                    }
                    
                    (SubmitCommentCommand as RelayCommand)?.RaiseCanExecuteChanged();
                }
            }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        public bool IsSubmitting
        {
            get => _isSubmitting;
            set => SetProperty(ref _isSubmitting, value);
        }

        public ICommand SubmitCommentCommand { get; }

        private bool CanSubmitComment()
        {
            return !string.IsNullOrWhiteSpace(CommentText) && !IsSubmitting && string.IsNullOrEmpty(ErrorMessage);
        }

        private void SubmitComment()
        {
            if (!CanSubmitComment())
                return;

            IsSubmitting = true;
            
            try
            {
                try
                {
                    ValidationHelper.ValidateComment(CommentText);
                }
                catch (ArgumentException ex)
                {
                    ErrorMessage = ex.Message;
                    return;
                }

                // Notify subscribers that a comment has been submitted
                CommentSubmitted?.Invoke(this, EventArgs.Empty);
                
                // Clear the comment text
                ClearComment();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error submitting comment: {ex.Message}";
            }
            finally
            {
                IsSubmitting = false;
            }
        }

        public void ClearComment()
        {
            CommentText = string.Empty;
            ErrorMessage = string.Empty;
        }
    }
}

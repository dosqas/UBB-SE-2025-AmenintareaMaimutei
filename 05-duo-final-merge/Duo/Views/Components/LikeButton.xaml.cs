using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media.Animation;
using System;

namespace Duo.Views.Components
{
    public sealed partial class LikeButton : UserControl
    {
        public event EventHandler<LikeButtonClickedEventArgs> LikeClicked;

        public LikeButton()
        {
            this.InitializeComponent();
        }

        public static readonly DependencyProperty LikeCountProperty =
            DependencyProperty.Register(nameof(LikeCount), typeof(int), typeof(LikeButton), new PropertyMetadata(0));

        public int LikeCount
        {
            get { return (int)GetValue(LikeCountProperty); }
            set { SetValue(LikeCountProperty, value); }
        }

        public static readonly DependencyProperty IsLikedProperty =
            DependencyProperty.Register(nameof(IsLiked), typeof(bool), typeof(LikeButton), new PropertyMetadata(false));

        public bool IsLiked
        {
            get { return (bool)GetValue(IsLikedProperty); }
            set { SetValue(IsLikedProperty, value); }
        }

        public static readonly DependencyProperty PostIdProperty =
            DependencyProperty.Register(nameof(PostId), typeof(int), typeof(LikeButton), new PropertyMetadata(0));

        public int PostId
        {
            get { return (int)GetValue(PostIdProperty); }
            set { SetValue(PostIdProperty, value); }
        }

        public static readonly DependencyProperty CommentIdProperty =
            DependencyProperty.Register(nameof(CommentId), typeof(int), typeof(LikeButton), new PropertyMetadata(0));

        public int CommentId
        {
            get { return (int)GetValue(CommentIdProperty); }
            set { SetValue(CommentIdProperty, value); }
        }

        private void LikeButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var heartAnimation = this.Resources["HeartAnimation"] as Storyboard;
                if (heartAnimation != null)
                {
                    heartAnimation.Begin();
                }

                LikeButtonClickedEventArgs args;

                if (PostId > 0)
                {
                    args = new LikeButtonClickedEventArgs(LikeTargetType.Post, PostId);
                    
                }
                else if (CommentId > 0)
                {
                    args = new LikeButtonClickedEventArgs(LikeTargetType.Comment, CommentId);
                }
                else
                {
                    return;
                }

                LikeClicked?.Invoke(this, args);
            }
            catch (System.Exception ex)
            {
                
            }
        }

        private void LikeButton_Tapped(object sender, TappedRoutedEventArgs e)
        {
            e.Handled = true;
        }

        public void IncrementLikeCount()
        {
            LikeCount++;
        }
    }

    public class LikeButtonClickedEventArgs : EventArgs
    {
        public LikeTargetType TargetType { get; }
        public int TargetId { get; }

        public LikeButtonClickedEventArgs(LikeTargetType targetType, int targetId)
        {
            TargetType = targetType;
            TargetId = targetId;
        }
    }

    public enum LikeTargetType
    {
        Post,
        Comment
    }
}
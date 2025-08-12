using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DuoClassLibrary.Models
{
    public class Comment
    {
        private int _id;
        private string _content;
        private DateTime _createdAt = DateTime.Now;
        private int _postId;
        private int _userId;
        private int? _parentCommentId;
        private int _likeCount = 0;
        private int _level = 1;
        private string _username;

        public Comment(int id, string content, int userId, int postId, int? parentCommentId, DateTime createdAt, int likeCount, int level)
        {
            _id = id;
            _content = content;
            _userId = userId;
            _postId = postId;
            _parentCommentId = parentCommentId;
            _createdAt = createdAt;
            _likeCount = likeCount;
            _level = level;
            _username = string.Empty;
        }

        public Comment()
        {
            _id = 0;
            _content = string.Empty;
            _createdAt = DateTime.Now;
            _likeCount = 0;
            _level = 1;
            _username = string.Empty;
        }

        [Key]
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        [ForeignKey("Post")]
        public int PostId
        {
            get { return _postId; }
            set { _postId = value; }
        }

        [ForeignKey("User")]
        public int UserId
        {
            get { return _userId; }
            set { _userId = value; }
        }

        [ForeignKey("ParentComment")]
        public int? ParentCommentId
        {
            get { return _parentCommentId; }
            set { _parentCommentId = value; }
        }

        public string Content
        {
            get { return _content; }
            set { _content = value; }
        }

        public DateTime CreatedAt
        {
            get { return _createdAt; }
            set { _createdAt = value; }
        }

        public int LikeCount
        {
            get { return _likeCount; }
            set { _likeCount = value; }
        }

        public int Level
        {
            get { return _level; }
            set { _level = value; }
        }

        [NotMapped]
        public string Username
        {
            get { return _username; }
            set { _username = value; }
        }
        public void IncrementLikeCount()
        {
            LikeCount++;
        }
    }
}
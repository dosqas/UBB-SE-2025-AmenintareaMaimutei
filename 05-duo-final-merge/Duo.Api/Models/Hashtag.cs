using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Duo.Api.Models
{
    public class Hashtag
    {
        private int _id;
        private string _tag;
        public Hashtag() { }

        public Hashtag(int id, string tag)
        {
            _id = id;
            _tag = tag;
        }

        public Hashtag(string tag)
        {
            _tag = tag;
            _id = 0;
        }

        [Key]
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public string Tag
        {
            get { return _tag; }
            set { _tag = value; }
        }
    }
}
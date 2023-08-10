﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace EBookStoreAPI.Models.EFModels
{
    public partial class Articles
    {
        public Articles()
        {
            UserArticleCollections = new HashSet<UserArticleCollections>();
        }

        public int Id { get; set; }
        public int BookId { get; set; }
        public int WriterId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int PageViews { get; set; }
        public bool Status { get; set; }
        public DateTime CreatedTime { get; set; }

        public virtual Books Book { get; set; }
        public virtual Writers Writer { get; set; }
        public virtual ICollection<UserArticleCollections> UserArticleCollections { get; set; }
    }
}
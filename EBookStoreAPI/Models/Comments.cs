﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace EBookStoreAPI.Models
{
    public partial class Comments
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public int UserId { get; set; }
        public int Scores { get; set; }
        public string Content { get; set; }

        public virtual Books Book { get; set; }
        public virtual Users User { get; set; }
    }
}
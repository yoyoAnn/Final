﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace EBookStoreAPI.Models
{
    public partial class EbookOrders
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int EbookId { get; set; }
        public decimal Payment { get; set; }
        public DateTime CreatedTime { get; set; }

        public virtual Ebooks Ebook { get; set; }
        public virtual Users User { get; set; }
    }
}
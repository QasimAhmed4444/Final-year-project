﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Final_year_project.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class Quiz_ApplicationEntities2 : DbContext
    {
        public Quiz_ApplicationEntities2()
            : base("name=Quiz_ApplicationEntities2")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<student> students { get; set; }
        public virtual DbSet<tbl_admin> tbl_admin { get; set; }
        public virtual DbSet<tbl_questions> tbl_questions { get; set; }
        public virtual DbSet<tbl_setExam> tbl_setExam { get; set; }
        public virtual DbSet<tbl_category> tbl_category { get; set; }
    }
}

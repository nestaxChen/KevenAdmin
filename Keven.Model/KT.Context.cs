﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码已从模板生成。
//
//     手动更改此文件可能导致应用程序出现意外的行为。
//     如果重新生成代码，将覆盖对此文件的手动更改。
// </auto-generated>
//------------------------------------------------------------------------------

namespace Keven.Model
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class Entities : DbContext
    {
        public Entities()
            : base("name=Entities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<PD_Areas> PD_Areas { get; set; }
        public virtual DbSet<PD_BASETYPE> PD_BASETYPE { get; set; }
        public virtual DbSet<PD_TYPE> PD_TYPE { get; set; }
        public virtual DbSet<SYS_FUNCTION> SYS_FUNCTION { get; set; }
        public virtual DbSet<SYS_ORGANIZATION> SYS_ORGANIZATION { get; set; }
        public virtual DbSet<SYS_PART_FUNC> SYS_PART_FUNC { get; set; }
        public virtual DbSet<TT_Advertising> TT_Advertising { get; set; }
        public virtual DbSet<TT_ApiLog> TT_ApiLog { get; set; }
        public virtual DbSet<TT_Finance> TT_Finance { get; set; }
        public virtual DbSet<TT_Kefu> TT_Kefu { get; set; }
        public virtual DbSet<TT_Message> TT_Message { get; set; }
        public virtual DbSet<TT_Sms> TT_Sms { get; set; }
        public virtual DbSet<TT_StateLog> TT_StateLog { get; set; }
        public virtual DbSet<TT_Trademark> TT_Trademark { get; set; }
        public virtual DbSet<TT_Trademark_Applyer> TT_Trademark_Applyer { get; set; }
        public virtual DbSet<TT_Trademark_Class> TT_Trademark_Class { get; set; }
        public virtual DbSet<TT_Trademark_Files> TT_Trademark_Files { get; set; }
        public virtual DbSet<TT_User> TT_User { get; set; }
        public virtual DbSet<UR_PART> UR_PART { get; set; }
        public virtual DbSet<UR_USERS> UR_USERS { get; set; }
    }
}

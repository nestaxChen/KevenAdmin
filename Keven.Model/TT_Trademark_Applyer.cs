//------------------------------------------------------------------------------
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
    using System.Collections.Generic;
    
    public partial class TT_Trademark_Applyer
    {
        public int Id { get; set; }
        public Nullable<int> UserId { get; set; }
        public string TemplateName { get; set; }
        public Nullable<System.DateTime> CreateDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<int> Type { get; set; }
        public string Name { get; set; }
        public string NameEn { get; set; }
        public Nullable<int> CerificateType { get; set; }
        public string CerificateNumber { get; set; }
        public string CerificateFile { get; set; }
        public string CerificateFile2 { get; set; }
        public Nullable<System.DateTime> CerificateDate { get; set; }
        public string BusinessNumber { get; set; }
        public string BusinessFile { get; set; }
        public string BusinessFileEn { get; set; }
        public Nullable<System.DateTime> BusinessDate { get; set; }
        public string DelegationFile { get; set; }
        public Nullable<int> Nationality { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string District { get; set; }
        public string Address { get; set; }
        public string AddressEn { get; set; }
        public string ReceiveName { get; set; }
        public string ReceivePhone { get; set; }
        public string ReceiveAddress { get; set; }
        public string ReceivePostcode { get; set; }
        public string ReceiveFax { get; set; }
        public string ReceiveEmail { get; set; }
        public string Status { get; set; }
        public Nullable<int> Auditor { get; set; }
        public Nullable<System.DateTime> AuditDate { get; set; }
        public string AuditMemo { get; set; }
        public Nullable<int> IsPrivacy { get; set; }
        public Nullable<int> KefuId { get; set; }
        public Nullable<int> IsDefault { get; set; }
    }
}

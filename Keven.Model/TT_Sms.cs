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
    
    public partial class TT_Sms
    {
        public int Id { get; set; }
        public Nullable<int> SmsType { get; set; }
        public Nullable<System.DateTime> SendDate { get; set; }
        public string Mobile { get; set; }
        public string Content { get; set; }
        public string Verification { get; set; }
        public string Response { get; set; }
        public string Memo { get; set; }
    }
}
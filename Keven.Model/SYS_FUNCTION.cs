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
    
    public partial class SYS_FUNCTION
    {
        public int FN_ID { get; set; }
        public Nullable<int> FN_APP_ID { get; set; }
        public string FN_SHORT_NAME { get; set; }
        public string FN_NAME { get; set; }
        public string FN_URL { get; set; }
        public Nullable<int> FN_PARENT_ID { get; set; }
        public Nullable<short> FN_IS_LEAF { get; set; }
        public string FN_TREE_LAYER { get; set; }
        public Nullable<int> FN_ORDER { get; set; }
        public string FN_REVERSE1 { get; set; }
        public string FN_REVERSE2 { get; set; }
    }
}
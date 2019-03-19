using System.Data.Entity;

namespace Keven.DAL
{
    /// <summary>
    /// 自定义的EF上下文容器类
    /// </summary>
    public class BaseDBContext : DbContext
    {
        /// <summary>
        /// 负责根据指定的数据库链接字符串，初始化EF
        /// </summary>
        public BaseDBContext() : base("name=KTEntities") { }
    }
}

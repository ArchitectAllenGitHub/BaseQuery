using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AllenDynamicExpressions
{
    /// <summary>
    /// 缓存反射属性
    /// </summary>
    public class CacheAll
    {
        public Dictionary<string, ChachePropertyInfo> PropertyInfoChache = new Dictionary<string, ChachePropertyInfo>();
    }
}

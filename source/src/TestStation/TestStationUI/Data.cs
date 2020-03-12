using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Testflow.Data.Sequence;

namespace TestStation
{
    /// <summary>
    /// 判断类型
    /// </summary>
    [Serializable()]//标记为可序列化
    public enum JudgeType
    {
        /// <summary>
        /// 布尔型true/false
        /// </summary>
        Boolean,
        /// <summary>
        /// 数字型
        /// </summary>
        Numeric,
        /// <summary>
        /// 字符型
        /// </summary>
        String,
        
        /// <summary>
        /// 类型
        /// </summary>
        Object
    }
}



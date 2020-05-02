using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Common.Extend
{
    /// <summary>
    /// 枚举扩展
    /// </summary>
    public static class EnumEx
    {
        /// <summary>
        /// 获取描述
        /// </summary>
        /// <param name="em"></param>
        /// <returns></returns>
        public static string GetDesc(this Enum em)
        {
            FieldInfo field = em.GetType().GetField(em.ToString());
            DescriptionAttribute attribute = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;

            return attribute?.Description;
        }
    }
}

using System.Collections;
using System;

namespace BuffSystem
{
    /// <summary>
    /// 用来标记Buff类
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class BuffAttribute : Attribute
    {
        public string buffID { get; }
        public string buffName { get; set; }

        public BuffAttribute(string buffID, string buffName)
        {
            this.buffID = buffID;
            this.buffName = buffName;
        }
    }
}

using System.Collections;
using System;

namespace BuffSystem
{
    /// <summary>
    /// Buff特性
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

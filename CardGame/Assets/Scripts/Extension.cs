using System.Collections;
using System.Collections.Generic;

namespace MyGame
{
    public static class Extension
    {
        public static bool IsNullOrEmpty(this ICollection self)
        {
            if (self == null || self.Count <= 0)
            {
                return true;
            }

            return false;
        }

        public static bool IsNullOrEmpty<T>(this ICollection<T> self) where T : ICollection<T>
        {
            if (self == null || self.Count <= 0)
            {
                return true;
            }

            return false;
        }
    }
}
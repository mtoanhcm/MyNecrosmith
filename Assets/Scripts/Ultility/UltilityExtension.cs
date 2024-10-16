using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ultility {
    public static class UltilityExtension
    {
        public static bool IsNulOrEmpty(this string input)
        {
            return string.IsNullOrEmpty(input);
        }
        
        public static bool IsZero(this Vector2 vector) { 
            return vector.x == 0 && vector.y == 0;
        }

        public static bool IsZero(this Vector3 vector)
        {
            return vector.x == 0 && vector.y == 0 && vector.z == 0;
        }
    }
}

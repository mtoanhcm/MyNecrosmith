using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Ultility {
    public static class UltilityExtension
    {
        public static bool IsWorldOverlap(this RectTransform originRect, RectTransform targetRect)
        {
            var currentRect1 = GetWorldRect(originRect);
            var currentRect2 = GetWorldRect(targetRect);

            return currentRect1.Overlaps(currentRect2);
            
            Rect GetWorldRect(RectTransform rectTransform)
            {
                var corners = new Vector3[4];
                rectTransform.GetWorldCorners(corners);

                var bottomLeft = corners[0];
                var topRight = corners[2];

                return new Rect(bottomLeft, topRight - bottomLeft);
            }
        }
        
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

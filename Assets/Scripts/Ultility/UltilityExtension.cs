using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameUtility {
    public static class UltilityExtension
    {
        public static List<Vector3> GetEquipmentPositionAroundCharacter(this Vector3 characterPosition, int totalEqupiment, 
            float initialRadius = 2f, float radiusIncrement = 2f, float minAngle = 40)
        {
            var positions = new List<Vector3>();
            var currentRadius = initialRadius;
            var maxEquipmentPerCircle = Mathf.FloorToInt(360f / minAngle); // Maximum number of equipment in one circle

            var currentIndex = 0;

            while (currentIndex < totalEqupiment)
            {
                var equipmentsInThisCircle = Mathf.Min(totalEqupiment - currentIndex, maxEquipmentPerCircle); // Number of equipment in the current circle
                var angleStep = 360f / equipmentsInThisCircle;

                for (var i = 0; i < equipmentsInThisCircle; i++)
                {
                    var angle = i * angleStep;
                    var position = new Vector3(
                        Mathf.Cos(angle * Mathf.Deg2Rad) * currentRadius,
                        0,
                        Mathf.Sin(angle * Mathf.Deg2Rad) * currentRadius
                    );

                    positions.Add(characterPosition + position);
                    currentIndex++;
                }

                currentRadius += radiusIncrement; // Increase the radius for the next circle
            }

            return positions;
        }
        
        public static void SetActive(this Component component, bool active)
        {
            if (component == null)
            {
                return;
            }

            if (component.gameObject.activeSelf == active)
            {
                return;
            }
            
            component.gameObject.SetActive(active);
        }
        
        public static bool IsWorldOverlap(this RectTransform originRect, RectTransform targetRect)
        {
            if (targetRect == null)
            {
                return false;
            }
            
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

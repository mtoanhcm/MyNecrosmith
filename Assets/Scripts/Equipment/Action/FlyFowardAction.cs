using System.Collections;
using Character;
using UnityEngine;

namespace Equipment
{
    public class FlyFowardAction : ActionBase
    {
        public override void Execute(CharacterBase caster, Transform target, EquipmentBase equipment)
        {
            caster.StartCoroutine(FlyFoward(caster, target, equipment));
        }

        private IEnumerator FlyFoward(CharacterBase caster, Transform target, EquipmentBase equipment)
        {
            var weaponData = equipment.Data as WeaponData;
            if (weaponData == null)
            {
                yield break;
            }
            
            var elapsedTime = 0f;
            var startPosition = equipment.transform.position;
            var targetDirection = (target.transform.position - startPosition).normalized;
            var targetPosition = startPosition + targetDirection * weaponData.AttackRadius;

            // Smoothly rotate equipment to face the target direction in 0.5 seconds
            var initialRotation = equipment.transform.rotation;
            var targetRotation = Quaternion.LookRotation(targetDirection) * Quaternion.Euler(90, 0, 0);
            while (elapsedTime < 0.5f)
            {
                equipment.transform.rotation =
                    Quaternion.Slerp(initialRotation, targetRotation, elapsedTime / 0.5f);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            equipment.transform.rotation = targetRotation;

            // Calculate travel duration based on your desired speed scaling
            // If AttackSpeed = 100 should move 1 cell in 0.1 second
            float distance = Vector3.Distance(startPosition, targetPosition);
            float cellSize = 1f; // Assuming 1 unit = 1 cell
            float cells = distance / cellSize;
            float duration = cells * (20f / weaponData.AttackSpeed); // sec per cell

            // Move towards target position
            elapsedTime = 0f;
            while (elapsedTime < duration)
            {
                equipment.transform.position = Vector3.Lerp(startPosition, targetPosition,
                    elapsedTime / duration);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            //equipment.transform.position = targetPosition;

            //// Move back to caster's current position while rotating to face upward
            //elapsedTime = 0f;
            //startPosition = targetPosition;
            //targetPosition = caster.transform.position;
            //initialRotation = equipment.transform.rotation;
            //targetRotation = Quaternion.Euler(0, 0, 0); // Rotate to face upwards

            //while (elapsedTime < weaponData.AttackSpeed)
            //{
            //    equipment.transform.SetPositionAndRotation(
            //        Vector3.Lerp(startPosition, targetPosition,
            //            elapsedTime / weaponData.AttackSpeed),
            //        Quaternion.Slerp(initialRotation, targetRotation,
            //            elapsedTime / weaponData.AttackSpeed)
            //        );
                
            //    elapsedTime += Time.deltaTime;
            //    yield return null;
            //}

            equipment.transform.SetPositionAndRotation(targetPosition, initialRotation);
        }
    }   
}

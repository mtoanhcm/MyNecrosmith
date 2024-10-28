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
            var elapsedTime = 0f;
            var startPosition = equipment.transform.position;
            var targetDirection = (target.transform.position - startPosition).normalized;
            var targetPosition = startPosition + targetDirection * equipment.Data.AttackRadius;

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

            // Move towards target position
            elapsedTime = 0f;
            while (elapsedTime < equipment.Data.AttackSpeed)
            {
                equipment.transform.position = Vector3.Lerp(startPosition, targetPosition,
                    elapsedTime / equipment.Data.AttackSpeed);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            equipment.transform.position = targetPosition;

            // Move back to caster's current position while rotating to face upward
            elapsedTime = 0f;
            startPosition = targetPosition;
            targetPosition = caster.transform.position;
            initialRotation = equipment.transform.rotation;
            targetRotation = Quaternion.Euler(0, 0, 0); // Rotate to face upwards

            while (elapsedTime < equipment.Data.AttackSpeed)
            {
                equipment.transform.SetPositionAndRotation(
                    Vector3.Lerp(startPosition, targetPosition,
                        elapsedTime / equipment.Data.AttackSpeed),
                    Quaternion.Slerp(initialRotation, targetRotation,
                        elapsedTime / equipment.Data.AttackSpeed)
                    );
                
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            equipment.transform.SetPositionAndRotation(targetPosition, initialRotation);
        }
    }   
}

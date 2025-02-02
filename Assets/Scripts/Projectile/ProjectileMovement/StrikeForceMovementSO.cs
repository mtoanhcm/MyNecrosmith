using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using InterfaceComp;
using Observer;

namespace Projectile.Movement
{
    [CreateAssetMenu(fileName = "StrikeForceMovementSO", menuName = "Config/Projectile/Movement/StrikeForce")]
    public class StrikeForceMovementSO : ProjectileMovementSO
    {
        public override void StartMovement(ProjectileBase projectile, UnityAction<ProjectileBase> checkApplyDamage)
        {
            // Calculate the target position based on spawn position, direction and range
            var targetPosition = projectile.Data.SpawnPosition + (projectile.Data.Direction * projectile.Data.AttackRange);
            
            // Calculate duration based on distance and speed
            var duration = projectile.Data.AttackRange / projectile.Data.MoveSpeed;

            // Setup the movement with DOTween
            projectile.transform
                .DOMove(targetPosition, duration)
                .SetEase(Ease.Linear)
                .OnUpdate(() =>
                {
                    // Check for health targets during movement
                    if (Physics.Raycast(projectile.transform.position, projectile.Data.Direction, out var hit, projectile.Data.MoveSpeed * Time.deltaTime))
                    {
                        if (hit.collider.TryGetComponent<IHealth>(out _))
                        {
                            checkApplyDamage?.Invoke(projectile);
                            ReturnProjectile(projectile);
                        }
                    }
                })
                .OnComplete(() => ReturnProjectile(projectile));
        }

        protected virtual void ReturnProjectile(ProjectileBase projectile)
        {
            projectile.ResetData();
            
            EventManager.Instance.TriggerEvent(new EventData.OnDespawnProjectile()
            {
                Projectile = projectile,
            });
        }
    }
}

using System;
using UnityEngine;
using DG.Tweening;

namespace Projectile.Movement
{
    [CreateAssetMenu(fileName = "StrikeForwardMovementSO", menuName = "baseConfig/Projectile/Movement/StrikeForward")]
    public class StrikeForwardMovementSO : ProjectileMovementSO
    {
        private Tween currentTween;
        
        public override void StartMovement(ProjectileBase projectile, Func<ProjectileBase, bool> checkApplyDamage)
        {
            // Calculate the target position based on spawn position, direction and range
            var targetPosition = projectile.Data.SpawnPosition + (projectile.Data.Direction * projectile.Data.AttackRange);
            
            // Calculate duration based on distance and speed
            var duration = projectile.Data.AttackRange / projectile.Data.MoveSpeed;

            // Setup the movement with DOTween
            bool hasTakeDamage = false;
            currentTween = projectile.transform
                .DOMove(targetPosition, duration)
                .SetEase(Ease.Linear)
                .OnUpdate(() =>
                {
                    if (!hasTakeDamage && checkApplyDamage(projectile))
                    {
                        currentTween.Complete();
                    }
                })
                .OnComplete(() =>
                {
                    if (!hasTakeDamage)
                    {
                        hasTakeDamage = true;
                        CompleteMovement(projectile);
                    }
                });
        }

        protected override void CompleteMovement(ProjectileBase projectile)
        {
            base.CompleteMovement(projectile);
            Debug.Log("Complete movement");
            currentTween?.Kill();
            projectile.Despawn();
        }
    }
}

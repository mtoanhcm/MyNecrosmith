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
            bool hasDealDamage = false;
            // Setup the movement with DOTween
            currentTween = projectile.transform
                .DOMove(targetPosition, duration)
                .SetEase(Ease.Linear)
                .OnUpdate(() =>
                {
                    if (hasDealDamage)
                    {
                        return;
                    }

                    if (checkApplyDamage(projectile))
                    {
                        //currentTween.Complete();
                        hasDealDamage = true; ;
                        CompleteMovement(projectile);
                        currentTween.Kill();
                    }
                })
                .OnComplete(() =>
                {
                    CompleteMovement(projectile);
                });
        }

        protected override void CompleteMovement(ProjectileBase projectile)
        {
            base.CompleteMovement(projectile);
            currentTween.Kill();
            projectile.Despawn();
        }
    }
}

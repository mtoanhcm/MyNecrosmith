using UnityEngine;

namespace Character
{
    public class CharacterAnimationController : MonoBehaviour
    {
        [SerializeField]
        private Animator animator;
        
        private static string MOVESPEED = "Speed_f";
        private static string ATTACK = "WeaponType_int";
        
        public void PlayMoveAnimation(float speed)
        {
            if (animator == null)
            {
                return;
            }
            
            animator.SetFloat(MOVESPEED, speed);
        }

        public void PlayIdleAnimation()
        {
            
        }

        public void PlayDeathAnimation()
        {
            
        }

        public void PlayAttackAnimation(int weaponType)
        {
            if (animator == null)
            {
                return;
            }
            
            animator.SetInteger(ATTACK, weaponType);
        }

        public void Reset()
        {
            PlayMoveAnimation(0);
            PlayAttackAnimation(0);
        }
    }   
}

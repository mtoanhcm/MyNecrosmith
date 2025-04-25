using UnityEngine;

namespace Character
{
    public class CharacterAnimationController : MonoBehaviour
    {
        [SerializeField]
        private Animator animator;

        private int velocityHash;

        private const string MOVESPEED = "Velocity";
        private const string ATTACK = "WeaponType_int";
        
        private void Awake()
        {
            if(animator == null)
            {
                Debug.LogError("Animator not found in children");
                return;
            }

            velocityHash = Animator.StringToHash(MOVESPEED);
        }

        public void PlayMoveAnimation(float velocity)
        {
            if (animator == null)
            {
                return;
            }

            animator.SetFloat(velocityHash, velocity);
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

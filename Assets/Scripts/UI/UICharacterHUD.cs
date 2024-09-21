using UnityEngine;

namespace  Character.HUD
{
    public class UICharacterHUD : MonoBehaviour
    {
        [SerializeField]
        private UIHealthBar healthBar;
        
        private CharacterBase character;

        private void Awake()
        {
            character = GetComponentInParent<CharacterBase>();
        }

        private void Start()
        {
            if (character == null)
            {
                Debug.LogError("Cannot find character for HUD");
                return;
            }
            
            healthBar.Init(character);
        }
    }   
}

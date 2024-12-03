using InterfaceComp;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Character
{
    public class CharacterBase : MonoBehaviour
    {
        public CharacterData Data { get;private set; }

        private Transform modelContainer;
        private CharacterHealth characterHealth;

        private void Awake()
        {
            characterHealth = GetComponent<CharacterHealth>();
            characterHealth.Init(this);
        }
        
        public virtual void Spawn(CharacterData data)
        {
            Data = data;
        }

        [Button]
        private void Test()
        {
            var aaa = GetComponent<IHealth>();
            Debug.Log(aaa);
        }
    }   
}

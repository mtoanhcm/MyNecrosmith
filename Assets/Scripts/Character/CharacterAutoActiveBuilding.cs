using UnityEngine;

namespace Character
{
    public class CharacterAutoActiveBuilding : MonoBehaviour
    {
        private CharacterBase character;
        private float delayTimeCheck;
        
        public void Init(CharacterBase characterBase)
        {
            character = characterBase;
        }

        private void Update()
        {
            if (character == null)
            {
                return;
            }
            
            if (delayTimeCheck > Time.time)
            {
                return;
            }

            delayTimeCheck = Time.time + 2f;
            var buildingAround = character.CharacterBrain.GetTargetBuildingAround();
            for (var i = 0; i < buildingAround.Length; i++)
            {
                if (buildingAround[i] == null || buildingAround[i].IsActive)
                {
                    continue;
                }
                
                buildingAround[i].SetActiveBuilding(true);
            }
        }
    }   
}

using System.Collections;
using UnityEngine;

namespace  Character.HUD
{
    public class UIHealthBar : MonoBehaviour
    {
        [SerializeField]
        private Transform healthBarTrans;
        
        private const float SMOOTH_PROGRESS = 0.2f;
        private float tempTargetValue;

        public void Init(CharacterBase character)
        {
            character.OnHPChange.AddListener(UpdateSlider);
            character.OnSpawnSuccess.AddListener(InitSliderForCharacterSpawn);

            return;
            
            void InitSliderForCharacterSpawn()
            {
                healthBarTrans.localScale = Vector3.one;
            }
        }
        
        private void UpdateSlider(float value)
        {
            tempTargetValue = value;
            
            StopAllCoroutines();
            StartCoroutine(RunProgress());
        }

        private IEnumerator RunProgress()
        {
            var elapsedTime = 0f;
            var currentScale = healthBarTrans.localScale.x;

            while (elapsedTime < SMOOTH_PROGRESS)
            {
                elapsedTime += Time.deltaTime * SMOOTH_PROGRESS;
                var newScale = Mathf.Lerp(currentScale, tempTargetValue, elapsedTime / SMOOTH_PROGRESS);
                
                healthBarTrans.localScale = new Vector3(newScale, healthBarTrans.localScale.y, healthBarTrans.localScale.z);
                
                yield return null;
            }
            
            healthBarTrans.localScale = new Vector3(tempTargetValue, healthBarTrans.localScale.y, healthBarTrans.localScale.z);
        }
    }   
}

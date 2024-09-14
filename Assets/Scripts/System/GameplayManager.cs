using System.Collections;
using UnityEngine;
using BehaviorDesigner.Runtime;

namespace Manager
{
    public class GameplayManager : MonoBehaviour
    {
        // Start is called before the first frame update
        private void Start()
        {
            StartCoroutine(OptimizeBehaviourDesign());
        }

        private IEnumerator OptimizeBehaviourDesign()
        {
            while (BehaviorManager.instance == null)
            {
                yield return new WaitForSeconds(0.5f);
            }
            
            BehaviorManager.instance.UpdateInterval = UpdateIntervalType.SpecifySeconds;
            BehaviorManager.instance.UpdateIntervalSeconds = 0.5f;
        }
    }   
}

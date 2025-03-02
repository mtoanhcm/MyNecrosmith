using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GameUtility
{
    public class Scanner<T> where T : Object
    {
        public List<T> ObjectAround { get; private set; }

        private readonly Collider[] colliders;
        private readonly int frequencyMilliseconds;
        private readonly LayerMask layerMask;
        private readonly Predicate<T> scanConditionHandler;
        private readonly Func<Vector3> getBasePosition;
        private readonly float radius;

        private bool isScanning;
        private bool isDebug;
        
        public Scanner(
            int fixAmountObjectScan,
            Func<Vector3> getBasePosition, // Allows dynamic base position retrieval
            float radius,
            int frequencySeconds,
            LayerMask layerMask,
            Predicate<T> scanConditionHandler = null,
            bool isDebug = false)
        {
            if (fixAmountObjectScan <= 0)
                throw new ArgumentException("fixAmountObjectScan must be greater than zero.");

            ObjectAround = new List<T>(fixAmountObjectScan);
            colliders = new Collider[fixAmountObjectScan];
            frequencyMilliseconds = frequencySeconds * 1000;
            this.layerMask = layerMask;
            this.scanConditionHandler = scanConditionHandler;
            this.getBasePosition = getBasePosition ?? throw new ArgumentNullException(nameof(getBasePosition));
            this.radius = radius;
            this.isDebug = isDebug;
            
            StartScanning().Forget();
        }

        public async UniTaskVoid StartScanning()
        {
            if (isScanning) return;

            isScanning = true;

            try
            {
                while (isScanning)
                {
                    Vector3 basePos = getBasePosition();

                    int objectFound = Physics.OverlapSphereNonAlloc(basePos, radius, colliders, 1 << layerMask, QueryTriggerInteraction.Collide);
                    
                    ObjectAround.Clear();

                    for (int i = 0; i < objectFound; i++)
                    {
                        T obj = colliders[i].GetComponent<T>();
                        if (obj == null) continue;

                        if (scanConditionHandler == null || scanConditionHandler(obj))
                        {
                            ObjectAround.Add(obj);
                        }
                    }
                    
                    await UniTask.Delay(frequencyMilliseconds);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Scanner<{typeof(T)}> encountered an error: {ex.Message}\n{ex.StackTrace}");
            }
            finally
            {
                isScanning = false;
            }
        }

        public void StopScanning()
        {
            isScanning = false;
        }
    }   
}
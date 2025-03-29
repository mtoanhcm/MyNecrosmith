using Spawner;
using UnityEngine;

namespace Building
{
    public class MinionCastle : BuildingBase
    {
        [SerializeField] private MinionSpawner spawner;
        
        // Update is called once per frame
        protected override void PlayActivation()
        {
            //Create equipment runtime
        }

        protected override void OnBuildingClaimed()
        {
            //Show gameover
        }
    }   
}

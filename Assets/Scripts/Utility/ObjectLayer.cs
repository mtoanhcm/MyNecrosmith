using UnityEngine;

namespace GameUtility
{
    public static class ObjectLayer
    {
        public static int EnemyLayerIndex => LayerMask.NameToLayer("Enemy");
        public static int MinionLayerIndex => LayerMask.NameToLayer("Minion");
        public static int EnemyBuildingLayerIndex => LayerMask.NameToLayer("EnemyBuilding");
        public static int MinionBuildingLayerIndex => LayerMask.NameToLayer("MinionBuilding");
    }   
}

using Projectile;
using UnityEngine;

namespace GameUtility
{
    public static class LayerMaskUtility
    {
        public const string ENEMY_LAYER = "Enemy";
        public const string ENEMY_BUILDING_LAYER = "EnemyBuilding";
        public const string MINION_LAYER = "Minion";
        public const string MINION_BUILDING_LAYER = "MinionBuilding";
        public const string PROJECTILE = "Projectile";
        public const string TERRAIN_LAYER = "Terrain";
        public const string OBSTACLE_LAYER = "Obstacle";

        private static LayerMask blockingLayer;
        private static LayerMask minionLayer;
        private static LayerMask enemyLayer;

        public static LayerMask GetTargetLayer(this ProjectileBase projectile)
        {
            if (IsInLayer(projectile.Data.Attacker, ENEMY_LAYER))
            {
                return GetMinionLayer() | GetBlockingLayer();
            }

            if (IsInLayer(projectile.Data.Attacker, MINION_LAYER))
            {
                return GetEnemyLayer() | GetBlockingLayer();
            }
            
            return GetMinionLayer() | GetEnemyLayer() | GetBlockingLayer();
        }

        public static LayerMask GetBlockingLayer()
        {
            // If not cached, create and cache it
            if (blockingLayer.value == 0)
            {
                blockingLayer = LayerMask.GetMask(TERRAIN_LAYER, OBSTACLE_LAYER);
            }
            
            return blockingLayer;
        }

        public static LayerMask GetMinionLayer()
        {
            // If not cached, create and cache it
            if (minionLayer.value == 0)
            {
                minionLayer = LayerMask.GetMask(MINION_LAYER, MINION_BUILDING_LAYER);
            }
            
            return minionLayer;
        }

        public static LayerMask GetEnemyLayer()
        {
            // If not cached, create and cache it
            if (enemyLayer.value == 0)
            {
                enemyLayer = LayerMask.GetMask(ENEMY_LAYER, ENEMY_BUILDING_LAYER);
            }
            
            return enemyLayer;
        }

        //Check by layer mask
        public static bool IsInLayer(GameObject obj, LayerMask mask)
        {
            return ((1 << obj.layer) & mask.value) != 0;
        }
        
        //Check by layer name
        public static bool IsInLayer(GameObject obj, string layerName)
        {
            return obj.layer == LayerMask.NameToLayer(layerName);
        }
    }
}

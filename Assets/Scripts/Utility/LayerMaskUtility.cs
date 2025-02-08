using Projectile;
using UnityEngine;

namespace GameUtility
{
    public static class LayerMaskUtility
    {
        public const string ENEMY_LAYER = "Enemy";
        public const string MINION_LAYER = "Minion";
        public const string PROJECTILE = "Projectile";
        public const string BUILDING_LAYER = "Building";
        public const string TERRAIN_LAYER = "Terrain";
        public const string OBSTACLE_LAYER = "Obstacle";

        private static LayerMask blockingLayer;
        
        public static LayerMask GetTargetLayer(this ProjectileBase projectile)
        {
            if (IsInLayer(projectile.Data.Attacker, ENEMY_LAYER))
            {
                return LayerMask.GetMask(MINION_LAYER) | GetBlockingLayer();
            }

            if (IsInLayer(projectile.Data.Attacker, MINION_LAYER))
            {
                return LayerMask.GetMask(ENEMY_LAYER) | GetBlockingLayer();
            }
            
            return LayerMask.GetMask(MINION_LAYER, ENEMY_LAYER) | GetBlockingLayer();
        }

        public static LayerMask GetBlockingLayer()
        {
            // If not cached, create and cache it
            if (blockingLayer.value == 0)
            {
                blockingLayer = LayerMask.GetMask(BUILDING_LAYER, TERRAIN_LAYER, OBSTACLE_LAYER);
            }
            
            return blockingLayer;
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

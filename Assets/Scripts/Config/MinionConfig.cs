using UnityEngine;

namespace Config
{
    [CreateAssetMenu(fileName = "MinionConfig", menuName = "Config/Character/MinionConfig")]
    public class MinionConfig : CharacterConfig
    {
        public CharacterRace Race;
        public Vector2Int InventorySize;
    }   
}

using UnityEngine;
using UnityEngine.Tilemaps;

namespace UnityEditor.Tilemaps
{
    static internal partial class AssetCreation
    {

        [MenuItem("Assets/Create/2D/Tiles/Terrain Tile", priority = (int)ETilesMenuItemOrder.TerrainTile)]
        static void CreateTerrainTile()
        {
            ProjectWindowUtil.CreateAsset(ScriptableObject.CreateInstance<TerrainTile>(), "New Terrain Tile.asset");
        }

        [MenuItem("Assets/Create/2D/Tiles/Terrain Obj Tile", priority = (int)ETilesMenuItemOrder.TerrainObjTile)]
        static void CreateTerrainObjTile()
        {
            ProjectWindowUtil.CreateAsset(ScriptableObject.CreateInstance<TerrainObjTile>(), "New Terrain Obj Tile.asset");
        }
    }
}
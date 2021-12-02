using System;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace UnityEngine.Tilemaps
{
    /// <summary>
    /// Terrain Tiles, similar to Pipeline Tiles, are tiles which take into consideration its orthogonal and diagonal neighboring tiles and displays a sprite depending on whether the neighboring tile is the same tile.
    /// </summary>
    [Serializable]
    public class TerrainObjTile : TileBase
    {
        /// <summary>
        /// The Sprites used for defining the Terrain.
        /// </summary>
        [SerializeField]
        public Sprite[] m_Sprites;

        /// <summary>
        /// This method is called when the tile is refreshed.
        /// </summary>
        /// <param name="position">Position of the Tile on the Tilemap.</param>
        /// <param name="tilemap">The Tilemap the tile is present on.</param>
        public override void RefreshTile(Vector3Int position, ITilemap tilemap)
        {
            for (int yd = -1; yd <= 1; yd++)
                for (int xd = -1; xd <= 1; xd++)
                {
                    Vector3Int pos = new Vector3Int(position.x + xd, position.y + yd, position.z);
                    if (TileValue(tilemap, pos))
                        tilemap.RefreshTile(pos);
                }
        }

        /// <summary>
        /// Retrieves any tile rendering data from the scripted tile.
        /// </summary>
        /// <param name="position">Position of the Tile on the Tilemap.</param>
        /// <param name="tilemap">The Tilemap the tile is present on.</param>
        /// <param name="tileData">Data to render the tile.</param>
        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
        {
            UpdateTile(position, tilemap, ref tileData);
        }

        private void UpdateTile(Vector3Int location, ITilemap tileMap, ref TileData tileData)
        {
            tileData.transform = Matrix4x4.identity;
            tileData.color = Color.white;

            int mask = TileValue(tileMap, location + new Vector3Int(0, 1, 0)) ? 1 : 0; //上
            mask += TileValue(tileMap, location + new Vector3Int(1, 1, 0)) ? 2 : 0;     //右上
            mask += TileValue(tileMap, location + new Vector3Int(1, 0, 0)) ? 4 : 0;     //右
            mask += TileValue(tileMap, location + new Vector3Int(1, -1, 0)) ? 8 : 0;    //右下
            mask += TileValue(tileMap, location + new Vector3Int(0, -1, 0)) ? 16 : 0;   //下
            mask += TileValue(tileMap, location + new Vector3Int(-1, -1, 0)) ? 32 : 0;  //左下
            mask += TileValue(tileMap, location + new Vector3Int(-1, 0, 0)) ? 64 : 0;   //左
            mask += TileValue(tileMap, location + new Vector3Int(-1, 1, 0)) ? 128 : 0;  //左上

            byte original = (byte)mask;
            if ((original | 254) < 255) { mask = mask & 125; }
            if ((original | 251) < 255) { mask = mask & 245; }
            if ((original | 239) < 255) { mask = mask & 215; }
            if ((original | 191) < 255) { mask = mask & 95; }

            int index = GetIndex((byte)mask);
            if (index >= 0 && index < m_Sprites.Length && TileValue(tileMap, location))
            {
                tileData.sprite = m_Sprites[index];
                tileData.transform = GetTransform((byte)mask);
                tileData.color = Color.white;
                tileData.flags = TileFlags.LockTransform | TileFlags.LockColor;
                tileData.colliderType = Tile.ColliderType.Sprite;
            }
        }

        private bool TileValue(ITilemap tileMap, Vector3Int position)
        {
            TileBase tile = tileMap.GetTile(position);
            return (tile != null && tile == this);
        }

        private int GetIndex(byte mask)
        {
            switch (mask)
            {
                case 0: return 0; //単体
                case 1: return 2; //上
                case 4: return 3; //右
                case 16: return 1; //下
                case 64: return 4; //左

                case 5: return 15; //右・上
                case 20: return 7; //右・下
                case 80: return 8; //左・下
                case 65: return 16; //左・上

                case 7: return 17; //上・右上・右
                case 28: return 9; //下・右下・右
                case 112: return 10; //下・左下・下
                case 193: return 18; //上・左上・左

                case 17: return 5; //上・下
                case 68: return 6; //右・左

                case 21: return 14; //上・右・下
                case 84: return 11; //下・右・左
                case 81: return 12; //上・下・左
                case 69: return 13; //上・右・左

                case 23: return 30; //下・上・右上・右
                case 92: return 23; //左・下・右下・右

                case 113: return 28; //上・下・左下・左
                case 197: return 29; //右・上・左上・左
                case 29: return 22; //上・下・右下・右
                case 116: return 19; //右・下・左下・左
                case 209: return 20; //下・上・左上・左
                case 71: return 21; //左・上・右上・右

                case 31: return 26; //上・右上・右・右下・下
                case 124: return 23; //右・右下・下・左下・左
                case 241: return 24; //上・左上・左・左下・下
                case 199: return 25; //左・左上・上・右上・右

                case 85: return 38; //上・下・左・右

                case 87: return 44; //上・下・左・右・右上
                case 93: return 36; //上・下・左・右・右下
                case 117: return 37; //上・下・左・右・左下
                case 213: return 45; //上・下・左・右・左上

                case 95: return 42; //上・下・左・右・右下・右上
                case 125: return 33; //上・下・左・右・左下・右下
                case 245: return 34; //上・下・左・右・左上・左下
                case 215: return 41; //上・下・左・右・左上・右上
                case 119: return 43; //上・下・左・右・左下・右上
                case 221: return 35; //上・下・左・右・左上・右下
                case 127: return 31; //左上以外
                case 253: return 32; //右上以外
                case 247: return 40; //右下以外
                case 223: return 39; //左下以外
                case 255: return 46; //全方位
            }
            return -1;
        }

        private Matrix4x4 GetTransform(byte mask)
        {
            return Matrix4x4.identity;
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(TerrainObjTile))]
    public class TerrainObjTileEditor : Editor
    {
        private TerrainObjTile tile { get { return (target as TerrainObjTile); } }

        /// <summary>
        /// OnEnable for TerrainObjTile.
        /// </summary>
        public void OnEnable()
        {
            if (tile.m_Sprites == null || tile.m_Sprites.Length != 47)
            {
                tile.m_Sprites = new Sprite[47];
                EditorUtility.SetDirty(tile);
            }
        }

        protected string spriteName = "";
        //対象番号スプライト取得
        protected Sprite getSprite(Sprite[] sprites, string spriteName, int num)
        {
            string valueStr = string.Format("{0:D3}", num);
            return System.Array.Find<Sprite>(sprites, (sprite) => sprite.name.Equals(spriteName + "_" + valueStr));
        }
        /// <summary>
        /// Draws an Inspector for the Terrain Tile.
        /// </summary>
        /// 

        [NonSerialized]
        private Texture2D spriteTex;

        public override void OnInspectorGUI()
        {
            EditorGUI.BeginChangeCheck();
            spriteTex = (Texture2D)EditorGUILayout.ObjectField("master", spriteTex, typeof(Texture2D), false, null);
            GUILayout.Label("CharaSprietName");
            spriteName = GUILayout.TextField(spriteName);

            //ボタンを表示
            if (GUILayout.Button("TileSpriteSet"))
            {

                if (spriteTex != null)
                {
                    //スプライトリストを取得
                    Sprite[] sprites = Resources.LoadAll<Sprite>(spriteTex.name);

                    //List<Sprite> sprites = NonResources.LoadAll<Sprite>("Asset/MapImg");
                    //Array.Sort(sprites);
                    for (int i = 0; i < tile.m_Sprites.Length; i++)
                    {
                        tile.m_Sprites[i] = getSprite(sprites, spriteName, i + 2);
                    }
                }
            }
            if (GUILayout.Button("Clear"))
            {
                for (int i = 0; i < tile.m_Sprites.Length; i++)
                {
                    tile.m_Sprites[i] = null;
                }
            }

            EditorGUILayout.LabelField("Place sprites shown based on the contents of the sprite.");
            EditorGUILayout.Space();
            
            float oldLabelWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 210;
            //1つ空き
            EditorGUILayout.LabelField("●1つ空き");
            EditorGUILayout.Separator();
            tile.m_Sprites[0] = (Sprite)EditorGUILayout.ObjectField("単体", tile.m_Sprites[0], typeof(Sprite), false, null);
            tile.m_Sprites[1] = (Sprite)EditorGUILayout.ObjectField("下", tile.m_Sprites[1], typeof(Sprite), false, null);
            tile.m_Sprites[2] = (Sprite)EditorGUILayout.ObjectField("上", tile.m_Sprites[2], typeof(Sprite), false, null);
            tile.m_Sprites[3] = (Sprite)EditorGUILayout.ObjectField("右", tile.m_Sprites[3], typeof(Sprite), false, null);
            tile.m_Sprites[4] = (Sprite)EditorGUILayout.ObjectField("左", tile.m_Sprites[4], typeof(Sprite), false, null);
            EditorGUILayout.Separator();

            //2つ空き
            EditorGUILayout.LabelField("●2つ空き");
            EditorGUILayout.Separator();
            tile.m_Sprites[5] = (Sprite)EditorGUILayout.ObjectField("上下", tile.m_Sprites[5], typeof(Sprite), false, null);
            tile.m_Sprites[6] = (Sprite)EditorGUILayout.ObjectField("左右", tile.m_Sprites[6], typeof(Sprite), false, null);
            tile.m_Sprites[7] = (Sprite)EditorGUILayout.ObjectField("右・下", tile.m_Sprites[7], typeof(Sprite), false, null);
            tile.m_Sprites[8] = (Sprite)EditorGUILayout.ObjectField("左・下", tile.m_Sprites[8], typeof(Sprite), false, null);
            tile.m_Sprites[15] = (Sprite)EditorGUILayout.ObjectField("上・右", tile.m_Sprites[15], typeof(Sprite), false, null);
            tile.m_Sprites[16] = (Sprite)EditorGUILayout.ObjectField("上・左", tile.m_Sprites[16], typeof(Sprite), false, null);
            EditorGUILayout.Separator();
            //3つ空き
            EditorGUILayout.LabelField("●3つ空き");
            EditorGUILayout.Separator();
            tile.m_Sprites[23] = (Sprite)EditorGUILayout.ObjectField("上辺", tile.m_Sprites[23], typeof(Sprite), false, null);
            tile.m_Sprites[24] = (Sprite)EditorGUILayout.ObjectField("右辺", tile.m_Sprites[24], typeof(Sprite), false, null);
            tile.m_Sprites[25] = (Sprite)EditorGUILayout.ObjectField("下辺", tile.m_Sprites[25], typeof(Sprite), false, null);
            tile.m_Sprites[26] = (Sprite)EditorGUILayout.ObjectField("左辺", tile.m_Sprites[26], typeof(Sprite), false, null);
            tile.m_Sprites[9] = (Sprite)EditorGUILayout.ObjectField("右下3つ", tile.m_Sprites[9], typeof(Sprite), false, null);
            tile.m_Sprites[10] = (Sprite)EditorGUILayout.ObjectField("左下3つ", tile.m_Sprites[10], typeof(Sprite), false, null);
            tile.m_Sprites[17] = (Sprite)EditorGUILayout.ObjectField("右上3つ", tile.m_Sprites[17], typeof(Sprite), false, null);
            tile.m_Sprites[18] = (Sprite)EditorGUILayout.ObjectField("左上3つ", tile.m_Sprites[18], typeof(Sprite), false, null);
            tile.m_Sprites[14] = (Sprite)EditorGUILayout.ObjectField("上・下・右", tile.m_Sprites[14], typeof(Sprite), false, null);
            tile.m_Sprites[12] = (Sprite)EditorGUILayout.ObjectField("上・下・左", tile.m_Sprites[12], typeof(Sprite), false, null);
            tile.m_Sprites[13] = (Sprite)EditorGUILayout.ObjectField("上・右・左", tile.m_Sprites[13], typeof(Sprite), false, null);
            tile.m_Sprites[11] = (Sprite)EditorGUILayout.ObjectField("下・右・左", tile.m_Sprites[11], typeof(Sprite), false, null);

            EditorGUILayout.Separator();
            //4つ空き
            EditorGUILayout.LabelField("●4つ空き");
            EditorGUILayout.Separator();
            tile.m_Sprites[38] = (Sprite)EditorGUILayout.ObjectField("四つ角", tile.m_Sprites[38], typeof(Sprite), false, null);
            tile.m_Sprites[22] = (Sprite)EditorGUILayout.ObjectField("上・右下3つ", tile.m_Sprites[22], typeof(Sprite), false, null);
            tile.m_Sprites[28] = (Sprite)EditorGUILayout.ObjectField("上・左下3つ", tile.m_Sprites[28], typeof(Sprite), false, null);
            tile.m_Sprites[30] = (Sprite)EditorGUILayout.ObjectField("下・右上3つ", tile.m_Sprites[30], typeof(Sprite), false, null);
            tile.m_Sprites[20] = (Sprite)EditorGUILayout.ObjectField("下・左上3つ", tile.m_Sprites[20], typeof(Sprite), false, null);
            tile.m_Sprites[29] = (Sprite)EditorGUILayout.ObjectField("右・左上3つ", tile.m_Sprites[29], typeof(Sprite), false, null);
            tile.m_Sprites[19] = (Sprite)EditorGUILayout.ObjectField("右・左下3つ", tile.m_Sprites[19], typeof(Sprite), false, null);
            tile.m_Sprites[21] = (Sprite)EditorGUILayout.ObjectField("左・右上3つ", tile.m_Sprites[21], typeof(Sprite), false, null);
            tile.m_Sprites[27] = (Sprite)EditorGUILayout.ObjectField("左・右下3つ", tile.m_Sprites[27], typeof(Sprite), false, null);
            EditorGUILayout.Separator();

            //5つ空き
            EditorGUILayout.LabelField("●5つ空き");
            EditorGUILayout.Separator();
            tile.m_Sprites[36] = (Sprite)EditorGUILayout.ObjectField("上・左・右下３つ", tile.m_Sprites[36], typeof(Sprite), false, null);
            tile.m_Sprites[37] = (Sprite)EditorGUILayout.ObjectField("上・右・左下３つ", tile.m_Sprites[37], typeof(Sprite), false, null);
            tile.m_Sprites[44] = (Sprite)EditorGUILayout.ObjectField("下・左・右上3つ", tile.m_Sprites[44], typeof(Sprite), false, null);
            tile.m_Sprites[45] = (Sprite)EditorGUILayout.ObjectField("下・右・左上3つ", tile.m_Sprites[45], typeof(Sprite), false, null);
            EditorGUILayout.Separator();
            //6つ空き
            EditorGUILayout.LabelField("●6つ空き");
            EditorGUILayout.Separator();
            tile.m_Sprites[34] = (Sprite)EditorGUILayout.ObjectField("右上・右下以外", tile.m_Sprites[34], typeof(Sprite), false, null);
            tile.m_Sprites[35] = (Sprite)EditorGUILayout.ObjectField("右上・左下以外", tile.m_Sprites[35], typeof(Sprite), false, null);
            tile.m_Sprites[33] = (Sprite)EditorGUILayout.ObjectField("左上・右上以外", tile.m_Sprites[33], typeof(Sprite), false, null);
            tile.m_Sprites[42] = (Sprite)EditorGUILayout.ObjectField("左上・左下以外", tile.m_Sprites[42], typeof(Sprite), false, null);
            tile.m_Sprites[41] = (Sprite)EditorGUILayout.ObjectField("右下・左下以外", tile.m_Sprites[41], typeof(Sprite), false, null);
            tile.m_Sprites[43] = (Sprite)EditorGUILayout.ObjectField("左上・右下以外", tile.m_Sprites[43], typeof(Sprite), false, null);
            EditorGUILayout.Separator();
            //7つ空き
            EditorGUILayout.LabelField("●7つ空き");
            EditorGUILayout.Separator();
            tile.m_Sprites[32] = (Sprite)EditorGUILayout.ObjectField("右上以外", tile.m_Sprites[32], typeof(Sprite), false, null);
            tile.m_Sprites[31] = (Sprite)EditorGUILayout.ObjectField("左上以外", tile.m_Sprites[31], typeof(Sprite), false, null);
            tile.m_Sprites[39] = (Sprite)EditorGUILayout.ObjectField("左下以外", tile.m_Sprites[39], typeof(Sprite), false, null);
            tile.m_Sprites[40] = (Sprite)EditorGUILayout.ObjectField("右下以外", tile.m_Sprites[40], typeof(Sprite), false, null);
            EditorGUILayout.Separator();
            //8つ空き
            EditorGUILayout.LabelField("●全方位");
            EditorGUILayout.Separator();
            tile.m_Sprites[46] = (Sprite)EditorGUILayout.ObjectField("全方位", tile.m_Sprites[46], typeof(Sprite), false, null);
            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(tile);
            }

            EditorGUIUtility.labelWidth = oldLabelWidth;
        }
    }
#endif
}

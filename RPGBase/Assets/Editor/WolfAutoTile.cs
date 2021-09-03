//━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
//※重要※  使用の際、ファイル名は必ず「WolfAutoTile.cs」にしてください。
//          アセット用クラス名とスクリプトのファイル名が同じでないと正しく動かない仕様のようです。
// 参考URL -> https://answers.unity.com/questions/1379538/when-building-assetbundles-some-scriptable-objects.html
//━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
using System;
using System.Linq;

#if UNITY_EDITOR
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

namespace UnityEngine.Tilemaps
{
    //─────────────────────────────────────────────
    //	シリアライズ
    //─────────────────────────────────────────────
    [Serializable]
    //━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
    //
    //	WolfAutoTile    ※オートタイルの情報保持や描画を行うクラス
    //
    //━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
    public class WolfAutoTile : TileBase
    {
        //─────────────────────────────────────────────
        //	スプライト情報を持たせるための変数
        //─────────────────────────────────────────────
        public bool pngOut;
        public Sprite[] m_Sprites;
        public int m_anip = 0;
        public float m_MinSpeed = 1f;
        public float m_MaxSpeed = 1f;
        public float m_AnimationStartTime;

        //─────────────────────────────────────────────
        //	タイルのリフレッシュ
        //─────────────────────────────────────────────
        public override void RefreshTile(Vector3Int location, ITilemap tileMap)
        {
            if (Event.current.shift) { return; }

            for (int yd = -1; yd <= 1; yd++)
                for (int xd = -1; xd <= 1; xd++)
                {
                    Vector3Int position = new Vector3Int(location.x + xd, location.y + yd, location.z);

                    if (TileValue(tileMap, position)) { tileMap.RefreshTile(position); }
                }
        }

        //─────────────────────────────────────────────
        //	タイルのデータ取得
        //─────────────────────────────────────────────
        public override void GetTileData(Vector3Int location, ITilemap tileMap, ref TileData tileData)
        {
            UpdateTile(location, tileMap, ref tileData);
        }

        //─────────────────────────────────────────────
        //  タイルの更新
        //─────────────────────────────────────────────
        private void UpdateTile(Vector3Int location, ITilemap tileMap, ref TileData tileData)
        {
            //─────────────────────────────────────────────
            //	スプライトが設定済みか？
            //─────────────────────────────────────────────
            if (m_Sprites[0] && m_anip > 0)
            {
                //─────────────────────────────────────────────
                //	周囲の状態で貼り付けるタイルのインデックスを決定
                //─────────────────────────────────────────────
                int mask = TileValue(tileMap, location + new Vector3Int(0, 1, 0)) ? 1 : 0;
                mask += TileValue(tileMap, location + new Vector3Int(1, 1, 0)) ? 2 : 0;
                mask += TileValue(tileMap, location + new Vector3Int(1, 0, 0)) ? 4 : 0;
                mask += TileValue(tileMap, location + new Vector3Int(1, -1, 0)) ? 8 : 0;
                mask += TileValue(tileMap, location + new Vector3Int(0, -1, 0)) ? 16 : 0;
                mask += TileValue(tileMap, location + new Vector3Int(-1, -1, 0)) ? 32 : 0;
                mask += TileValue(tileMap, location + new Vector3Int(-1, 0, 0)) ? 64 : 0;
                mask += TileValue(tileMap, location + new Vector3Int(-1, 1, 0)) ? 128 : 0;
                int index = GetIndex((byte)mask);
                //─────────────────────────────────────────────
                //	インデックスが正常値でタイルの変化が必要ある場合
                //─────────────────────────────────────────────
                if (index >= 0 && index < 47 && TileValue(tileMap, location))
                {
                    tileData.transform = Matrix4x4.identity;
                    tileData.sprite = m_Sprites[index];
                    tileData.color = Color.white;
                    tileData.flags = (TileFlags.LockTransform | TileFlags.LockColor);
                    tileData.colliderType = Tile.ColliderType.Sprite;
                }
            }
        }
        public override bool GetTileAnimationData(Vector3Int location, ITilemap tileMap, ref TileAnimationData tileAnimationData)
        {
            if (m_Sprites[0] && m_anip > 1)
            {
                int mask = TileValue(tileMap, location + new Vector3Int(0, 1, 0)) ? 1 : 0;
                mask += TileValue(tileMap, location + new Vector3Int(1, 1, 0)) ? 2 : 0;
                mask += TileValue(tileMap, location + new Vector3Int(1, 0, 0)) ? 4 : 0;
                mask += TileValue(tileMap, location + new Vector3Int(1, -1, 0)) ? 8 : 0;
                mask += TileValue(tileMap, location + new Vector3Int(0, -1, 0)) ? 16 : 0;
                mask += TileValue(tileMap, location + new Vector3Int(-1, -1, 0)) ? 32 : 0;
                mask += TileValue(tileMap, location + new Vector3Int(-1, 0, 0)) ? 64 : 0;
                mask += TileValue(tileMap, location + new Vector3Int(-1, 1, 0)) ? 128 : 0;
                int index = GetIndex((byte)mask);
                Sprite[] sp = new Sprite[m_anip];
                for (int a = 0; a < m_anip; a++)
                {
                    sp[a] = m_Sprites[index + a * 47];
                }
                tileAnimationData.animatedSprites = sp;
                tileAnimationData.animationSpeed = Random.Range(m_MinSpeed, m_MaxSpeed);
                tileAnimationData.animationStartTime = m_AnimationStartTime;
                return true;
            }
            return false;
        }

        //─────────────────────────────────────────────
        //	判定場所にあるタイル
        //─────────────────────────────────────────────
        private bool TileValue(ITilemap tileMap, Vector3Int position)
        {
            TileBase tile = tileMap.GetTile(position);
            return (tile != null && tile == this);
        }

        //─────────────────────────────────────────────
        //	インデックス計算
        //─────────────────────────────────────────────
        private int GetIndex(byte mask)
        {
            string[] patternTexts = {
                "x0x111x0",
                "x11111x0",
                "x111x0x0",
                "x10111x0",
                "x11101x0",
                "01111111",
                "11111101",
                "x0x1x0x0",
                "x0x11111",
                "11111111",
                "1111x0x1",
                "x0x10111",
                "1101x0x1",
                "11011111",
                "11110111",
                "x0x1x0x1",
                "x0x0x111",
                "11x0x111",
                "11x0x0x1",
                "x0x11101",
                "0111x0x1",
                "01110111",
                "11011101",
                "x0x0x0x1",
                "x0x101x0",
                "x10101x0",
                "x101x0x0",
                "01x0x111",
                "11x0x101",
                "11010101",
                "01010111",
                "11010111",
                "x0x10101",
                "01010101",
                "0101x0x1",
                "11110101",
                "01011111",
                "01110101",
                "01011101",
                "01111101",
                "x0x0x101",
                "01x0x101",
                "01x0x0x1",
                "x0x0x1x0",
                "x1x0x1x0",
                "x1x0x0x0",
                "x0x0x0x0"
            };
            int index = -1;
            for (int j = 0; j < patternTexts.Length; j++)
            {
                bool flag = true;
                for (int i = 0; i < 8; i++)
                {
                    if (patternTexts[j][i] != 'x')
                    {
                        char currentBitChar = ((mask & (byte)Mathf.Pow(2, 7 - i)) != 0) ? '1' : '0';
                        if (patternTexts[j][i] != currentBitChar)
                        {
                            flag = false;
                            break;
                        }
                    }
                }
                if (flag)
                {
                    index = j;
                    break;
                }
            }
            return index;

        }
#if UNITY_EDITOR
        //─────────────────────────────────────────────
        //	オートタイルアセットを作成する処理
        //─────────────────────────────────────────────
        [MenuItem("Assets/Create/WolfAuto Tile")]
        public static void CreateTerrainTile()
        {
            string path = EditorUtility.SaveFilePanelInProject("Save WolfAuto Tile", "New WolfAuto Tile", "asset", "Save WolfAuto Tile", "Assets");

            if (path == "")
                return;
            WolfAutoTile Obj = ScriptableObject.CreateInstance<WolfAutoTile>();
            AssetDatabase.CreateAsset(Obj, path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

#endif
    }

#if UNITY_EDITOR
    //━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
    //
    //	OverrideFile    ※メタデータを変えずファイルの操作を行うクラス
    //
    //━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
    public class OverrideFile : AssetPostprocessor
    {

        static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromPath)
        {
            if (Event.current == null || Event.current.type != EventType.DragPerform) { return; }

            foreach (var asset in importedAssets)
            {
                var rootAsset = ParentFile(asset);
                if (rootAsset == null)
                    continue;

                if (EditorUtility.DisplayDialog("override", rootAsset + "を上書きしますか？", "上書き", "両方残す"))
                {
                    File.Copy(asset, rootAsset, true);
                    AssetDatabase.DeleteAsset(asset);

                    AssetDatabase.ImportAsset(rootAsset);
                    AssetDatabase.Refresh();
                }
            }
        }

        static string ParentFile(string name)
        {
            var match = Regex.Match(name, @"(?<item>.*) 1.(?<extension>.*)");
            if (!match.Success)
                return null;

            return string.Format("{0}.{1}", match.Groups["item"], match.Groups["extension"]);
        }
    }

    //━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
    //
    //	class SpriteDivider    ※テクスチャをマルチスプライトに分割するクラス
    //
    //━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
    static public class SpriteDivider
    {
        //─────────────────────────────────────────────
        //	メイン処理
        //─────────────────────────────────────────────
        static public void DividSprite(string texturePath, int horizontalCount, int verticalCount)
        {
            //─────────────────────────────────────────────
            //	テクスチャインポート
            //─────────────────────────────────────────────
            TextureImporter importer = TextureImporter.GetAtPath(texturePath) as TextureImporter;

            //─────────────────────────────────────────────
            //	テクスチャを分割しやすい設定に
            //─────────────────────────────────────────────
            importer.textureType = TextureImporterType.Sprite;
            importer.spriteImportMode = SpriteImportMode.Multiple;
            importer.filterMode = FilterMode.Point;

            //─────────────────────────────────────────────
            //	設定更新
            //─────────────────────────────────────────────
            EditorUtility.SetDirty(importer);
            AssetDatabase.ImportAsset(texturePath, ImportAssetOptions.ForceUpdate);

            Texture texture = AssetDatabase.LoadAssetAtPath(texturePath, typeof(Texture)) as Texture;

            //─────────────────────────────────────────────
            //	PixelsPerUnitをテクスチャの長辺に
            //─────────────────────────────────────────────
            importer.spritePixelsPerUnit = Mathf.Max(texture.width / horizontalCount, texture.height / verticalCount);

            //─────────────────────────────────────────────
            //	分割を行う
            //─────────────────────────────────────────────
            importer.spritesheet = CreateSpriteMetaDataArray(texture, horizontalCount, verticalCount);

            //─────────────────────────────────────────────
            //	設定更新
            //─────────────────────────────────────────────
            EditorUtility.SetDirty(importer);
            AssetDatabase.ImportAsset(texturePath, ImportAssetOptions.ForceUpdate);
        }

        //─────────────────────────────────────────────
        //	スプライトを分割する処理
        //─────────────────────────────────────────────
        static SpriteMetaData[] CreateSpriteMetaDataArray(Texture texture, int horizontalCount, int verticalCount)
        {
            //─────────────────────────────────────────────
            //	テクスチャの幅と高さ
            //─────────────────────────────────────────────
            float spriteWidth = texture.width / horizontalCount;
            float spriteHeight = texture.height / verticalCount;

            return Enumerable
                .Range(0, horizontalCount * verticalCount)
                .Select(index =>
                {
                    int x = index % horizontalCount;
                    int y = index / horizontalCount;

                    return new SpriteMetaData
                    {

                        //─────────────────────────────────────────────
                        //	名前と位置とサイズを設定
                        //─────────────────────────────────────────────
                        name = string.Format("{0}_{1}", texture.name, index),
                        rect = new Rect(spriteWidth * x, texture.height - spriteHeight * (y + 1), spriteWidth, spriteHeight)
                    };
                })
                .ToArray();
        }
    }

    //━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
    //
    //	class PattternsGenerate    ※オートタイルを生成するクラス
    //
    //━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
    static public class PatternsGenerate
    {
        //─────────────────────────────────────────────
        //	縦5分割したオートタイルをさらに4分割したスプライトを持たせる配列
        //─────────────────────────────────────────────
        static private Sprite[,] m_Segments = new Sprite[5, 4];

        //─────────────────────────────────────────────
        //	↑の配置を決めるパターンの配列
        //─────────────────────────────────────────────
        static private int[][] m_Patterns = new int[][]
        {
                        new int[] {0,2,1,4},
                        new int[] {2,2,4,4},
                        new int[] {2,0,4,1},
                        new int[] {2,2,3,4},
                        new int[] {2,2,4,3},
                        new int[] {3,4,4,4},
                        new int[] {4,3,4,4},
                        new int[] {0,0,1,1},
                        new int[] {1,4,1,4},
                        new int[] {4,4,4,4},
                        new int[] {4,1,4,1},
                        new int[] {1,4,1,3},
                        new int[] {4,1,3,1},
                        new int[] {4,4,3,4},
                        new int[] {4,4,4,3},
                        new int[] {1,1,1,1},

                        new int[] {1,4,0,2},
                        new int[] {4,4,2,2},
                        new int[] {4,1,2,0},
                        new int[] {1,3,1,4},
                        new int[] {3,1,4,1},
                        new int[] {3,4,4,3},
                        new int[] {4,3,3,4},
                        new int[] {1,1,0,0},

                        new int[] {0,2,1,3},
                        new int[] {2,2,3,3},
                        new int[] {2,0,3,1},
                        new int[] {3,4,2,2},
                        new int[] {4,3,2,2},
                        new int[] {4,3,3,3},
                        new int[] {3,4,3,3},
                        new int[] {4,4,3,3},

                        new int[] {1,3,1,3},

                        new int[] {3,3,3,3},

                        new int[] {3,1,3,1},
                        new int[] {4,3,4,3},
                        new int[] {3,4,3,4},
                        new int[] {3,3,4,3},
                        new int[] {3,3,3,4},
                        new int[] {3,3,4,4},

                        new int[] {1,3,0,2},
                        new int[] {3,3,2,2},
                        new int[] {3,1,2,0},
                        new int[] {0,2,0,2},
                        new int[] {2,2,2,2},
                        new int[] {2,0,2,0},
                        new int[] {0,0,0,0}

        };

        //─────────────────────────────────────────────
        //	テクスチャから47パターンを生成する
        //─────────────────────────────────────────────
        static public void GeneratePatterns(WolfAutoTile tile)
        {
            //─────────────────────────────────────────────
            //	すでにあるタイルテクスチャを消す
            //─────────────────────────────────────────────
            var allAsset = AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(tile));
            foreach (var asset in allAsset)
            {
                if (!(asset is WolfAutoTile)) { Object.DestroyImmediate(asset, true); }
            }

            //─────────────────────────────────────────────
            //	渡されたスプライトのテクスチャを生成
            //─────────────────────────────────────────────
            Texture2D tex = tile.m_Sprites[0].texture;
            tex = tile.m_Sprites[0].texture;

            //─────────────────────────────────────────────
            //	元のテクスチャの名前を確保
            //─────────────────────────────────────────────
            string nam = tex.name;

            //─────────────────────────────────────────────
            //	アニメーションのパターン総数
            //─────────────────────────────────────────────
            int anp = (int)tex.width / ((int)tex.height / 5);

            //─────────────────────────────────────────────
            //	パスとアニメ数を保管
            //─────────────────────────────────────────────
            tile.m_anip = anp;

            //─────────────────────────────────────────────
            //	テクスチャ生成
            //─────────────────────────────────────────────
            int t_height = ((int)tex.height / 5 + 2) * 47;
            int t_width = ((int)tex.height / 5 + 2) * anp;
            Texture2D m_Tex = new Texture2D(t_width, t_height, TextureFormat.ARGB32, false);
            Color[] texArray_ = new Color[t_width * t_height];
            for (int c = 0; c < texArray_.Length; c++) { texArray_[c] = new Color(0, 0, 0, 0); }
            m_Tex.SetPixels(texArray_);
            m_Tex.Apply();

            //─────────────────────────────────────────────
            //	スプライト全体の初期化
            //─────────────────────────────────────────────
            tile.m_Sprites = new Sprite[47 * anp];
            for (int a = 0; a < anp; a++)
            {
                //─────────────────────────────────────────────
                //	1パターンのサイズや位置
                //─────────────────────────────────────────────
                int height = (int)tex.height / 5;
                int width = (int)tex.height / 5;
                int x = width * a;
                int y = (int)tex.height;
                int height_half = height / 2;
                int width_half = width / 2;

                //─────────────────────────────────────────────
                //	分割スプライトの配列初期化
                //─────────────────────────────────────────────
                m_Segments = new Sprite[5, 4];

                //─────────────────────────────────────────────
                //	分割スプライト生成
                //─────────────────────────────────────────────
                for (int i = 0; i < 5; i++)
                {
                    y -= height;
                    m_Segments[i, 0] = Sprite.Create(tex, new Rect(x, y, width_half, height_half), Vector2.zero);
                    m_Segments[i, 1] = Sprite.Create(tex, new Rect(x + width_half, y, width_half, height_half), Vector2.zero);
                    m_Segments[i, 2] = Sprite.Create(tex, new Rect(x, y + height_half, width_half, height_half), Vector2.zero);
                    m_Segments[i, 3] = Sprite.Create(tex, new Rect(x + width_half, y + height_half, width_half, height_half), Vector2.zero);

                }

                //─────────────────────────────────────────────
                //	サイズを再設定
                //─────────────────────────────────────────────
                width_half = (int)m_Segments[0, 0].rect.width;
                height_half = (int)m_Segments[0, 0].rect.height;
                width = width_half * 2;
                height = height_half * 2;

                //─────────────────────────────────────────────
                //	スポイト用の配列
                //─────────────────────────────────────────────
                Color[] texArray = new Color[width * height];

                //─────────────────────────────────────────────
                //	分割スプライトと配置を入れ替えつつ47パターンの全ドットの色をコピーしていく
                //─────────────────────────────────────────────
                for (int r = 0; r < 47; r++)
                {
                    int[] TypeIndex = m_Patterns[r];
                    int[] fixedArray = new int[4];
                    fixedArray[0] = TypeIndex[2];
                    fixedArray[1] = TypeIndex[3];
                    fixedArray[2] = TypeIndex[0];
                    fixedArray[3] = TypeIndex[1];

                    Color[][] texs = new Color[4][];
                    for (int i = 0; i < 4; i++)
                    {
                        x = (int)m_Segments[fixedArray[i], i].rect.x;
                        y = (int)m_Segments[fixedArray[i], i].rect.y;
                        int w = (int)m_Segments[fixedArray[i], i].rect.width;
                        int h = (int)m_Segments[fixedArray[i], i].rect.height;
                        texs[i] = m_Segments[fixedArray[i], i].texture.GetPixels(x, y, w, h);
                    }

                    //─────────────────────────────────────────────
                    //	パターンにあった配置で色を設置
                    //─────────────────────────────────────────────
                    for (int i = 0; i < height_half; i++)
                    {
                        Array.Copy(texs[0], i * width_half, texArray, i * width, width_half);
                    }

                    for (int i = 0; i < height_half; i++)
                    {
                        Array.Copy(texs[1], i * width_half, texArray, i * width + width_half, width_half);
                    }

                    for (int i = 0; i < height_half; i++)
                    {
                        Array.Copy(texs[2], i * width_half, texArray, (i + height_half) * width, width_half);
                    }

                    for (int i = 0; i < height_half; i++)
                    {
                        Array.Copy(texs[3], i * width_half, texArray, (i + height_half) * width + width_half, width_half);
                    }

                    x = (width + 2) * a;
                    y = (height + 2) * r;
                    m_Tex.SetPixels(x + 1, y + 1, width, height, texArray);
                }
            }
            //─────────────────────────────────────────────
            //	取得した色を乗せて分割しやすい設定に
            //─────────────────────────────────────────────
            m_Tex.filterMode = FilterMode.Point;
            m_Tex.wrapMode = TextureWrapMode.Clamp;
            m_Tex.name = nam;
            m_Tex.Apply();
            AssetDatabase.AddObjectToAsset(m_Tex, tile);

            allAsset = AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(tile));
            foreach (var asset in allAsset)
            {
                if (asset is Texture2D)
                {
                    var reTex = asset as Texture2D;
                    for (int a = 0; a < anp; a++)
                    {
                        for (int r = 0; r < 47; r++)
                        {
                            if (m_Tex.name == reTex.name)
                            {
                                int height = (int)tex.height / 5;
                                int width = (int)tex.height / 5;
                                int x = (width + 2) * a;
                                int y = (height + 2) * r;
                                Sprite sp = Sprite.Create(reTex, new Rect(x, y, width + 2, height + 2), new Vector2(0.5f, 0.5f), (float)width);
                                sp.name = m_Tex.name + "_" + (a * 47 + r).ToString();
                                AssetDatabase.AddObjectToAsset(sp, tile);
                            }
                        }
                    }
                }
            }

            allAsset = AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(tile));
            foreach (var asset in allAsset)
            {
                if (asset is Sprite)
                {
                    var reTex = asset as Sprite;
                    for (int a = 0; a < anp; a++)
                    {
                        for (int r = 0; r < 47; r++)
                        {
                            if (reTex.name == m_Tex.name + "_" + (a * 47 + r).ToString())
                            {
                                //─────────────────────────────────────────────
                                //	基本スプライトタイル上書き
                                //─────────────────────────────────────────────
                                tile.m_Sprites[a * 47 + r] = reTex;
                            }
                        }
                    }
                }
            }

            AssetDatabase.ImportAsset(AssetDatabase.GetAssetPath(tile));
        }
    }

    [CustomEditor(typeof(WolfAutoTile))]
    //━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
    //
    //	class WolfAutoTileEditor    ※設定画面
    //
    //━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
    public class WolfAutoTileEditor : Editor
    {
        private WolfAutoTile tile { get { return (target as WolfAutoTile); } }

        //─────────────────────────────────────────────
        //	タイルファイルが選択された時
        //─────────────────────────────────────────────
        public void OnEnable()
        {
            //Debug.Log("OnEnable");
            if (tile.m_Sprites == null) { tile.m_Sprites = new Sprite[47]; }
        }
        //─────────────────────────────────────────────
        //	設定画面の描画
        //─────────────────────────────────────────────
        public override void OnInspectorGUI()
        {
            //Debug.Log("OnInspectorGUI");"以下にコンバートテクスチャが保存されます。"
            EditorGUILayout.LabelField("ウディタ規格のオートタイルをスロットしてください。");
            EditorGUILayout.Space();
            EditorGUI.BeginChangeCheck();
            tile.pngOut = EditorGUILayout.ToggleLeft("Assets/に連結画像を書き出す", tile.pngOut);
            if (EditorGUI.EndChangeCheck())
            {
                if (tile.pngOut && tile.m_Sprites[46])
                {
                    string path = Application.dataPath + "/" + tile.m_Sprites[46].texture.name + ".png";
                    var bytes = tile.m_Sprites[46].texture.EncodeToPNG();
                    File.WriteAllBytes(path, bytes);
                }
                EditorUtility.SetDirty(tile);

                //─────────────────────────────────────────────
                //	プロジェクトの更新
                //─────────────────────────────────────────────
                AssetDatabase.Refresh();
            }

            EditorGUI.BeginChangeCheck();
            tile.m_Sprites[46] = (Sprite)EditorGUILayout.ObjectField("基本タイル画像", tile.m_Sprites[46], typeof(Sprite), false, null);

            if (EditorGUI.EndChangeCheck())
            {
                //─────────────────────────────────────────────
                //	画像が設定されたら
                //─────────────────────────────────────────────
                if (tile.m_Sprites[46])
                {
                    tile.m_Sprites[0] = tile.m_Sprites[46];
                    //─────────────────────────────────────────────
                    //	画像を参照可能にする
                    //─────────────────────────────────────────────
                    string texturePath = (AssetDatabase.GetAssetPath(tile.m_Sprites[0].texture));
                    TextureImporter importer = TextureImporter.GetAtPath(texturePath) as TextureImporter;
                    importer.isReadable = true;

                    //─────────────────────────────────────────────
                    //	画像の設定更新
                    //─────────────────────────────────────────────
                    EditorUtility.SetDirty(importer);
                    AssetDatabase.ImportAsset(texturePath, ImportAssetOptions.ForceUpdate);

                    //─────────────────────────────────────────────
                    //	47パターン画像とスプライトの生成
                    //─────────────────────────────────────────────
                    PatternsGenerate.GeneratePatterns(tile);

                    if (tile.pngOut && tile.m_Sprites[46])
                    {
                        string path = Application.dataPath + "/" + tile.m_Sprites[46].texture.name + ".png";
                        var bytes = tile.m_Sprites[46].texture.EncodeToPNG();
                        File.WriteAllBytes(path, bytes);
                    }

                    //─────────────────────────────────────────────
                    //	タイルの設定更新
                    //─────────────────────────────────────────────
                    EditorUtility.SetDirty(tile);

                    //─────────────────────────────────────────────
                    //	プロジェクトの更新
                    //─────────────────────────────────────────────
                    AssetDatabase.Refresh();
                }
            }

            //─────────────────────────────────────────────
            //	アニメタイルの設定
            //─────────────────────────────────────────────
            if (tile.m_anip > 1)
            {
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("アニメタイルの情報を入れてください   " + tile.m_anip + "パターン");
                EditorGUILayout.Space();
                float minSpeed = EditorGUILayout.FloatField("最低速度", tile.m_MinSpeed);
                float maxSpeed = EditorGUILayout.FloatField("最高速度", tile.m_MaxSpeed);
                if (minSpeed < 0.0f)
                    minSpeed = 0.0f;
                if (maxSpeed < 0.0f)
                    maxSpeed = 0.0f;
                if (maxSpeed < minSpeed)
                    maxSpeed = minSpeed;
                tile.m_MinSpeed = minSpeed;
                tile.m_MaxSpeed = maxSpeed;

                tile.m_AnimationStartTime = EditorGUILayout.FloatField("開始時間", tile.m_AnimationStartTime);
                if (EditorGUI.EndChangeCheck()) { EditorUtility.SetDirty(tile); }
            }
        }
    }

    //━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
    //
    //	class CreateWAutoTiles    ※複数の画像ファイルを連続でオートタイルにするウィンドウ
    //
    //━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
    public class CreateWAutoTiles : EditorWindow
    {
        static CreateWAutoTiles window;

        [MenuItem("Window/CreateAutoWTiles")]
        static void Open()
        {
            if (window == null)
            {
                window = CreateInstance<CreateWAutoTiles>();
            }
            window.ShowUtility();
        }
        string lb1 = "";
        string lb2 = "";
        int c = 0;
        Object[] Obj;
        string path = "";
        void Update()
        {
            if (Obj != null && c < Obj.Length)
            {
                lb1 = "お待ち下さい。";
                lb2 = c + 1 + " / " + Obj.Length;
                Repaint();
                Object o = Obj[c];
                Sprite sp = o as Sprite;
                Texture2D tx = o as Texture2D;
                if (sp || tx)
                {
                    if (tx) { sp = Sprite.Create(tx, new Rect(0, 0, tx.width, tx.height), Vector2.zero); }
                    if (path == "")
                    {
                        path = EditorUtility.SaveFilePanelInProject("Save WolfAuto Tiles", "New WolfAuto Tiles", "", "Save WolfAuto Tiles", "Assets");
                        if (path == "")
                        {
                            c = 0;
                            Obj = null;
                            path = "";
                        }
                        return;
                    }
                    WolfAutoTile tile = ScriptableObject.CreateInstance<WolfAutoTile>();
                    tile.m_Sprites = new Sprite[47];
                    AssetDatabase.CreateAsset(tile, path + "_" + c + ".asset");
                    AssetDatabase.SaveAssets();

                    tile = AssetDatabase.LoadAssetAtPath(path + "_" + c + ".asset", typeof(WolfAutoTile)) as WolfAutoTile;
                    tile.m_Sprites = new Sprite[47];
                    tile.m_Sprites[0] = sp;

                    //─────────────────────────────────────────────
                    //	nullチェック
                    //─────────────────────────────────────────────
                    if (!tile.m_Sprites[0])
                    {
                        Debug.Log("null Area002:" + tile);
                        return;
                    }

                    //─────────────────────────────────────────────
                    //	画像を参照可能にする
                    //─────────────────────────────────────────────
                    string texturePath = (AssetDatabase.GetAssetPath(tile.m_Sprites[0].texture));
                    TextureImporter importer = TextureImporter.GetAtPath(texturePath) as TextureImporter;
                    importer.isReadable = true;

                    //─────────────────────────────────────────────
                    //	画像の設定更新
                    //─────────────────────────────────────────────
                    EditorUtility.SetDirty(importer);
                    AssetDatabase.ImportAsset(texturePath, ImportAssetOptions.ForceUpdate);
                    AssetDatabase.Refresh();

                    //─────────────────────────────────────────────
                    //	47パターン画像とスプライトの生成
                    //─────────────────────────────────────────────
                    PatternsGenerate.GeneratePatterns(tile);

                    //─────────────────────────────────────────────
                    //	nullチェック
                    //─────────────────────────────────────────────
                    if (!tile)
                    {
                        Debug.Log("null Area001:" + tile);
                        return;
                    }
                    else
                    {
                        string path = Application.dataPath + "/" + tile.m_Sprites[46].texture.name + ".png";
                        var bytes = tile.m_Sprites[46].texture.EncodeToPNG();
                        File.WriteAllBytes(path, bytes);
                        //─────────────────────────────────────────────
                        //	タイルの設定更新
                        //─────────────────────────────────────────────
                        EditorUtility.SetDirty(tile);
                    }

                    //─────────────────────────────────────────────
                    //	プロジェクトの更新
                    //─────────────────────────────────────────────
                    AssetDatabase.Refresh();
                    Resources.UnloadUnusedAssets();
                }
                c += 1;
            }
            else
            {
                lb1 = "オートタイル画像をドロップしてください。";
                lb2 = "指定のファイル名+連番で一括ファイル化します。";
                c = 0;
                Obj = null;
                path = "";
                Repaint();
            }
        }
        void OnGUI()
        {
            EditorGUILayout.LabelField(lb1);
            EditorGUILayout.LabelField(lb2);
            EditorGUILayout.Space();

            var evt = Event.current;
            var dropArea = GUILayoutUtility.GetRect(0.0f, 0.0f, GUILayout.ExpandHeight(true));
            GUI.Box(dropArea, "Drag & Drop");
            if (Obj == null)
            {
                c = 0;
                Obj = null;
                path = "";

                switch (evt.type)
                {
                    case EventType.DragUpdated:
                    case EventType.DragPerform:
                        if (!dropArea.Contains(evt.mousePosition)) break;
                        DragAndDrop.visualMode = DragAndDropVisualMode.Generic;
                        if (evt.type == EventType.DragPerform)
                        {
                            DragAndDrop.AcceptDrag();
                            {
                                while (Obj == null && DragAndDrop.objectReferences != null)
                                {
                                    c = 0;
                                    Obj = DragAndDrop.objectReferences;
                                    path = "";

                                    //─────────────────────────────────────────────
                                    //	nullチェック
                                    //─────────────────────────────────────────────
                                    if (!Obj[0])
                                    {
                                        Debug.Log("null Area003:" + Obj);
                                    }
                                }
                            }
                        }
                        Event.current.Use();
                        break;
                }
            }
        }
    }

    //━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
    //
    //	class HierarchyChange    ※初期化による不具合に対応するクラス
    //
    //━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━
    static class HierarchyChange
    {
        static bool sceneChange = false;

        [InitializeOnLoadMethod]
        static void HC()
        {
            //─────────────────────────────────────────────
            //	すべてのアセンブリが更新されたらシーンを読み込み直す
            //─────────────────────────────────────────────
            AssemblyReloadEvents.afterAssemblyReload += () =>
            {
                if (!EditorApplication.isPlayingOrWillChangePlaymode)
                {
                    Debug.Log("update");
                    sceneChange = true;
                }
            };
            EditorApplication.update += OnOpened;
        }

        static void OnOpened()
        {
            if (sceneChange)
            {
                sceneChange = false;
                EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
                EditorSceneManager.OpenScene(EditorSceneManager.GetActiveScene().path);
            }
        }
    }
#endif
}
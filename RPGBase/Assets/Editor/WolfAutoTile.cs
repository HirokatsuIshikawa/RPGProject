//������������������������������������������������������������������������������������������
//���d�v��  �g�p�̍ہA�t�@�C�����͕K���uWolfAutoTile.cs�v�ɂ��Ă��������B
//          �A�Z�b�g�p�N���X���ƃX�N���v�g�̃t�@�C�����������łȂ��Ɛ����������Ȃ��d�l�̂悤�ł��B
// �Q�lURL -> https://answers.unity.com/questions/1379538/when-building-assetbundles-some-scriptable-objects.html
//������������������������������������������������������������������������������������������
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
    //������������������������������������������������������������������������������������������
    //	�V���A���C�Y
    //������������������������������������������������������������������������������������������
    [Serializable]
    //������������������������������������������������������������������������������������������
    //
    //	WolfAutoTile    ���I�[�g�^�C���̏��ێ���`����s���N���X
    //
    //������������������������������������������������������������������������������������������
    public class WolfAutoTile : TileBase
    {
        //������������������������������������������������������������������������������������������
        //	�X�v���C�g�����������邽�߂̕ϐ�
        //������������������������������������������������������������������������������������������
        public bool pngOut;
        public Sprite[] m_Sprites;
        public int m_anip = 0;
        public float m_MinSpeed = 1f;
        public float m_MaxSpeed = 1f;
        public float m_AnimationStartTime;

        //������������������������������������������������������������������������������������������
        //	�^�C���̃��t���b�V��
        //������������������������������������������������������������������������������������������
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

        //������������������������������������������������������������������������������������������
        //	�^�C���̃f�[�^�擾
        //������������������������������������������������������������������������������������������
        public override void GetTileData(Vector3Int location, ITilemap tileMap, ref TileData tileData)
        {
            UpdateTile(location, tileMap, ref tileData);
        }

        //������������������������������������������������������������������������������������������
        //  �^�C���̍X�V
        //������������������������������������������������������������������������������������������
        private void UpdateTile(Vector3Int location, ITilemap tileMap, ref TileData tileData)
        {
            //������������������������������������������������������������������������������������������
            //	�X�v���C�g���ݒ�ς݂��H
            //������������������������������������������������������������������������������������������
            if (m_Sprites[0] && m_anip > 0)
            {
                //������������������������������������������������������������������������������������������
                //	���͂̏�Ԃœ\��t����^�C���̃C���f�b�N�X������
                //������������������������������������������������������������������������������������������
                int mask = TileValue(tileMap, location + new Vector3Int(0, 1, 0)) ? 1 : 0;
                mask += TileValue(tileMap, location + new Vector3Int(1, 1, 0)) ? 2 : 0;
                mask += TileValue(tileMap, location + new Vector3Int(1, 0, 0)) ? 4 : 0;
                mask += TileValue(tileMap, location + new Vector3Int(1, -1, 0)) ? 8 : 0;
                mask += TileValue(tileMap, location + new Vector3Int(0, -1, 0)) ? 16 : 0;
                mask += TileValue(tileMap, location + new Vector3Int(-1, -1, 0)) ? 32 : 0;
                mask += TileValue(tileMap, location + new Vector3Int(-1, 0, 0)) ? 64 : 0;
                mask += TileValue(tileMap, location + new Vector3Int(-1, 1, 0)) ? 128 : 0;
                int index = GetIndex((byte)mask);
                //������������������������������������������������������������������������������������������
                //	�C���f�b�N�X������l�Ń^�C���̕ω����K�v����ꍇ
                //������������������������������������������������������������������������������������������
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

        //������������������������������������������������������������������������������������������
        //	����ꏊ�ɂ���^�C��
        //������������������������������������������������������������������������������������������
        private bool TileValue(ITilemap tileMap, Vector3Int position)
        {
            TileBase tile = tileMap.GetTile(position);
            return (tile != null && tile == this);
        }

        //������������������������������������������������������������������������������������������
        //	�C���f�b�N�X�v�Z
        //������������������������������������������������������������������������������������������
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
        //������������������������������������������������������������������������������������������
        //	�I�[�g�^�C���A�Z�b�g���쐬���鏈��
        //������������������������������������������������������������������������������������������
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
    //������������������������������������������������������������������������������������������
    //
    //	OverrideFile    �����^�f�[�^��ς����t�@�C���̑�����s���N���X
    //
    //������������������������������������������������������������������������������������������
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

                if (EditorUtility.DisplayDialog("override", rootAsset + "���㏑�����܂����H", "�㏑��", "�����c��"))
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

    //������������������������������������������������������������������������������������������
    //
    //	class SpriteDivider    ���e�N�X�`�����}���`�X�v���C�g�ɕ�������N���X
    //
    //������������������������������������������������������������������������������������������
    static public class SpriteDivider
    {
        //������������������������������������������������������������������������������������������
        //	���C������
        //������������������������������������������������������������������������������������������
        static public void DividSprite(string texturePath, int horizontalCount, int verticalCount)
        {
            //������������������������������������������������������������������������������������������
            //	�e�N�X�`���C���|�[�g
            //������������������������������������������������������������������������������������������
            TextureImporter importer = TextureImporter.GetAtPath(texturePath) as TextureImporter;

            //������������������������������������������������������������������������������������������
            //	�e�N�X�`���𕪊����₷���ݒ��
            //������������������������������������������������������������������������������������������
            importer.textureType = TextureImporterType.Sprite;
            importer.spriteImportMode = SpriteImportMode.Multiple;
            importer.filterMode = FilterMode.Point;

            //������������������������������������������������������������������������������������������
            //	�ݒ�X�V
            //������������������������������������������������������������������������������������������
            EditorUtility.SetDirty(importer);
            AssetDatabase.ImportAsset(texturePath, ImportAssetOptions.ForceUpdate);

            Texture texture = AssetDatabase.LoadAssetAtPath(texturePath, typeof(Texture)) as Texture;

            //������������������������������������������������������������������������������������������
            //	PixelsPerUnit���e�N�X�`���̒��ӂ�
            //������������������������������������������������������������������������������������������
            importer.spritePixelsPerUnit = Mathf.Max(texture.width / horizontalCount, texture.height / verticalCount);

            //������������������������������������������������������������������������������������������
            //	�������s��
            //������������������������������������������������������������������������������������������
            importer.spritesheet = CreateSpriteMetaDataArray(texture, horizontalCount, verticalCount);

            //������������������������������������������������������������������������������������������
            //	�ݒ�X�V
            //������������������������������������������������������������������������������������������
            EditorUtility.SetDirty(importer);
            AssetDatabase.ImportAsset(texturePath, ImportAssetOptions.ForceUpdate);
        }

        //������������������������������������������������������������������������������������������
        //	�X�v���C�g�𕪊����鏈��
        //������������������������������������������������������������������������������������������
        static SpriteMetaData[] CreateSpriteMetaDataArray(Texture texture, int horizontalCount, int verticalCount)
        {
            //������������������������������������������������������������������������������������������
            //	�e�N�X�`���̕��ƍ���
            //������������������������������������������������������������������������������������������
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

                        //������������������������������������������������������������������������������������������
                        //	���O�ƈʒu�ƃT�C�Y��ݒ�
                        //������������������������������������������������������������������������������������������
                        name = string.Format("{0}_{1}", texture.name, index),
                        rect = new Rect(spriteWidth * x, texture.height - spriteHeight * (y + 1), spriteWidth, spriteHeight)
                    };
                })
                .ToArray();
        }
    }

    //������������������������������������������������������������������������������������������
    //
    //	class PattternsGenerate    ���I�[�g�^�C���𐶐�����N���X
    //
    //������������������������������������������������������������������������������������������
    static public class PatternsGenerate
    {
        //������������������������������������������������������������������������������������������
        //	�c5���������I�[�g�^�C���������4���������X�v���C�g����������z��
        //������������������������������������������������������������������������������������������
        static private Sprite[,] m_Segments = new Sprite[5, 4];

        //������������������������������������������������������������������������������������������
        //	���̔z�u�����߂�p�^�[���̔z��
        //������������������������������������������������������������������������������������������
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

        //������������������������������������������������������������������������������������������
        //	�e�N�X�`������47�p�^�[���𐶐�����
        //������������������������������������������������������������������������������������������
        static public void GeneratePatterns(WolfAutoTile tile)
        {
            //������������������������������������������������������������������������������������������
            //	���łɂ���^�C���e�N�X�`��������
            //������������������������������������������������������������������������������������������
            var allAsset = AssetDatabase.LoadAllAssetsAtPath(AssetDatabase.GetAssetPath(tile));
            foreach (var asset in allAsset)
            {
                if (!(asset is WolfAutoTile)) { Object.DestroyImmediate(asset, true); }
            }

            //������������������������������������������������������������������������������������������
            //	�n���ꂽ�X�v���C�g�̃e�N�X�`���𐶐�
            //������������������������������������������������������������������������������������������
            Texture2D tex = tile.m_Sprites[0].texture;
            tex = tile.m_Sprites[0].texture;

            //������������������������������������������������������������������������������������������
            //	���̃e�N�X�`���̖��O���m��
            //������������������������������������������������������������������������������������������
            string nam = tex.name;

            //������������������������������������������������������������������������������������������
            //	�A�j���[�V�����̃p�^�[������
            //������������������������������������������������������������������������������������������
            int anp = (int)tex.width / ((int)tex.height / 5);

            //������������������������������������������������������������������������������������������
            //	�p�X�ƃA�j������ۊ�
            //������������������������������������������������������������������������������������������
            tile.m_anip = anp;

            //������������������������������������������������������������������������������������������
            //	�e�N�X�`������
            //������������������������������������������������������������������������������������������
            int t_height = ((int)tex.height / 5 + 2) * 47;
            int t_width = ((int)tex.height / 5 + 2) * anp;
            Texture2D m_Tex = new Texture2D(t_width, t_height, TextureFormat.ARGB32, false);
            Color[] texArray_ = new Color[t_width * t_height];
            for (int c = 0; c < texArray_.Length; c++) { texArray_[c] = new Color(0, 0, 0, 0); }
            m_Tex.SetPixels(texArray_);
            m_Tex.Apply();

            //������������������������������������������������������������������������������������������
            //	�X�v���C�g�S�̂̏�����
            //������������������������������������������������������������������������������������������
            tile.m_Sprites = new Sprite[47 * anp];
            for (int a = 0; a < anp; a++)
            {
                //������������������������������������������������������������������������������������������
                //	1�p�^�[���̃T�C�Y��ʒu
                //������������������������������������������������������������������������������������������
                int height = (int)tex.height / 5;
                int width = (int)tex.height / 5;
                int x = width * a;
                int y = (int)tex.height;
                int height_half = height / 2;
                int width_half = width / 2;

                //������������������������������������������������������������������������������������������
                //	�����X�v���C�g�̔z�񏉊���
                //������������������������������������������������������������������������������������������
                m_Segments = new Sprite[5, 4];

                //������������������������������������������������������������������������������������������
                //	�����X�v���C�g����
                //������������������������������������������������������������������������������������������
                for (int i = 0; i < 5; i++)
                {
                    y -= height;
                    m_Segments[i, 0] = Sprite.Create(tex, new Rect(x, y, width_half, height_half), Vector2.zero);
                    m_Segments[i, 1] = Sprite.Create(tex, new Rect(x + width_half, y, width_half, height_half), Vector2.zero);
                    m_Segments[i, 2] = Sprite.Create(tex, new Rect(x, y + height_half, width_half, height_half), Vector2.zero);
                    m_Segments[i, 3] = Sprite.Create(tex, new Rect(x + width_half, y + height_half, width_half, height_half), Vector2.zero);

                }

                //������������������������������������������������������������������������������������������
                //	�T�C�Y���Đݒ�
                //������������������������������������������������������������������������������������������
                width_half = (int)m_Segments[0, 0].rect.width;
                height_half = (int)m_Segments[0, 0].rect.height;
                width = width_half * 2;
                height = height_half * 2;

                //������������������������������������������������������������������������������������������
                //	�X�|�C�g�p�̔z��
                //������������������������������������������������������������������������������������������
                Color[] texArray = new Color[width * height];

                //������������������������������������������������������������������������������������������
                //	�����X�v���C�g�Ɣz�u�����ւ���47�p�^�[���̑S�h�b�g�̐F���R�s�[���Ă���
                //������������������������������������������������������������������������������������������
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

                    //������������������������������������������������������������������������������������������
                    //	�p�^�[���ɂ������z�u�ŐF��ݒu
                    //������������������������������������������������������������������������������������������
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
            //������������������������������������������������������������������������������������������
            //	�擾�����F���悹�ĕ������₷���ݒ��
            //������������������������������������������������������������������������������������������
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
                                //������������������������������������������������������������������������������������������
                                //	��{�X�v���C�g�^�C���㏑��
                                //������������������������������������������������������������������������������������������
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
    //������������������������������������������������������������������������������������������
    //
    //	class WolfAutoTileEditor    ���ݒ���
    //
    //������������������������������������������������������������������������������������������
    public class WolfAutoTileEditor : Editor
    {
        private WolfAutoTile tile { get { return (target as WolfAutoTile); } }

        //������������������������������������������������������������������������������������������
        //	�^�C���t�@�C�����I�����ꂽ��
        //������������������������������������������������������������������������������������������
        public void OnEnable()
        {
            //Debug.Log("OnEnable");
            if (tile.m_Sprites == null) { tile.m_Sprites = new Sprite[47]; }
        }
        //������������������������������������������������������������������������������������������
        //	�ݒ��ʂ̕`��
        //������������������������������������������������������������������������������������������
        public override void OnInspectorGUI()
        {
            //Debug.Log("OnInspectorGUI");"�ȉ��ɃR���o�[�g�e�N�X�`�����ۑ�����܂��B"
            EditorGUILayout.LabelField("�E�f�B�^�K�i�̃I�[�g�^�C�����X���b�g���Ă��������B");
            EditorGUILayout.Space();
            EditorGUI.BeginChangeCheck();
            tile.pngOut = EditorGUILayout.ToggleLeft("Assets/�ɘA���摜�������o��", tile.pngOut);
            if (EditorGUI.EndChangeCheck())
            {
                if (tile.pngOut && tile.m_Sprites[46])
                {
                    string path = Application.dataPath + "/" + tile.m_Sprites[46].texture.name + ".png";
                    var bytes = tile.m_Sprites[46].texture.EncodeToPNG();
                    File.WriteAllBytes(path, bytes);
                }
                EditorUtility.SetDirty(tile);

                //������������������������������������������������������������������������������������������
                //	�v���W�F�N�g�̍X�V
                //������������������������������������������������������������������������������������������
                AssetDatabase.Refresh();
            }

            EditorGUI.BeginChangeCheck();
            tile.m_Sprites[46] = (Sprite)EditorGUILayout.ObjectField("��{�^�C���摜", tile.m_Sprites[46], typeof(Sprite), false, null);

            if (EditorGUI.EndChangeCheck())
            {
                //������������������������������������������������������������������������������������������
                //	�摜���ݒ肳�ꂽ��
                //������������������������������������������������������������������������������������������
                if (tile.m_Sprites[46])
                {
                    tile.m_Sprites[0] = tile.m_Sprites[46];
                    //������������������������������������������������������������������������������������������
                    //	�摜���Q�Ɖ\�ɂ���
                    //������������������������������������������������������������������������������������������
                    string texturePath = (AssetDatabase.GetAssetPath(tile.m_Sprites[0].texture));
                    TextureImporter importer = TextureImporter.GetAtPath(texturePath) as TextureImporter;
                    importer.isReadable = true;

                    //������������������������������������������������������������������������������������������
                    //	�摜�̐ݒ�X�V
                    //������������������������������������������������������������������������������������������
                    EditorUtility.SetDirty(importer);
                    AssetDatabase.ImportAsset(texturePath, ImportAssetOptions.ForceUpdate);

                    //������������������������������������������������������������������������������������������
                    //	47�p�^�[���摜�ƃX�v���C�g�̐���
                    //������������������������������������������������������������������������������������������
                    PatternsGenerate.GeneratePatterns(tile);

                    if (tile.pngOut && tile.m_Sprites[46])
                    {
                        string path = Application.dataPath + "/" + tile.m_Sprites[46].texture.name + ".png";
                        var bytes = tile.m_Sprites[46].texture.EncodeToPNG();
                        File.WriteAllBytes(path, bytes);
                    }

                    //������������������������������������������������������������������������������������������
                    //	�^�C���̐ݒ�X�V
                    //������������������������������������������������������������������������������������������
                    EditorUtility.SetDirty(tile);

                    //������������������������������������������������������������������������������������������
                    //	�v���W�F�N�g�̍X�V
                    //������������������������������������������������������������������������������������������
                    AssetDatabase.Refresh();
                }
            }

            //������������������������������������������������������������������������������������������
            //	�A�j���^�C���̐ݒ�
            //������������������������������������������������������������������������������������������
            if (tile.m_anip > 1)
            {
                EditorGUI.BeginChangeCheck();
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("�A�j���^�C���̏������Ă�������   " + tile.m_anip + "�p�^�[��");
                EditorGUILayout.Space();
                float minSpeed = EditorGUILayout.FloatField("�Œᑬ�x", tile.m_MinSpeed);
                float maxSpeed = EditorGUILayout.FloatField("�ō����x", tile.m_MaxSpeed);
                if (minSpeed < 0.0f)
                    minSpeed = 0.0f;
                if (maxSpeed < 0.0f)
                    maxSpeed = 0.0f;
                if (maxSpeed < minSpeed)
                    maxSpeed = minSpeed;
                tile.m_MinSpeed = minSpeed;
                tile.m_MaxSpeed = maxSpeed;

                tile.m_AnimationStartTime = EditorGUILayout.FloatField("�J�n����", tile.m_AnimationStartTime);
                if (EditorGUI.EndChangeCheck()) { EditorUtility.SetDirty(tile); }
            }
        }
    }

    //������������������������������������������������������������������������������������������
    //
    //	class CreateWAutoTiles    �������̉摜�t�@�C����A���ŃI�[�g�^�C���ɂ���E�B���h�E
    //
    //������������������������������������������������������������������������������������������
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
                lb1 = "���҂��������B";
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

                    //������������������������������������������������������������������������������������������
                    //	null�`�F�b�N
                    //������������������������������������������������������������������������������������������
                    if (!tile.m_Sprites[0])
                    {
                        Debug.Log("null Area002:" + tile);
                        return;
                    }

                    //������������������������������������������������������������������������������������������
                    //	�摜���Q�Ɖ\�ɂ���
                    //������������������������������������������������������������������������������������������
                    string texturePath = (AssetDatabase.GetAssetPath(tile.m_Sprites[0].texture));
                    TextureImporter importer = TextureImporter.GetAtPath(texturePath) as TextureImporter;
                    importer.isReadable = true;

                    //������������������������������������������������������������������������������������������
                    //	�摜�̐ݒ�X�V
                    //������������������������������������������������������������������������������������������
                    EditorUtility.SetDirty(importer);
                    AssetDatabase.ImportAsset(texturePath, ImportAssetOptions.ForceUpdate);
                    AssetDatabase.Refresh();

                    //������������������������������������������������������������������������������������������
                    //	47�p�^�[���摜�ƃX�v���C�g�̐���
                    //������������������������������������������������������������������������������������������
                    PatternsGenerate.GeneratePatterns(tile);

                    //������������������������������������������������������������������������������������������
                    //	null�`�F�b�N
                    //������������������������������������������������������������������������������������������
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
                        //������������������������������������������������������������������������������������������
                        //	�^�C���̐ݒ�X�V
                        //������������������������������������������������������������������������������������������
                        EditorUtility.SetDirty(tile);
                    }

                    //������������������������������������������������������������������������������������������
                    //	�v���W�F�N�g�̍X�V
                    //������������������������������������������������������������������������������������������
                    AssetDatabase.Refresh();
                    Resources.UnloadUnusedAssets();
                }
                c += 1;
            }
            else
            {
                lb1 = "�I�[�g�^�C���摜���h���b�v���Ă��������B";
                lb2 = "�w��̃t�@�C����+�A�Ԃňꊇ�t�@�C�������܂��B";
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

                                    //������������������������������������������������������������������������������������������
                                    //	null�`�F�b�N
                                    //������������������������������������������������������������������������������������������
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

    //������������������������������������������������������������������������������������������
    //
    //	class HierarchyChange    ���������ɂ��s��ɑΉ�����N���X
    //
    //������������������������������������������������������������������������������������������
    static class HierarchyChange
    {
        static bool sceneChange = false;

        [InitializeOnLoadMethod]
        static void HC()
        {
            //������������������������������������������������������������������������������������������
            //	���ׂẴA�Z���u�����X�V���ꂽ��V�[����ǂݍ��ݒ���
            //������������������������������������������������������������������������������������������
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
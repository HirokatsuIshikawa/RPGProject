#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ItemDataBase))]//�g������N���X���w��
public class ItemDataBaseEditor : Editor
{
    private const string PATH = "Assets/DataBase/item/";

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("DataSet"))
        {
            //csv��ǂݍ���
            var path = EditorUtility.OpenFilePanel("pos text", "", "csv");
            if (string.IsNullOrEmpty(path))
            {
                return;
            }
            //�ǂݍ��݃f�[�^�Z�b�g
            StreamReader reader = new StreamReader(path);
            //�^�[�Q�b�g�ݒ�
            ItemDataBase dataBase = (ItemDataBase)target;
            //���X�g������
            dataBase.GetItemLists().Clear();
            setPos(reader, dataBase);
        }
    }

    //�^�C�~���O�����炵�Ă̈ʒu�Z�b�g
    private void setPos(StreamReader reader, ItemDataBase dataBase)
    {
        //�X�v���C�g���X�g���擾
        Sprite[] sprites = Resources.LoadAll<Sprite>("");

        //�����񂪓ǂݍ��߂Ȃ��Ȃ�܂ŌJ��Ԃ�
        for (int i = 0; i < 256; i++)
        {
            string text = reader.ReadLine();
            //�ǂݍ��߂Ȃ��Ȃ����甲����
            if (text == null)
            {
                break;
            }
            //�R�����g�A�E�g�͔�΂�
            if (text.IndexOf("//") == 0)
            {
                continue;
            }
            //�J���}��؂�ŕ���
            string[] csvData = text.Split(',');
            //�f�[�^�쐬
            ItemData data = CreateInstance<ItemData>();
            //�f�[�^�ݒ�
            data.id = int.Parse(csvData[1]);
            data.itemName = csvData[2];
            data.value = int.Parse(csvData[3]);
            //�f�[�^�o�^
            AssetDatabase.CreateAsset(data, PATH + data.id + "_" + data.itemName + ".asset");
            //���X�g�o�^
            dataBase.GetItemLists().Add(data);
            EditorUtility.SetDirty(data);
        }
        //�A�Z�b�g�̃Z�[�u
        AssetDatabase.SaveAssets();
    }
}

#endif
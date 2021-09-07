using UnityEngine;

[CreateAssetMenu(fileName = "Material", menuName = "ItemData/MaterialData")]//  Create����CreateShelter�Ƃ������j���[��\�����AShelter���쐬����
public class MaterialData : ScriptableObject
{

    [SerializeField]
    public int id;//�@ID
    [SerializeField]
    public string itemName;//�@���O
    [SerializeField]
    public int rarity;  //���A���e�B
    [SerializeField]
    public Sprite icon;//�@���ꏊ�̃A�C�R��

    public Sprite GetIcon()//  �A�C�R������͂�����A
    {
        return icon;//  icon�ɕԂ�
    }
}
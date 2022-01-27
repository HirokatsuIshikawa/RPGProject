#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlayerAnime))]//拡張するクラスを指定
public class PlayerAnimeEditor : CharaAnimeEditor
{
}

#endif
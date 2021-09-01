using UnityEngine;

public class MapObject : MonoBehaviour
{
    public string changeMapName;
    public Vector2 movePos;
    public CharaAnime.DIRECTION direction;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {

        Debug.Log(gameObject.name + " : " + "Trigger");
        //マップ変更中は実行しない
        if (ContentManager.instance.changingMap)
        {
            ContentManager.instance.moveContactFlg = true;
            return;
        }
        else
        {
            if (ContentManager.instance.moveContactFlg == false)
            {
                ContentManager.instance.changeMapStart(changeMapName, movePos, direction);
                ContentManager.instance.moveContactFlg = true;
            }
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log(gameObject.name + " : " + "TriggerExit");
        //if (ContentManager.instance.changingMap) return;
        ContentManager.instance.moveContactFlg = false;
    }

}

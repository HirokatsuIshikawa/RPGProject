using UnityEngine;

public class MapObject : MonoBehaviour
{
    public string changeMapName;
    public Vector2 movePos;


    private void OnTriggerEnter2D(Collider2D collision)
    {

        Debug.Log(gameObject.name + " : " + "Trigger");
        if (ContentManager.instance.moveContactFlg || ContentManager.instance.changingMap) return;
        ContentManager.instance.moveContactFlg = true;
        ContentManager.instance.changeMap(changeMapName, movePos);
    }

    
    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log(gameObject.name + " : " + "TriggerExit");
        //if (ContentManager.instance.changingMap) return;
        ContentManager.instance.moveContactFlg = false;
    }
    
}

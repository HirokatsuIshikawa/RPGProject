using UnityEngine;
using static EventObject;

public class TalkAction : MonoBehaviour
{
    public CharaAnime anime = null;
    public EventObject eventObj = null;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(ContentManager.instance.actionCoolTime > 0)
        {
            return;
        }
        if(ContentManager.instance.nowEventType != EventObject.EventType.None)
        {
            return;
        }
        /*
        //画面タップ時の操作
        //UIに当たったらスクロールしない
        if (Input.touchCount >= 1)
        {
            if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId) == false)
            {
                //画面タップしたとき
                if (Input.GetMouseButtonDown(0))
                {

                }
            }
        }
        */
        //画面タップ時の操作
        if (TouchState.instance.getTouchEndedSoon())
        {
            if(eventObj != null)
            {
                if (eventObj.eventStartType == EventStartType.Tap)
                {
                    eventObj.eventStart();
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        //Debug.Log(gameObject.name + " : " + "TalkActTrigger");
        eventObj = collision.gameObject.GetComponent<EventObject>();
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        //Debug.Log(gameObject.name + " : " + "TalkActTriggerExit");
        //if (ContentManager.instance.changingMap) return;
        //ContentManager.instance.eventAreaEntryFlg = false;
        eventObj = null;
    }
}

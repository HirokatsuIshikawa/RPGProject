using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventObject : MonoBehaviour
{
    public enum EventStartType
    {
        None,       //起動しない
        Entry,      //当たり判定に進入
        Tap,        //当たり判定が当たっているときにタップ
        InMap,      //マップ開始時
        Trigger     //外部からのトリガー
    }

    public enum EventType
    {
        None,
        Talk,
        Move

    }
    
    public EventStartType eventStartType = EventStartType.Tap;
    public CharaAnime charaAnime = null;
    public List<string> msg = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void eventStart()
    {
        ContentManager.instance.nowEventFlg = true;
        ContentManager.instance.eventStart(msg);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(gameObject.name + " : " + "TalkTrigger");
        if(eventStartType == EventStartType.Entry)
        {
            if (ContentManager.instance.isEventing())
            {
                ContentManager.instance.eventAreaEntryFlg = true;
                return;
            }
            else
            {
                if (ContentManager.instance.eventAreaEntryFlg == false)
                {
                    ContentManager.instance.eventAreaEntryFlg = true;
                    eventStart();
                }
            }
        }
    }
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log(gameObject.name + " : " + "TalkTriggerExit");
        ContentManager.instance.eventAreaEntryFlg = false;
    }
}

using System.Collections.Generic;
using UnityEngine;

public class EventObject : MonoBehaviour
{
    public enum EventStartType
    {
        None,       //起動しない
        Auto,       //同マップに存在したら自動実行
        Entry,      //当たり判定に進入
        Tap,        //当たり判定が当たっているときにタップ
        InMap,      //マップ開始時
        Trigger     //外部からのトリガー
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
        ContentManager.instance.process = ContentManager.SystemProcess.Talk;
        ContentManager.instance.eventStart(msg);
    }
    
}

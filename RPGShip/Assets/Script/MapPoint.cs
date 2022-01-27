using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPoint : MonoBehaviour
{
    public List<MapPoint> connectMapList;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void TouchPoint()
    {
        ContentManager.instance.SetMapMenu(true, this);
        /*
        ContentManager.instance.playerMapPoint.transform.position = 
            new Vector3(
                this.transform.position.x, 
                this.transform.position.y, 
                ContentManager.instance.playerMapPoint.transform.position.z
                );
        Debug.Log("離した瞬間2");
        */
    }


}

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
        ShipContentManager.instance.playerMapPoint.transform.position = 
            new Vector3(
                this.transform.position.x, 
                this.transform.position.y, 
                ShipContentManager.instance.playerMapPoint.transform.position.z
                );
        Debug.Log("ó£ÇµÇΩèuä‘2");
    }


}

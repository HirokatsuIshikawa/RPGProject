using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    // 追加
    private Vector3 screenPoint;
    private Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    // 追加
    public void OnMouseDown()
    {
        if (ContentManager.instance.process == ContentManager.SystemProcess.None)
        {
            this.screenPoint = Camera.main.WorldToScreenPoint(transform.position);
            this.offset = transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
        }
    }
    public void OnMouseUp()
    {

    }
    // 追加
    public void OnMouseDrag()
    {
        if (ContentManager.instance.process == ContentManager.SystemProcess.None)
        {
            Vector3 currentScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
            Vector3 currentPosition = Camera.main.ScreenToWorldPoint(currentScreenPoint) + this.offset;
            transform.position = currentPosition;
        }
    }


}

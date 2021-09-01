using System.Collections;
using UnityEngine;

//カメラコントローラー
public class CameraControll : MonoBehaviour
{
    //メインカメラ
    public Transform cameraTransform;
    //追尾ターゲット
    [SerializeField]
    private Transform targetTransform;
    //変更後追尾ターゲット
    private Transform changeTargetTransform;
    // Start is called before the first frame update
    void Start()
    {
        //カメラがない場合は現在のものから取得
        if (cameraTransform == null)
        {
            cameraTransform = this.transform;
        }

    }

    // Update is called once per frame
    void Update()
    {
        //ターゲットがあれば追尾
        if (targetTransform != null)
        {
            cameraTransform.position = new Vector3(targetTransform.position.x, targetTransform.position.y, this.transform.position.z);
        }
    }

    public void changeTarget(Transform transform, float time = 1.0f, iTween.EaseType ease = iTween.EaseType.linear)
    {
        targetTransform = transform;
        clearTarget();
        Hashtable parameters = new Hashtable();
        parameters.Add("x", transform.position.x);
        parameters.Add("y", transform.position.y);
        parameters.Add("islocal", true);
        parameters.Add("time", time);
        parameters.Add("oncomplete", "CompleteHandler");
        parameters.Add("easeType", ease.ToString());
        iTween.MoveTo(gameObject, parameters);
    }

    public void CompleteHandler()
    {
        setTarget(changeTargetTransform);
    }
    
    public void setTarget(Transform transform)
    {
        targetTransform = transform;
    }

    public void clearTarget()
    {
        targetTransform = null;
    }

}

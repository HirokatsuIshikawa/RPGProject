using Cinemachine;
using System.Collections;
using UnityEngine;

//カメラコントローラー
public class CameraControll : MonoBehaviour
{
    //メインカメラ
    public Transform cameraTransform;

    //[SerializeField]
    //private CinemachineVirtualCamera virtualCamera;
    [SerializeField]
    private CinemachineConfiner cinemachineCOnfiner;
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
        /*
        if (virtualCamera == null)
        {
            virtualCamera = this.GetComponent<CinemachineVirtualCamera>();
        }
        */
        if (cinemachineCOnfiner == null)
        {
            cinemachineCOnfiner = this.GetComponent<CinemachineConfiner>();
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
        clearTarget();

        changeTargetTransform = transform;
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
        changeTargetTransform = null;
    }

    public void setTarget(Transform transform)
    {
        targetTransform = transform;
        //virtualCamera.LookAt = transform;
    }

    public void clearTarget()
    {
        targetTransform = null;
        //virtualCamera.LookAt = null;
    }

    //カメラ移動制限コライダーのターゲットを設定
    public void setCameraBound(CompositeCollider2D colider)
    {
        cinemachineCOnfiner.m_BoundingShape2D = colider;
    }

}

using UnityEngine;

public class MiniCameraControll : MonoBehaviour
{
    public Transform targetTransform;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        //ターゲットがあれば追尾
        if (targetTransform != null)
        {
            transform.position = new Vector3(targetTransform.position.x, targetTransform.position.y, this.transform.position.z);
        }
    }
}

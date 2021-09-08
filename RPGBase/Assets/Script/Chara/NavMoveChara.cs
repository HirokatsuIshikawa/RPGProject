using UnityEngine;
using UnityEngine.AI;

public class NavMoveChara : MonoBehaviour
{
    public enum CHASE_TYPE
    {
        NONE,
        POS,
        OBJ
    }

    public CHASE_TYPE chaseType = CHASE_TYPE.NONE;
    public CharaAnime anime;
    public GameObject target;
    public Vector3 targetPos;
    private NavMeshAgent agent;
    public Animator animator;
    public float distance;
    float time;
    public Vector3 beforePos;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    //ターゲット設定・物体
    public void setTarget(GameObject obj)
    {
        anime.autoMove = true;
        agent.isStopped = false;
        target = gameObject;
        chaseType = CHASE_TYPE.OBJ;
    }

    //ターゲット設定・位置
    public void setTarget(Vector3 pos)
    {
        anime.autoMove = true;
        agent.isStopped = false;
        targetPos = pos;
        chaseType = CHASE_TYPE.POS;
    }


    void FixedUpdate()
    {
        float now = 0;
        Vector3 targetNowPos = Vector3.zero;

        if (chaseType == CHASE_TYPE.NONE)
        {
            return;
        }
        else if (chaseType == CHASE_TYPE.POS)
        {
            targetNowPos = targetPos;
        }
        else if (chaseType == CHASE_TYPE.OBJ)
        {
            targetNowPos = target.transform.position;
        }
        //現在の距離
        now = Vector2.Distance(transform.position, targetNowPos);
        if (distance < now)
        {
            //目的地を設定・移動
            agent.destination = targetNowPos;
            //方向取得
            Vector3 vector = transform.position - beforePos;
            anime.inputAxis = vector.normalized;
        }
        else
        {
            finishWalk();
        }
        
        //setStateToAnimator(vector: vector != Vector2.zero ? vector : (Vector2?)null);
        beforePos = transform.position;
    }


    private void finishWalk()
    {
        chaseType = CHASE_TYPE.NONE;
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        anime.autoMove = false;
        anime.inputAxis = Vector3.zero;
        target = null;
        targetPos = Vector3.zero;
        agent.enabled = false;
        this.enabled = false;
    }



    private void setStateToAnimator(Vector2? vector)
    {
        if (!vector.HasValue)
        {
            this.animator.speed = 0.0f;
            return;
        }
        this.animator.speed = 1.0f;
        this.animator.SetFloat("x", vector.Value.x);
        this.animator.SetFloat("y", vector.Value.y);
    }
}
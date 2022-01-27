using System.Collections.Generic;
using UnityEngine;

public class MoveChara : MonoBehaviour
{
    public enum CHASE_TYPE
    {
        NONE,
        POS,
        OBJ,
        ROOT
    }

    public CHASE_TYPE chaseType = CHASE_TYPE.NONE;
    public CharaAnime anime;
    public List<Transform> targetList;
    public List<Vector3> targetPosList;
    public int targetIndex;
    public Animator animator;
    public float distance;
    float time;
    public Vector3 beforePos;

    void Start()
    {
    }

    //ターゲット設定・物体
    public void setTarget(Transform obj)
    {
        targetList.Clear();
        targetIndex = 0;
        targetList.Add(obj);
        chaseType = CHASE_TYPE.OBJ;
    }

    //ターゲット設定・物体リスト
    public void setTarget(List<Transform> objList)
    {
        targetList.Clear();
        targetIndex = 0;
        targetList = objList;
        chaseType = CHASE_TYPE.OBJ;
    }

    //ターゲット設定・位置リスト
    public void setTarget(List<Vector3> pos)
    {
        targetPosList.Clear();
        targetPosList = pos;
        targetIndex = 0;
        chaseType = CHASE_TYPE.POS;
    }
    //ターゲット設定・位置
    public void setTarget(Vector3 pos)
    {
        targetPosList.Clear();
        targetPosList.Add(pos);
        targetIndex = 0;
        chaseType = CHASE_TYPE.POS;
    }


    //ルート設定・位置リスト
    public void setTargetRoot(List<Vector3> pos)
    {
        targetPosList = pos;
        targetIndex = 0;
        chaseType = CHASE_TYPE.ROOT;
    }
    
    //ルート設定・物体リスト
    public void setTargetRoot(List<Transform> objList)
    {
        targetList.Clear();
        targetIndex = 0;
        targetList = objList;
        chaseType = CHASE_TYPE.ROOT;
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
            targetNowPos = targetPosList[targetIndex];
        }
        else if (chaseType == CHASE_TYPE.OBJ)
        {
            targetNowPos = targetList[targetIndex].position;
        }
        //現在の距離
        now = Vector2.Distance(transform.position, targetNowPos);
        if (distance < now)
        {
            //目的地を設定・移動
            Vector2 vector = targetNowPos - transform.position;            
            //方向取得
            //Vector3 vector = transform.position - beforePos;
            anime.inputAxis = vector.normalized;
        }
        else
        {
            if (chaseType == CHASE_TYPE.ROOT)
            {
                roopWalk();
            }
            else
            {
                finishWalk();
            }
        }
        
        beforePos = transform.position;
    }
    
    private void finishWalk()
    {
        chaseType = CHASE_TYPE.NONE;
        anime.inputAxis = Vector3.zero;
        targetList.Clear();
        targetPosList.Clear();
        this.enabled = false;
        targetIndex = 0;
    }

    private void roopWalk()
    {
        targetIndex = 0;
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
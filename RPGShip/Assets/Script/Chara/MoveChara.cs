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

    //�^�[�Q�b�g�ݒ�E����
    public void setTarget(Transform obj)
    {
        targetList.Clear();
        targetIndex = 0;
        targetList.Add(obj);
        chaseType = CHASE_TYPE.OBJ;
    }

    //�^�[�Q�b�g�ݒ�E���̃��X�g
    public void setTarget(List<Transform> objList)
    {
        targetList.Clear();
        targetIndex = 0;
        targetList = objList;
        chaseType = CHASE_TYPE.OBJ;
    }

    //�^�[�Q�b�g�ݒ�E�ʒu���X�g
    public void setTarget(List<Vector3> pos)
    {
        targetPosList.Clear();
        targetPosList = pos;
        targetIndex = 0;
        chaseType = CHASE_TYPE.POS;
    }
    //�^�[�Q�b�g�ݒ�E�ʒu
    public void setTarget(Vector3 pos)
    {
        targetPosList.Clear();
        targetPosList.Add(pos);
        targetIndex = 0;
        chaseType = CHASE_TYPE.POS;
    }


    //���[�g�ݒ�E�ʒu���X�g
    public void setTargetRoot(List<Vector3> pos)
    {
        targetPosList = pos;
        targetIndex = 0;
        chaseType = CHASE_TYPE.ROOT;
    }
    
    //���[�g�ݒ�E���̃��X�g
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
        //���݂̋���
        now = Vector2.Distance(transform.position, targetNowPos);
        if (distance < now)
        {
            //�ړI�n��ݒ�E�ړ�
            Vector2 vector = targetNowPos - transform.position;            
            //�����擾
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
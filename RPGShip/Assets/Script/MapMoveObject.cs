using UnityEngine;

public class MapMoveObject : MonoBehaviour
{
    public string changeMapName;
    public Vector2 movePos;
    public CharaAnime.DIRECTION direction;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {

        Debug.Log(gameObject.name + " : " + "Trigger");
        //�}�b�v�ύX�Ȃǂ̃C�x���g���͎��s���Ȃ����ǐڐG�t���O�𗧂Ă�
        if (ContentManager.instance.isEventing())
        {
            ContentManager.instance.eventAreaEntryFlg = true;
            return;
        }
        else
        {
            if (ContentManager.instance.eventAreaEntryFlg == false)
            {
                ContentManager.instance.changeMapStart(changeMapName, movePos, direction);
                ContentManager.instance.eventAreaEntryFlg = true;
            }
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log(gameObject.name + " : " + "TriggerExit");
        //if (ContentManager.instance.changingMap) return;
        ContentManager.instance.eventAreaEntryFlg = false;
    }

}

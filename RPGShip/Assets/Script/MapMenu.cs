using UnityEngine;
using UnityEngine.UI;

public class MapMenu : MonoBehaviour
{
    public Button moveButton;
    public Button organizationButton;
    public Button infoButton;
    public Button supplyButton;
    public Button shopButton;
    public Button workButton;
    public Button closeButton;

    public MapPoint selectMapPoint;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetButtonEnable(MapPoint point, MapPoint nowPoint)
    {
        selectMapPoint = point;
        if (point == nowPoint)
        {
            moveButton.interactable = false;
        }
        else
        {
            moveButton.interactable = true;
            infoButton.interactable = false;
            supplyButton.interactable = false;
            shopButton.interactable = false;
            workButton.interactable = false;
        }



    }

    public void PushMoveButton()
    {
        AskPopup popup = ContentManager.instance.askPopup;
        popup.init();
        popup.SetEvent(ContentManager.instance.MovePoint);
        popup.SetDiscriptionText("移動しますか？");
        popup.gameObject.SetActive(true);
    }

    public void PushCloseButton()
    {
        ContentManager.instance.SetMapMenu(false);
    }

}

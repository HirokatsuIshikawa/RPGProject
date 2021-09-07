using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField]
    public GameObject MenuObject;
    [SerializeField]
    public Button openMenuButton;





    public void buttonOnMenu()
    {
        MenuObject.SetActive(true);
        openMenuButton.gameObject.SetActive(false);
        ContentManager.instance.menuFlg = true;
    }

    public void buttonCloseMenu()
    {
        MenuObject.SetActive(false);
        openMenuButton.gameObject.SetActive(true);
        ContentManager.instance.menuFlg = false;
    }

}

using UnityEngine;

public class MenuActiveController : MonoBehaviour
{
    [SerializeField]
    private GameObject      menuParent = null;

    private MenuController  menuController = null;


    public void AwakeInitialize()
    {
        menuParent = transform.GetChild(0).gameObject;
        if (menuParent != null)
        {
            menuController = menuParent.GetComponent<MenuController>();
            if (menuController != null)
            {
                menuController.AwakeInitialize();
            }
        }
    }

    public void StartInitialize()
    {
        if (menuController != null)
        {
            menuController.StartInitialize();
        }
        if (menuParent.activeSelf)
        {
            menuParent.SetActive(false);
        }
    }

    public void MenuActiveUpdate()
    {
        if(GetItemMessage.Instance.ItemData != null) { return; }
        if (GameManager.GameState == GameManager.GameStateEnum.Pose)
        {
            if (!menuParent.activeSelf)
            {
                menuParent.SetActive(true);
                menuController.OpenMenuInitilaize();
            }
        }
        else
        {
            if (menuParent.activeSelf)
            {
                menuParent.SetActive(false);
            }
        }
        menuController.MenuUpdate();
    }
}

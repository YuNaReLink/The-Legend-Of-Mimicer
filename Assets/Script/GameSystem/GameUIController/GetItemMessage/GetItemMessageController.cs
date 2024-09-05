using UnityEngine;
using UnityEngine.UI;

public class GetItemMessage : MonoBehaviour
{
    private static GetItemMessage instance;
    public static GetItemMessage Instance => instance;

    [SerializeField]
    private GameObject itemWindow = null;
    [SerializeField]
    private Image itemIcon = null;
    [SerializeField]
    private Text nameText = null;
    [SerializeField]
    private Text explanationText = null;
    
    private ItemData itemData = null;
    public ItemData ItemData => itemData;
    public void SetItemData(ItemData _itemData)
    {
        itemData = _itemData;
    }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    private void Start()
    {
        itemWindow?.SetActive(false);
    }

    private void Update()
    {
        if(itemData == null) { return; }
        if (!itemWindow.activeSelf)
        {
            SetItemInformation();
        }
        else
        {
            ItemWindowEnd();
        }
    }

    private void SetItemInformation()
    {
        itemIcon.sprite = itemData.ItemIcon;
        nameText.text = itemData.ItemName + "‚ðŽè‚É“ü‚ê‚½!";
        explanationText.text = itemData.ItemExplanation;
        itemWindow?.SetActive(true);
        GameManager.GameState = GameManager.GameStateEnum.Pose;
    }

    private void ItemWindowEnd()
    {
        if (!InputManager.GetItemButton()) { return; }
        itemIcon.sprite = null;
        nameText.text = "";
        explanationText.text = "";
        itemWindow?.SetActive(false);
        itemData = null;
        GameManager.GameState = GameManager.GameStateEnum.Game;
    }
}

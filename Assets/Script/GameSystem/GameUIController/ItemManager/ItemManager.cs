using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour
{
    private PlayerController playerController = null;
    public void SetPlayerController(PlayerController _playerController)
    {
        playerController = _playerController; 
    }

    [SerializeField]
    private ItemDataBase itemDataBase = null;
    //�A�C�e���A�C�R����icon���w��
    [SerializeField]
    private List<GameObject> iconObjectList = new List<GameObject>();

    //�g�O���O���[�v�ł���Inventory���w��
    [SerializeField]
    private ToggleGroup toggleGroup = null;

    private enum ExplanationNumber
    {
        ItemName,
        ItemExplanation
    }

    [SerializeField]
    private List<Text> itemTextList = new List<Text>();

    //�A�C�e�����Ǘ�
    private Dictionary<ItemData,int> itemNumber = new Dictionary<ItemData,int>();

    //�������Ǘ�
    [SerializeField]
    private List<ItemData> getItemList = new List<ItemData>();

    //�A�C�R���Ǘ��̃��X�g
    [SerializeField]
    private List<Image> iconList = new List<Image>();


    //�A�C�e���g���{�^���őI������N���X
    private MenuToggleController itemToggleController = null;
    public MenuToggleController GetItemToggleController() { return itemToggleController; }

    private GameUIController gameUIController = null;

    public void AwakeInitialize()
    {
        //�e����PlayerController���擾
        PlayerConnectionUI playerConnectionUI = GetComponentInParent<PlayerConnectionUI>();
        if (playerConnectionUI != null)
        {
            PlayerController controller = playerConnectionUI.GetPlayerController();
            playerController = controller;
        }

        //�A�C�e���t���[�����擾
        int childCount = transform.childCount;
        GameObject g = null;
        for(int i = 0; i < childCount; i++)
        {
            g = transform.GetChild(i).gameObject;
            ToggleGroup _toggleGroup = g.GetComponent<ToggleGroup>();
            if(_toggleGroup != null)
            {
                toggleGroup = _toggleGroup;
            }
            if(g != null)
            {
                int count = g.transform.childCount;
                GameObject cg = null;
                for(int j = 0; j < count; j++)
                {
                    cg = g.transform.GetChild(j).gameObject;
                    if(cg != null)
                    {
                        if(i < 1)
                        {
                            iconObjectList.Add(cg);
                        }
                        else
                        {
                            Text text = null;
                            text = cg.GetComponent<Text>();
                            itemTextList.Add(text);
                        }
                    }
                }
            }
        }
        g = null;
        Image image = null;
        //�A�C�R���摜�̃A�^�b�`
        for(int i = 0;i < iconObjectList.Count; i++)
        {
            g = iconObjectList[i].transform.GetChild(1).gameObject;
            image = g.GetComponent<Image>();
            iconList.Add(image);
        }
        itemToggleController = GetComponentInChildren<MenuToggleController>();
        if(itemToggleController != null)
        {
            List<Toggle> toggles = new List<Toggle>();
            Toggle toggle = null;
            g = null;
            for(int i = 0; i < iconObjectList.Count; i++)
            {
                g = iconObjectList[i];
                toggle = g.GetComponent<Toggle>();
                if(toggle != null)
                {
                    toggles.Add(toggle);
                }
            }
            itemToggleController.ToggleList = toggles;
        }
        gameUIController = GetComponentInParent<GameUIController>();
    }

    public void StartInitialize()
    {
        //�������A�C�e���̏���
        for (int i = 0; i < itemDataBase.GetItemList().Count; i++)
        {
            //�A�C�e������S��0��
            itemNumber.Add(itemDataBase.GetItemList()[i], 0);
        }

        GetItemUpdate();
    }

    public void GetItemUpdate()
    {
        //�������X�V����
        
        //���������X�g�̃N���A
        getItemList.Clear();

        for(int i = 0; i<itemNumber.Count; i++)
        {
            var e = itemNumber[itemDataBase.GetItemList()[i]];

            if(e > 0)
            {
                getItemList.Add(itemDataBase.GetItemList()[i]);
            }
        }

        for(int i = 0;i < iconList.Count; i++)
        {
            iconList[i].sprite = null;
            iconList[i].color = new Color(1f, 1f, 1f, 1f);
        }

        //���������X�g�̗v�f�������J��Ԃ�
        for(int i = 0;i < getItemList.Count; i++)
        {
            var f = getItemList[i];
            //f�̃A�C�R�������X�g�ɑ��
            iconList[ItemFrameSelect(f.ItemType)].sprite = f.ItemIcon;
        }

        for (int i = 0; i < iconList.Count; i++)
        {
            if(iconList[i].sprite == null)
            {
                iconList[i].color = new Color(0f, 0f, 0f, 1f);
            }
        }

    }
    /// <summary>
    /// �A�C�e���摜���Z�b�g���邽�߂ɃA�C�e���X���b�g�̗v�f����Ԃ��Ƃ���
    /// </summary>
    /// <param name="itemType"></param>
    /// <returns></returns>
    private int ItemFrameSelect(ItemData.Itemtype itemType)
    {
        int num = 0;
        switch (itemType)
        {
            case ItemData.Itemtype.Sowrd:
                num = 0;
                break;
            case ItemData.Itemtype.Shield:
                num = 1;
                break;
            case ItemData.Itemtype.CrossBow:
                num = 2;
                break;
        }
        return num;
    }


    public void ItemManagerUpdate()
    {
        if (!ItemHaveCheck()) { return; }
        ToolInventoryController toolInventory = playerController.GetToolController();
        for(int i = 0; i < toolInventory.GetInventoryData().ToolItemList.Count; i++)
        {
            if (toolInventory.GetInventoryData().ToolItemList[i] == null){continue;}
            ToolController toolController = toolInventory.GetInventoryData().ToolItemList[i].GetComponent<ToolController>();
            if(toolController != null)
            {
                ItemData itemData = toolController.GetItemData();
                for(int j = 0; j < itemDataBase.GetItemList().Count; j++)
                {
                    if(itemDataBase.GetItemList()[j] == null){ return;}
                    if (itemDataBase.GetItemList()[j] == itemData)
                    {
                        if(itemNumber[itemDataBase.GetItemList()[j]] != 0) { break; }
                        itemNumber[itemDataBase.GetItemList()[j]] = 1;
                        return;
                    }
                }
            }
        }
    }
    /// <summary> 
    ///�v���C���[�̃C���x���g�������A�C�e���̃f�[�^��0���`�F�b�N
    ///0�Ȃ瑁�����^�[��
    /// </summary>
    /// <returns></returns>
    private bool ItemHaveCheck()
    {
        ToolInventoryController toolInventory = playerController.GetToolController();
        if (toolInventory == null) { return false; }
        int nullCount = 0;
        for(int i = 0; i < toolInventory.GetInventoryData().ToolItemList.Count; i++)
        {
            if (toolInventory.GetInventoryData().ToolItemList[i] == null)
            {
                nullCount++;
            }
        }
        if (nullCount == toolInventory.GetInventoryData().ToolItemList.Count)
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// �A�C�e���X���b�g�̃{�^�������������ɌĂяo���֐�
    /// </summary>
    public void SlotUpdate()
    {
        //�X���b�g�X�V����
        gameUIController.GetCanvasSoundController().PlaySESound((int)CanvasSoundController.CanvasSoundTag.Select);
        //�I������Ă���g�O��(�I������Ă���A�C�e���X���b�g)����

        int parse = 0;
        Toggle tal = toggleGroup.ActiveToggles().FirstOrDefault();
        GameObject iconObject = tal.gameObject.transform.GetChild(1).gameObject;
        Image iconImage = iconObject.GetComponent<Image>();
        if (iconImage.sprite != null)
        {
            for (int j = 0; j < getItemList.Count; j++)
            {
                if (getItemList[j].ItemIcon == iconImage.sprite)
                {
                    parse = j;
                    break;
                }
            }

            //���������X�g�̗v�f����y�ȏォ�ǂ������m�F����
            if (getItemList.Count >= parse)
            {
                if (getItemList.Count == 0) { return; }
                //�I������Ă���g�O���̓A�C�R���\������Ă���

                //���������X�g��y-1�Ԃ̖��O�ƌ���z,k�ɑ��
                string z = getItemList[parse].ItemName;

                itemTextList[0].text = z;

                string j = getItemList[parse].ItemExplanation;
                itemTextList[1].text = j;
            }
            else
            {
                for (int i = 0; i < itemTextList.Count; i++)
                {
                    itemTextList[i].text = null;
                }
            }
        }
        else
        {
            for (int i = 0; i < itemTextList.Count; i++)
            {
                itemTextList[i].text = null;
            }
        }
        
    }

}

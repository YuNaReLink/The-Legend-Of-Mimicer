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
    //アイテムアイコンのiconを指定
    [SerializeField]
    private List<GameObject> iconObjectList = new List<GameObject>();

    //トグルグループであるInventoryを指定
    [SerializeField]
    private ToggleGroup toggleGroup = null;

    private enum ExplanationNumber
    {
        ItemName,
        ItemExplanation
    }

    [SerializeField]
    private List<Text> itemTextList = new List<Text>();

    //アイテム数管理
    private Dictionary<ItemData,int> itemNumber = new Dictionary<ItemData,int>();

    //持ち物管理
    [SerializeField]
    private List<ItemData> getItemList = new List<ItemData>();

    //アイコン管理のリスト
    [SerializeField]
    private List<Image> iconList = new List<Image>();


    //アイテム枠をボタンで選択するクラス
    private MenuToggleController itemToggleController = null;
    public MenuToggleController GetItemToggleController() { return itemToggleController; }

    private GameUIController gameUIController = null;

    public void AwakeInitialize()
    {
        //親からPlayerControllerを取得
        PlayerConnectionUI playerConnectionUI = GetComponentInParent<PlayerConnectionUI>();
        if (playerConnectionUI != null)
        {
            PlayerController controller = playerConnectionUI.GetPlayerController();
            playerController = controller;
        }

        //アイテムフレームを取得
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
        //アイコン画像のアタッチ
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
        //初期化アイテムの処理
        for (int i = 0; i < itemDataBase.GetItemList().Count; i++)
        {
            //アイテム数を全て0に
            itemNumber.Add(itemDataBase.GetItemList()[i], 0);
        }

        GetItemUpdate();
    }

    public void GetItemUpdate()
    {
        //持ち物更新処理
        
        //持ち物リストのクリア
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

        //持ち物リストの要素数だけ繰り返す
        for(int i = 0;i < getItemList.Count; i++)
        {
            var f = getItemList[i];
            //fのアイコンをリストに代入
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
    /// アイテム画像をセットするためにアイテムスロットの要素数を返すところ
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
    ///プレイヤーのインベントリが持つアイテムのデータが0かチェック
    ///0なら早期リターン
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
    /// アイテムスロットのボタンを押した時に呼び出す関数
    /// </summary>
    public void SlotUpdate()
    {
        //スロット更新処理
        gameUIController.GetCanvasSoundController().PlaySESound((int)CanvasSoundController.CanvasSoundTag.Select);
        //選択されているトグル(選択されているアイテムスロット)を代入

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

            //持ち物リストの要素数がy以上かどうかを確認する
            if (getItemList.Count >= parse)
            {
                if (getItemList.Count == 0) { return; }
                //選択されているトグルはアイコン表示されている

                //持ち物リストのy-1番の名前と個数をz,kに代入
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

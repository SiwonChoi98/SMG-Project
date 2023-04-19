using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static bool inventoryActivated = false; // 인벤토리를 눌렀을 경우 다른 공격이나 이동 기능을 막는 것, 우리 게임에는 딱히..?, 왜냐하면 우리는 공격 버튼은 따로 있어서 괜찮을 것 같기도 하다.

    // 인벤토리 키를 눌렀을 경우 인벤토리 활성화, 필요한 컴포넌트
    [SerializeField]
    private GameObject go_InventoryBase;
    [SerializeField] 
    private GameObject go_DefaultSlot; // 이 부분은 스킬 슬롯이 될 것이다.
    [SerializeField]
    private Slot[] slots; // 하나를 스킬 슬롯으로 해서 스킬만 넣고, 다른 slot을 하나 만들어서 포션은 여기에만 넣어주자.


    // Start is called before the first frame update
    private void Start()
    {
        slots = go_DefaultSlot.GetComponentsInChildren<Slot>();
    }

    // Update is called once per frame
    private void Update()
    {
        TryOpenInventory();
    }


    private void TryOpenInventory()
    {
        if(Input.GetKeyDown(KeyCode.I)) 
        { 
            inventoryActivated = !inventoryActivated;

            if(inventoryActivated) 
            {
                OpenInventory();
            }
            else
            {
                CloseInventory();
            }
        }
    }

    private void OpenInventory()
    {
        go_InventoryBase.SetActive(true);
    }

    private void CloseInventory()
    {
        go_InventoryBase.SetActive(false);
    }

    public void AcquireItem(Item _item, int _count = 1)
    {
        if(_item.itemType != Item.EItemType.Buff) // 버프류가 아닌 경우에만 인벤토리에 넣어준다. 또한 인벤토리가 다 차있어도 안된다.
        {
            for (int i = 0; i < slots.Length; i++) // 슬롯에 이미 아이템이 있는 경우
            {
                if (slots[i].item != null)
                {
                    if (slots[i].item.itemName == _item.itemName)
                    {
                        slots[i].SetSlotCount(_count);
                        return;
                    }
                }
               
            }

            for (int i = 0; i < slots.Length; i++) // 슬롯에 아이템이 없으면, 빈 공간에 넣어준다.
            {
                if (slots[i].item == null)
                {
                    slots[i].AddItem(_item, _count);
                    return;
                }
            }
        }
       
    }
}

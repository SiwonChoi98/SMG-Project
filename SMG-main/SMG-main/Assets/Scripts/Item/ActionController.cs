using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionController : MonoBehaviour
{
    [SerializeField]
    private float range; // 습득 가능한 최대 거리.

    private bool pickupActivated = false; // 습득 가능할 시 true

    private RaycastHit hitInfo; // 충돌체의 정보 저장.

    // 아이템 레이어에만 반응하도록 레이어 마스크를 설정
    [SerializeField]
    private LayerMask layerMask;

    // 필요한 컴포넌트
    [SerializeField]
    private Text actionText;
    [SerializeField]
    private Inventory theInventory;

    // Update is called once per frame
    private void Update()
    {
        TryAction();
    }

    private void TryAction()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            CheckItem();
            CanPickUp();
        }

        
    }

    private void CheckItem() // 현재 방식은 플레이어에서 발사하는 Raycast가 거리 내에 닿으면 아이템 정보 뜸 , 이것을 그냥 플레이어 쪽으로 옮겨되 돌 것 같다.
    {
        if (Physics.Raycast(transform.position + Vector3.up * 0.1f, transform.forward, 
            out hitInfo, range, layerMask)) // transform.TransformDirection(Vector3.forward)는 transfrom.forward와 같은 역할이다.
        {
            if (hitInfo.transform.tag == "Item")
            {
      
                ItemInfoAppear();
            }
        }

        else
        {
            InfoDisappear();
        }

    }

    private void CanPickUp()
    {
        if (pickupActivated)
        {
            if (hitInfo.transform != null)
            {
                Debug.Log(hitInfo.transform.GetComponent<ItemPickUp>().item.itemName + " 획득했습니다.");
                theInventory.AcquireItem(hitInfo.transform.GetComponent<ItemPickUp>().item);
                Destroy(hitInfo.transform.gameObject); // 정보 제거
                InfoDisappear();
            }
        }
     
    }

    // 아이템 정보 생성
    private void ItemInfoAppear() 
    {
        pickupActivated = true;
        actionText.gameObject.SetActive(true);
        actionText.text = hitInfo.transform.GetComponent<ItemPickUp>().item.itemName + " 획득 " + "<color=yellow>" + "(E)" + "</color>"; // 이렇게 하면 마지막 E키만 노란 글자로 나온다.


    }

    // 아이템 정보 제거
    private void InfoDisappear()
    {
        pickupActivated = false;
        actionText.gameObject.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position + Vector3.up * 0.1f, transform.forward * range);
    }
}

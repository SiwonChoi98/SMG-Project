using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("플레이어 체력관련 이미지")]
    public Player player;
    [SerializeField] private Transform playerHealthImageTrans;
    [SerializeField] private Image playerHealthImage;
    [SerializeField] private Image playerCanvasHealthImage;
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    private void LateUpdate()
    {
        GUI();
    }
    private void GUI()
    {
        playerHealthImageTrans.position = player.transform.position + new Vector3(0, 2f, 0); //플레이어 체력 위치 플레이어 머리위에
        playerHealthImage.fillAmount = Mathf.Lerp(playerHealthImage.fillAmount, (float)player.CurHealth / player.MaxHealth / 1 / 1, Time.deltaTime * 5); //플레이어 체력
        playerCanvasHealthImage.fillAmount = Mathf.Lerp(playerHealthImage.fillAmount, (float)player.CurHealth / player.MaxHealth / 1 / 1, Time.deltaTime * 5); //플레이어 캔버스 체력
    }
}

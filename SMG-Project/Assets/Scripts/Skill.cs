using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    // 여기 Variables의 public도 진짜 필요한 것만 public으로 처리하자.
    #region Variables

    public Rigidbody rigid_Player;

    public ParticleSystem[] particleSystems;

    public int damage;

    public float attackForce; // 해당 스킬을 사용할 때 플레이어에게 가해지는 힘 ex) 일반 공격할 때 살짝씩 앞으로 나가는 것

    [SerializeField]
    private float coolTime;

    [SerializeField]
    private float calcCoolTime = 0.0f;

    //[HideInInspector]

    public LayerMask targetMask;

    [SerializeField]
    public bool IsAvailable => calcCoolTime >= coolTime;

    #endregion Variables


    protected virtual void Awake() 
    {
        rigid_Player = GetComponentInParent<Rigidbody>(); // 부모로부터 Rigidbody를 받아온다. 현재 Rigidbody는 Player 하나만 존재한다.
        particleSystems = GetComponentsInChildren<ParticleSystem>(); // 자식들로부터 ParticleSystem 들을 받아온다.
    }


    protected virtual void Start()
    {
        calcCoolTime = coolTime;
    }

    protected void Update()
    {
        if (calcCoolTime < coolTime) 
        {
            calcCoolTime += Time.deltaTime;
        }
    }


    // 공격할 때 때린 상대방과, 어디서 스킬을 발생시킬 것인지를 가져온다.
    // 플레이어의 검과 가깝고 붙어있는 스킬들은 자체적으로 조정하기 떄문에 필요없지만, 소한하는 형태의 스킬은 생성 위치를 전달해줘야 한다. 
    public abstract void ExcuteAttack(GameObject target = null, Transform startPoint = null); // 애니메이션 이벤트 함수 Attack()에 호출
    public abstract void ExcuteParticleSystem(); // 애니메이션을 실행할 때 같이 호출
}

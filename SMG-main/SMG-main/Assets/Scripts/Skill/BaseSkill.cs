using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 스킬이 플레이어에 붙어있어야 하는지, 아니면 소환되어야 하는지
public enum ESkillParticleType : int
{
    Attach,
    Spawn
}

// 나중에 플레이어가 스킬을 슬롯에 넣으면 그때 그게 어떤 스킬인지 확인. 스킬은 그리고 한종류로 통일, 단 스킬이 강화되면, 그것을 체크하는 bool값을 둬서 강화하는 방식으로.
// 예를 들어 1번에 찌르기를 넣었는데, 스킬이 강회되어서 더 큰 찌르기가 나가야 한다.
// 그러면 그 강화요소를 bool값으로 넣어서 true인 경우엔 다른 크기의 Partilce과 ManualCollision을 틀어주는거로 하자.
public enum ESkillType : int 
{
    Thrust,
    GiantSword,
    GroundBreak,
    UpperSlash,
    Shield,
    Baldo,
    NormalAttack
}

public abstract class BaseSkill : MonoBehaviour 
{
    // 여기 Variables의 public도 진짜 필요한 것만 public으로 처리하자.
    #region Variables

    public Rigidbody rigid_Player;

    public ParticleSystem[] particleSystems;

    public int damage;

    public float attackForce; // 해당 공격 행동을 할 때 나아가는 방향에 곱해주는 정도.


    public ESkillType mSkillType;

    public ESkillParticleType mParticleType;

    [SerializeField]
    private float coolTime;

    [SerializeField]
    private float calcCoolTime = 0.0f;

    //[HideInInspector]

    public LayerMask targetMask;

    [SerializeField]
    public bool IsAvailable => calcCoolTime >= coolTime;

    public GameObject effectPrefab; // 타격했을 때 생기는 프리팹

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
    public abstract void ExcuteAttack(GameObject target = null, Transform startPoint = null); // 공격 실행, 애니메이션 이벤트 함수 Attack()에 호출
    public abstract void ExcuteParticleSystem(); // 파티클 발생, 애니메이션을 실행할 때 같이 호출
    public abstract void ExitParticleSystem(); // 파티클 제거, 애니메이션을 실행할 때 같이 호출
}

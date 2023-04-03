using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Skill : MonoBehaviour
{
    // ���� Variables�� public�� ��¥ �ʿ��� �͸� public���� ó������.
    #region Variables

    public Rigidbody rigid_Player;

    public ParticleSystem[] particleSystems;

    public int damage;

    public float attackForce; // �ش� ��ų�� ����� �� �÷��̾�� �������� �� ex) �Ϲ� ������ �� ��¦�� ������ ������ ��

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
        rigid_Player = GetComponentInParent<Rigidbody>(); // �θ�κ��� Rigidbody�� �޾ƿ´�. ���� Rigidbody�� Player �ϳ��� �����Ѵ�.
        particleSystems = GetComponentsInChildren<ParticleSystem>(); // �ڽĵ�κ��� ParticleSystem ���� �޾ƿ´�.
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


    // ������ �� ���� �����, ��� ��ų�� �߻���ų �������� �����´�.
    // �÷��̾��� �˰� ������ �پ��ִ� ��ų���� ��ü������ �����ϱ� ������ �ʿ������, �����ϴ� ������ ��ų�� ���� ��ġ�� ��������� �Ѵ�. 
    public abstract void ExcuteAttack(GameObject target = null, Transform startPoint = null); // �ִϸ��̼� �̺�Ʈ �Լ� Attack()�� ȣ��
    public abstract void ExcuteParticleSystem(); // �ִϸ��̼��� ������ �� ���� ȣ��
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//2024-05-22: CUSTOM UNITY TEMPLATE 

public class Monster : MonoBehaviour, IOnDamage 
{
    //���� ���� Enum 
    public enum MonsterStatus
    {
        Walking, //�ȱ�  
        Attack,   //���� 
        GetAttacked, //�ǰ�
        Death //��� 
    }

    [Header("���� �ɷ�ġ(ü��, ���ݷ�, �ӵ�)")]
    [SerializeField] private int monsterHealth = 300; 
    [SerializeField] private int monsterDamage = 10; 
    [SerializeField] private float monsterSpeed = 10f;
    [SerializeField] private MonsterStat monsterData = null;

    private Animator animator = null;
    private int status = 0;
    private int previous_status = 0;
    private bool Attackon = false;

    #region["������Ʈ�� Ȱ��ȭ�ɶ����� ����Ǵ� �޼ҵ�"] 
    private void OnEnable()
    {
        animator = GetComponent<Animator>();
        status = (int)MonsterStatus.Walking; 
        //Invoke("DestroySelf", 60f);
        SetMonsterStat(); 
    }
    #endregion

    #region["���� ���� �����ϱ�"] 
    private void SetMonsterStat()
    {
        monsterHealth = monsterData.monsterHealth;
        monsterDamage = monsterData.monsterDamage;
       // Debug.Log("Current Monster Health: " + monsterHealth);
      //  Debug.Log("Current Monster Damage: " + monsterDamage); 
    }
    #endregion

    #region["60�ʰ� ���� �ڿ� �ڵ����� ��Ȱ��ȭ"] 
    private void DestroySelf()
    {
        SpawnManager.instance.FadeMonster(this); 
    }
    #endregion

    #region["�ǰ� �޼ҵ�"] 
    public void OnDamage(int playerDamage)
    {
        previous_status = status;
        animator.SetTrigger("Hitted");
        status = (int)MonsterStatus.GetAttacked; 
        //������ ü�� - �÷��̾��� ���ݷ� 
        monsterHealth -= playerDamage; 
        if (monsterHealth <= 0) 
        {
            StartCoroutine(MonsterDeathCoroutine()); 
            //���� ���, ��� ��� 
        }
        else if(Attackon == false)
        {
            animator.SetTrigger("Walk");
            status = (int)MonsterStatus.Walking;
        }
        else
        {
            animator.SetTrigger("Attack");
            status = (int)MonsterStatus.Attack; 
        }
    }
    #endregion

    private IEnumerator MonsterDeathCoroutine()
    {
        animator.SetTrigger("Death");
        status = (int)MonsterStatus.Death;
        yield return new WaitForSeconds(1f); 
        SpawnManager.instance.FadeMonster(this);
        yield break; 
    }

    #region["��ǥ�������� �̵�"] 
    public void Move(Transform vrplayer_transform)
    { 
        transform.LookAt(vrplayer_transform.position); 
        if(status == (int)MonsterStatus.Walking)
        {
            transform.position = Vector3.MoveTowards(transform.position, vrplayer_transform.position, monsterSpeed * Time.deltaTime);
        }
    }
    #endregion

    #region["�ɷ�ġ ��ȭ"]
    public void StrengthMonster()
    {
        //1��(1��������)���� ���� ü���� 100, ���� �������� 5�� �����Ѵ�. 
        monsterData.monsterHealth += 100;
        monsterData.monsterDamage += 5; 
    }
    #endregion

    private void OnTriggerEnter(Collider _collider)
    {
        Debug.Log("collider.name: " + _collider.name); 
        if(_collider.name.Equals("Barrier"))  
        {
            SpawnManager.instance.FadeMonster(this); //���� ��Ȱ��ȭ 
            //_collider.GetComponent<IOnDamage>().OnDamage(monsterDamage); //�� �ʿ� �������� ����
        } 
        if(_collider.name.Equals("AttackSphere"))
        {
            status = (int)MonsterStatus.Attack;
            animator.SetTrigger("Attack");
            //���� �ִϸ��̼� ���
            Attackon = true;
        }
        //IOnDamage �������̽��� ��ӹ޴� ������Ʈ���Դ� �������� ���� �� �ִ�. 
        if(_collider.GetComponent<IOnDamage>() != null)
        {
            _collider.GetComponent<IOnDamage>().OnDamage(monsterDamage); 
        }
    }
}

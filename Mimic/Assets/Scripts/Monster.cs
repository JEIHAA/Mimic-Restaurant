using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//2024-05-22: CUSTOM UNITY TEMPLATE 

public class Monster : MonoBehaviour, IOnDamage 
{
    [Header("���� �ɷ�ġ(ü��, ���ݷ�, �ӵ�)")]
    [SerializeField] private int monsterHealth = 300; 
    [SerializeField] private int monsterDamage = 10; 
    [SerializeField] private float monsterSpeed = 10f;
    [SerializeField] private MonsterStat monsterData = null;

    #region["������Ʈ�� Ȱ��ȭ�ɶ����� ����Ǵ� �޼ҵ�"] 
    private void OnEnable()
    {
        Invoke("DestroySelf", 60f);
        SetMonsterStat(); 
    }
    #endregion

    #region["���� ���� �����ϱ�"] 
    private void SetMonsterStat()
    {
        monsterHealth = monsterData.monsterHealth;
        monsterDamage = monsterData.monsterDamage;
        Debug.Log("Current Monster Health: " + monsterHealth);
        Debug.Log("Current Monster Damage: " + monsterDamage); 
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
        //������ ü�� - �÷��̾��� ���ݷ� 
        monsterHealth -= playerDamage; 
        if (monsterHealth <= 0) 
        {
            SpawnManager.instance.FadeMonster(this); 
            //���� ���, ��� ��� 
        }
    }
    #endregion

    #region["��ǥ�������� �̵�"] 
    public void Move()
    {
        //�����δ� ���� ������ ������ ���������� �ȵ�. 
        transform.Translate(Vector3.forward * monsterSpeed * Time.deltaTime); 
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
    }
}

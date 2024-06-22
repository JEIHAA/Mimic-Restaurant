using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//2024-05-22: CUSTOM UNITY TEMPLATE 

public class Monster : MonoBehaviour, IOnDamage 
{
    //몬스터 상태 Enum 
    public enum MonsterStatus
    {
        Walking, //걷기  
        Attack,   //공격 
        GetAttacked, //피격
        Death //사망 
    }

    [Header("몬스터 능력치(체력, 공격력, 속도)")]
    [SerializeField] private int monsterHealth = 300; 
    [SerializeField] private int monsterDamage = 10; 
    [SerializeField] private float monsterSpeed = 10f;
    [SerializeField] private MonsterStat monsterData = null;

    private Animator animator = null;
    private int status = 0;

    #region["오브젝트가 활성화될때마다 실행되는 메소드"] 
    private void OnEnable()
    {
        animator = GetComponent<Animator>();     
        //Invoke("DestroySelf", 60f);
        SetMonsterStat(); 
    }
    #endregion

    #region["몬스터 스탯 설정하기"] 
    private void SetMonsterStat()
    {
        monsterHealth = monsterData.monsterHealth;
        monsterDamage = monsterData.monsterDamage;
        Debug.Log("Current Monster Health: " + monsterHealth);
        Debug.Log("Current Monster Damage: " + monsterDamage); 
    }
    #endregion

    #region["60초가 지난 뒤에 자동으로 비활성화"] 
    private void DestroySelf()
    {
        SpawnManager.instance.FadeMonster(this); 
    }
    #endregion

    #region["피격 메소드"] 
    public void OnDamage(int playerDamage)
    {
        animator.SetTrigger("Hitted");
        status = (int)MonsterStatus.GetAttacked; 
        //몬스터의 체력 - 플레이어의 공격력 
        monsterHealth -= playerDamage; 
        if (monsterHealth <= 0) 
        {
            animator.SetTrigger("Death");
            status = (int)MonsterStatus.Death; 
            SpawnManager.instance.FadeMonster(this); 
            //몬스터 사망, 고기 드랍 
        }
        animator.SetTrigger("Walk");
        status = (int)MonsterStatus.Walking; 
    }
    #endregion

    #region["목표지점으로 이동"] 
    public void Move(Transform vrplayer_transform)
    { 
        if(status == (int)MonsterStatus.Walking)
        {
            transform.LookAt(vrplayer_transform.position); 
            transform.position = Vector3.MoveTowards(transform.position, vrplayer_transform.position, monsterSpeed * Time.deltaTime);
        }
    }
    #endregion

    #region["능력치 강화"]
    public void StrengthMonster()
    {
        //1일(1스테이지)마다 몬스터 체력은 100, 몬스터 데미지는 5씩 증가한다. 
        monsterData.monsterHealth += 100;
        monsterData.monsterDamage += 5; 
    }
    #endregion

    private void OnTriggerEnter(Collider _collider)
    {
        Debug.Log("collider.name: " + _collider.name); 
        if(_collider.name.Equals("Barrier"))  
        {
            SpawnManager.instance.FadeMonster(this); //몬스터 비활성화 
            //_collider.GetComponent<IOnDamage>().OnDamage(monsterDamage); //방어막 쪽에 데미지를 입힘
        }
        if(_collider.name.Equals("VRPlayer"))
        {
            status = (int)MonsterStatus.Attack;
            animator.SetTrigger("Attack");
            //공격 애니메이션 재생
        }
        //IOnDamage 인터페이스를 상속받는 오브젝트에게는 데미지를 입힐 수 있다. 
        if(_collider.GetComponent<IOnDamage>() != null)
        {
            _collider.GetComponent<IOnDamage>().OnDamage(monsterDamage); 
        }
    }
}

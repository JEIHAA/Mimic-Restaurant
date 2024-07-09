using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

//2024-05-22: CUSTOM UNITY TEMPLATE 

public class Monster : MonoBehaviourPun, IOnDamage
{
    public delegate void OnDeathDelegate(GameObject _meat, int _meat_num);
    private OnDeathDelegate ondeathcallback = null;
    public OnDeathDelegate OnDeathCallBack
    {
        set { ondeathcallback = value; }
    }

    //몬스터 상태 Enum 
    public enum MonsterStatus
    {
        Walking, //걷기  
        Attack,   //공격 
        GetAttacked, //피격
        Death //사망 
    }

    [Header("몬스터 능력치(체력, 공격력, 속도)")]
    private int monsterHealth = 300; 
    private int monsterDamage = 10;
    private int meatnum = 3;
    private GameObject spawnpoint = null;

    [SerializeField] private float monsterSpeed = 10f;
    [SerializeField] private MonsterStat monsterData = null;
    [Header("고기 오브젝트")]
    [SerializeField] private GameObject steak = null;

    private Animator animator = null;
    private int status = 0;
    private int previous_status = 0;
    private bool Attackon = false;
    private bool isEscape = false;
    private bool isRotate = false; 

    #region["시작포인트 설정"]
    public void SetSpawnPoint(GameObject _spawnpoint)
    {
        spawnpoint = _spawnpoint;
        Debug.Log("spawnpoint: " + spawnpoint); 
    }
    #endregion

    #region["오브젝트가 활성화될때마다 실행되는 메소드"] 
    private void OnEnable()
    { 
        animator = GetComponent<Animator>();
        status = (int)MonsterStatus.Walking;
        Attackon = false;
        isRotate = false;
        isEscape = false; 
        //Invoke("DestroySelf", 60f);
        SetMonsterStat(); 
    }
    #endregion

    #region["몬스터 스탯 설정하기"] 
    private void SetMonsterStat()
    {
        monsterHealth = monsterData.monsterHealth;
        monsterDamage = monsterData.monsterDamage;
        meatnum = monsterData.meatnum; 
    }
    #endregion


    #region["피격 메소드"] 
    public void OnDamage(int playerDamage, GameObject _object)
    {
        previous_status = status;
        animator.SetTrigger("Hitted");
        status = (int)MonsterStatus.GetAttacked; 
        //몬스터의 체력 - 플레이어의 공격력 
        monsterHealth -= playerDamage; 
        if (monsterHealth <= 0) 
        {
            StartCoroutine(MonsterDeathCoroutine()); 
            //몬스터 사망, 고기 드랍 
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
        steak = PhotonNetwork.Instantiate("Prefabs\\Food\\Ingredient\\Steak", new Vector3(-1.2f, 1.5f, -1.5f), Quaternion.identity); 
        ondeathcallback?.Invoke(steak, meatnum); 
        SpawnManager.instance.FadeMonster(this);
        yield break; 
    }

    #region["목표지점으로 이동"] 
    public void Move(Transform vrplayer_transform)
    { 
        transform.LookAt(vrplayer_transform.position); 
        if(status == (int)MonsterStatus.Walking && !isEscape)
        {
            //걷는 상태인데 도망가지는 않을때. 
            transform.position = Vector3.MoveTowards(transform.position, vrplayer_transform.position, monsterSpeed * Time.deltaTime);
        }
    }
    #endregion

    public void MonsterEscape() 
    {
        Debug.Log("Here?"); 
        spawnpoint.GetComponent<Collider>().enabled = true;
        isRotate = true; 
        isEscape = true;
        Invoke("OnFade", 10f); 
        StartCoroutine(EscapeMonsterCoroutine(spawnpoint.transform));       
    }

    #region["도망갈때는 따로 코루틴을 돌린다. => 프레임 드랍 없기를 바래야지..."] 
    private IEnumerator EscapeMonsterCoroutine(Transform _startpoint)
    {
        while (status == (int)MonsterStatus.Walking && isEscape)
        {
            transform.LookAt(_startpoint.position);
            transform.position = Vector3.MoveTowards(transform.position, _startpoint.position, monsterSpeed * Time.deltaTime);
            yield return new WaitForEndOfFrame(); 
        }
        yield break; 
    }
    #endregion

    private void OnTriggerEnter(Collider _collider)
    {
        if(_collider.name.Equals("Barrier"))  
        {
            SpawnManager.instance.FadeMonster(this); //몬스터 비활성화 
        } 
        if(_collider.name.Equals("AttackSphere"))
        {
            if(MeatManager.instance.GetMeatNum() <= 0)
            {
                status = (int)MonsterStatus.Attack;
                animator.SetTrigger("Attack");
                //공격 애니메이션 재생
                Attackon = true;
            }
            else
            {
                if(!isEscape)
                {
                    isEscape = true;
                    MeatManager.instance.LoseMeatByMonster(gameObject);
                    MonsterEscape();
                }
            }
        }
        if(_collider.name.Contains("SpawnPoint"))
        {
            Debug.Log("Why Not Here?");
            isRotate = false;
            isEscape = false;
            spawnpoint.GetComponent<Collider>().enabled = false;
            SpawnManager.instance.FadeMonster(this);
            Destroy(GetComponentInChildren<Steak>().gameObject); 
        }
        //IOnDamage 인터페이스를 상속받는 오브젝트에게는 데미지를 입힐 수 있다. 
        if(_collider.GetComponent<IOnDamage>() != null)
        {
            _collider.GetComponent<IOnDamage>().OnDamage(monsterDamage, gameObject); 
        }
    }


    private void OnFade()
    {
        if (isEscape && GetComponentInChildren<Steak>().gameObject != null)
        {
            spawnpoint.GetComponent<Collider>().enabled = false;
            SpawnManager.instance.FadeMonster(this);
            Destroy(GetComponentInChildren<Steak>().gameObject);
        }
    }
}

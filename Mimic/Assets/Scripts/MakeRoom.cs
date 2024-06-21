using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/* 2024-06-21 ���α�
 
 */

public class MakeRoom : MonoBehaviour
{
    [SerializeField] private string GameVersion = "1.0";
    [SerializeField] private string nickname = string.Empty;
    [SerializeField] private string nextScene = string.Empty;

    /*ȣ�� ����: ���� ������Ʈ�� Ȱ��ȭ�Ǿ��� ��, �� ��ũ��Ʈ�� ��ü�� ó�� �ε�� �� ȣ��˴ϴ�.
    �� �뵵: ��ũ��Ʈ �ʱ�ȭ, ���� ������Ʈ�� ������Ʈ�� ���۷��� ���� ��.
    Ư¡: ��� Awake �޼���� ���� �ε�� �� �ٷ� ȣ��˴ϴ�.
     
     
     */

    // ��ü�� �ε�� �� ���� ���� �����
    private void Awake()
    {
        
    }

    /* Start �޼���
     * ȣ�� ����: ù �������� ���۵Ǳ� ������ ȣ��˴ϴ�. ��, ��� Awake �޼��尡 ȣ��� �Ŀ� ȣ��˴ϴ�.
     * �� �뵵: ���� ���� �� �����ؾ� �� �͵�, ���� ������Ʈ�� Ȱ��ȭ�� �Ŀ� �ʱ�ȭ�� �ʿ��� �͵�.
     * ���� ���� �� �����ؾ� �� �͵�, ���� ������Ʈ�� Ȱ��ȭ�� �Ŀ� �ʱ�ȭ�� �ʿ��� �͵�.
    */

    // ù ������ ���� �����
    private void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    void Update()
    {
        
    }
}

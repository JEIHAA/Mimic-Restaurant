using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

//2024-05-22: CUSTOM UNITY TEMPLATE 

public class WelcomeController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI welcometext = null;

    public static WelcomeController instance = null; //Singleton 

    private string id = string.Empty;

    private void Awake()
    {
        instance = this; 
    }

    #region["���̵� ����"] 
    public void SetText(string _id)
    {
        welcometext.text = _id + "�� ȯ���մϴ�.";
        id = _id; 
    }
    #endregion

    #region["���� ���� ��ư"] 
    public void StartButton()
    {
        //�� ����� Scene�� �̵� 
        SceneManager.LoadScene("Scene_RoomName");
        //���̵� �̱������� ������ �ϱ� ������ ���⼭ ���� ������Ʈ�� �ı����� ����. 
        DontDestroyOnLoad(gameObject);
    }
    #endregion

    #region["�̱��� ��ü���� ���̵� �ҷ����� ���� ���"] 
    public string getId()
    {
        return id; 
    }
    #endregion

}

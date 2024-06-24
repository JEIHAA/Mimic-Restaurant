using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBoard : MonoBehaviour
{
    [SerializeField] private List<GameObject> keyboards = new List<GameObject>();

    private void Start()
    {
        // �ʱ⿡ ��� Ű���带 ��Ȱ��ȭ
        foreach (GameObject keyboard in keyboards)
        {
            keyboard.SetActive(false);
        }
    }

    public void OnTextClicked(string textboxName)
    {
        // ��� Ű���带 �����
        foreach (GameObject keyboard in keyboards)
        {
            keyboard.SetActive(false);
        }

        // �ؽ�Ʈ�ڽ� �̸��� ���� ������ Ű���带 ã�� Ȱ��ȭ
        foreach (GameObject keyboard in keyboards)
        {
            if (keyboard.name == textboxName + "Keyboard")
            {
                keyboard.SetActive(true);
                break; // ���ϴ� Ű���带 ã������ ���� ����
            }
        }
    }
}

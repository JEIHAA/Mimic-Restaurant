using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyBoard : MonoBehaviour
{
    [SerializeField] private List<GameObject> keyboards = new List<GameObject>();

    private void Start()
    {
        // 초기에 모든 키보드를 비활성화
        foreach (GameObject keyboard in keyboards)
        {
            keyboard.SetActive(false);
        }
    }

    public void OnTextClicked(string textboxName)
    {
        // 모든 키보드를 숨기기
        foreach (GameObject keyboard in keyboards)
        {
            keyboard.SetActive(false);
        }

        // 텍스트박스 이름에 따라 적절한 키보드를 찾아 활성화
        foreach (GameObject keyboard in keyboards)
        {
            if (keyboard.name == textboxName + "Keyboard")
            {
                keyboard.SetActive(true);
                break; // 원하는 키보드를 찾았으면 루프 종료
            }
        }
    }
}

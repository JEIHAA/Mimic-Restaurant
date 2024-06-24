using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class KeyBoard : MonoBehaviour
{
    [SerializeField] private List<GameObject> keyboards = new List<GameObject>();

    private void Start()
    {
        foreach (GameObject keyboard in keyboards)
        {
            keyboard.SetActive(false);
        }
    }

    public void OnTextClicked(string textName)
    {
        foreach (GameObject keyboard in keyboards)
        {
            keyboard.SetActive(false);
        }

        GameObject keyboardToShow = keyboards.Find(k => k.name == textName);
        if (keyboardToShow != null)
        {
            keyboardToShow.SetActive(true);
        }
    }
}

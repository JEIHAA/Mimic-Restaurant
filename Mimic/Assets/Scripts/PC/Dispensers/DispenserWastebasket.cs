using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DispenserWastebasket : MonoBehaviour, IDispenser
{
    public IEnumerator GenerateFood(GameObject _food)
    {
        yield break;
    }

    public bool GetIsGenerate()
    {
        return false;
    }

    public void OperateDispenser(GameObject _player)
    {
        Debug.Log("버리기");
        //탄 고기만 버릴 수 있게 함. 
        if (_player.GetComponentInChildren<BindFood>().Food.GetComponent<Ingredients>()?.State == CookState.Burn)
        {
            Destroy(_player.GetComponentInChildren<BindFood>().Food);
        }
    }

}

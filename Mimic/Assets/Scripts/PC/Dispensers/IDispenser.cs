using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static BurgerInfo;

public interface IDispenser
{
    public void OperateDispenser(GameObject _go);
    public IEnumerator GenerateFood(GameObject _food);
    public bool GetIsGenerate();
}

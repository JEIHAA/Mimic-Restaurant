using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FoodStat", menuName = "ScriptableObjects/FoodStat", order = 2)]
public class FoodStat : ScriptableObject 
{
    public int money;
    public int hungerrestore; 
}

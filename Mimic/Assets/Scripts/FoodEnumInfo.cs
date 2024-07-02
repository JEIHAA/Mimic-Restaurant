using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//2024-05-22: CUSTOM UNITY TEMPLATE 

public class FoodEnumInfo 
{
    public enum Food
    {
        Hamburger,       //�ܹ��� 
        FrenchFries,     //����Ƣ�� 
        Drink            //����� 
    }

    //�ܹ���
    public enum Hamburger
    {
        Hamburger,        //�ܹ��� 
        Cheeseburger,     //ġ���ܹ��� 
        Shrimpburger,     //�����ܹ��� 
        Octopusburger     //�����ܹ��� 
    }

    //����Ƣ��
    public enum FrenchFries
    {
        FrenchFries,     //����Ƣ��
        CheeseFries,     //ġ�� ����Ƣ�� 
        VolcanoFries     //�����̳� ����Ƣ�� 
    }

    //�����
    public enum Drink
    {
        ColaDrink,        //�ݶ�
        CiderDrink,       //���̴�(����� �ƴ�) 
        EnergyDrink,      //������ ���� 
        WaterMelonDrink   //���� ���� 
    }

}

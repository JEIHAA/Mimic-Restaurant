using System.Collections;
using UnityEngine;

public class DispenserHamburger : MonoBehaviour, IDispenser
{
    [SerializeField] private Transform foodGenerator;
    [SerializeField] private GameObject output;
    [SerializeField] private bool isGenerate = false;

    public bool GetIsGenerate()
    {
        return isGenerate;
    }

    public void OperateDispenser(GameObject _player)
    {
        Debug.Log(this.name+"사용");        
        GameObject food = _player.GetComponentInChildren<BindFood>().Food;
        
        if (food == null && output != null)
        {
            Debug.Log("음식 가져감");
            _player.GetComponentInChildren<BindFood>().Food = output;
            output = null;
            return;
        }
        else if (food == null)
        {
            Debug.Log("재료가 필요합니다!");
            return;
        }
        else if (isGenerate)
        {
            Debug.Log("이미 사용중입니다!");
            return;
        }
        else if (food.GetComponent<Ingredients>() == null || !food.GetComponent<Ingredients>().IsCooked)
        {
            Debug.Log("구워진 재료가 필요합니다!");
            return;
        }
        else if (output == null)
        {
            Debug.Log("조리 시작");
            isGenerate = true;
            _player.GetComponentInChildren<BindFood>().Food = null;
            StartCoroutine(GenerateFood(food));
        }
        else { Debug.Log("뭐가 문제임?");  }
    }

    public IEnumerator GenerateFood(GameObject _food)
    {   
        if (_food.GetComponent<Ingredients>().IsCooked)
        {
            _food.GetComponent<Ingredients>().IsCooking = true;
            _food.transform.transform.parent = null;
            _food.transform.transform.position = foodGenerator.position;
            Debug.Log("음식 내려놓음");

            yield return new WaitForSeconds(3f);

            _food.GetComponent<Ingredients>().IsCooking = false;
            Destroy(_food);
            output = Instantiate(_food.GetComponent<Ingredients>().NextLevel, foodGenerator.position, Quaternion.identity);
        }
        isGenerate = false;
    }
}

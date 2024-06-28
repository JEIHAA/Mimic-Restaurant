using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conveyor : MonoBehaviour
{ 
    [SerializeField] private float speed = 1f;
    [SerializeField] private Vector3 direction = Vector3.forward;
    [SerializeField] private int capacity = 0;
    [SerializeField] private int maxCapacity = 10;

    [SerializeField] private Material material;
    [SerializeField] private List<Collision> collisions = new List<Collision>();
    [SerializeField] private List<Rigidbody> rbs = new List<Rigidbody>();
    [SerializeField] private LayerMask target;

    private void Start()
    {
        //material = GetComponent<Material>();
    }

    private void OnCollisionEnter(Collision _collision)
    {
        ++capacity;
        Debug.Log(capacity);
        if (_collision.gameObject.layer == LayerMask.NameToLayer("Food"))
        {
            rbs.Add(_collision.gameObject.GetComponent<Rigidbody>());
        }
        else
        { 
            // Æ¨°Ü³»±â
        }
    }
    private void OnCollisionStay(Collision _collision)
    {
        if (ActiveConveyor(capacity)) 
        {
            MoveObject(rbs);
        }
    }

    private void OnCollisionExit(Collision _collision)
    {
        --capacity;
        rbs.Remove(_collision.gameObject.GetComponent<Rigidbody>());
        if (ActiveConveyor(capacity))
        {
            MoveObject(rbs);
        }

            
    }

    private void MoveObject(List<Rigidbody> _rbs) 
    {
        if (rbs.Count > 0)
        {
            Vector3 movement = direction.normalized * speed * Time.deltaTime;//direction.normalized * speed * Time.deltaTime;
            foreach (Rigidbody rb in rbs) 
            {
                rb.MovePosition(rb.position + movement);
            }
        }
    }

    private bool ActiveConveyor(int _capacity) 
    {
        //Debug.Log(_capacity);
        if (_capacity <= maxCapacity)
        {
            material.SetFloat("_ScrollSpeed", 0.5f);
            return true;
        }
        else
        { 
            material.SetFloat("_ScrollSpeed", 0f);
            return false;
        }
    }

}

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
    [SerializeField] private LayerMask target;

    private void Start()
    {
        //material = GetComponent<Material>();
    }

    private void OnCollisionEnter(Collision _collision)
    {
        ++capacity;
        Debug.Log(capacity);
        if (_collision.gameObject.layer == target)
        {
            collisions.Add(_collision);
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
            MoveObject(collisions);
        }
    }

    private void OnCollisionExit(Collision _collision)
    {
        --capacity;
        collisions.Remove(_collision);
        ActiveConveyor(capacity);
    }

    private void MoveObject(List<Collision> _collisions) 
    {
        List<Rigidbody> rbs = new List<Rigidbody>();
        foreach (Collision _collision in _collisions) 
        {
            rbs.Add(_collision.gameObject.GetComponent<Rigidbody>());
        }
        
        if (rbs.Count > 0)
        {
            Vector3 movement = direction.normalized * speed * Time.deltaTime;
            foreach (Rigidbody rb in rbs) 
            {
                rb.MovePosition(rb.position + movement);
            }
        }
    }

    private bool ActiveConveyor(int _capacity) 
    {
        Debug.Log(_capacity);
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

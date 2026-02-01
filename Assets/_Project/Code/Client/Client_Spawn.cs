using System;
using UnityEngine;

public class Client_Spawn : MonoBehaviour
{
    [SerializeField] private int max_clients;
    public int Max_Clients => max_clients;

    [SerializeField] private float distance;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public Vector3 GetClientPos(int id)
    {
        if(id < 0) throw new Exception("ID is < 0");


        return transform.position + transform.forward * distance * id;
    }

    void OnDrawGizmos()
    {
        for (int i = 0; i < max_clients; i++)
        {
            Gizmos.DrawSphere(GetClientPos(i), 0.1f);
        }
    }
}

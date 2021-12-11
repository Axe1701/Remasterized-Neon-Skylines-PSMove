using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class turretmov : MonoBehaviour
{
    // Start is called before the first frame update
    private float _speed = 0.5f;


    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * Time.deltaTime * 4 * _speed;
    }
}
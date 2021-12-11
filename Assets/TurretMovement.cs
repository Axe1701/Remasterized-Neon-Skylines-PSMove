using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class TurretMovement : NetworkBehaviour
{
    // Start is called before the first frame update
    public float _speed = 0f;
    public float speed = 16f;
    public GameObject turret;

    // Update is called once per frame
    void Update()
    {
        _speed = Mathf.Lerp(_speed, speed, Time.deltaTime * .25f);
        transform.position += transform.forward * Time.deltaTime * 3 * _speed;
    }

    public void DestroyTurret()
    {
        Destroy(turret);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public float destroyTime;
    public float speed;

	// Use this for initialization
	void Start () {

        Destroy(gameObject, destroyTime);

	}
	
	// Update is called once per frame
	void Update () {

        transform.Rotate(Vector3.forward * speed * Time.deltaTime);

	}
}

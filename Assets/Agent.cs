using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour {
    Rigidbody2D rg;
    GameObject target;
    public Network network;
	// Use this for initialization
	void Start () {
        rg = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Target");
        network.fitness = 0;
    }
	// Update is called once per frame
	void Update () {
        GenericAlgorithm.NetworkRoutine(network,this,transform);
        network.fitness += (1/Vector2.Distance(gameObject.transform.position, target.transform.position));
	}
    public void AddRotation(float amount)
    {
        rg.rotation += amount;
    }
    public Network GetNetwork()
    {
        return network;
    }
    public float GetAngularVelocity()
    {
        return rg.angularVelocity;
    }
}

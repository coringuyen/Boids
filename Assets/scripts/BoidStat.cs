using UnityEngine;
using System.Collections;

public class BoidStat : MonoBehaviour {

    //To Do:
    // initialize fish
    // with the following variables
    // velocity vector = random velocity, position vector ie: transform.position = random position
    // calculate your rules
    // add the rules to the velocity vector
    // add the velocity vector to the position vector

    
    public Vector3 velocity;

    void Start()
    {
        //velocity = new Vector3(0.1f, 0.1f, 0.1f);
        velocity = new Vector3(Random.Range(0.2f, -0.2f), Random.Range(0.5f, -0.2f), Random.Range(0.2f, -0.20f));
    }
}
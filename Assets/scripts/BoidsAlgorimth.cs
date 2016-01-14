using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class BoidsAlgorimth : MonoBehaviour {
    // list of gameobject
    public List<GameObject> fishgold;

    public int count = 0; // how many time fish need to spawn
    public GameObject BoidPreb; // the fish object to spawn
    public GameObject TargetPreb; // fish target to go to

    public Slider cohension_inc; 
    public Slider alignment_inc;
    public Slider seperation_inc;
 

    Vector3 position; // give random position when fish spawn
    Vector3 v1, v2, v3, v4; // assign to the rules
    Vector3 pcj; // centre of mass
    Vector3 pvj; // perceive velocity, average all boids velocity except itself
    Vector3 force; // to return new position for bounding box
    float limitedVelocity; // limited velocity
    
    // limited value for bounding box
    public int xmin, xmax, ymin, ymax, zmin, zmax;
    BoidsGUI boidsgui;
    GameObject Target;

    void Start()
    {
        boidsgui = GetComponent<BoidsGUI>();

        limitedVelocity = 0.4f;
        for (int i = 0; i < count; ++i) // spawn gameobject then add it to the list
        {
            GameObject fish;
            position = new Vector3(Random.Range(10.0f, -10.0f), Random.Range(1.5f, 10.0f), Random.Range(10.0f, -10.0f));
            fish = Instantiate(BoidPreb) as GameObject;
            fish.transform.position = position;
            fish.transform.parent = gameObject.transform;
            fishgold.Add(fish);
        }

        Target = Instantiate(TargetPreb) as GameObject;
        Target.transform.position = new Vector3(0, 35, -40);

    }
 
    void Update()
    {
        move_all_boids_to_new_position();
    }
  
    void limited_velocity(GameObject boid) // they wont move too fast
    {
        if (boid.GetComponent<BoidStat>().velocity.magnitude > limitedVelocity)
        {
            boid.GetComponent<BoidStat>().velocity = boid.GetComponent<BoidStat>().velocity.normalized * 0.25f;
        }
    }

    public void ExitButton()
    {
            Application.Quit();
    }

	void move_all_boids_to_new_position() // move to the position that they should be
	{
        foreach(GameObject boid in fishgold)
        {
            v1 = cohesion  (boid) * cohension_inc.value * 0.1f;
            v2 = seperation(boid) * seperation_inc.value * 0.1f;
            v3 = alignment (boid) * alignment_inc.value * 0.1f;
            v4 = bounding_box(boid) * 0.01f;

            boid.GetComponent<BoidStat>().velocity = boid.GetComponent<BoidStat>().velocity + v1 + v2 + v3 + v4;
            boid.GetComponent<BoidStat>().transform.up = boid.GetComponent<BoidStat>().velocity.normalized;
            limited_velocity(boid);
            boid.transform.position += boid.GetComponent<BoidStat>().velocity;

        }
	}

    Vector3 bounding_box(GameObject currentBoid) // a limited distance boids need to be in 
    {
        Vector3 boid_position = currentBoid.transform.position;
        
        if (boid_position.x < xmin)      { force.x = xmin - boid_position.x; }
        else if (boid_position.x > xmax) { force.x = xmax - boid_position.x; }

        if (boid_position.y < ymin)      { force.y = ymin - boid_position.y; }
        else if (boid_position.y > ymax) { force.y = ymax - boid_position.y; }

        if (boid_position.z < zmin)      { force.z = zmin - boid_position.z; }
        else if (boid_position.z > zmax) { force.z = zmax - boid_position.z; }

        return force;
    }

    Vector3 cohesion(GameObject boid) // come together by centre of mass
    {
        if (Target && boidsgui.Target.isOn == true)
        {
            return (Target.transform.position - boid.transform.position) / 100; 
        }

        else foreach (GameObject f in fishgold)
        {
            if (f != boid)
            { pcj += f.transform.position; }
        }

        pcj = pcj / (fishgold.Count - 1);
        return (pcj - boid.transform.position) / 100;
    }

    Vector3 seperation(GameObject boid) // they won't be on top of each other or touch each other
    {
        Vector3 displacement = new Vector3(0,0,0);

        foreach (GameObject f in fishgold)
        {
            if (f != boid)
            {
                if (Vector3.Distance(f.transform.position, boid.transform.position) < 6) 
                { displacement = displacement - (f.transform.position - boid.transform.position); }
            }
        }

        return displacement;
    }

    Vector3 alignment(GameObject boid) // boids match velocity
    {
        foreach (GameObject f in fishgold)
        {
            if (f != boid)
            { pvj += f.GetComponent<BoidStat>().velocity; }
        }

        pvj = pvj / (fishgold.Count - 1);
        return (pvj - boid.GetComponent<BoidStat>().velocity) / 8;
    }
}

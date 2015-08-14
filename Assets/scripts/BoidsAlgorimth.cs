using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class BoidsAlgorimth : MonoBehaviour {

    public List<GameObject> fishgold;

    //public float alignment_insc, seperation_insc = 0;
    public int count = 0;
    public GameObject spawnfish;
    public GameObject target;
    public Slider cohension_inc;
    public Slider alignment_inc;
    public Slider seperation_inc;

    Vector3 pos;
    Vector3 v1, v2, v3, v4;
    Vector3 pcj;
    Vector3 pvj;
    Vector3 force;
    public float limited = 0;
    //int ground_lvl = -60;
    bool perch;

    // limited value for bounding box
    public int xmin, xmax, ymin, ymax, zmin, zmax;
  
    void Start()
    {
        for (int i = 0; i < count; ++i)
        {
            GameObject fish;
            pos = new Vector3(Random.Range(10.0f, -10.0f), Random.Range(1.5f, 10.0f), Random.Range(10.0f, -10.0f));
            fish = Instantiate(spawnfish) as GameObject;
            fish.transform.position = pos;
            fishgold.Add(fish);
        }
        
    }

    void Update()
    {
        move_all_boids_to_new_position();
    }

    //float magnitude(Vector3 vel) // vector size
    //{
    //    return Mathf.Sqrt(vel.x * vel.x + vel.y * vel.y + vel.z * vel.z);
    //}

    void limited_velocity(GameObject myFish) // they wont move too fast
    {
        if (myFish.GetComponent<BoidStat>().velocity.magnitude > limited)
        {
            myFish.GetComponent<BoidStat>().velocity = myFish.GetComponent<BoidStat>().velocity.normalized * 0.05f;
        }
    }

	void move_all_boids_to_new_position() // move to the position that they should be
	{
        foreach(GameObject myFish in fishgold)
        {
            v1 = cohesion  (myFish) * cohension_inc.value;
            v2 = seperation(myFish) * seperation_inc.value;
            v3 = alignment (myFish) * alignment_inc.value;
            v4 = bounding_box(myFish) * 0.01f;

            myFish.GetComponent<BoidStat>().velocity = myFish.GetComponent<BoidStat>().velocity + v1 + v2 + v3 + v4;
            limited_velocity(myFish);
            myFish.transform.position += myFish.GetComponent<BoidStat>().velocity;
        }
	}

    Vector3 bounding_box(GameObject currentFish) // boids wont move too far away
    {
        Vector3 fish_position = currentFish.transform.position;

        if (fish_position.x < xmin)      { force.x = xmin - fish_position.x; }
        else if (fish_position.x > xmax) { force.x = xmax - fish_position.x; }

        if (fish_position.y < ymin)      { force.y = ymin - fish_position.y; }
        else if (fish_position.y > ymax) { force.y = ymax - fish_position.y; }

        if (fish_position.z < zmin)      { force.z = zmin - fish_position.z; }
        else if (fish_position.z > zmax) { force.z = zmax - fish_position.z; }

        return force;
    }

    Vector3 cohesion(GameObject myFish) // come together by centre of mass
    {
        if (target)
        {
            return (target.transform.position - myFish.transform.position) / 100;
        }

        else foreach (GameObject f in fishgold)
        {
            if (f != myFish)
            { pcj += f.transform.position; }
        }

        pcj = pcj / (fishgold.Count - 1);
        return (pcj - myFish.transform.position) / 100;
    }

    Vector3 seperation(GameObject myFish) // every boids have their own territory
    {
        Vector3 displacement = new Vector3(0,0,0);

        foreach (GameObject f in fishgold)
        {
            if (f != myFish)
            {
                if (Vector3.Distance(f.transform.position, myFish.transform.position) < 4) 
                { displacement = displacement - (f.transform.position - myFish.transform.position); }
            }
        }

        return displacement;
    }

    Vector3 alignment(GameObject myFish) // boids match velocity
    {
        foreach (GameObject f in fishgold)
        {
            if (f != myFish)
            { pvj += f.GetComponent<BoidStat>().velocity; }
        }

        pvj = pvj / (fishgold.Count - 1);
        return (pvj - myFish.GetComponent<BoidStat>().velocity) / 8;
    }
}

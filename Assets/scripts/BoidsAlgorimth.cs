using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class BoidsAlgorimth : MonoBehaviour {
    // list of gameobject
    public List<GameObject> fishgold;

    public int count = 0; // how many time fish need to spawn
    public GameObject spawnfish; // the fish object to spawn
    public GameObject target; // fish target to go to 
    public GameObject bigFish; 

    public Slider cohension_inc; 
    public Slider alignment_inc;
    public Slider seperation_inc;
 

    Vector3 pos; // give random position when fish spawn
    Vector3 v1, v2, v3, v4; // assign to the rules
    Vector3 pcj; // centre of mass
    Vector3 pvj; // perceive velocity, average all boids velocity except itself
    Vector3 force; // to return new position for bounding box
    float limitedVelocity; // limited velocity
    //int ground_lvl = -60;
    bool perch;

    
    // limited value for bounding box
    public int xmin, xmax, ymin, ymax, zmin, zmax;
  
    void Start()
    {
        limitedVelocity = 0.1f;
        for (int i = 0; i < count; ++i) // spawn gameobject then add it to the list
        {
            GameObject fish;
            pos = new Vector3(Random.Range(10.0f, -10.0f), Random.Range(1.5f, 10.0f), Random.Range(10.0f, -10.0f));
            fish = Instantiate(spawnfish) as GameObject;
            fish.transform.position = pos;
            fish.transform.parent = gameObject.transform;
            fishgold.Add(fish);
        }

        GameObject Bigfish;
        Bigfish = Instantiate(bigFish) as GameObject;
        Bigfish.transform.position = new Vector3(20, 12, 8);
    }
 
    void Update()
    {
        move_all_boids_to_new_position();
        move_big_fish();
    }
  
    void limited_velocity(GameObject myFish) // they wont move too fast
    {
        if (myFish.GetComponent<BoidStat>().velocity.magnitude > limitedVelocity)
        {
            myFish.GetComponent<BoidStat>().velocity = myFish.GetComponent<BoidStat>().velocity.normalized * 0.25f;
        }
    }
    public void ExitButton()
    {
            Application.Quit();
    }

	void move_all_boids_to_new_position() // move to the position that they should be
	{
        foreach(GameObject myFish in fishgold)
        {
            v1 = cohesion  (myFish) * cohension_inc.value * 0.1f;
            v2 = seperation(myFish) * seperation_inc.value * 0.1f;
            v3 = alignment (myFish) * alignment_inc.value * 0.1f;
            v4 = bounding_box(myFish) * 0.01f;

            myFish.GetComponent<BoidStat>().velocity = myFish.GetComponent<BoidStat>().velocity + v1 + v2 + v3 + v4;
            myFish.GetComponent<BoidStat>().transform.up = myFish.GetComponent<BoidStat>().velocity.normalized;
            limited_velocity(myFish);
            myFish.transform.position += myFish.GetComponent<BoidStat>().velocity;

        }
	}

    void move_big_fish()
    {
        v1 = cohesion(bigFish) * 0.1f;
        v2 = seperation(bigFish) * 0.25f;
        v3 = alignment(bigFish) * 0.1f;
        v4 = bounding_box(bigFish) * 0.01f;

        bigFish.GetComponent<BoidStat>().velocity = bigFish.GetComponent<BoidStat>().velocity + v1 + v2 + v3 + v4;
        bigFish.GetComponent<BoidStat>().transform.up = bigFish.GetComponent<BoidStat>().velocity.normalized;
        limited_velocity(bigFish);
        bigFish.transform.position += bigFish.GetComponent<BoidStat>().velocity;
    }

    Vector3 bounding_box(GameObject currentFish) // a limited distance boids need to be in 
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

    Vector3 seperation(GameObject myFish) // they won't be on top of each other or touch each other
    {
        Vector3 displacement = new Vector3(0,0,0);

        foreach (GameObject f in fishgold)
        {
            if (f != myFish)
            {
                if (Vector3.Distance(f.transform.position, myFish.transform.position) < 6) 
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

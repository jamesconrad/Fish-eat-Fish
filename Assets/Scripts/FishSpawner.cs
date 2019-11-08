using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishSpawner : MonoBehaviour
{

    public int maxFish = 40;
    public int minEdibleFish = 5;//minimum edible fish at any given time
    public Gradient sizeGradient;
    public float minMass = 0.25f;
    public float maxMass = 10f;

    public bool spawnFish = false;

    public Fish playerFish;

    // Start is called before the first frame update
    void Start()
    {

    }


    void Update()
    {
        if (spawnFish && transform.childCount < maxFish)
            GenerateRandomFish();
    }

    private GameObject GenerateRandomFish()
    {
        GameObject fishGO = Instantiate(Resources.Load("Fish")) as GameObject;
        fishGO.transform.SetParent(transform);
        Fish fish = fishGO.GetComponent<Fish>();
        Material fishMat = fishGO.GetComponent<SpriteRenderer>().material;
        PolygonCollider2D fishPoly = fishGO.GetComponent<PolygonCollider2D>();

        float randMass = RandomMass();
        fish.SetMass(randMass);
        fishMat.color = sizeGradient.Evaluate(MassTo01Range(randMass));


        //select a point off screen within 2 viewports of player
        float spawnX = Random.Range(-1f, 1f) >= 0 ? Random.Range(-1f, -1f): Random.Range(1f,1f);
        float spawnY = Random.Range(-1f, 1f);
        Vector3 spawnPos = Camera.main.ViewportToWorldPoint(new Vector3(spawnX, spawnY, 0));
        spawnPos.z = 0;
        fishGO.transform.position = spawnPos;
        fishGO.name = randMass.ToString();

        return fishGO;
    }

    private float MassTo01Range(float mass)
    {
        return (mass - minMass) / (maxMass - minMass);
    }

    private float RandomMass()
    {
        Fish[] spawnedFish = GetComponentsInChildren<Fish>();
        float averageMass = 0;
        int edibleFish = 0;
        foreach (Fish f in spawnedFish)
        {
            averageMass += f.m_mass;
            if (f.m_mass < playerFish.m_mass)
                edibleFish++;
        }
        averageMass /= spawnedFish.Length;

        //not enough fish we can eat, spawn a smaller fish
        if (edibleFish < minEdibleFish)
            return Random.Range(minMass, playerFish.m_mass) - 0.05f;

        //number of edible fish is ok, now we want to try to balance the average to be slightly higher than player mass
        return Random.Range(minMass, maxMass);
    }
}

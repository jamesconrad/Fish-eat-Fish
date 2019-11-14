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
    public float spawnDistance;
    public bool spawnFish = false;
    public float maxAverageStep;
    private float halfAverageStep;
    private float massRange;

    public Fish playerFish;
    public DepthCameraEffects dcEffect;
    public InputHandler.State state = InputHandler.State.Menu;

    // Start is called before the first frame update
    void Start()
    {
        massRange = maxMass - minMass;
        halfAverageStep = maxAverageStep / 2;
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

        float randMass = RandomMass();
        fish.SetMass(randMass);
        fishMat.color = sizeGradient.Evaluate(MassTo01Range(randMass));

        float spawnX = Random.Range(-1f, 1f);
        float spawnY = Random.Range(-1f, 1f);
        Vector3 spawnPos = (new Vector3(spawnX, spawnY, 0)).normalized * spawnDistance + Camera.main.transform.position;
        spawnPos.z = 0;
        spawnPos.y = Mathf.Clamp(spawnPos.y, dcEffect.minY, dcEffect.maxY);
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
        float totalMass = 0;
        int edibleFish = 0;
        foreach (Fish f in spawnedFish)
        {
            totalMass += f.m_mass;
            if (state == InputHandler.State.Game && f.m_mass < playerFish.m_mass)
                edibleFish++;
        }
        float averageMass = totalMass / spawnedFish.Length;

        //not enough fish we can eat, spawn a smaller fish
        if (state == InputHandler.State.Game && edibleFish < minEdibleFish)
            return Random.Range(minMass, playerFish.m_mass) - 0.05f;

        //now we need to try and get the average mass between its ratio equal to the average mass for current height
        //new value x required to set average to value a
        //x = a * newNumValues - sumValues
        float targetAverageMass = dcEffect.normalizedHeight * massRange + minMass;
        float averageStepRequired = targetAverageMass - averageMass;
        float newAverage;
        if (averageStepRequired >= halfAverageStep)
            newAverage = averageMass + Random.Range(halfAverageStep, maxAverageStep);
        else if (averageStepRequired <= halfAverageStep * -1)
            newAverage = averageMass - Random.Range(halfAverageStep, maxAverageStep);
        else
            newAverage = averageMass + Random.Range(-Mathf.Abs(averageStepRequired), Mathf.Abs(averageStepRequired));
        //print("AVG mass:" + averageMass + " DESIRED next mass: " + newAverage);

        return Mathf.Clamp(newAverage * (spawnedFish.Length + 1) - totalMass, minMass, maxMass); //return the mass needed to set the average mass to newAverage
    }

    public void DeleteAllFish()
    {
        foreach(Transform t in transform)
            Destroy(t.gameObject);
    }
}

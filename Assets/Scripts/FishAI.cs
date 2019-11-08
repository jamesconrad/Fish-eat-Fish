using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishAI : MonoBehaviour
{
    private Fish fish;
    private Vector3 direction;
    private bool onScreen = false;
    private float screenTimer;
    // Start is called before the first frame update
    void Start()
    {
        screenTimer = 0;
        fish = GetComponent<Fish>();
        if (transform.position.x < Camera.main.transform.position.x)
            direction = new Vector3(1, 0, 0) * 10;
        else
            direction = new Vector3(-1, 0, 0) * 10;
    }

    // Update is called once per frame
    void Update()
    {
        fish.GoTo(transform.position + direction);
        Vector3 viewPos = Camera.main.WorldToViewportPoint(transform.position);
        viewPos.z = 0;
        if (viewPos.magnitude < 2)
        {
            screenTimer = 0;
            onScreen = true;
        }
        else
            onScreen = false;

        if (!onScreen)
        {
            screenTimer += Time.smoothDeltaTime;

            if (screenTimer >= 5)
                Destroy(gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishAI : MonoBehaviour
{
    private Fish fish;
    private Vector3 direction;
    private bool onScreen = false;
    private float screenTimer;
    private static Transform player;
    public float maxRange;

    private float time;
    // Start is called before the first frame update
    void Start()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;
        screenTimer = 0;
        fish = GetComponent<Fish>();
        if (transform.position.x < Camera.main.transform.position.x)
        {
            direction = new Vector3(1, 0, 0);
        }
        else
        {
            direction = new Vector3(-1, 0, 0);
            //GetComponent<SpriteRenderer>().flipX = true;
        }
        time = Random.Range(0f, 360f);
        transform.position = new Vector3(transform.position.x, transform.position.y, Random.Range(-0.1f, 0.1f));
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.smoothDeltaTime;
        direction.y = Mathf.Sin(time);
        fish.GoTo(transform.position + direction);
        Vector3 toPlayer = transform.position - player.position;
        toPlayer.z = 0;
        if (toPlayer.magnitude < maxRange)
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

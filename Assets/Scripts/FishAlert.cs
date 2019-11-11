using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishAlert : MonoBehaviour
{
    private SpriteRenderer sr;
    private Fish fish;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        fish = GetComponentInParent<Fish>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        //if (collision.tag == "Player")
        //    return;

        Fish other = collision.gameObject.GetComponent<Fish>();
        if (other.m_mass > fish.m_mass)
            sr.enabled = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //if (collision.tag == "Player")
        //    return;

        Fish other = collision.gameObject.GetComponent<Fish>();
        if (other.m_mass > fish.m_mass)
            sr.enabled = false;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundParallax : MonoBehaviour
{
    public SpriteRenderer[] parallaxSprites;
    public float foregroundMoveSpeed = 1.0f;
    public float backgroundMoveSpeed = 2.0f;
    private float[] parallaxScalars;
    public Transform reference;

    private void Start()
    {
        parallaxScalars = new float[parallaxSprites.Length];
        for (int i = 0; i < parallaxSprites.Length; i++)
        {
            parallaxScalars[i] = Mathf.Lerp(backgroundMoveSpeed, foregroundMoveSpeed, (float)i / (parallaxSprites.Length - 1));
        }
    }

    private void Update()
    {
        for (int i = 0; i < parallaxSprites.Length; i++)
        {
            parallaxSprites[i].material.SetTextureOffset("_MainTex", new Vector2(reference.position.x * parallaxScalars[i], 0));
        }
    }
}

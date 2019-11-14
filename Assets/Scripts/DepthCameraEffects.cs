using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DepthCameraEffects : MonoBehaviour
{
    public Light frontDirLight;
    public Light backDirLight;
    public Transform target;
    public Gradient backgroundGradient;
    public float maxY;
    public float minY;
    public float maxLuminance;
    public float minLuminance;
    private float yRange;
    private float luminanceRange;

    public InputHandler.State state = InputHandler.State.Menu;

    public float normalizedHeight { private set; get; }

    private void Start()
    {
        yRange = maxY - minY;
        luminanceRange = maxLuminance - minLuminance;
    }

    // Update is called once per frame
    void Update()
    {
        //calculate values for the various effects, all based on a minmax range using camera y value
        float camY;

        if (state == InputHandler.State.Game)
            camY = Mathf.Clamp(target.position.y, minY, maxY);
        else
            camY = Mathf.Clamp((Mathf.Cos(Time.realtimeSinceStartup/15) + 1) / 2 * yRange + minY, minY, maxY);
        normalizedHeight = 1 - (camY - minY) / yRange; //flip range so 0 is at max height, 1 is at min
        float luminosity = luminanceRange - (normalizedHeight * luminanceRange + minLuminance);
        Color bgColor = backgroundGradient.Evaluate(normalizedHeight);

        //assign values

        if (state == InputHandler.State.Game)
            transform.position = new Vector3(target.position.x, camY, transform.position.z);
        else
            transform.position = new Vector3(transform.position.x, camY, transform.position.z);
        frontDirLight.intensity = luminosity;
        backDirLight.intensity = luminosity;
        Camera.main.backgroundColor = bgColor;
    }
}

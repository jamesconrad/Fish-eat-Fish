using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour
{
    public Rigidbody2D rb2d { get; private set; }
    private bool eaten = false;
    private Vector3 target;
    private bool arrived = true;
    private static float baseArea = 0;
    public float m_mass;

    private void Start()
    {
        if (name == "Player")
            SetMass(0.75f);
        rb2d = GetComponent<Rigidbody2D>();
        if (baseArea == 0)
            CalculateBaseArea();

    }

    private void Update()
    {
        if (eaten)
            Destroy(gameObject);

        //movement
        if (!arrived)
        {
            float speed = 0;
            if (name == "Player")
                speed = 1f;
            else
                speed = 1f / m_mass;
            rb2d.MovePosition(Vector3.MoveTowards(rb2d.position, target, speed * Time.smoothDeltaTime));
        }
    }

    public void SetMass(float mass)
    {
        m_mass = mass;
        float neededArea = mass; //area = mass / density; density is 1 by default and will remain at 1 for all fish
        float scalar = neededArea / baseArea;

        //to increase the area we need the sqrt of the edge scalars
        Vector3 newScale = new Vector3(1, 1, 0) * Mathf.Abs(Mathf.Sqrt(scalar));
        newScale.z = 1;
        transform.localScale = newScale;

        StartCoroutine("SetMassNextFrame");
    }

    private IEnumerator SetMassNextFrame()
    {
        yield return null;
        rb2d.mass = m_mass;
        //print("Mass: " + rb2d.mass);
    }

    public void GoTo(Vector3 pos)
    {
        target = pos;
        arrived = false;
        //orient to correct direction
        Quaternion q = new Quaternion();
        if (target.x - transform.position.x > 0)
        {
            q.eulerAngles = new Vector3(0, 0, 0);
            transform.localRotation = q;
        }
        else
        {
            q.eulerAngles = new Vector3(0, 180, 0);
            transform.localRotation = q;
        }
    }
    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision. != "Player")
    //        return;
    //    Fish other = collision.gameObject.GetComponent<Fish>();
    //    if (other.m_mass > m_mass)
    //        eaten = true;
    //    if (m_mass > other.m_mass)
    //    {
    //        SetMass(m_mass + Mathf.Sqrt(other.m_mass) / 4);
    //        other.eaten = true;
    //    }
    //}

    private void OnDestroy()
    {
        //call gameover
        if (transform.tag != "Player")
            return;
        FindObjectOfType<InputHandler>().StopGame();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != "Player")
            return;
        //print(name + " vs " + collision.gameObject.name);
        Fish other = collision.gameObject.GetComponent<Fish>();
        if (other.m_mass > m_mass)
        {
            other.SetMass(other.m_mass + Mathf.Sqrt(m_mass) / 4);
            eaten = true;
        }
        if (m_mass > other.m_mass)
        {
            SetMass(m_mass + Mathf.Sqrt(other.m_mass) / 4);
            other.eaten = true;
        }
    
    }
    private void CalculateBaseArea()
    {
        //create a temporary object to check its mass at scale 1
        PolygonCollider2D fishPoly = GetComponent<PolygonCollider2D>();

        //code from Box2D area calculation of polygon colliders

        float area = 0f;
        Vector2 s = new Vector2(0f, 0f); //reference point for forming triangles

        for (int i = 0; i < fishPoly.points.Length; ++i)
        {
            s += fishPoly.points[i];
        }
        s *= 1f / fishPoly.points.Length;

        for (int i = 0; i < fishPoly.points.Length; ++i)
        {
            // Triangle vertices.
            Vector2 e1 = fishPoly.points[i] - s;
            Vector2 e2 = i + 1 < fishPoly.points.Length ? fishPoly.points[i + 1] - s : fishPoly.points[0] - s;

            //2d cross product
            float D = e1.x * e2.y - e1.y * e2.x;

            float triangleArea = 0.5f * D;
            area += triangleArea;
        }

        baseArea = area;
    }
}

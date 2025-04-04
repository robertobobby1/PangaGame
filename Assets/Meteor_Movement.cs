using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor_Movement : MonoBehaviour
{
    public float maxHeight;
    public float horizSpeed;
    public float gravity;
    public float size;
    private Vector2 velocity;
    private float floor = -4f;
    private float wall = 8.9f;
    private Rigidbody2D rb;
    private bool initialize;
    private Game_Manager gm;

    private void Awake()
    {
        initialize = true;
        gm = GameObject.FindGameObjectWithTag("Finish").GetComponent<Game_Manager>();
    }

    public void Initialize(float size, float difficulty, bool dir)
    {
        float lerpValue = Mathf.InverseLerp(0.4f, 5f, size);
        float newHeight = Mathf.Lerp(110, 400, lerpValue);
        float newGravity = Mathf.Lerp(0.1f, 1.2f, 1-lerpValue);
        if (size > 3.5f)
        {
            maxHeight = randomWithBias(newHeight - 30, newHeight + 30, 0.5f);
            gravity = randomWithBias(newGravity - 0.4f, newGravity + 0.1f, 2);
        }
        else 
        {
            maxHeight = newHeight;
            gravity = newGravity;
        }
        horizSpeed = Random.Range(1.5f,6);
        if (dir) 
        {
            velocity = new Vector2(horizSpeed, 0);
        }
        else
            velocity = new Vector2(-horizSpeed, 0);

        rb = GetComponent<Rigidbody2D>();
        transform.localScale *= size;
        floor += size / 2;
        wall -= size / 2;
    }

    float randomWithBias(float low, float high, float bias)
    {
        float r = Random.Range(0, 1);    // random between 0 and 1
        r = Mathf.Pow(r, bias);
        return low + (high - low) * r;
    }

    private void FixedUpdate()
    {
        if (initialize) 
        {
            initialize = false;
            return;
        }
        Move();
    }

    private void Move()
    {
        Vector2 currentPos = transform.position;
        
        if (currentPos.y < floor) 
        {
                velocity.y = Mathf.Sqrt(2*gravity*maxHeight);  
        }     
        else
            velocity.y -= gravity;
        if (currentPos.x > wall || currentPos.x < -wall)
            velocity.x = -velocity.x;

        rb.MovePosition((Vector2)transform.position + velocity * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if (transform.localScale.x > 3.4f) 
        {
            gm.CreateMeteor(1, transform.position, true);
            gm.CreateMeteor(1, transform.position, false);
        }
        gm.RemoveBall(transform.gameObject);
        if (!collision.gameObject.tag.Equals("Player"))
            Destroy(collision.gameObject);
        Destroy(gameObject);
    }
}

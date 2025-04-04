using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Player_Controller : MonoBehaviour
{
    Camera cam;
    public float Speed;
    private Animator anim;
    public GameObject bullet;
    public float bulletSpeed;
    public float fireInterval;
    private float timer;
    private bool isShooting;
    private AudioSource shots;
    private int health;
    public TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        anim = GetComponent<Animator>();
        timer = fireInterval;
        isShooting = false;
        shots = GameObject.FindGameObjectWithTag("Respawn").GetComponent<AudioSource>();
        health = 5;
        text.text = "Health: 9";
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (Input.GetMouseButton(0))
        {
            if (timer < 0)
            {
                timer = fireInterval;
                Shoot(cam.ScreenToWorldPoint(Input.mousePosition));
            }
            isShooting = true;
        }
        else
        {
            isShooting = false;
        }

        if (isShooting && !shots.isPlaying)
        {
            shots.Play();
        }
        else if (!isShooting)
            shots.Stop();
    }

    

    // Update is called once per frame
    void FixedUpdate()
    {
        Move(cam.ScreenToWorldPoint(Input.mousePosition));
    }

    private void Move(Vector2 target)
    {
        Vector2 playerPos = new Vector2(transform.position.x, transform.position.y);
        float moveMultiplier = Vector2.Distance(playerPos, target)*0.1f;
        int direction = -1;
        transform.localScale = new Vector3(-1,1,1);
        float diff = target.x - playerPos.x;

        // Change players direction apperance
        if (diff > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
            direction = 1;
        }

        // Animator stops running even moving a little
        if (Mathf.Abs(diff) < 1f)
            anim.SetBool("isRunning", false);

        // Dont move is mouse close to player
        if (Mathf.Abs(diff) < 0.1f)
            return;

        transform.position = new Vector3(transform.position.x + (direction * moveMultiplier * Speed), transform.position.y, transform.position.z);
        anim.SetBool("isRunning", true);
        
    }

    void Shoot(Vector2 target) 
    {
        Vector2 dir = target - (Vector2)transform.position;
        dir.Normalize();
        GameObject newBullet = Instantiate(bullet, transform.position, Quaternion.Euler(dir));
        newBullet.GetComponent<Rigidbody2D>().AddForce(dir * bulletSpeed);
        StartCoroutine(expireBullet(newBullet));
    }

    IEnumerator expireBullet(GameObject bullet) 
    {
        yield return new WaitForSeconds(1);
        Destroy(bullet);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        health--;
        text.text = "Health: " + health.ToString();
        Debug.Log("hit");
        if (health <= 0)
            SceneManager.LoadScene(0);
    }

}

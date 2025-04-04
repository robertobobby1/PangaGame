using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Game_Manager : MonoBehaviour
{
    public GameObject Meteor;
    private List<GameObject> Balls;
    private int difficulty;
    private int score;
    public TextMeshProUGUI text;
    
    // Start is called before the first frame update
    void Start()
    {
        score = 0;
        Balls = new List<GameObject>();
        difficulty = 1;
        text.text = "Score: 0";
    }

    // Update is called once per frame
    void Update()
    {
        if (Balls.Count == 0)
        {
            for (int i = 0; i < difficulty; i++) 
            {
                CreateMeteor(1, new Vector3(Random.Range(-5.5f, 5.5f), Random.Range(6f, 9f), 0), true);
            }
            difficulty++;
        }
            
    }

    void NewWave() 
    {

    }

    public void CreateMeteor(float difficulty, Vector2 initPos, bool dir) 
    {
        float size = Random.Range(0.4f, 3.8f);
        GameObject newMeteor = Instantiate(Meteor, initPos, transform.rotation);
        newMeteor.GetComponent<Meteor_Movement>().Initialize(size, difficulty, dir);
        Balls.Add(newMeteor);
    }

    public void RemoveBall(GameObject ball) 
    {
        score += (int)ball.transform.localScale.x * 50;
        Balls.Remove(ball);
        text.text = score.ToString();
    }

    
}

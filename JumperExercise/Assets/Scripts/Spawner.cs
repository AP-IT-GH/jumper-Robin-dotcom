using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject obstacle;
    public GameObject coin;
    public float spawnTime = 3f;
    private float counter = 0f;
    // Start is called before the first frame update
    
    
    private void Spawn()
    {
        int random = Random.Range(0, 6);
        if (random > 2)
        {
            GameObject a = Instantiate(coin);
            a.tag = "Obstacle";
            a.transform.localPosition = new Vector3(3f, 0.5f, 0);
        }
        else
        {
            GameObject b = Instantiate(obstacle);
            b.tag = "Coin";
            b.transform.localPosition = new Vector3(3f, 0.5f, 0);
        }
        
        
    }
    // Update is called once per frame
    void Update()
    {
        if (counter <= 0)
        {
             counter = spawnTime;
            Spawn();
        }
        counter -= Time.deltaTime;
    }
}

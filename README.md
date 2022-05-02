# jumper-Robin-dotcom
jumper-Robin-dotcom created by GitHub Classroom

# 1. Aanmaken van project
Eerst moeten we het project aanmaken via Unity Hub. Maak een 3D project aan, geef het een naam en plaats het in de juiste folder.

![image](https://user-images.githubusercontent.com/72873870/166316705-6a956a46-2d97-4f9d-aa2c-f56cb65e770e.png)

Zorg ervoor dat de MLAgents package versie 2.0.1 is geinstalleerd in je project. Dit doe je via de packet manager van Unity.

![image](https://user-images.githubusercontent.com/72873870/166317072-8eed398b-b2fe-4724-90bb-aac081d6cb82.png)

# 2. Aanmaken van Unity wereld

We maken een unity wereld aan met volgende zaken, een cube (de agent), twee spheres (een coin en een obstacle) en een plane (floor). Al deze elementen plaats je samen in een leeg game object, genaamd trainingsarea. Dit heeft als voordeel dat je er een prefab van kan maken om het leerproces te versnellen. Ook maken we van de twee spheres een prefab zodat we deze later kunnen gebruiken in het spawner script.

![image](https://user-images.githubusercontent.com/72873870/166317439-42fb68af-78bf-4ce3-93cb-01303b09ee46.png)

# 3. Scripts

Het eerste script dat we aanmaken is voor de Agent:

using System.Collections;
using System.Collections.Generic;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using UnityEngine;

public class JumperAgent : Agent
{
    public Transform coin;
    public Transform obstacle;

    public override void OnEpisodeBegin()
    {
        this.transform.localPosition = new Vector3(-7f, 0.5f, 0);
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(this.transform.localPosition);
        sensor.AddObservation(coin.localPosition);
        sensor.AddObservation(obstacle.localPosition);

    }

    public float jumpSpeed = 2f;
    public override void OnActionReceived(ActionBuffers actions)
    {
        //Acties
        Vector3 controlSignal = Vector3.zero;

        controlSignal.y = actions.ContinuousActions[0];

        this.transform.Translate(controlSignal * jumpSpeed);

        //Beloningen
        AddReward(0.0001f);
        
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var continuousActionsOut = actionsOut.ContinuousActions;

        continuousActionsOut[0] = Input.GetAxis("Vertical");
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle") == true)
        {
            AddReward(-1f);
            Destroy(collision.gameObject);
            EndEpisode();
        }
        if (collision.gameObject.CompareTag("Coin") == true)
        {
            AddReward(1f);
            Destroy(collision.gameObject);
        }
    }
}

Daarna maken we een script Spawner aan. Dit script zal ervoor zorgen dat obstakels worden ingeladen in een bepaald tijdsinterval: 


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


We maken een script Target aan om de ingeladen objecten te laten bewegen. Dit script zorgt er ook voor dat de objecten worden uitgeladen wanneer nodig:


    private Rigidbody2D rb;
    private float speed;
    // Start is called before the first frame update
    public void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        speed = Random.Range(0.1f, 1f);
            

    }

    // Update is called once per frame
    public void Update()
    {
        this.transform.Translate(-speed, 0, 0);
        if (this.transform.localPosition.x > -10)
        {
            Destroy(this.gameObject);
        }
        if (this.transform.localPosition.y < -10)
        {
            Destroy(this.gameObject);
        }
        
    }
# 4. Project

Na het aanmaken van de scripts moeten we ervoor zorgen dat alle gameobjects zijn gebonden aan de juiste scripts, bijvoorbeeld in het Agent script:

![image](https://user-images.githubusercontent.com/72873870/166319573-3a96af0d-3e0a-4cdd-8890-ec307526b3d4.png)



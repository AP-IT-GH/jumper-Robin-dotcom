using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
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
}

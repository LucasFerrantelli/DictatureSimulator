using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    float delayBeforeDeath = 800;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position += new Vector3(0, 0, -0.15f);
        delayBeforeDeath--;
        if(delayBeforeDeath <= 0)
        {
            Destroy(this.gameObject);
        }

    }

    void OnTriggerEnter(Collider collision)
    {
        EnemyBehavior _enemy = collision.gameObject.GetComponent<EnemyBehavior>();

        if (_enemy)
        {

            _enemy.Die();
        }
    }
}

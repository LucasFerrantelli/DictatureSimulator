using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndDoor : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnTriggerEnter(Collider collision)
    {
        EnemyBehavior _enemy = collision.gameObject.GetComponent<EnemyBehavior>();
        
        if(_enemy)
        {

            _enemy.Kamikaze();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

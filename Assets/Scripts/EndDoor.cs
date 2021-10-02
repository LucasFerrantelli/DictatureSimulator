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
        print("a");
        if(_enemy)
        {
            print("b");
            GameManager.Instance.baseHP--;
            Destroy(_enemy.gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

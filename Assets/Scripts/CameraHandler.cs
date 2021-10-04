using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraHandler : MonoBehaviour
{
    public static CameraHandler Instance;
    Vector3 defaultPos;

    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
        defaultPos = transform.position;
        //StartCoroutine(Delay());
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(3f);

    }

    public int remainingShakeDuration;

    private void FixedUpdate()
    {
        if(remainingShakeDuration > 0)
        {
            remainingShakeDuration--;
            if(remainingShakeDuration % 2== 0)
            {
                transform.position += Random.insideUnitSphere;
            }
            else
            {
                transform.position = defaultPos;
            }
            
        }
        else
        {
            transform.position = defaultPos;
        }
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Telegraph : MonoBehaviour
{

    public List<GameObject> telegraphs;
    public int theOne;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < telegraphs.Capacity; i++)
        {
            if(theOne == i)
            {
                telegraphs[i].SetActive(true);
            }
            else
            telegraphs[i].SetActive(false);

        } 
    }
}

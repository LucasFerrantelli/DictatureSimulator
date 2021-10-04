using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Flag : MonoBehaviour
{
    public Image bg;
    public Image lines;
    public Image logo;

    public List<Sprite> logos;
    public List<Color> logoColors;
    public List<Color> lineColors;

    public int logoIndex;
    public int lineColorIndex;
    public int logoColorIndex;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void GenerateRandomFlag()
    {
        logoIndex = Random.Range(0, logos.Capacity);
        lineColorIndex = Random.Range(0, lineColors.Capacity);
        logoColorIndex = Random.Range(0, logoColors.Capacity);
    }


    void Update()
    {
        lines.color = lineColors[lineColorIndex];
        logo.color = logoColors[logoColorIndex];
        logo.sprite = logos[logoIndex];
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header ("Balancing")]
    public int baseHP;
    public float defaultTime = 100;


    [Header ("Settings/debug")]
    public GameState gameState;
    public float currentTime;
    public float gameSpeed;

    [Header("References")]
    public Text hpBaseTextDisplay;

    public enum GameState { InFight, Preparation, LawVoting}

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        Init();
    }

    void Init()
    {
        currentTime = defaultTime;
    }

    void FixedUpdate()
    {
        currentTime -= gameSpeed / 60;
    }

    // Update is called once per frame
    void Update()
    {
        hpBaseTextDisplay.text = baseHP.ToString();
    }
}

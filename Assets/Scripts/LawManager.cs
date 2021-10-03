using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LawManager : MonoBehaviour
{
    public enum Law {EcoloIncrease, EcoloDecrease, FarRightIncrease, FarRightDecrease, IncreaseOverallDifficulty, taxeIncrease, greve}

    public List<Law> currentLaws;
    [Serializable]
    public struct LawEvent
    {
        public string eventName;
        public string lawOneDescription;
        public string lawTwoDescription;
        public float weight;
        public bool once;
        public List<Law> lawOne;
        public List<Law> lawTwo;

    }
    public List<LawEvent> lawEvents;

    // Start is called before the first frame update
    void Start()
    {
        print(PickRandomEvent().eventName);
        print(PickRandomEvent().eventName);
        print(PickRandomEvent().eventName);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public LawEvent PickRandomEvent()
    {
        float totalWeight = 0;
        for (int i = 0; i < lawEvents.Count; i++)
        {
            totalWeight += lawEvents[i].weight;
        }
        float _random = 0;
        _random = UnityEngine.Random.Range(0, totalWeight);
        for (int i = 0; i < lawEvents.Count; i++)
        {
            _random -= lawEvents[i].weight;
            if(_random <= 0)
            {

                if (lawEvents[i].once == true)
                {
                    LawEvent _lawEvent = lawEvents[i];
                    _lawEvent.weight = 0;
                    lawEvents[i] = _lawEvent;
                }

                return lawEvents[i];
                
            }
        }
        return lawEvents[0];
    }


    public void ApplyLaw(Law law)
    {
        switch (law)
        {
            case Law.EcoloIncrease:
                GameManager.Instance.familiesScores[0] += 0.2f;
                break;
            case Law.EcoloDecrease:
                GameManager.Instance.familiesScores[0] -= 0.15f;
                break;
               
            case Law.taxeIncrease:
                break;
            case Law.greve:
                break;
            default:
                break;
        }
    }

    void IncreaseMobFamilyStat()
    {

    }
}



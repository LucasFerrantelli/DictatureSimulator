using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class LawManager : MonoBehaviour
{
    public enum Law {EcoloIncrease, EcoloDecrease, FarRightIncrease, FarRightDecrease, IncreaseOverallDifficulty, taxeIncrease, greve}

    public static LawManager Instance;

    public LawEvent currentDiscussedEvent;

    public List<Law> currentLaws;
    [Serializable]
    public struct LawEvent
    {
        public string eventName;
        public string eventDescription;
        public string lawOneDescription;
        public string lawTwoDescription;
        public float weight;
        public bool once;
        public List<Law> lawOne;
        public List<Law> lawTwo;

    }
    public List<LawEvent> lawEvents;

    [Header("LawsRef")]
    public bool grassSpawners;




    // Start is called before the first frame update
    void Awake()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public LawEvent PickRandomEvent()
    {
        print("pick");
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

    public TMP_Text eventName;
    public TMP_Text eventDescription;
    public TMP_Text lawOneDescription;
    public TMP_Text lawTwoDescription;

    public void SetUpEventLaw(LawEvent _lawEvent)
    {
        currentDiscussedEvent = _lawEvent;
        eventName.text = _lawEvent.eventName;
        eventDescription.text = _lawEvent.eventDescription;
        lawOneDescription.text = _lawEvent.lawOneDescription;
        lawTwoDescription.text = _lawEvent.lawTwoDescription;
    }

    public void LawConfirm(int lawValidated)
    {

        if (lawValidated == 0)
        {
            ApplyAllSubLaws(currentDiscussedEvent.lawOne);
        }
        else
        {
            ApplyAllSubLaws(currentDiscussedEvent.lawTwo);
        }
        
        

    }

    public void ApplyAllSubLaws(List<Law> _sublaws)
    {
        for (int i = 0; i < _sublaws.Capacity; i++)
        {
            ApplyLaw(_sublaws[i]);
        }
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



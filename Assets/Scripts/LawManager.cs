using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;

public class LawManager : MonoBehaviour
{
    [Multiline]
    public string test;

    public static LawManager Instance;

    public LawEvent currentDiscussedEvent;

    public List<LawStructure> currentLaws;
    [Serializable]
    public class LawEvent
    {
        [Serializable]
        public class UnfoldingLawEvent
        {
        public string eventName;
        [Multiline]
        public string eventDescription;
        [Multiline]
        public string lawOneDescription;
        [Multiline]
        public string lawTwoDescription;
        public float weight;
        public bool once;

        }
        public UnfoldingLawEvent unfold;
        public List<LawStructure> lawOne;
        public List<LawStructure> lawTwo;

    }
    public List<LawEvent> lawEvents;

    [Header("LawsRef")]
    public bool grassSpawners;
    [Serializable]
    public struct LawStructure
    {
        public Law law;
        [Header("If Applicable")]
        public EnemyBehavior.EnemyType enemy;
        public stats stat;
        public float value;
    }


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
            totalWeight += lawEvents[i].unfold.weight;
        }
        float _random = 0;
        _random = UnityEngine.Random.Range(0, totalWeight);
        for (int i = 0; i < lawEvents.Count; i++)
        {
            _random -= lawEvents[i].unfold.weight;
            if(_random <= 0)
            {

                if (lawEvents[i].unfold.once == true)
                {
                    LawEvent _lawEvent = lawEvents[i];
                    _lawEvent.unfold.weight = 0;
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
        eventName.text = _lawEvent.unfold.eventName;
        eventDescription.text = _lawEvent.unfold.eventDescription;
        lawOneDescription.text = _lawEvent.unfold.lawOneDescription;
        lawTwoDescription.text = _lawEvent.unfold.lawTwoDescription;
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


    public void ApplyAllSubLaws(List<LawStructure> _sublawsStructs)
    {
        for (int i = 0; i < _sublawsStructs.Capacity; i++)
        {
            ApplyLaw(_sublawsStructs[i]);
        }
    }

    public enum Law { IncreaseFamilyStat,
                     WalkOnGrass, CantWalkOnGrass,
                       UnlockTurret, IncreaseTurretCost, DecreaseTurretCost, 
                    IncreaseOverallDifficulty, IncreaseOverallSpeed, IncreaseDayTime,moneyIncrease,
                      IncreaseFreezeDuration, IncreaseSlowDuration, IncreasePoisonDamage, IncreaseSlowAmount}

    public void ApplyLaw(LawStructure lawstruct)
    {
        switch (lawstruct.law)
        {
            case Law.IncreaseFamilyStat:
                IncreaseMobFamilyStat(lawstruct.enemy, lawstruct.stat, lawstruct.value);
                break;         
            case Law.moneyIncrease:
                GameManager.Instance.moneyPerDay += lawstruct.value;
                break;
            case Law.IncreaseOverallDifficulty:
                GameManager.Instance.difficulty += lawstruct.value;
                break;
            case Law.IncreaseOverallSpeed:
                GameManager.Instance.gameSpeed += lawstruct.value;
                break;
            case Law.IncreaseDayTime:
                GameManager.Instance.defaultTime += lawstruct.value;
                break;
            case Law.IncreaseFreezeDuration:
                GameManager.Instance.freezeDuration += lawstruct.value;
                break;
            case Law.IncreaseSlowDuration:
                GameManager.Instance.slowDuration += lawstruct.value;
                break;
            case Law.IncreasePoisonDamage:
                GameManager.Instance.poisonDamage += lawstruct.value;
                break;
            case Law.IncreaseSlowAmount:
                GameManager.Instance.slowPercent += lawstruct.value;
                break;
            case Law.WalkOnGrass:
                grassSpawners = true;
                break;
            case Law.CantWalkOnGrass:
                grassSpawners = false;
                break;

            default:
                break;
        }
    }

    void IncreaseMobFamilyStat(EnemyBehavior.EnemyType _enemyType, stats stat, float value)
    {
        int _index = 0;

        switch (_enemyType)
        {
            case EnemyBehavior.EnemyType.Hippie:
                _index = 0;
                break;
            case EnemyBehavior.EnemyType.KKK:
                _index = 1;
                break;
            case EnemyBehavior.EnemyType.Biker:
                _index = 2;
                break;
            case EnemyBehavior.EnemyType.Nudist:
                _index = 3;
                break;
            case EnemyBehavior.EnemyType.Army:
                _index = 4;
                break;
            default:
                break;
        }

        GameManager.EnemyFamily _familyInst = GameManager.Instance.families[_index];
        float _familyScoreInst = GameManager.Instance.familiesScores[_index];

        switch (stat)
        {
            case stats.hp:
                _familyInst.prefab.GetComponent<EnemyBehavior>().hpAdded += value;
                
                break;
            case stats.speed:
                _familyInst.prefab.GetComponent<EnemyBehavior>().speedAdditioner += value;
                break;
            case stats.score:
                _familyScoreInst += value;
                break;
            default:
                break;
        }
        GameManager.Instance.familiesScores[_index] = _familyScoreInst;
        GameManager.Instance.families[_index] = _familyInst;


    }
}

public enum stats
{
    none,hp, speed, score
}



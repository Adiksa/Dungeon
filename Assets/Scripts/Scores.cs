using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UnityIntEvent : UnityEvent <int> { }

public class Scores : MonoBehaviour
{
    public static int mapScore;
    public static int totalScore;
    public static Scores instance;
    private IEnumerator corutine;
    public UnityIntEvent OnMapScoreChangedEvent;
    private void Awake()
    {

        if(instance==null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
        OnMapScoreChangedEvent = new UnityIntEvent();
    }
    void Start()
    {
        mapScore = 1000;
        totalScore = 0;
        corutine = MapScore();
        StartCoroutine(corutine);
    }

    public void Restart()
    {
        StopCoroutine(corutine);
        mapScore = 1000;
        StartCoroutine(corutine);
    }

    public void RestartGame()
    {
        StopCoroutine(corutine);
        totalScore = 0;
        mapScore = 1000;
        StartCoroutine(corutine);
    }

    private IEnumerator MapScore()
    {
        while (mapScore > 1)
        {
            mapScore = mapScore - 10;
            OnMapScoreChangedEvent.Invoke(mapScore);
            yield return new WaitForSeconds(1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

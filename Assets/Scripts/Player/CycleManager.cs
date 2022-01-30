using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CycleManager : MonoBehaviour
{
    public Text cycleText, timeText;

    public enum Phases { Day, Night }

    public Phases CurrentPhase;

    public float timePerPhase = 36f;

    public float currentTime = 0f;

    public int passedPhases;

    public string currentClockTime;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;

        passedPhases = (int)(currentTime / timePerPhase);

        CurrentPhase = passedPhases % 2 == 0 ? Phases.Day : Phases.Night;

        currentClockTime = GetCurrentClockTime();

        timeText.text = currentClockTime;

        cycleText.text = CurrentPhase.ToString();
    }

    private string GetCurrentClockTime()
    {
        var time = (6 + (currentTime / timePerPhase * 12)) % 24;
        var hours = (int)time;
        var minutes = (int)((time % 1) * 100 * 0.6);
        string hoursString = hours < 10 ? "0" + hours : hours.ToString();
        string minutesString = minutes < 10 ? "0" + minutes : minutes.ToString();
        return $"{hoursString}:{minutesString}";
    }
}

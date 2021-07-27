using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Dice : MonoBehaviour {
    [Header("Text Objects")]
    public TextMeshProUGUI outcome;
    public TextMeshProUGUI highscore;
    [Header("Input Field")]
    public TMP_Dropdown NumberValue;
    [Header("Best Time")]
    public TextMeshProUGUI bestTime;
    [Header("PrevNext Gameobject")]
    public GameObject PrevNext;
    [Header("GameOver Gameobject")]
    public GameObject GameOver;
    [Header("Timeout Gameobject")]
    public GameObject Timeout;
    [Header("Audio Clips")]
    public AudioSource myFx;
    public AudioClip LcFx;
    public AudioClip GoFx;
    public AudioClip ToFx;
    [Header("Background Audio")]
    public AudioSource BGM;
    [Header("Timer")]
    public float timeValue = 60;
    public TextMeshProUGUI timer;

    private bool TimerStarted;
    private bool flag;
    private bool functionCalled;

    private void Start() {
        Restart();
        SetBestTime();
        TimerStarted = false;
        flag = false;
        functionCalled = false;
    }

    private void SetHighestNumber() {
        if (PlayerPrefs.GetInt("Highscore" + (NumberValue.value + 1), 0) <= 9) {
            highscore.text = "0" + PlayerPrefs.GetInt("Highscore" + (NumberValue.value + 1), 0).ToString();
        } else {
            highscore.text = PlayerPrefs.GetInt("Highscore" + (NumberValue.value + 1), 0).ToString();
        }
    }

    private void SetBestTime() {
        bestTime.text = PlayerPrefs.GetInt("TimeBest", 60).ToString() + "s";
    }

    private void Update() {
        if (TimerStarted) {
            if (timeValue > 0) {
                timeValue -= Time.deltaTime;
            } else {
                DisplayTimeoutPanel();
            }
            DisplayTime(timeValue);
        }
    }

    private void DisplayTimeoutPanel() {
        if (functionCalled == false) {
            timeValue = 0;
            BGM.mute = true;
            myFx.PlayOneShot(ToFx);
            Timeout.SetActive(true);
        }
        functionCalled = true;
    }

    private void DisplayTime(float timeToDisplay) {
        if (timeToDisplay < 0) {
            timeToDisplay = 0;
        }

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timer.text = string.Format("{0:0}:{1:00}", minutes, seconds);
    }

    public void stopTimer() {
        Time.timeScale = 0;
    }

    public void resumeTimer() {
        Time.timeScale = 1;
    }

    public void RollDice() {
        TimerStarted = true;
        int Number = Random.Range(1, 6 * (NumberValue.value + 1) + 1);
        if (Number <= 9) {
            outcome.text = "0" + Number.ToString();
        } else {
            outcome.text = Number.ToString();
        }

        if (Number > PlayerPrefs.GetInt("Highscore" + (NumberValue.value + 1).ToString(), 0)) {
            PlayerPrefs.SetInt("Highscore" + (NumberValue.value + 1).ToString(), Number);
            if (Number <= 9) {
                highscore.text = "0" + Number.ToString();
            } else {
                highscore.text = Number.ToString();
            }
        }

        for (int i = 0; i < 9; i++) {
            if (PlayerPrefs.GetInt("Highscore" + (i + 1).ToString(), 0) == 6 * (i + 1)) {
                flag = true;
            } else {
                flag = false;
                break;
            }
        }

        if (Number == 6 * (NumberValue.value + 1)) {
            if (flag == false) {  //9 is the last item
                myFx.PlayOneShot(LcFx);
                stopTimer();
                PrevNext.SetActive(true);
            }
        }

        if (flag) {
            GameOver.SetActive(true);
            if (timeValue > 60 - PlayerPrefs.GetInt("TimeBest", 60)) {
                PlayerPrefs.SetInt("TimeBest", 60 - (int)timeValue);
                bestTime.text = PlayerPrefs.GetInt("TimeBest", 60).ToString() + "s";
            }
            stopTimer();
            BGM.mute = true;
            myFx.PlayOneShot(GoFx);
        }
    }

    public void OnChangeDice() {
        SetHighestNumber();
        outcome.text = "--";
    }

    public void PrevClicked() {
        NumberValue.value--;
        SetHighestNumber();
        outcome.text = "--";
    }

    public void NextClicked() {
        NumberValue.value++;
        SetHighestNumber();
        outcome.text = "--";
    }

    public void Restart() {
        timeValue = 60;
        timer.text = "1:00";
        TimerStarted = false;
        functionCalled = false;
        NumberValue.value = 0;
        outcome.text = "--";
        resumeTimer();
        ResetHighestNumber();
        Update();
    }

    public void ResetBest() {
        PlayerPrefs.DeleteKey("TimeBest");
        SetBestTime();

    }

    public void ResetHighestNumber() {
        for (int i = 0; i < 9; i++) { //9 is the last item
            PlayerPrefs.DeleteKey("Highscore" + (i + 1).ToString());
        }
        highscore.text = "00";
    }

    public void ExitGame() {
        Application.Quit();
    }
}

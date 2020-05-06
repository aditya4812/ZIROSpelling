using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {
    public Button submitButton;
    public bool buttonClicked;
    RobotSpelling robot;
    public float repeatTime;
    public AudioClip myclip;
    public static bool muted = false;
    public int pageCount;

    void Start () {
        pageCount = 1;
        this.gameObject.AddComponent<AudioSource> ();
        this.GetComponent<AudioSource> ().clip = myclip;
        RobotSpelling.highScore = PlayerPrefs.GetInt ("highScoreSave"); //remembers past high score
        repeatTime = 0;
        robot = GameObject.Find ("Main Camera").GetComponent<RobotSpelling> ();
        buttonClicked = false;
        if (name == "Submit Button") {
            submitButton.onClick.AddListener (incrementAttempt);

        }
        if (muted) {
            GameObject.Find ("Unmute").GetComponent<RawImage> ().color = new Color (1, 0, 0, 0); //clear
            GameObject.Find ("Mute").GetComponent<RawImage> ().color = new Color (1, 1, 1, 1); //white
        } else {
            GameObject.Find ("Mute").GetComponent<RawImage> ().color = new Color (1, 0, 0, 0); //clear
            GameObject.Find ("Unmute").GetComponent<RawImage> ().color = new Color (1, 1, 1, 1); //white
        }

    }

    void Update () {
        if (SceneManager.GetActiveScene ().name == "Homepage") {
            GameObject.Find ("High Score").GetComponent<Text> ().text = "High Score: " + RobotSpelling.highScore.ToString ();
        } //displays high score
        if (SceneManager.GetActiveScene ().name == "SampleScene" && robot.timeVal == true && repeatTime != 0) {
            robot.spinZiro (repeatTime);
        } //makes robot repeat prompt if replay button pressed

        //decides which page to show
        if (pageCount == 1) {
            GameObject.Find ("i1").GetComponent<RawImage> ().color = new Color (1, 1, 1, 1); //white
            GameObject.Find ("i2").GetComponent<RawImage> ().color = new Color (1, 1, 1, 0); //clear
            GameObject.Find ("i3").GetComponent<RawImage> ().color = new Color (1, 1, 1, 0); //clear
            GameObject.Find ("i4").GetComponent<RawImage> ().color = new Color (1, 1, 1, 0); //clear
        } else if (pageCount == 2) {
            GameObject.Find ("i2").GetComponent<RawImage> ().color = new Color (1, 1, 1, 1); //white
            GameObject.Find ("i1").GetComponent<RawImage> ().color = new Color (1, 1, 1, 0); //clear
            GameObject.Find ("i3").GetComponent<RawImage> ().color = new Color (1, 1, 1, 0); //clear
            GameObject.Find ("i4").GetComponent<RawImage> ().color = new Color (1, 1, 1, 0); //clear
        } else if (pageCount == 3) {
            GameObject.Find ("i3").GetComponent<RawImage> ().color = new Color (1, 1, 1, 1); //white
            GameObject.Find ("i2").GetComponent<RawImage> ().color = new Color (1, 1, 1, 0); //clear
            GameObject.Find ("i1").GetComponent<RawImage> ().color = new Color (1, 1, 1, 0); //clear
            GameObject.Find ("i4").GetComponent<RawImage> ().color = new Color (1, 1, 1, 0); //clear
        } else if (pageCount == 4) {
            GameObject.Find ("i4").GetComponent<RawImage> ().color = new Color (1, 1, 1, 1); //white
            GameObject.Find ("i2").GetComponent<RawImage> ().color = new Color (1, 1, 1, 0); //clear
            GameObject.Find ("i3").GetComponent<RawImage> ().color = new Color (1, 1, 1, 0); //clear
            GameObject.Find ("i1").GetComponent<RawImage> ().color = new Color (1, 1, 1, 0); //clear
        }
    }

    public void OnPointerDown (PointerEventData eventData) {
        //SYNTAX: add 'else if' for each additional button, and include what to do on click inside that block
        this.GetComponent<AudioSource> ().Play (); //plays click
        if (name == "Start") {
            SceneManager.LoadScene ("SampleScene");
        } else if (name == "Home") {
            SceneManager.LoadScene ("Homepage");
            if (RobotSpelling.scoreboard >= RobotSpelling.highScore) {
                RobotSpelling.highScore = RobotSpelling.scoreboard;
                //saves high score to memory to remember on startup
                PlayerPrefs.SetInt ("highScoreSave", RobotSpelling.highScore);
                PlayerPrefs.Save ();
            } //sets high score before exiting game
            RobotSpelling.scoreboard = 0;
        } else if (name == "Instruction Button") {
            SceneManager.LoadScene ("Instructions");

        } else if (name == "Submit Button") {
            buttonClicked = true;
        } else if (name == "Replay") {
            repeatTime = Time.timeSinceLevelLoad;
            robot.timeVal = true;
        } else if (name == "Settings Button") {
            SceneManager.LoadScene ("Settings");
        } else if (name == "Mute" || name == "Unmute") {
            AudioListener.volume = 1 - AudioListener.volume;
            muted = !muted;
            if (muted) {
                GameObject.Find ("Unmute").GetComponent<RawImage> ().color = new Color (1, 0, 0, 0); //clear
                GameObject.Find ("Mute").GetComponent<RawImage> ().color = new Color (1, 1, 1, 1); //white
            } else {
                GameObject.Find ("Mute").GetComponent<RawImage> ().color = new Color (1, 0, 0, 0); //clear
                GameObject.Find ("Unmute").GetComponent<RawImage> ().color = new Color (1, 1, 1, 1); //white
            }
        } else if (name == "Reset Score") {
            PlayerPrefs.SetInt ("highScoreSave", 0);
            PlayerPrefs.Save ();
            RobotSpelling.highScore = PlayerPrefs.GetInt ("highScoreSave"); //remembers past high score
        } else if (name == "Previous") {
            if (GameObject.Find ("Next").GetComponent<ButtonHandler> ().pageCount >= 2) {
                GameObject.Find ("Next").GetComponent<ButtonHandler> ().pageCount--;

            }
        } else if (name == "Next") {
            if (pageCount <= 3) {
                pageCount++;
            }
        }
        StartCoroutine (WaitTime (0.5f));

    }

    public void OnPointerUp (PointerEventData eventData) {
        if (name == "Submit Button") {
            buttonClicked = false;
        }
    }
    public void incrementAttempt () {

        if (robot.winStatus == 1 && (robot.inputSize) != 0) {
            RobotSpelling.scoreboard += 100 * robot.answerChar.Count; //100 per letter 
        } // adds to scoreboard if attempt is correct
        else if (robot.inputSize != 0) {
            RobotSpelling.scoreboard -= 100; //-100 for each incorrect attempt
        }

        if ((robot.inputSize) >= 1) {
            (robot.attemptNum) ++;
        } //makes sure user only gets 3 tries
        (robot.inputSize) = 0;

    }

    IEnumerator WaitTime (float seconds) {

        yield return new WaitForSeconds (seconds);

    }

}
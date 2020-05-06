using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RobotSpelling : MonoBehaviour {

    RawImageTexture pic1;
    RawImageTexture pic2;
    RawImageTexture pic3;
    private int m_T = 0;
    private string s_T = "";
    private string s_M = "";
    public bool timeVal; //clockwise spinning enabled
    public float spin; //number of spins
    public bool congrats; //cues celebration
    public bool dissapoint; //cues sad react
    public int iterationNum; //increases variation in reactions
    public string chosenImage; //object id of pic
    public string imageAnswer; // correct answer of pic
    public List<char> answerChar = new List<char> (); // answer split into characters
    public List<char> inputChar = new List<char> (); //array of user input on console CHECK IF ANSWERCHAR = INPUT CHAR TO WIN GAME
    public List<int> randomList = new List<int> (); //holds random object id's of correct char
    public int charLocations; //random location of correct char
    public string chosenLetter; //object id of letter
    public bool populateSwitch; //makes sure pictures and letters are only generated once per letter
    public string lettersRemaining; //string that displays letters has left to click
    public float xPos; //x position of clicked letters
    public int xLocation; // relative position of last clicked letter 
    public int winStatus; // 1 = win, 2 = lose
    public int attemptNum; // makes sure user stays within 3 attempts 
    public int inputSize = 0; // tracks how many letters have been selected
    public bool reactionStart; //cues reaction to start
    public static int scoreboard = 0;
    public static int highScore = 0;
    public bool clickEnabled; //allows clicking of letters
    public bool reactOnce; //makes sure reaction doesnt repeat every frame

    // Start is called before the first frame update
    void Start () {
        pic1 = GameObject.Find ("pic1").GetComponent<RawImageTexture> ();
        pic2 = GameObject.Find ("pic2").GetComponent<RawImageTexture> ();
        pic3 = GameObject.Find ("pic3").GetComponent<RawImageTexture> ();
        spin = Random.Range (1, 4); //1-3
        chosenImage = "pic" + spin.ToString ();
        timeVal = true;
        congrats = false;
        dissapoint = false;
        iterationNum = 1;
        populateSwitch = true;
        winStatus = 0;
        attemptNum = 0;
        //inputSize = 0;
        reactionStart = false;
        clickEnabled = true;
        for (int i = 0; i < 100; i++) {
            stopZiro ();
        }
        reactOnce = true;

    }

    // Update is called once per frame
    void Update () {
        GameObject.Find ("Scoreboard").GetComponent<Text> ().text = "Score: " + scoreboard.ToString ();
        GameObject.Find ("Attempt").GetComponent<Text> ().text = "Attempts Remaining: " + (3 - attemptNum).ToString ();

        if (timeVal == true && GameObject.Find ("Replay").GetComponent<ButtonHandler> ().repeatTime == 0) {

            if (populateSwitch == true) {
                while (pic1.picName == pic2.picName || pic1.picName == pic3.picName || pic2.picName == pic3.picName) {
                    pic2.changeImage ();
                    pic3.changeImage ();

                } // loop makes sure that each picture is different

                imageAnswer = GameObject.Find (chosenImage).GetComponent<RawImageTexture> ().picName;
                lettersRemaining = imageAnswer;
                answerChar.AddRange (imageAnswer);
                xPos = (Screen.width * 0.5f) - 0.5f * ((145 * Screen.width / 2436) * answerChar.Count - 104);
                xLocation = -1;

                //puts required letters at random locations
                for (int i = 0; i < answerChar.Count; i++) {
                    randLocation (); //adds a random, unique location to randomList
                    chosenLetter = "letter" + randomList[i].ToString (); //assigns location to letter ID
                    GameObject.Find (chosenLetter).GetComponent<LetterTexture> ().changeTo (answerChar[i]); //changes letter at given ID to letter from answerChar
                    Debug.Log (chosenLetter);

                }

                //if else block makes sure that the first letter from 2 incorrect pictures appears in word bank
                if (chosenImage == "pic1") {
                    randLocation ();
                    chosenLetter = "letter" + randomList[randomList.Count - 1].ToString (); //assigns location to letter ID
                    Debug.Log (chosenLetter);
                    GameObject.Find (chosenLetter).GetComponent<LetterTexture> ().changeTo (GameObject.Find ("pic2").GetComponent<RawImageTexture> ().picName[0]);
                    randLocation ();
                    chosenLetter = "letter" + randomList[randomList.Count - 1].ToString (); //assigns location to letter ID
                    Debug.Log (chosenLetter);
                    GameObject.Find (chosenLetter).GetComponent<LetterTexture> ().changeTo (GameObject.Find ("pic3").GetComponent<RawImageTexture> ().picName[0]);
                } else if (chosenImage == "pic2") {
                    randLocation ();
                    chosenLetter = "letter" + randomList[randomList.Count - 1].ToString (); //assigns location to letter ID
                    Debug.Log (chosenLetter);
                    GameObject.Find (chosenLetter).GetComponent<LetterTexture> ().changeTo (GameObject.Find ("pic3").GetComponent<RawImageTexture> ().picName[0]);
                    randLocation ();
                    chosenLetter = "letter" + randomList[randomList.Count - 1].ToString (); //assigns location to letter ID
                    Debug.Log (chosenLetter);
                    GameObject.Find (chosenLetter).GetComponent<LetterTexture> ().changeTo (GameObject.Find ("pic1").GetComponent<RawImageTexture> ().picName[0]);
                } else if (chosenImage == "pic3") {
                    randLocation ();
                    chosenLetter = "letter" + randomList[randomList.Count - 1].ToString (); //assigns location to letter ID
                    Debug.Log (chosenLetter);
                    GameObject.Find (chosenLetter).GetComponent<LetterTexture> ().changeTo (GameObject.Find ("pic2").GetComponent<RawImageTexture> ().picName[0]);
                    randLocation ();
                    chosenLetter = "letter" + randomList[randomList.Count - 1].ToString (); //assigns location to letter ID
                    Debug.Log (chosenLetter);
                    GameObject.Find (chosenLetter).GetComponent<LetterTexture> ().changeTo (GameObject.Find ("pic1").GetComponent<RawImageTexture> ().picName[0]);
                }

                populateSwitch = false; //makes sure this process only happens once
            } //populates all letter options while dispersing answerChar at random locations

            spinZiro (0);
            Debug.Log (imageAnswer);
        } else {
            if (reactionStart) {
                if (reactOnce) {
                    //Debug.Log (">>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>");
                    stopZiro (); // stops spinning
                    if (winStatus == 1) {
                        congrats = true;
                    } else if (winStatus == 2) {
                        dissapoint = true;
                    }
                    reactionZiro ();
                    GameObject.Find ("Attempt").GetComponent<Text> ().text = ""; //erases attempt number text block
                    reactOnce = false;
                }

                //wait for 1.5 seconds and reset scene
                StartCoroutine (WaitTime (1.5f));
            }
        } //each if-else block simulates one page of the game

    }

    void randLocation () { // List of length answerChar.Count containing random letter locations
        charLocations = Random.Range (1, 16); //1-15
        while (randomList.Contains (charLocations)) {
            charLocations = Random.Range (1, 16); //1-15
            if (!randomList.Contains (charLocations)) {
                break;
            }
        }
        randomList.Add (charLocations);

    }

    void stopZiro () {
        m_T = 0;
        s_M = "[0,0,4,3,0,0,0]";
        s_T += m_T.ToString ();
        for (int ii = 0; ii < 6; ii++) {
            s_T += ",";
            s_T += m_T.ToString ();
        }

        string mssg = "{\"pType\":7,\"m_T\":[" + s_T + "],\"m_M\":" + s_M + "}";
        gameObject.GetComponent<UDPClient> ().SendValue (mssg);
        //timeVal = !(timeVal);
        s_T = "";

    }
    public void spinZiro (float x) { //x resets clock to 0 when method is called
        Debug.Log ((Time.timeSinceLevelLoad - x) + "..." + spin.ToString ());
        timeVal = true;
        if ((Time.timeSinceLevelLoad - x) >= 3) {
            timeVal = false;
        }
        if (timeVal == false) {
            m_T = 0;
            s_M = "[0,0,4,3,0,0,0]";
        } else if (timeVal == true) {
            if ((Time.timeSinceLevelLoad - x) <= 2) {
                if (spin == 1) { //left arm (green)
                    m_T = 60;
                    s_M = "[0,0,0,3,0,0,0]";
                } else if (spin == 2) { //both arms
                    m_T = 60;
                    s_M = "[0,0,4,3,0,0,0]";
                } else if (spin == 3) { //right arm (blue)
                    m_T = 60;
                    s_M = "[0,0,4,0,0,0,0]";
                }
            } else {
                m_T = 0;
                s_M = "[0,0,4,3,0,0,0]";
            }

        }
        s_T += m_T.ToString ();
        for (int ii = 0; ii < 6; ii++) {
            s_T += ",";
            s_T += m_T.ToString ();
        }

        string mssg = "{\"pType\":7,\"m_T\":[" + s_T + "],\"m_M\":" + s_M + "}";
        gameObject.GetComponent<UDPClient> ().SendValue (mssg);
        s_T = "";
    }
    void reactionZiro () {
        if (congrats == true) {
            for (int i = 0; i < 200; i++) { //turn left, arms up
                s_M = "[1,1,4,3,0,0]";
                s_T = "[40,40,40,40,40,40]";
                string mssg = "{\"pType\":7,\"m_T\":" + s_T + ",\"m_M\":" + s_M + "}";
                gameObject.GetComponent<UDPClient> ().SendValue (mssg);
                s_T = "";
            }
            for (int i = 0; i < 200; i++) { //turn right, arms down
                s_M = "[2,2,3,4,0,0]";
                s_T = "[40,40,40,40,40,40]";
                string mssg = "{\"pType\":7,\"m_T\":" + s_T + ",\"m_M\":" + s_M + "}";
                gameObject.GetComponent<UDPClient> ().SendValue (mssg);
                s_T = "";
            }
            for (int i = 0; i < 50; i++) { //reset
                stopZiro ();
            }
            congrats = false;

        } else if (dissapoint == true) {

            for (int i = 0; i < 400; i++) { // arms: /\
                s_M = "[0,0,3,4,0,0,0]";
                s_T = "[40,40,60,60,40,40]";
                string mssg = "{\"pType\":7,\"m_T\":" + s_T + ",\"m_M\":" + s_M + "}";
                gameObject.GetComponent<UDPClient> ().SendValue (mssg);
                s_T = "";
            }
            for (int i = 0; i < 50; i++) { //reset
                stopZiro ();
            }

            dissapoint = false;
        }

    }

    IEnumerator WaitTime (float seconds) {

        yield return new WaitForSeconds (seconds);
        inputSize = 0;
        reactionStart = false;
        SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);

    }

}
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LetterTexture : MonoBehaviour, IPointerClickHandler {
    RawImage l_RawImage;
    RobotSpelling robot;
    ButtonHandler submit;
    public Texture[] l_Texture;
    public string letterName;
    public bool correctLetter; //marks whether the letter selected is correct (if it's found in the answer)
    private Vector3 originPosition;
    public Vector3 originalLocation;
    private Quaternion originRotation;
    public float shake_decay = 1f;
    public float shake_intensity = 0.05f;
    private float temp_shake_intensity = 0;
    public int rand;
    public int index;
    public bool selectedLetter; // marks whether a letter has been selected
    public string inputCheck;
    public bool inputChecker; //bool to generate the input string
    public bool shake;
    public int letterPosition;
    public float startWidth;
    public AudioClip unclickSound;

    void Start () {
        //OnMouseDown ();
        robot = GameObject.Find ("Main Camera").GetComponent<RobotSpelling> ();
        submit = GameObject.Find ("Submit Button").GetComponent<ButtonHandler> ();
        correctLetter = false;
        selectedLetter = false;
        inputChecker = true;
        inputCheck = "";
        changeImage ();
        originalLocation = transform.position;
        shake = false;
        GameObject.Find ("Retry").GetComponent<RawImage> ().color = new Color (1, 0, 0, 0); //clear
        GameObject.Find ("RetryText").GetComponent<Text> ().color = new Color (1, 0, 0, 0); //clear
        letterPosition = -1;
        startWidth = Screen.width;
        this.gameObject.AddComponent<AudioSource> ();
        this.GetComponent<AudioSource> ().clip = unclickSound;

    }
    void Update () {
        if ((submit.buttonClicked) == true && selectedLetter) {

            if (inputChecker == true) {
                for (int i = 0; i < (robot.inputChar).Count; i++) {
                    inputCheck += (robot.inputChar) [i].ToString ();
                }
                inputChecker = false;

            } //generates string from user input List

            if (inputCheck == (robot.imageAnswer)) { //IF USER INPUT IS CORRECT
                GetComponent<RawImage> ().color = new Color (0, 1, 0, 1); //green
                GameObject.Find ("Retry").GetComponent<RawImage> ().color = new Color (0, 1, 0, 0.7f); //green
                GameObject.Find ("RetryText").GetComponent<Text> ().color = new Color (0, 0, 0, 1); //black
                GameObject.Find ("RetryText").GetComponent<Text> ().text = "Correct!!!";
                (robot.winStatus) = 1;
                robot.reactionStart = true;
            } else { //IF USER INPUT IS INCORRECT
                robot.clickEnabled = false; //doesnt let user click any other letters until letters return to word bank
                GetComponent<RawImage> ().color = new Color (1, 0, 0, 1); //red
                if ((robot.attemptNum) <= 1) {
                    GameObject.Find ("Retry").GetComponent<RawImage> ().color = new Color (1, 0, 0, 0.7f); //red
                    GameObject.Find ("RetryText").GetComponent<Text> ().color = new Color (0, 0, 0, 1); //black
                    StartCoroutine (WaitTime (3));
                } else {
                    GameObject.Find ("Retry").GetComponent<RawImage> ().color = new Color (1, 0, 0, 0.7f); //red
                    GameObject.Find ("RetryText").GetComponent<Text> ().color = new Color (0, 0, 0, 1); //black
                    GameObject.Find ("RetryText").GetComponent<Text> ().fontSize = 40;
                    GameObject.Find ("RetryText").GetComponent<Text> ().text = "Sorry, the answer is:  <color=#ffffffff>" + robot.imageAnswer.ToUpper () + "</color>";
                    originPosition = transform.position;
                    (robot.winStatus) = 2;
                    robot.reactionStart = true;
                } //letters go back to word bank 3 times, then game stops after 3 wrong attempts

            }

            if (temp_shake_intensity > 0 && !correctLetter) {
                shake = true;

            } //tells Update() to shake incorrect choices

        } //only shakes once button is clicked

        if (shake && temp_shake_intensity > 0 && !correctLetter && transform.position.y != GameObject.Find ("shadow letter").transform.position.y) {

            transform.rotation = new Quaternion (
                originRotation.x + Random.Range (-temp_shake_intensity, temp_shake_intensity) * 5f,
                originRotation.y + Random.Range (-temp_shake_intensity, temp_shake_intensity) * 5f,
                originRotation.z + Random.Range (-temp_shake_intensity, temp_shake_intensity) * 5f,
                originRotation.w + Random.Range (-temp_shake_intensity, temp_shake_intensity) * 5f);
            temp_shake_intensity -= shake_decay;
        } else {
            shake = false;
            //stops shaking and sets letters upright in word bank
            transform.rotation = new Quaternion (GameObject.Find ("shadow letter").transform.rotation.x, GameObject.Find ("shadow letter").transform.rotation.y, GameObject.Find ("shadow letter").transform.rotation.z, GameObject.Find ("shadow letter").transform.rotation.w);
        } //shakes incorrect choices

    }

    public void changeImage () {
        rand = Random.Range (0, 26); //0-25
        //Fetch the RawImage component from the GameObject
        l_RawImage = GetComponent<RawImage> ();
        //Change the Texture to be the one you define in the Inspector
        l_RawImage.texture = l_Texture[rand];
        letterName = (l_RawImage.texture).ToString ().Replace (" (UnityEngine.Texture2D)", "");

    }

    public void changeTo (char c) { //changes image to specific texture (parameter is letter name)
        index = 0;

        if (c == 'a') {
            index = 0;
        } else if (c == 'b') {
            index = 1;

        } else if (c == 'c') {
            index = 2;

        } else if (c == 'd') {
            index = 3;

        } else if (c == 'e') {
            index = 4;

        } else if (c == 'f') {
            index = 5;

        } else if (c == 'g') {
            index = 6;

        } else if (c == 'h') {
            index = 7;

        } else if (c == 'i') {
            index = 8;

        } else if (c == 'j') {
            index = 9;

        } else if (c == 'k') {
            index = 10;

        } else if (c == 'l') {
            index = 11;

        } else if (c == 'm') {
            index = 12;

        } else if (c == 'n') {
            index = 13;

        } else if (c == 'o') {
            index = 14;

        } else if (c == 'p') {
            index = 15;

        } else if (c == 'q') {
            index = 16;

        } else if (c == 'r') {
            index = 17;

        } else if (c == 's') {
            index = 18;

        } else if (c == 't') {
            index = 19;

        } else if (c == 'u') {
            index = 20;

        } else if (c == 'v') {
            index = 21;

        } else if (c == 'w') {
            index = 22;

        } else if (c == 'x') {
            index = 23;

        } else if (c == 'y') {
            index = 24;

        } else if (c == 'z') {
            index = 25;

        }
        l_RawImage = GetComponent<RawImage> ();
        //Change the Texture to be the one you define in the Inspector
        l_RawImage.texture = l_Texture[index];
        letterName = (l_RawImage.texture).ToString ().Replace (" (UnityEngine.Texture2D)", "");
        correctLetter = true;
    }

    public void OnPointerClick (PointerEventData eventData) {
        if (robot.clickEnabled) { //only if click function is enabled

            //makes sure more letters than answer length arent selected
            if ((robot.inputChar).Count <= (robot.answerChar).Count) {

                //handles backspace for last clicked letter
                if (selectedLetter && robot.xLocation == letterPosition) {
                    if (correctLetter) {
                        correctLetter = false;
                        (robot.lettersRemaining) += letterName;
                    } else {
                        correctLetter = true;
                        temp_shake_intensity = 0;
                    }
                    selectedLetter = false;
                    robot.inputChar.RemoveAt (robot.inputChar.Count - 1);
                    robot.inputSize--;
                    this.GetComponent<AudioSource> ().Play ();
                    Vector3 moveBack = new Vector3 (originalLocation.x * Screen.width / startWidth, GameObject.Find ("shadow letter").transform.position.y, originalLocation.z);
                    StartCoroutine (Move_Routine (this.transform, transform.position, moveBack));
                    GetComponent<RawImage> ().color = new Color (1, 1, 1, 1);
                    robot.xPos -= (145 * Screen.width / startWidth);
                    robot.xLocation--;
                    letterPosition = -1;

                }
                //handles entering of unclicked letters
                else if (!selectedLetter && (robot.inputChar).Count + 1 <= (robot.answerChar).Count) {

                    //marks letter as selected
                    selectedLetter = true;
                    //adds user selection to the input array, later used to verify correctness
                    (robot.inputChar).Add (letterName.ToCharArray () [0]);
                    (robot.inputSize) ++;

                    //plays sound
                    GameObject.Find ("Submit Button").GetComponent<AudioSource> ().Play ();

                    //moves inputs to console & increments x Position
                    (robot.xPos) = (Screen.width * 0.5f) - 0.5f * ((145 * Screen.width / startWidth) * robot.answerChar.Count - (145 * Screen.width / startWidth)) + (145 * Screen.width / startWidth) * (robot.xLocation + 1);
                    Vector3 moveConsole = new Vector3 ((robot.xPos), Screen.height * 0.5f, transform.position.z);
                    StartCoroutine (Move_Routine (this.transform, transform.position, moveConsole));

                    //changes selected letters to yellow
                    GetComponent<RawImage> ().color = new Color (1, 0.807f, 0, 1);
                    //updates values for position of letter relative to other selected letters
                    (robot.xLocation) ++;
                    letterPosition = robot.xLocation;

                    //removes clicked correct letters from temp string to avoid duplicates
                    if ((robot.lettersRemaining).Contains (letterName) && (robot.answerChar) [(robot.xLocation)] == letterName.ToCharArray () [0]) {
                        correctLetter = true;
                        (robot.lettersRemaining) = (robot.lettersRemaining).Remove (((robot.lettersRemaining).IndexOf (letterName)), 1);
                    }

                    //prepares incorrect letter choices for shaking 
                    else {
                        correctLetter = false;
                        originPosition = transform.position;
                        originRotation = transform.rotation;
                        temp_shake_intensity = shake_intensity;
                    }
                }
            }
        }

    }

    public void resetLetter () {
        Vector3 moveBack = new Vector3 (originalLocation.x * Screen.width / startWidth, GameObject.Find ("shadow letter").transform.position.y, originalLocation.z);
        StartCoroutine (Move_Routine (this.transform, transform.position, moveBack));
        GetComponent<RawImage> ().color = new Color (1, 1, 1, 1); //white
        correctLetter = false;
        selectedLetter = false;
        inputChecker = true;
        inputCheck = "";
        originPosition = transform.position;
        (robot.inputChar).Clear ();
        (robot.xPos) -= (145 * Screen.width / startWidth) * ((robot.xLocation) + 1);
        (robot.xLocation) = -1;
        (robot.lettersRemaining) = (robot.imageAnswer);
        GameObject.Find ("Retry").GetComponent<RawImage> ().color = new Color (1, 0, 0, 0); //clear
        GameObject.Find ("RetryText").GetComponent<Text> ().color = new Color (1, 0, 0, 0); //clear
        robot.clickEnabled = true; //enables clicking
    }

    IEnumerator WaitTime (float seconds) {

        yield return new WaitForSeconds (seconds);
        resetLetter ();

    }
    IEnumerator Move_Routine (Transform transform, Vector3 from, Vector3 to) {
        float step = (4) * Time.deltaTime;
        float t = 0f;
        while (t < 1f) {
            t += step;
            transform.position = Vector3.Lerp (from, to, Mathf.SmoothStep (0f, 1f, t));
            yield return null;
        }
    }
}
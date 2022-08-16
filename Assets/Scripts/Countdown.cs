using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Countdown : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI countText;
    public PlayerMovement[] players;

    public int countdownTime;

    public string gameText;
    
    public bool roundIsOver;
    public bool canPlayerMove;


    void Awake()
    {
        players = FindObjectsOfType<PlayerMovement>();
    }

    /*
     * Sets the countdown timer 
     */
    void Start()
    {
        countdownTime = 3;
        gameText = "Go!";
        roundIsOver = false;
        //Debug.Log("CountdownTime from START is: " + countdownTime);
        StartCoroutine(RoundCounter());
    }

    /*
     * Count down for the round to start
     */
    IEnumerator StartCounter()
    {
        //Debug.Log("Countdown Started!!!!! " + "CountDown time is: " + countdownTime);
        while (countdownTime > 0)
        {
            countText.text = countdownTime.ToString();
            countdownTime--;
            yield return new WaitForSecondsRealtime(1);
            //Debug.Log("STARTCOUNTER: " + countdownTime);
        }

        countText.text = gameText;

        canPlayerMove = true;

        StartCoroutine(PlayersCanMove());

        yield return null;
    }

    /*
     * Count down for the round time limit 
     */
    IEnumerator RoundCounter()
    {
       yield return StartCounter();

        gameText = "Round Over";
        countdownTime = 5;

        while (countdownTime > 0)
        {
            countText.text = countdownTime.ToString();
            countdownTime--;
            yield return new WaitForSecondsRealtime(1);
            //Debug.Log("ROUNDCOUNTER: " + countdownTime);
        }

       StartCoroutine(EndRound());
    }

    /*
     * Starts once the round start counter finishes 
     * This method sends a broadcast to check if
     * player can move or not
     * Notifies the GameManager that the round is over
     */
    private IEnumerator PlayersCanMove()
    {
        yield return new WaitForSecondsRealtime(1);

        countText.text = null;
        gameObject.BroadcastMessage("EnablePlayerMovement", canPlayerMove);

    }

    private IEnumerator EndRound()
    {
        yield return new WaitForSecondsRealtime(1);
        gameObject.BroadcastMessage("RoundEnded", true);
    }

    /*
     * Starts this script over again
     * Resets the player movement to false
     */
    public void Reset()
    {
        Start();
        canPlayerMove = true;
        gameObject.BroadcastMessage("DisablePlayerMovement", canPlayerMove);
    }
}
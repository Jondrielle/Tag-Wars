using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class GameManager : MonoBehaviour
{
    //Private Variables
    private float distanceBetweenPlayers;
    public PlayerManager player1;
    public PlayerManager player2;

    //Public Variables
    public Cinemachine.CinemachineVirtualCamera virtualCam;
    public TextMeshProUGUI roundDisplay;
    public PlayerManager winnerPlayer;
    public TextMeshProUGUI countText;
    public PlayerManager[] players;
    public Spawner spawnerScript;
    public AudioSource timerSound;
    public TilemapGenerator map;
    public BannerVisual banner;
    public RawImage tagPrompt;
    public GameObject endMenu;
    public Camera mainCamera;
    public Text winner;
    public GameObject bush;

    //public Countdown counter;
    public string roundWinner;
    public string gameWinner;
    public string gameText;

    public float countdownTime;
    public float maxWins;
    public float round;

    /*
     *This sets the round to 1 everytime a new game is started
     */
    void Awake()
    {
        round = 1;
        players = FindObjectsOfType<PlayerManager>();
        SetPlayers();
    }

    /*
     * Sets up the game
     */

    void Start()
    {
        SpawnGroundObjects();

        spawnerScript = GetComponent<Spawner>();

        mainCamera.gameObject.SetActive(false);
        tagPrompt.gameObject.SetActive(false);
        endMenu.gameObject.SetActive(false);

        //banner.Proc(this);
        countdownTime = 3;
        gameText = "Go!";
        maxWins = 0;
        //counter = GetComponentInChildren<Countdown>();

        roundDisplay.text = "Round " + round;
        roundWinner = null;
        winner.text = "";

        RandomlySetTagger();
        StartCoroutine(GameLoop());

    }

    void Update()
    {
        //Get distance between both players
        distanceBetweenPlayers = Vector3.Distance(player1.transform.position, player2.transform.position);

        //if the distance is equal to or less than 3 then 
        //display UI tag prompt 
        if (distanceBetweenPlayers <= 3)
        {
            PlayerManager tagger = FindTagger();
            if (tagger.playerCollision.GetOpponentPlayer().GetIsImmune() == false)
            {
                tagPrompt.gameObject.SetActive(true);
                tagger.playerCollision.SetCanTagHappen(true);
            }
            // tagger.SetPlayerCameraSize(tagger.GetPlayerCameraSize()/2);
        }
        else
        {
            tagPrompt.gameObject.SetActive(false);
            PlayerManager tagger = FindTagger();
            tagger.playerCollision.SetCanTagHappen(false);
        }
    }

    void SpawnGroundObjects()
    {
        float bushCount = 0;

        while (bushCount < 50)
        {
            GameObject bushClone = Instantiate(bush, new Vector3(SpawnXLocation(), SpawnYLocation(), (float)-.21), Quaternion.identity);
            bushCount++;
        }
    }

    /*
     * 
     * Runs Game Loop
     * 
     */
    public IEnumerator GameLoop()
    {
        yield return StartCoroutine(SetUpRound());

        countdownTime = 3;
        gameText = "Go!";
        yield return StartCoroutine(StartRound());

        countdownTime = 30;
        gameText = "Round Over";
        //spawnerScript.enabled = true;
        yield return StartCoroutine(StartRound());

        yield return StartCoroutine(ManageEndOfRound());

        yield return new WaitForSecondsRealtime(1);
    }

    //Disable all players movement 
    public IEnumerator SetUpRound()
    {
        foreach (PlayerManager player in players)
        {
            player.DisableMovement();
        }
        yield return new WaitForSecondsRealtime(1);
    }

    //Enable all players movement 
    public IEnumerator StartRound()
    {
        while (countdownTime > 0)
        {
            countText.text = countdownTime.ToString();
            countdownTime--;
            timerSound.Play();
            yield return new WaitForSecondsRealtime(1);
            //Debug.Log("STARTCOUNTER: " + countdownTime);
        }

        countText.text = gameText;
        EnableMovement();

        yield return new WaitForSecondsRealtime(1);
    }

    /*
     * 
     * If the round is less than 3 and
     * ended then go to EndRound
     * else then ends the entire game
     * 
     */
    public IEnumerator ManageEndOfRound()
    {
        StopAllCoroutines();
        if (round < 3)
            yield return StartCoroutine(EndRound());
        else
        {
            tagPrompt.gameObject.SetActive(false);
            //print("Turned off tag prompt");
            countText.text = "Game Over";
            DisplayRoundWinner();
            FindGameWinner();
            EndEntireGame();
        }
    }

    /*
     * 
     * End the round and increment the round 
     * Display the roundwinner
     * Reset values and game loop
     * 
     */
    public IEnumerator EndRound()
    {
        spawnerScript.enabled = false;
        DisableMovement();
        round++;
        float currentRound = round--;
        roundDisplay.text = "Round " + currentRound;
        DisplayRoundWinner();

        yield return StartCoroutine(ResetValues());

        StopAllCoroutines();
        StartCoroutine(GameLoop());

        //yield return new WaitForSecondsRealtime(1);
    }

    /*
     * 
     * Randomly sets Hider and Tagger 
     * only once before the GameLoop begins
     * 
     */
    public void RandomlySetTagger()
    {
        int randomNewTagger = UnityEngine.Random.Range(0, players.Length);
        PlayerManager newTagger = players[randomNewTagger];
        PlayerManager newHider;
        foreach (PlayerManager player in players)
        {
            if (newTagger.Equals(player))
            {
                newTagger.UpdateLabel("Tagger");
                newTagger.SetAbilitySlider(false);
            }
            else
            {
                newHider = player;
                newHider.UpdateLabel("Hider");
                newHider.SetAbilitySlider(true);
            }
        }

    }

    /*
     * Loops through all of the characters and 
     * change the characters roles
     */
    public void SwapTagger()
    {
        //print("Inside of tagger swap");
        foreach (PlayerManager player in players)
        {
            if (player.gameObject.tag == "Tagger")
                player.gameObject.tag = "Hider";
            else
                player.gameObject.tag = "Tagger";

            player.UpdateLabel();
            player.PlayerSpawnLocation();
            player.SetAbilitySlider();
        }
    }

    /*
     * 
     * Reset roundWinner
     * Reset new Tagger
     * Reset winner text to empty
     * 
     */
    private IEnumerator ResetValues()
    {
        yield return new WaitForSecondsRealtime(2);
        winner.text = "";
        roundWinner = null;
        SwapTagger();
    }

    //Displays each rounds winner     
    void DisplayRoundWinner()
    {
        if (roundWinner == null)
        {
            foreach (PlayerManager player in players)
            {
                if (player.gameObject.tag == "Hider")
                {
                    //print("The hider is: " + player.name);
                    player.SetWinCount();
                    //print(player.name + " :The players win count is now: " + player.playerWins);
                    roundWinner = player.gameObject.name + " won Round " + round;
                    winner.text = roundWinner;
                    break;
                }
            }
        }
        else
        {
            winner.text = roundWinner;
        }

    }

    //Displays the winner of the game
    void DisplayGameWinner()
    {
        winner.text = "Winner of game is " + gameWinner;
    }

    //Find the Games winner 
    void FindGameWinner()
    {
        foreach (PlayerManager player in players)
        {
            if (maxWins < player.GetWinCount())
            {
                maxWins = player.GetWinCount();
                gameWinner = player.gameObject.name;
                //print("MAXWINS: " + maxWins);
                //print("GAMEWINNER: " + gameWinner);
            }
            //print("MAXWINS2: " + maxWins);
            //print("GAMEWINNER2: " + gameWinner);
        }
        DisplayGameWinner();
    }

    //Enable all players movement
    public void EnableMovement()
    {
        foreach (PlayerManager player in players)
        {
            player.EnableMovement();
        }
    }

    //Disable all players movement
    public void DisableMovement()
    {
        foreach (PlayerManager player in players)
        {
            player.DisableMovement();
        }
    }

    /*
    * 
    * called when the PlayerColliderController registers that someone was tagged
    * alerts the BannerVisual to proc
    * 
    */
    [ContextMenu("Proc Banner Movement")]
    public void SomeoneWasTagged()
    {
        banner.Proc(this);
    }

    //Find the tagger for the round
    public PlayerManager FindTagger()
    {
        PlayerManager tagger;
        if (player1.tag == "Tagger")
            tagger = player1;
        else
            tagger = player2;

        return tagger;
    }

    //End the entire game and zoom on winner
    public void EndEntireGame()
    {
        //print("*********************over***********************");
        //tagPrompt.gameObject.SetActive(false);
        foreach (PlayerManager player in players)
        {
            PlayerMovement movement = player.gameObject.GetComponent<PlayerMovement>();
            movement.enabled = false;
            player.GetComponent<Animator>().enabled = false;
            if (winner.text.Contains(player.gameObject.name))
            {
                winnerPlayer = player;
                //AudioListener listener = player.gameObject.GetComponent<AudioListener>();
                mainCamera.gameObject.SetActive(true);
                mainCamera.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, -1);
                //listener.enabled = false;
            }
        }
        virtualCam.Follow = winnerPlayer.gameObject.transform;
        virtualCam.LookAt = winnerPlayer.gameObject.transform;
        spawnerScript.enabled = false;
        endMenu.gameObject.SetActive(true);
    }

    //Loop and set the players to a variable
    public void SetPlayers()
    {
        foreach (PlayerManager player in players)
        {
            if (player1 == null)
                player1 = player;
            else
                player2 = player;
        }
    }

    //Set the winner of the round
    public void SetRoundWinner(string round_Winner)
    {
        roundWinner = round_Winner;
    }

    public float SpawnXLocation()
    {
        GameObject xTile = map.ReturnRandomTile();
        //print("Spawning new x location: " + xTile);
        float xTilePosition = xTile.transform.position.x;
        return xTilePosition;
    }
    public float SpawnYLocation()
    {
        GameObject xTile = map.ReturnRandomTile();
        //print("Spawning new x location: " + xTile);
        float xTilePosition = xTile.transform.position.x;
        return xTilePosition;
    }
}

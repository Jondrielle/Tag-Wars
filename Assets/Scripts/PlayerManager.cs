using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using Random = UnityEngine.Random;

public class PlayerManager : MonoBehaviour
{
    //Serialized Variables
    [SerializeField] SpecialAbilities ability;

    //Private Variables
    private Vector3 spawnLocation;
    //private float doopleCount;

    //Public Variables
    public PlayerColliderController playerCollision;
    List<string> specialAbilitiesList = new List<string>();
    public TextMeshProUGUI playerLabel;
    public PlayerManager enemyPlayer;
    public GameManager gameManager;
    public PlayerMovement player;
    public Rect playerCameraRect;
    public Slider abilitySlider;
    public TilemapGenerator map;
    public Camera playerCamera;
    public Text roundWinner;
    public Material playerGlowMaterial;

    public float distanceBetweenPlayers;
    public float playerCameraSize;
    

    public int playerWins;

    public string horizontalAxis;
    public string verticalAxis;


    // Start is called before the first frame update
    void Awake()
    {
        horizontalAxis = SetHorizontalAxis();
        verticalAxis = SetVerticalAxis();

        playerCollision = this.gameObject.GetComponent<PlayerColliderController>();
        player = this.gameObject.GetComponent<PlayerMovement>();
        gameManager = FindObjectOfType<GameManager>();

        //playerGlowMaterial.shader = this.gameObject.GetComponent<Shader>();
        Shader glowShader = playerGlowMaterial.shader;
        //glowShader.FindPropertyIndex("_Thickness");
        //print(playerGlowMaterial.shader = glowShader);
        //print(playerGlowMaterial.GetFloat("Thickness"));
        //playerGlowMaterial.SetFloat("_Thickness", 0);
        //playerGlowMaterial.shader = this.gameObject.GetComponent<Shader>();
        //print(playerGlowMaterial.GetFloat("_Thickness"));
//        playerGlow = this.GetComponent<Shader>
        
  //      print(playerGlow.GetPropertyDescription(1));
       // playerGlow = this.GetComponent<Shader>();
        //print("The thickness is: " + playerGlow.GetComponent<Material>().GetFloat("Thickness"));

        
        playerWins = 0;
        
        playerLabel.text = player.gameObject.tag;
        
        playerCameraSize = playerCamera.orthographicSize;
        playerCameraRect = playerCamera.rect;
        distanceBetweenPlayers = Vector3.Distance(enemyPlayer.transform.position, transform.position);
        abilitySlider.value = abilitySlider.minValue;
        specialAbilitiesList.Add("DoopleGanger");
        specialAbilitiesList.Add("Immunity");
    }

    private void Start()
    {
        abilitySlider.value = abilitySlider.minValue;
        spawnLocation = this.gameObject.transform.position;
        SetAbilitySlider();
        player.OnDisable();
        roundWinner.text = null;
        //doopleCount = 2;
        
    }

    /*
     * 
     * Randomly spawns each characters new location
     * in random places on the map
     * 
     */
    public void PlayerSpawnLocation()
    {
        this.gameObject.transform.position = new Vector3(RespawnPlayerXLocation(), RespawnPlayerYLocation(),
            gameObject.transform.position.z);
    }

    /*
     * Resets abilityslider if it reached max value
     */
    private void Update()
    {

        if (abilitySlider.value == abilitySlider.maxValue)
        {
            abilitySlider.value = 0;   
            SelectRandomSpecialAbility();
        }

        distanceBetweenPlayers = Vector3.Distance(this.transform.position, enemyPlayer.transform.position);

        if(this.gameObject.tag == "Tagger" && distanceBetweenPlayers < 16)
            playerCamera.orthographicSize = 10;
        else
            ResetPlayerCameraSize();
    }

    private void CheckIfCameraInMap()
    {
        float cameraX = playerCamera.transform.position.x;
        float cameraY = playerCamera.transform.position.y;
        //map.GetBorderTiles(playerCamera);
        //make sure x is not greater than 115.5 or less than - 42.8
       //make sure y is not less than -18.4 or greater than  143
       if(cameraX > 115.5 || cameraX < -42.8 || cameraY > 143 || cameraY < -18.4)
        {
            print("You can not move the camera. OUT OF BOUNDS!!!!!!!!!");
        }
    }
    /*
     * 
     * Randomly selects random special ability 
     * when the ability gauge is filled completely
     * 
     */
    private void SelectRandomSpecialAbility()
    {
        int getRandomIndex = Random.Range(0, 2);
        print("Random index value is: " + getRandomIndex);
        //if (getRandomIndex == 1)
        //{
        //    print("IMMUNITY ACTIVATED");
        //    IsImmune();
        //}
        //else
        //{
            print("Do doopleganger");
            ability.CreateDoopleGanger();
        //}
    }

    /*
     * Increments win count if the player won the round
     */
    public void IncrementWinCount(){

        //Increment player win count
        SetWinCount();

        //creates a string of the tagger winning the round
        string round_Winner = player.name + " won the round";

        //sets the gameManager's roundWinner text to the tagger string 
        gameManager.SetRoundWinner(round_Winner);
        //gameManager.round++;
        //gameManager.round = gameManager.round++;
        StartCoroutine(gameManager.ManageEndOfRound());
        //print("Round winner is: " + gameManager.roundWinner);
        //gameManager.IsRoundOver(true);
        
        //End the round 
       // gameManager.RoundEnded();
    }

    /*
     * Calls PlayerMovement Script to enable movement
     */
    public void EnableMovement()
    {
        player.OnEnable();
    }

    /*
     * Calls PlayerMovement Script to disable movement
     */
    public void DisableMovement()
    {
        player.OnDisable();
    }

    /*
     * This updates the players
     * components to fit their role
     */
    public void UpdateLabel()
    {
        playerLabel.text = player.gameObject.tag;
        abilitySlider.value = 0;
        //abilitySlider.value = abilitySlider.maxValue;
    }

    /*
     * 
     * @param newTagName - renames the players tag
     * to fit the players role
     * 
     * Used when at the beginning of the entire game
     * to set the players role 
     * 
     */
    public void UpdateLabel(string newTagName)
    {
        player.gameObject.tag = newTagName;
        playerLabel.text = player.gameObject.tag;
        //abilitySlider.value = abilitySlider.maxValue;
    }

    /*
     * Sets the ability gauge to active 
     */
    public void IsActivateAbilityGauge(bool setGauge)
    {
        abilitySlider.gameObject.SetActive(setGauge);
    }

    /*
     * Activate and Deactivate slider based on
     * the player's tag name
     */
    public void SetAbilitySlider()
    {
        if (player.gameObject.tag == ("Tagger"))
        {
            abilitySlider.gameObject.SetActive(false);
        }
        else
        {
            abilitySlider.gameObject.SetActive(true);
            abilitySlider.value = 0;
        }
    }

    /*
     * @param - disableSlider checks if the slider
     * should or shouldnt be active
     * 
     * Activate and Deactivate slider based on
     * the player's tag name
     */
    public void SetAbilitySlider(bool disableSlider)
    {
        abilitySlider.gameObject.SetActive(disableSlider);
        abilitySlider.value = 0;
    }

    /*
     * 
     * Sets each player horizontal axis input
     * 
     */
    public string SetHorizontalAxis()
    {
        if (player.gameObject.name == "Player")
        {
            horizontalAxis = "Horizontal";
        }
        else
            horizontalAxis = "Horizontal2";

        return horizontalAxis;
    }

    /*
     * 
     * Sets each player vertical axis input
     * 
     */
    public string SetVerticalAxis()
    {
        if (player.gameObject.name == "Player")
        {
            verticalAxis = "Vertical";
        }
        else
            verticalAxis = "Vertical2";

        return verticalAxis;
    }

    /*
     * 
     * Randomly picks an X location on the map
     * so the player can respawn at the x position
     * 
     */
    public float RespawnPlayerXLocation()
    {
        GameObject xTile = map.ReturnRandomTile();
        //print("Spawning new x location: " + xTile);
        float xTilePosition = xTile.transform.position.x;
        return xTilePosition;
    }

    /*
     * 
     * Randomly picks an Y location on the map
     * so the player can respawn at the y position
     * 
     */
    public float RespawnPlayerYLocation()
    {
        GameObject yTile = map.ReturnRandomTile();
        float yTilePosition = yTile.transform.position.y;
        return yTilePosition;
    }

    /*
     * 
     * Sets the players win count when they win
     * 
     */
    public void SetWinCount()
    {
        playerWins++;
    }

    /*
     * 
     * @return playerWins to give the
     * GameManager what is the players 
     * win count
     * 
     */
    public int GetWinCount()
    {
        return playerWins;
    }

    /*
     * 
     * Resets each players camera size
     * 
     */
    public void ResetPlayerCameraSize()
    {
        playerCamera.orthographicSize = playerCameraSize;
        playerCamera.rect = new Rect(playerCameraRect);
    }

    /*
     * 
     * If the abilitySlider and 
     * the immunity ability is activated then
     * set the immune in playerCollision to 
     * true
     * 
     */
    public void IsImmune()
    {
        playerCollision.SetIsImmune(true);
        //print("Immune value is: " + playerCollision.isImmune);
    }

    /*
     * 
     * @Param cameraSize - set cameraSize
     * 
     */
    public void SetPlayerCameraSize(float cameraSize)
    {
        playerCamera.rect = new Rect(playerCameraRect);
        playerCameraSize = cameraSize;
    }

    //@return playerCameraSize
    public float GetPlayerCameraSize()
    {
        return playerCameraSize;
    }
}

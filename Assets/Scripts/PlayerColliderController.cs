using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Video;
using System.Threading.Tasks;

public class PlayerColliderController: MonoBehaviour
{
    //Serialized Variables 
    [SerializeField] PlayerManager playerManager;

    //private variables
    private PlayerColliderController opponent;
    private PlayerManager[] players;
    private bool canTagHappen;
    public bool isImmune;

    //public variables
    public AudioSource candyCrunchSound;
    public AudioSource crateBreakSound;
    public AnimationClip crateBreaking;
    public BoxCollider2D feetCollider;
    public GameObject candybarPrefab;
    public PlayerMovement movement;
    public Animator crateAnimator;
    public AudioSource waterSound;
    public AudioSource grassSound;
    public SpriteRenderer render;
    public Rigidbody2D player;
    public Animator animator;


    // Start is called before the first frame update
    private void Awake()
    {
        //Get Components
        players = FindObjectsOfType<PlayerManager>();
        movement = playerManager.GetComponent<PlayerMovement>();
        player = GetComponent<Rigidbody2D>();
        render = GetComponent<SpriteRenderer>();
        opponent = GetOpponentPlayer();

        //Set canTagHappen to false
        canTagHappen = false;
    }

    void Start()
    {
        //Hide video and player is visible
        player.gameObject.SetActive(true);

        //Set audiosource
        candyCrunchSound = playerManager.GetComponent<AudioSource>();
       
        //player does not have immunity on start
        isImmune = false;
    }

    private void Update()
    {
        //check if the tagger is in the tagging range
        if (canTagHappen)
            CheckIfTagButtonPressed();
        if(GetIsImmune() == true)
            StartCoroutine(ElapsedImmuneTime(6));
    }

    // called the first frame a player collides with something
    private void OnTriggerEnter2D(Collider2D collider)
    {
        //if (collider.gameObject.tag == ("WaterTile") && collider.gameObject.CompareTag("FeetCollider"))
        //{
        //    print("Entered water tile");
        //    print(collider.gameObject.name);
        //    EnteredWaterTile();
        //    waterSound.Play();
        //}
    }

    /*
     * Check if the player is still in collision with any of
     * these objects to do different things 
     */
    private void OnTriggerStay2D(Collider2D collider)
    {
        if (collider.gameObject.tag == ("Pickupable") && this.gameObject.tag == "Hider")
        {
            candyCrunchSound.Play();
            Destroy(collider.gameObject);            
            playerManager.abilitySlider.value += 10;
        }
        if (collider.gameObject.tag == ("Bush"))
        {
            grassSound.Play();//not loud enough audio
            //print("Grass Sound");
            render.enabled = false;
            playerManager.abilitySlider.gameObject.SetActive(false);
            playerManager.playerLabel.gameObject.SetActive(false);
        }
        if (collider.gameObject.tag == "WaterTile" && this.feetCollider.IsTouching(collider))
        {
            EnteredWaterTile();
        }
        if (collider.gameObject.tag == ("Border"))
        {
            player.BroadcastMessage("OnDisable");
        }
    }

    /*
     * 
     * Check if the player exited a collision 
     * in order to do different things
     * 
     */
    private void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.gameObject.tag == ("Bush") )
        {
            grassSound.Stop();
            render.enabled = true;
            playerManager.abilitySlider.gameObject.SetActive(true);
            playerManager.playerLabel.gameObject.SetActive(true);
            //grassSound.Stop();//not loud enough audio
        }
        else if (collider.gameObject.tag == ("Border"))
        {
            player.BroadcastMessage("OnEnable");
        }
        if (collider.gameObject.tag == ("WaterTile"))
        {
            //print("leaving water tile");
            ExitedWaterTile();
        }
    }

    /*
     * Check if the player collided with any of
     * these objects to do different things 
     */
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == ("Breakable") && this.gameObject.tag == "Hider")
        {
            Transform positionOfGameObject = collision.gameObject.transform;
            GameObject crate = collision.gameObject;
            crateAnimator = crate.GetComponent<Animator>();
            crateBreakSound.Play();
            crateAnimator.SetBool("Exploding", true);
            Destroy(crate, animator.GetCurrentAnimatorStateInfo(0).length);
            CreateInstance(positionOfGameObject);
        }
        if(collision.gameObject.tag == "Nonbreakable" && this.gameObject.tag == "Hider")
        {
            //print("Hider cant move in here");
        }
        if(collision.gameObject.tag == "Hider" && this.gameObject.tag == "Tagger")
        {
            if(opponent.GetIsImmune() == true)
            {
                SetCanTagHappen(false);
                //print("Immunity is:" + GetIsImmune());
                //print("You cant tag the hider.They are immune to tagging right now");

            }
            else if(opponent.GetIsImmune() == false)
            {
                //print("Immunity is:" + GetIsImmune());
                //print("Hider can be tagged now");
            }

        }
    }

    /*
     *
     * @param positionOfGameObject
     * Creates an instance of a candybar and 
     * randomly destorys it
     * Resets the backgroundMusic to 1
     * 
     */
    void CreateInstance(Transform positionOfGameObject)
    {
        GameObject candyBar = Instantiate(candybarPrefab, new Vector3(positionOfGameObject.position.x,
                positionOfGameObject.position.y, positionOfGameObject.position.z),
                Quaternion.identity);
        Destroy(candyBar, Random.Range(5, 15));
    }

    // these are called when the player enters and exit a tile of water
    public void EnteredWaterTile(){
        //print("Entered the water tile");
        if (isImmune == false)
        {
            //print("Immune is not activated");
            BecomeSlowed();
            //WaterTileCount++;
        }
        else if (isImmune == true)
        {
            //print(WaterTileCount);
            //print("Immune is activated");
            // yield return StartCoroutine(ElapsedImmuneTime());
            //print("Immune is: " + GetIsImmune());
            //WaterTileCount = 0;
            //ExitedWaterTile();
            print("Do nothing in entering watertilw");
        }

    }

    private void ExitedWaterTile(){
        //print("WATERTILECOUNT IS: " + WaterTileCount);
        //WaterTileCount--;
        //print("AFTER WATERTILECOUNT IS: " + WaterTileCount);
        //if (WaterTileCount <= 0)
        //{
            StopBeingSlowed();
            //print("No longer slowed down");
        //}
    }

    private void BecomeSlowed(){
        player.BroadcastMessage("MovementControllerBecomeSlowed");
    }

    public void StopBeingSlowed()
    {
        //print("Inside of stop being slowed");
        player.BroadcastMessage("MovementControllerStopBecomingSlowed");
    }

    /*
     *
     * @param waitTime - This is used to 
     * decide how long to wait after the video 
     * plays in order to disable it and 
     * reset background music sound 
     *
     */
    IEnumerator WaitTilTimeIsUp(float waitTime, GameObject crate)
    {
        yield return new WaitForSecondsRealtime(waitTime);
        //Destroy(crate,);
    }

    //Time limit for immunity
    public IEnumerator ElapsedImmuneTime(float timer)
    {
        yield return new WaitForSecondsRealtime(timer);
        print("Time is over");
        SetIsImmune(false);
        print("Printing isImmune: " + GetIsImmune());
    }

    /*
     * 
     * If x key is pressed then show banner
     * and increment the players win count
     * 
     */
    public void CheckIfTagButtonPressed()
    {
        print("Entered tag button method");
        if (Input.GetKeyDown("x")){
            playerManager.gameManager.SomeoneWasTagged();
            playerManager.IncrementWinCount();
        }        
    }

    //Get the opponent
    public PlayerColliderController GetOpponentPlayer()
    {
        foreach (PlayerManager player in players)
        {
            if (player != this.playerManager)
            {
                opponent = player.playerCollision;
            }
        }
        return opponent;
    }

    //Get isImmune
    public bool GetIsImmune()
    {
        return isImmune;
    }

    /*
     * 
     * @param setImmuneValue - sets isImmune 
     * 
     */
    public void SetIsImmune(bool setImmuneValue)
    {
        isImmune = setImmuneValue;
    }

    /*
     * 
     * @param tagCanHappen - sets canTagHappen
     * 
     */
    public void SetCanTagHappen(bool tagCanHappen)
    {
        canTagHappen = tagCanHappen;
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    //Serializble Variables
    [SerializeField] PlayerManager playerManager;

    //Private Variables
    private BoxCollider2D boxCollider;
    private Vector3 moveDelta;
    private RaycastHit2D hit;

    private float currentDashTime;
    private float startigDashTime;
    private float currentDashCD;
    string currentState;

    //Public Variables
    public AudioSource tileWalkSound;
    public Image RadialDashCDFiller;
    public GameObject CageSprite;
    public AudioSource runSound;
    public Animator animator;
    public KeyCode dashKey;
    public KeyCode runKey;

    public float movespeedMultiplier = 5;
    public float runningSpeedMultiplier;
    public float slowMultiplier = 1f;
    public float dashSpeedMultiplier;
    public float maxDashTime;
    public float maxDashCD;

    public bool facingRight;
    public bool isMoving;
    public bool running;
    public bool dashing;


    const string DASHRIGHT = "Dash_Right";
    const string DASHLEFT = "Dash_Left";
    const string DASHDOWN = "Dash_Down";
    const string RIGHT = "Walk_Right";
    const string LEFT = "Walk_Left";
    const string DOWN = "Walk_Down";
    const string DASHUP = "Dash_Up";
    const string UP = "Walk_Up";
    const string IDLE = "Idle";
    const string TAG = "Tag";

    private void Start()
    {
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        isMoving = false;
        currentDashTime = startigDashTime;
    }

    
    /*
      * 
      * Logic of moving character
      * with animation logic
      * 
      */
    private void FixedUpdate()
    {
        //float x and y gets set to 0
        float x = 0;
        float y = 0;

        //Check if the horizontal or vertical axis != 0
        if (Input.GetAxisRaw(playerManager.horizontalAxis) != 0 || Input.GetAxisRaw(playerManager.verticalAxis) != 0)
        {
            //Save the axis in its respective variable
            x = Input.GetAxisRaw(playerManager.horizontalAxis);
            y = Input.GetAxisRaw(playerManager.verticalAxis);

            //check if the horizontal axis is greater or lower 
            //Then check if the player is dashing in their respective 
            //horizontal direction
            if (x > 0)
            {
                if (Input.GetKey(dashKey) && dashing == true)
                    ChangeState(DASHRIGHT);
                else
                    ChangeState(RIGHT);            
            }
            else if(x < 0)
            {
                if (Input.GetKey(dashKey))
                    ChangeState(DASHLEFT);
                else
                    ChangeState(LEFT);
            }

            else if (y > 0)
            {
                if (Input.GetKey(dashKey))
                    ChangeState(DASHUP);
                else
                    ChangeState(UP);
            }

            else if (y < 0)
            {
                if (Input.GetKey(dashKey) && dashing == true)
                    ChangeState(DASHDOWN);
                else
                    ChangeState(DOWN);
            }
        }
        else
        {
            ChangeState(IDLE);
        }

        moveDelta = new Vector3(x, y, 0);

        hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, new Vector2(0, moveDelta.y), Mathf.Abs(moveDelta.y * Time.deltaTime), LayerMask.GetMask("Actor", "Blocking"));

        if(currentDashCD > 0){
            currentDashCD -= Time.deltaTime;
            RadialDashCDFiller.fillAmount = currentDashCD / maxDashCD;
        }

        if(dashing){
            transform.Translate(moveDelta * Time.deltaTime * dashSpeedMultiplier * movespeedMultiplier);
            // decrement current time by 
            currentDashTime -= Time.deltaTime;
            if(currentDashTime <= 0){
                // stop dashing
                dashing = false;
            }
        } else if (running)
        {
            transform.Translate(moveDelta * Time.deltaTime * movespeedMultiplier * runningSpeedMultiplier * slowMultiplier);
        }
        else
            transform.Translate(moveDelta * Time.deltaTime * movespeedMultiplier * slowMultiplier);
    }

    /*
     * 
     * Checks if run key is pressed
     * and cooldown for run
     * 
     */
    void Update()
    {
        if (Input.GetKey(runKey))
            running = true;
        else
             running = false;

        if (Input.GetKey(dashKey))
        {
            if(!dashing &&  currentDashCD <= 0 ){
                currentDashCD = maxDashCD;
                dashing = true;
                currentDashTime = maxDashTime;
            }
            //startigDashTime = Time.deltaTime;
        }
        // else
        // {
        //     dashing = false;
        // }
    }

    /*
     * Disables Movement
     */
    public void OnDisable()
    {
        movespeedMultiplier = 0;
        ShowCage();
    }

    /*
     * Enables Movement
     */
    public void OnEnable()
    {
        //print("OnEnable");
        movespeedMultiplier = 5;
        HideCage();
    }

    // these activate and deactivate the 'cage' sprite that shows when the player can't move
    public void ShowCage(){
            CageSprite.SetActive(true);
        }

    public void HideCage(){
            CageSprite.SetActive(false);

        }

    //Slows the stunned player down
    public IEnumerator StunPlayer()
    {
        movespeedMultiplier = 2.5f;
        yield return new WaitForSecondsRealtime(8);
    }

    public void MovementControllerBecomeSlowed(){
        slowMultiplier = 0.4f;
    }

    public void MovementControllerStopBecomingSlowed(){
        slowMultiplier = 1f;
    }

    /*
     * 
     * @Param newState - Changes the animation state
     * 
     */
    public void ChangeState(string newState)
    {
        //print(newState);
        if(currentState == newState)
        {
            return;
        }
        else
        {
            animator.Play(newState);
        }

        currentState = newState;
    }

    //Play walk sound if the player is walking on a tile
    public void PlaySound()
    {
        tileWalkSound.Play();
    }

    //Play run sound when the player is running
    public void PlayRunSound()
    {
        runSound.Play();
    }

}


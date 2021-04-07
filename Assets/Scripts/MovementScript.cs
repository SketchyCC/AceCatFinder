using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameEvents;
using System;
using System.Runtime.InteropServices.ComTypes;

public class MovementScript : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;

    public float speed;
    float cachespeed;
    public float gravity;
    public float jumpHeight;
    Vector3 velocity;
    public bool isSneaking = false;
    bool stopMovement = false;
    bool isLooking = false;
    float horizontal;
    float vertical;

    //Speed Boost
    float boostCounter = 0;
    float speedUpgrade = 1.3f;

    //smooth moves
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    float t;
    float time;

    //groundDetection
    bool isGrounded;
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    //animation parameters
    private Animator animator;
    private int VelocityHash;
    private float AnimVelocity = 1;

    //footsteps
    public AudioSource Playeraudio;
    public AudioClip[] Footsteppool;
    int Footsteppoolnumber;
    float timewaited;
    float timetowait;


    private void Start()
    {
        animator = GetComponent<Animator>();
        VelocityHash = Animator.StringToHash("Velocity");

        cachespeed = speed;

        Footsteppoolnumber = Footsteppool.Length;
        timetowait = 10 / (speed + 1.6f);
        timewaited = timetowait;

        controller = GetComponent<CharacterController>();
        cam = Camera.main.transform;
        
    }

    void Update()
    {
        if (isLooking)
        {
            stopMovement = isLooking;
        }
        if (!stopMovement)
        {
            Walking();
            Sneaking();
            Jumping();
        }
        else if (stopMovement || isLooking)
        {
            Jumping(); //so gravity still works
            GameEventManager.Raise(new WalkingEvent(!stopMovement));
            if (isSneaking)
            {
                Sneaking();
            }
        }
    }

    public void Jumping()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        if (!stopMovement && Input.GetButtonDown("Jump") && isGrounded && !isSneaking)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2 * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    public void Walking()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        bool isWalking = false;

        Vector3 direction = new Vector3(horizontal, 0f, vertical);

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized/4 * speed * Time.deltaTime);
            
            isWalking = true;

            StartCoroutine(LerpVelocity());

            if (isGrounded)
            {
                PlayFootsteps(speed);
            }
        }
        else if (direction.magnitude <= 0.1f) {
            isWalking = false;
            AnimVelocity = 1f;
            speed = cachespeed;
            time = 0;
        }

        animator.SetFloat(VelocityHash, AnimVelocity);

        GameEventManager.Raise(new WalkingEvent(isWalking));
    }

    IEnumerator LerpVelocity()
    {        

        while (time < 50f)
        {
            t = time /50f;
            t = t * t * t * (t * (6f * t - 15f) + 10f);
            AnimVelocity = Mathf.Lerp(1.0f, 1.25f, t);
            speed = Mathf.Lerp(cachespeed, cachespeed * 1.5f, t);
            time += Time.deltaTime;            
            yield return null;
        }
        AnimVelocity = 1.25f;
        speed = cachespeed *1.5f;

    }

    public void Sneaking()
    {//sneaking actives and deactivates with the key press

        if (Input.GetKeyDown(KeyCode.LeftShift) || stopMovement)
        {
            if (!isSneaking)
            {
                speed /= 2;
                isSneaking = true;
                PlayFootsteps(speed);
            } else if (isSneaking || Input.GetKeyDown(KeyCode.LeftControl))
            {
                speed = cachespeed;
                isSneaking = false;
            }
            GameEventManager.Raise(new SneakEvent(isSneaking));
        }
    }

    protected virtual void OnEnable()
    {
        GameEventManager.AddListener<BagOpenEvent>(OnBagUpdate);
        GameEventManager.AddListener<SwingEvent>(OnSwingUpdate);
        GameEventManager.AddListener<SpeedUpgradeBought>(OnSpeedUpgradeBought);
        GameEventManager.AddListener<CommunityBoardLook>(OnLookUpdate);
        GameEventManager.AddListener<CommunityBoardLeave>(OnLeaveUpdate);
        GameEventManager.AddListener<UIOpened>(OnUIUpdate);
    }

    protected virtual void OnDisable()
    {
        GameEventManager.RemoveListener<SpeedUpgradeBought>(OnSpeedUpgradeBought);
        GameEventManager.RemoveListener<BagOpenEvent>(OnBagUpdate);
        GameEventManager.RemoveListener<SwingEvent>(OnSwingUpdate);
        GameEventManager.RemoveListener<CommunityBoardLook>(OnLookUpdate);
        GameEventManager.RemoveListener<CommunityBoardLeave>(OnLeaveUpdate);
        GameEventManager.RemoveListener<UIOpened>(OnUIUpdate);
    }

    private void OnBagUpdate(BagOpenEvent e)
    {
        if (e.openBag)
        {
            stopMovement = e.openBag;            
        }
        else if (isLooking && !e.openBag) { } 
        else
        {
            StartCoroutine(MoveAgain(0.95f));
        }
    }

    private void OnSwingUpdate(SwingEvent e)
    {
        stopMovement = true;
        StartCoroutine(MoveAgain(1.3f));
    }

    private void OnSpeedUpgradeBought(SpeedUpgradeBought e)
    {
        boostCounter = e.speed;
        if (boostCounter > 0)
        {
            speed *= speedUpgrade;
            cachespeed *= speedUpgrade;
        }
        else 
        {
            DownGradeSpeed();
        }
    }

    IEnumerator MoveAgain(float time)
    {
        yield return new WaitForSeconds(time);
        stopMovement = false;        
    }

    void DownGradeSpeed()
    {
        speed /= speedUpgrade;
        cachespeed /= speedUpgrade;
    }

    void PlayFootsteps(float speed)
    {
        if (timewaited >= timetowait)
        {
            timetowait = 10 / (speed/4 + 1.6f); //function to adjust footstep frequencies to different walking speeds
            Playeraudio.clip = Footsteppool[UnityEngine.Random.Range(0, Footsteppoolnumber)];
            Playeraudio.Play();            
            timewaited = 0;
        }
        else
        {
            timewaited += Time.deltaTime;
        }
    }
    private void OnLookUpdate(CommunityBoardLook e)
    {
        stopMovement = true;
        isLooking = true;
    }
    private void OnLeaveUpdate(CommunityBoardLeave e)
    {
        stopMovement = false;
        isLooking = false;
    }
    private void OnUIUpdate(UIOpened e)
    {
        stopMovement = e.UIisopened;
        isLooking = e.UIisopened;
    }
}

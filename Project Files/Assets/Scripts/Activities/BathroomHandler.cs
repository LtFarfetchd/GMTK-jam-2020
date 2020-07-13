using UnityEngine;
using ActivityVariety = ActivitiesHandlerController.ActivityVariety;
using Room = HouseController.Room;

public class BathroomHandler : MonoBehaviour
{
    public GameObject handWindup, handKnock, victory, defeat, knock, intro;
    public GameObject player;
    public GameObject house;
    public ActivityVariety activityVariety = ActivityVariety.PROBLEM;
    public Room room = Room.BOTTOM_RIGHT;
    public AudioClip knockClip, victoryClip;
    public AudioSource knocker;
    public int necessaryKnocks;
    public int allowedTime;
    public int endFrames = 75;
    public int introFrames = 50;
    private int knocksCompleted;
    private PlayerController playerController;
    private HouseController houseController;
    private bool justKnocked, victorious, defeated;
    private SpriteRenderer 
        handKnockRenderer, handWindupRenderer, 
        victoryRenderer, defeatRenderer, knockRenderer, introRenderer;
    private int knockTimer = 0;
    private int timeElapsed = 0;
    private int endTime = -1;

    void Awake()
    {
        handWindupRenderer = handWindup.GetComponent<SpriteRenderer>();
        handKnockRenderer = handKnock.GetComponent<SpriteRenderer>();
        victoryRenderer = victory.GetComponent<SpriteRenderer>();
        defeatRenderer = defeat.GetComponent<SpriteRenderer>();
        knockRenderer = knock.GetComponent<SpriteRenderer>();
        introRenderer = intro.GetComponent<SpriteRenderer>();
        playerController = (PlayerController)player.GetComponent<MonoBehaviour>();
        houseController = (HouseController)house.GetComponent<MonoBehaviour>();

        SetUp();
    }

    void Update()
    {
        if (timeElapsed == introFrames)
            introRenderer.enabled = false;

        timeElapsed++;

        if (victorious || defeated)
        {
            if ((timeElapsed - endTime) == endFrames)
            {
                if (victorious)
                    houseController.HealStat(activityVariety, room);
                SetUp();
                playerController.InitiateDisengagement();
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0) && !justKnocked)
                Knock();
            else if (justKnocked)
            {
                knockTimer++;
                if (knockTimer == 5)
                    WindUp();
            }
            
            if (endTime == -1 && knocksCompleted > necessaryKnocks)
            {
                endTime = timeElapsed;
                victorious = true;
                victoryRenderer.enabled = true;
                knocker.clip = victoryClip;
                knocker.Play();
            }

            if (endTime == - 1 && timeElapsed > allowedTime)
            {
                endTime = timeElapsed;
                defeated = true;
                defeatRenderer.enabled = true;
            }
        }        
    }

    private void Knock()
    {
        knocksCompleted++;
        justKnocked = true;
        handKnockRenderer.enabled = true;
        handWindupRenderer.enabled = false;
        knockTimer = 0;
        knockRenderer.enabled = true;
        knocker.Play();
    }

    private void WindUp()
    {
        justKnocked = false;
        handKnockRenderer.enabled = false;
        handWindupRenderer.enabled = true;
        knockRenderer.enabled = false;
    }

    private void SetUp()
    {
        knocker.clip = knockClip;
        introRenderer.enabled = true;
        victoryRenderer.enabled = false;
        defeatRenderer.enabled = false;
        knockRenderer.enabled = false;
        endTime = -1;
        timeElapsed = 0;
        knocksCompleted = 0;
        knockTimer = 0;
        victorious = false;
        defeated = false;
        WindUp();
    }
}
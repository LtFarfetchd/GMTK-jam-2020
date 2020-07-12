using UnityEngine;

public class BeerPongHandler : MonoBehaviour
{
    public GameObject player, hand, ball, cupback, cupfront, background;
    private PlayerController playerController;
    private float throwDistance = 0f;
    private float throwSpeed = 0.1f;
    private float throwMax = 10000f;
    private float throwRangeMin = 6000f;
    private float throwRangeMax = 8000f;


    void Start()
    {
        playerController = (PlayerController)player.GetComponent<MonoBehaviour>();
    }

    void Awake()
    {
        throwDistance = 0f;
        throwSpeed = 0.1f;
        float thisx = this.transform.position.x;
        float thisy = this.transform.position.y;
        float cupoffset = 0.5f;
        background.transform.position = new Vector3(thisx, thisy, background.transform.position.z);
        hand.transform.position = new Vector3(thisx - 1.55f, thisy + 0.15f, hand.transform.position.z);
        cupback.transform.position = new Vector3(thisx, thisy + cupoffset, cupback.transform.position.z);
        cupfront.transform.position = new Vector3(thisx, thisy + cupoffset, cupfront.transform.position.z);
        // reset the game's state here and prepare to start
    }

    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            if(throwDistance >= throwRangeMin && throwDistance <= throwRangeMax)
            {
                WinAnimation();
            }
            else
            {
                LoseAnimation();
            }
        }
        // placeholder disengagement. The game should determine when to disengage according to game rules
        if (Input.GetMouseButtonDown(0))
        {
            playerController.InitiateDisengagement();
        }
        if (throwDistance + throwSpeed < throwMax)
        {
            throwDistance += throwSpeed;
            throwSpeed += 0.01f;            
        }
        else
        {
            throwDistance = throwMax;
            LoseAnimation();
        }
    }

    private void WinAnimation()
    {
        //animation
        playerController.InitiateDisengagement();
    }

    private void LoseAnimation()
    {
        //animation
        Awake();
    }
}
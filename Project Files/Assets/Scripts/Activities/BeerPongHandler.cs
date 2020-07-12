using UnityEngine;

public class BeerPongHandler : MonoBehaviour
{
    public GameObject player;
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

    void OnAwake()
    {
        throwDistance = 0f;
        throwSpeed = 0.1f;
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
                LoseAnimation(false);
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
            LoseAnimation(true);
        }
    }

    private void WinAnimation()
    {
        playerController.InitiateDisengagement();
    }

    private void LoseAnimation(bool toofar)
    {
        OnAwake();
    }
}
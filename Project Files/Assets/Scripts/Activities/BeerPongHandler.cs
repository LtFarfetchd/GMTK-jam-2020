using UnityEngine;

public class BeerPongHandler : MonoBehaviour
{
    public GameObject player, hand, ball, cupback, cupfront, background;
    public Sprite handBall, handNoBall;
    private PlayerController playerController;
    private float throwDistance = 0f;
    private float throwSpeed = 0.1f;
    private float throwMax = 10000f;
    private float throwRangeMin = 4000f;
    private float throwRangeMax = 6000f;
    private float animationChange = 0.0001f, animationStart;
    private bool thrown = false;


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
        ball.transform.position = new Vector3(10, 10, -6);
        hand.GetComponent<SpriteRenderer>().sprite = handBall;
        thrown = false;
        animationStart = 0.017f;
        // reset the game's state here and prepare to start
    }

    void Update()
    {
        if (thrown == false)
        {
            if (Input.GetKeyDown("space"))
            {
                hand.GetComponent<SpriteRenderer>().sprite = handNoBall;
                ball.transform.position = new Vector3(hand.transform.position.x, hand.transform.position.y - 0.5f, -6);
                thrown = true;
            }

            if (throwDistance + throwSpeed < throwMax)
            {
                throwDistance += throwSpeed;
                throwSpeed += 0.05f;            
            }
            else
            {
                throwDistance = throwMax;
                thrown = true;
            }

            float newx = this.transform.position.x + ((throwDistance*2f/throwMax - 1f) * 1.55f);
            hand.transform.position = new Vector3( newx, hand.transform.position.y, hand.transform.position.z);

        }
        else
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
    }

    private void WinAnimation()
    {        
        if(ball.transform.position.x < cupback.transform.position.x)
        {
            ball.transform.position = new Vector3(ball.transform.position.x + 0.001f, ball.transform.position.y + animationStart, ball.transform.position.z);
        }
        else if(ball.transform.position.x > cupback.transform.position.x)
        {
            ball.transform.position = new Vector3(ball.transform.position.x - 0.001f, ball.transform.position.y + animationStart, ball.transform.position.z);
        }
        else
        {
            ball.transform.position = new Vector3(ball.transform.position.x, ball.transform.position.y + animationStart, ball.transform.position.z);
        }

        if(animationStart <= 0)
        {
            ball.transform.position = new Vector3(ball.transform.position.x, ball.transform.position.y, -4);
        }

        if (animationStart <= 0 && ball.transform.position.y <= cupback.transform.position.y)
        {
            playerController.InitiateDisengagement();
        }
        animationStart -= animationChange;
        //playerController.InitiateDisengagement();
    }

    private void LoseAnimation()
    {
        ball.transform.position = new Vector3(ball.transform.position.x, ball.transform.position.y + animationStart, ball.transform.position.z);
        if (animationStart <= 0 && ball.transform.position.y <= cupback.transform.position.y - 0.35f)
        {
            Awake();
        }
        animationStart -= animationChange;
    }
}
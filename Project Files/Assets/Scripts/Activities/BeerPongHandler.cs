using UnityEngine;
using ActivityVariety = ActivitiesHandlerController.ActivityVariety;
using Room = HouseController.Room;

public class BeerPongHandler : MonoBehaviour
{
    public GameObject player, house, hand, ball, cupback, cupfront, background;
    public ActivityVariety activityVariety = ActivityVariety.OBLIGATION;
    public Room room = Room.TOP_LEFT;
    public Sprite handBall, handNoBall;
    public float handStartSpeed = 0.005f, handAcceleration = 0.0001f, 
        ballStartSpeed = 0.1f, ballAcceleration = 0.0001f, ballCurve = 0.0025f;
    public float victoryToleranceDistance = 1f;
    private PlayerController playerController;
    private HouseController houseController;
    private Vector3 handStartPos;
    private float handSpeed, handMaximumX, ballSpeed, ballMaximumY;
    private float winRangeMinX, winRangeMaxX;
    private bool thrown = false, hasWon = false, isInitialised = false, ballResorted = false;
    private int ballStartSortingOrder;

    void Start()
    {
        playerController = (PlayerController)player.GetComponent<MonoBehaviour>();
        houseController = (HouseController)house.GetComponent<MonoBehaviour>();
    }

    void Update()
    {
        if (!isInitialised)
            Initialise();

        if (thrown)
            MoveBall(hasWon);
        else
            MoveHand();
    }

    private void Reset()
    {
        handSpeed = handStartSpeed;
        hand.GetComponent<SpriteRenderer>().sprite = handBall;
        hand.transform.position = handStartPos;
        ball.SetActive(false);
        ball.GetComponent<SpriteRenderer>().sortingOrder = ballStartSortingOrder;
        thrown = false;
        ballSpeed = ballStartSpeed;
        ballResorted = false;
        hasWon = false;
    }

    private void Initialise()
    {
        SpriteRenderer cupRenderer = cupback.GetComponent<SpriteRenderer>();
        winRangeMinX = cupRenderer.bounds.min.x - victoryToleranceDistance;
        winRangeMaxX = cupRenderer.bounds.max.x + victoryToleranceDistance;
        ballMaximumY = cupRenderer.bounds.max.y + 
            (cupRenderer.bounds.max.y - cupRenderer.bounds.min.y); 
        SpriteRenderer backgroundRenderer = background.GetComponent<SpriteRenderer>();
        SpriteRenderer handRenderer = hand.GetComponent<SpriteRenderer>();
        handMaximumX = backgroundRenderer.bounds.max.x - 
            (handRenderer.bounds.max.x - handRenderer.bounds.min.x);
        handStartPos = hand.transform.position;
        ballStartSortingOrder = ball.GetComponent<SpriteRenderer>().sortingOrder;
        Reset();
        isInitialised = true;
    }

    private void MoveHand()
    {
        if (Input.GetMouseButtonDown(0))
        {
            hand.GetComponent<SpriteRenderer>().sprite = handNoBall;
            ball.SetActive(true);
            ball.transform.position = hand.transform.position;
            thrown = true;
            float handX = hand.transform.position.x;
            hasWon = (handX >= winRangeMinX && handX <= winRangeMaxX);
        }
        
        handSpeed += Mathf.Sign(handSpeed) * handAcceleration;
        float targetX = hand.transform.position.x + handSpeed;
        if (handStartPos.x < targetX && targetX < handMaximumX)
            hand.transform.position = new Vector3(targetX, hand.transform.position.y, hand.transform.position.z);
        else
            handSpeed *= -1;
    }

    private void MoveBall(bool hasWon)
    {        
        if (hasWon)
        {
            float xOffsetFromCup = cupback.transform.position.x - ball.transform.position.x;
            float xMove = ballCurve * (xOffsetFromCup != 0 ? Mathf.Sign(xOffsetFromCup) : 0f);
            ball.transform.position = new Vector3(ball.transform.position.x + xMove, ball.transform.position.y + ballSpeed, ball.transform.position.z);
            
            if(!ballResorted && ballSpeed <= 0)
            {
                ballResorted = true;
                int cupBackSortingOrder = cupback.GetComponent<SpriteRenderer>().sortingOrder;
                int cupFrontSortingOrder = cupfront.GetComponent<SpriteRenderer>().sortingOrder;
                // take the average of the sorting orders to guarantee the ball falls between the back and front
                ball.GetComponent<SpriteRenderer>().sortingOrder = (int)(((float)(cupBackSortingOrder + cupFrontSortingOrder)) / 2f);
            }

            if (ballSpeed <= 0 && ball.transform.position.y <= cupback.transform.position.y)
            {
                Reset();
                houseController.HealStat(activityVariety, room);
                playerController.InitiateDisengagement();
            }
        }
        else 
        {
            ball.transform.position = new Vector3(ball.transform.position.x, ball.transform.position.y + ballSpeed, ball.transform.position.z);
            if (ballSpeed <= 0 && ball.transform.position.y <= hand.transform.position.y)
            {
                Reset();
            }
        }
        ballSpeed -= ballAcceleration;
    }
}
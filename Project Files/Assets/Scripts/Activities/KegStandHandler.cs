using UnityEngine;

public class KegStandHandler : MonoBehaviour
{
    public GameObject player;
    private PlayerController playerController;

    void Start()
    {
        playerController = (PlayerController)player.GetComponent<MonoBehaviour>();
    }

    void OnAwake()
    {
        // reset the game's state here and prepare to start
    }

    void Update()
    {
        // placeholder disengagement. The game should determine when to disengage according to game rules
        if (Input.GetMouseButtonDown(0))
        {
            playerController.InitiateDisengagement();
        }
    }
}
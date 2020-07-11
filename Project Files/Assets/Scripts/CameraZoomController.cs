using UnityEngine;

public class CameraZoomController : MonoBehaviour
{
    private enum State {
        ZOOMING,
        RETURNING,
        STATIC
    }
    private State state;
    private Vector3 cameraStart;
    private Vector2 path;
    private Vector2 playerStart;
    private Vector2 playerTarget;
    private float playerXTarget, playerYTarget;
    public GameObject player;
    public GameObject house;

    public void ZoomIn(Vector2 playerStart, Vector2 playerTarget) 
    {
        this.playerStart = playerStart;
        this.playerTarget = playerTarget;
        path = playerTarget - (Vector2)cameraStart;
        state = State.ZOOMING;
    }

    public void ZoomOut() => this.state = State.RETURNING;

    void Start()
    {
        state = State.STATIC;
        cameraStart = transform.position;
    }

    void Update()
    {
        if (state == State.ZOOMING || state == State.RETURNING)
        {
            float movementProportion = 
                Vector2.Distance(playerStart, (Vector2)player.transform.position)
                / Vector2.Distance(playerStart, playerTarget);

            Vector2 newPos = Vector2.Lerp((Vector2)cameraStart, playerTarget, movementProportion);
            transform.position = new Vector3(newPos.x, newPos.y, cameraStart.z);

            if ((Vector2)transform.position == (state == State.ZOOMING ? playerTarget : (Vector2)cameraStart))
                state = (state == State.ZOOMING ? State.RETURNING : State.STATIC);
        }
    }
}

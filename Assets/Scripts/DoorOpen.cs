using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem; // Make sure this is included
using UnityEngine.SceneManagement;

public class DoorOpen : MonoBehaviour
{
    public Transform endPos;
    public float speed = 1.0f;

    private bool moving = false;
    private bool opening = true;
    private Vector3 startPos;
    private float delay = 0.0f;

    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        Debug.Log("StartPos: " + startPos);
        if (moving)
        {
            if (opening)
            {
                MoveDoor(endPos.position);
            }
            else
            {
                MoveDoor(startPos);
            }
        }
    }

    void MoveDoor(Vector3 goalPos)
    {
        float dist = Vector3.Distance(transform.position, goalPos);
        if (dist >= 0.001f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                goalPos,
                speed * Time.deltaTime
            );
        }
        else
        {
            if (opening)
            {
                delay += Time.deltaTime;
                if (delay > 0)
                {
                    opening = false;
                }
            }
            else
            {
                moving = false;
                opening = true;
            }
        }
    }

    public bool Moving
    {
        get { return moving; }
        set { moving = value; }
    }
}

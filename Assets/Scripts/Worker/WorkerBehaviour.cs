using System.Collections.Generic;
using UnityEngine;

public class WorkerBehaviour : MonoBehaviour
{
    public WorkerState currentState;
    public Action currentAction;
    public Storage bag;

    void Start()
    {
        currentState.position = transform.position;
    }

    void Update()
    {
    }
}

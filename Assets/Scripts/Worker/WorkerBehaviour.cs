using System.Collections.Generic;
using UnityEngine;

public class WorkerBehaviour : MonoBehaviour
{
    public WorkerState currentState;
    public Action currentAction;
    public Storage bag;
    public List<WorkerState> task = new List<WorkerState>();
    public int i = 0;

    void Awake()
    {
        currentState = new WorkerState
        {
            position = transform.position,
            power = 5f,
            energy = 1000f
        };
        currentAction = gameObject.AddComponent<Motion>();

        currentAction.nextAction = gameObject.AddComponent<Motion>();

        currentAction.nextAction.nextAction = currentAction;
        currentAction.Initialize(1f, currentState);
        currentAction.nextAction.Initialize(1f, currentState);

        WorkerState nextState = currentState.Clone();
        nextState.position = transform.position + 3f * Vector3.up;
        task.Add(nextState);
        nextState = currentState.Clone();
        nextState.position = transform.position + 3f * Vector3.right;
        task.Add(nextState);
    }

    void Update()
    {
        transform.position = currentState;
        if (currentAction.IsCompleted)
        {
            currentAction = currentAction.nextAction;
            if (i == 0) { i++; }
        }
        if (currentAction.IsIdle)
        {
            currentAction.Do(task[i]);
        }
    }
}

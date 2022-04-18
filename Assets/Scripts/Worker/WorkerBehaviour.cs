using System.Collections.Generic;
using UnityEngine;

public class WorkerBehaviour : MonoBehaviour
{
    public WorkerState currentState;
    public Action currentAction;
    public Storage bag;
    public List<WorkerState> task = new List<WorkerState>();
    public GameObject[] spots;
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
        currentAction.Initialize(1f, currentState);

        Action formerAction = currentAction;
        for (int i = 0; i < spots.Length; i++)
        {
            WorkerState nextState = currentState.Clone();
            nextState.position = spots[i].transform.position;
            task.Add(nextState);
            if (i < spots.Length - 1)
            {
                formerAction.nextAction = gameObject.AddComponent<Motion>();
                formerAction = formerAction.nextAction;
                formerAction.Initialize(1f, currentState);
            }
        }
    }

    void Update()
    {
        transform.position = currentState;
        if (currentAction.IsCompleted)
        {
            currentAction = currentAction.nextAction;
            if (currentAction != null)
            {
                i++;
            }
        }
        if (currentAction.IsIdle)
        {
            currentAction.Do(task[i]);
        }
    }
}

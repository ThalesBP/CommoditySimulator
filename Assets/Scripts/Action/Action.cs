using System;
using UnityEngine;

[System.Serializable]
public abstract class Action : MonoBehaviour
{
    protected enum ActionStatus { idle, waiting, acting, completed };
    protected ActionStatus status;

    private float counter, delay, delta;
    protected float lerpScale;

    public float energy;
    public Action nextAction;
    public WorkerState CurrentState { get { return currentState; } }
    public WorkerState FinalState { get { return finalState; } }
    
    [SerializeField]
    protected WorkerState currentState { get; set; }
    protected WorkerState finalState { get; set; }
    public virtual void Initialize(float energy, WorkerState workerState)
    {
        this.energy = energy;
        this.currentState = workerState;
    }

    public float LerpScale
    {
        get { return Mathf.Clamp(lerpScale, 0f, 1f); }
    }

    public bool timeScaled = true;

    /// <summary>
    /// Returns true if status is idle.
    /// </summary>
    /// <value><c>true</c> if idle; otherwise, <c>false</c>.</value>
    public bool IsIdle { get { return (status == ActionStatus.idle); } }
    public bool IsCompleted { get { return (status == ActionStatus.completed); } }

    /// Use this for initialization
	protected void Awake()
    {
        delay = 0f;
        counter = delta = 1f;
        status = ActionStatus.idle;
    }

    /// Update is called once per frame
    protected void Update()
    {
        switch (status)
        {
            case ActionStatus.idle:
                Resting(); // Those methods can replace a concrete class in future
                break;
            case ActionStatus.acting:
                currentState.time += StepTime();
                Executing();
                break;
            case ActionStatus.waiting:
                Casting();
                break;
            case ActionStatus.completed:
                Completed();
                break;
        }
        Counter();
    }

    /// <summary>
    /// Counts and checks the timer.
    /// </summary>
    private void Counter()
    {
        lerpScale = (counter - delay) / delta;

        if (status != ActionStatus.idle)
        {
            if (counter < delay)
            {
                status = ActionStatus.waiting;
                counter += StepTime();
            }
            else if (counter < delay + delta)
            {
                status = ActionStatus.acting;
                counter += StepTime();
            }
            else
                status = ActionStatus.completed;
        }
    }

    private float StepTime()
    {
        if (timeScaled)
            return Time.deltaTime;
        else
            return Time.unscaledDeltaTime;
    }

    public abstract void Resting(); // This method may be 
    public abstract void Executing();
    public abstract void Casting();
    public abstract void Completed();

    public abstract WorkerState StateRequiredTo(WorkerState desired);
    public abstract WorkerState FinalStateTo(WorkerState initial);
    public abstract float EnergyNecessary(WorkerState desired);

    public float TimeToDo(WorkerState state)
    {
        if (currentState.IsEnoughTo(StateRequiredTo(state)))
        {
            return EnergyNecessary(state) / currentState.power;
        }
        else
        {
            throw new Exception("Non positive power");
        }
    }

    #region Do functions

    /// <summary>
    /// Moves the card from where it is to destiny place.
    /// </summary>
    /// <param name="destiny">Destiny place to reach.</param>
    /// <param name="deltaTime">Time it takes to reach it.</param>
    public float Do(WorkerState state)
    {
        counter = 0f;
        delta = TimeToDo(state);
        status = ActionStatus.waiting;
        lerpScale = (counter - delay) / delta;
        finalState = state;
        return delta;
    }

    /// <summary>
    /// Moves the card from where it is to destiny place after some time.
    /// </summary>
    /// <param name="destiny">Destiny place to reach.</param>
    /// <param name="deltaTime">Time it takes to reach it.</param>
    /// <param name="delayTime">Time it waits to start moving.</param>
    public float Do(WorkerState state, float delayTime)
    {
        delay = delayTime;
        return Do(state) + delayTime;
    }
    #endregion
}

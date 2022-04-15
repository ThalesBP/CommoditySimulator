using System;
using UnityEngine;

public abstract class Action : MonoBehaviour
{
    public Action(float energy)
    {
        if (energy < 0)
        {
            throw new Exception("Negative Energy");
        }
        this.energy = energy;
    }

    private enum ActionStatus { idle, waiting, acting, updating };
    private ActionStatus status;

    private float counter, delay, delta;
    private float lerpScale;

    private readonly float energy;

    public float LerpScale
    {
        get { return Mathf.Clamp(lerpScale, 0f, 1f); }
    }

    public bool timeScaled = true;

    /// <summary>
    /// Returns true if status is idle.
    /// </summary>
    /// <value><c>true</c> if idle; otherwise, <c>false</c>.</value>
    public bool IsIdle
    {
        get { return (status == ActionStatus.idle); }
    }


    /// Use this for initialization
	void Awake()
    {
        delay = 0f;
        counter = delta = 1f;
        status = ActionStatus.idle;
    }

    /// Update is called once per frame
    void Update()
    {
        Counter();
        switch (status)
        {
            case ActionStatus.idle:
                Resting(); // Those methods can replace a concrete class in future
                break;
            case ActionStatus.acting:
                Executing();
                break;
            case ActionStatus.waiting:
                Casting();
                break;
            case ActionStatus.updating:
                Completed();
                status = ActionStatus.idle;
                break;
        }
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
                if (timeScaled)
                    counter += Time.deltaTime;
                else
                    counter += Time.unscaledDeltaTime;
            }
            else if (counter < delay + delta)
            {
                status = ActionStatus.acting;
                if (timeScaled)
                    counter += Time.deltaTime;
                else
                    counter += Time.unscaledDeltaTime;
            }
            else
                status = ActionStatus.updating;
        }
    }

    public abstract void Resting();
    public abstract void Executing();
    public abstract void Casting();
    public abstract void Completed();

    public abstract WorkerState StateRequired();
    public abstract WorkerState FinalState();

    public float TimeToDo(WorkerState state)
    {
        if (state.IsEnoughTo(StateRequired()))
        {
            return state.energy / state.power;
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
        status = ActionStatus.acting;
        lerpScale = (counter - delay) / delta;
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

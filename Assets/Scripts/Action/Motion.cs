using UnityEngine;

/// <summary>
/// Controls the motion of a Vector3 element along the time.
/// </summary>
/// 
public class Motion : Action {

    private Vector3 initial, value, final;
    public Vector3 Value
    {
        get { return value; }
    }
    public static implicit operator Vector3(Motion motion)
    {
        return motion.value;
    }
    public override void Initialize(float energy, WorkerState workerState)
    {
        base.Initialize(energy, workerState);
    }

    /// Use this for initialization
	new void Awake ()
    {
        base.Awake();
        value = transform.position;
	}
	
	/// Update is called once per frame
	new void Update () 
    {
        base.Update();
	}

    public override void Resting()
    {
        this.initial = this.value = currentState.position;
    }
    public override void Executing() 
    {
        Vector3 nextPosition = Vector3.LerpUnclamped(initial, final, Mathf.MoveTowards(0f, 1f, lerpScale));
        currentState.energy -= this.energy * (nextPosition - value).magnitude;
        value = nextPosition;
        currentState.position = value;
    }
    public override void Casting() 
    {
        final = finalState;
        Debug.Log("Time To Do: " + TimeToDo(finalState)
            + ", Energy: " + EnergyNecessary(finalState)
            + ", Distance: " + (final - initial).magnitude);
    }
    public override void Completed() { }

    public override WorkerState StateRequiredTo(WorkerState desired) 
    {
        WorkerState stateRequired = new WorkerState();
        stateRequired.position = currentState.position;
        stateRequired.energy = EnergyNecessary(desired);
        return stateRequired;
    }
    public override WorkerState FinalStateTo(WorkerState desired) {
        WorkerState finalState = desired.Clone();
        finalState.time += this.TimeToDo(desired);
        finalState.energy -= this.EnergyNecessary(desired);
        finalState.position = desired;
        return finalState;
    }

    public override float EnergyNecessary(WorkerState desired)
    {
        return energy * (desired.position - currentState.position).magnitude;
    }
}

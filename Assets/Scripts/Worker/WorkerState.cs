using UnityEngine;
public class WorkerState {
    public Vector3 position;
    public float power;
    public float energy;
    public Tool equipment;
    public Storage materials;

    public static implicit operator Vector3(WorkerState state)
    {
        return state.position;
    }

    public static implicit operator Tool(WorkerState state)
    {
        return state.equipment;
    }

    public static implicit operator Storage(WorkerState state)
    {
        return state.materials;
    }

    public static WorkerState operator -(WorkerState next, WorkerState actual)
    {
        WorkerState distance = new WorkerState();
        if (next.position != null) { distance.position = next.position - actual.position; }
        distance.power = next.power - actual.power;
        distance.energy = next.energy - actual.energy;
        distance.equipment = next.equipment == null? null :
            next.equipment.Equals(actual.equipment) ? null : next.equipment;
        distance.materials = next.materials == null ? null : next.materials - actual.materials;
        return distance;
    }

    public bool IsEnoughTo(WorkerState required)
    {
        WorkerState diff = this - required;
        return !(
            ((diff.position != null) && (diff.position.magnitude > float.Epsilon)) ||
            (diff.energy > float.Epsilon) ||
            (diff.equipment != null) ||
            ((diff.materials != null) && (diff.materials.HasDebt())));
    }
}

using UnityEngine;

[System.Serializable]
public class WorkerState {
    public Vector3 position;
    public float power;
    public float energy;
    public float time;
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
        distance.time = next.time - actual.time;
        distance.equipment = next.equipment == null? null :
            next.equipment.Equals(actual.equipment) ? null : next.equipment;
        distance.materials = next.materials == null ? null : next.materials - actual.materials;
        return distance;
    }

    public bool IsEnoughTo(WorkerState required)
    {
        WorkerState diff = this - required;
        return diff.IsEnough();
    }

    public bool IsEnough()
    {
        return !(
            ((this.position != null) && (this.position.magnitude > float.Epsilon)) ||
            (this.energy < 0f) ||
            (this.equipment != null) ||
            ((this.materials != null) && (this.materials.HasDebt)));
    }

    public WorkerState Clone()
    {
        WorkerState clone = new WorkerState();
        clone.position = new Vector3(this.position.x, this.position.y, this.position.z);
        clone.power = this.power;
        clone.energy = this.energy;
        clone.time = this.time;
        clone.equipment = this.equipment != null ? (Tool)this.equipment.Clone() : null;
        clone.materials = this.materials != null ? this.materials.Clone() : null;
        return clone;
    }

    public override string ToString()
    {
        string state = position != null ? "Position: " + position.ToString() + ", " : "";
        state += string.Format("Power: {0}, Energy: {1}, Time: {2}", power, energy, time);
        state += equipment != null ? "," + equipment.Name : "";
        state += materials != null ? "," + materials.Amount : "";
        return state;
    }
}

using System.Collections.Generic;

public class Tool : Product
{
    // Create a new product in context
    public Tool(string name, float energy, Storage requirements, float power) : base (name, energy, requirements)
    {
        Power = power;
    }

    public Tool(Tool baseline) : this (baseline.Name, baseline.Energy, baseline.Requirements, baseline.Power) { }

    public Tool(Tool baseline, Dictionary<string, Work> works) : this(baseline)
    {
        Power = baseline.Power;
    }

    public new Amountable Produce(int amount, Dictionary<string, Work> works, Storage feedstock)
    {
        feedstock.Consume(Requirements);
        return new Tool(this, works);
    }

    public float Power { get; }
}

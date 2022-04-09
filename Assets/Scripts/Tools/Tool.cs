using System.Collections.Generic;

public class Tool : Product
{
    // Create a new product in context
    public Tool(string name, Dictionary<Amountable, int> requirements, float power) : base (name, requirements)
    {
        Power = power;
    }

    public Tool(Tool baseline, Dictionary<string, Work> works) : base(baseline, works, 1)
    {
        Power = baseline.Power;
    }

    public new Amountable Produce(int amount, Dictionary<string, Work> works, Dictionary<Amountable, int> feedstock)
    {
        Consume(feedstock);
        return new Tool(this, works);
    }

    public float Power { get; }
}

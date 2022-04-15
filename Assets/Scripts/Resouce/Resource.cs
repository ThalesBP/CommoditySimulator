using System.Collections.Generic;

public abstract class Resource : Amountable
{
    public Dictionary<Amountable, int> Requirements { get; }
    public float Energy { get; }

    public abstract bool HasRequisits(Dictionary<Amountable, int> feedstock, float energy);

    public abstract float Consume(Dictionary<Amountable, int> feedstock);
}

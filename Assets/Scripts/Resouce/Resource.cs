using System.Collections.Generic;

public interface Resource : Amountable
{
    public Dictionary<Amountable, int> Requirements { get; }
    public float Energy { get; }

    public bool HasRequisits(Dictionary<Amountable, int> feedstock, float energy);

    public float Consume(Dictionary<Amountable, int> feedstock);
}

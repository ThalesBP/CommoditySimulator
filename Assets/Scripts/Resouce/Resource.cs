using System.Collections.Generic;

public abstract class Resource : Amountable
{
    public Resource(string name, float energy, Storage requirements) : base(name, 1)
    {
        Energy = energy;
        Requirements = requirements;
    }

    public Storage Requirements { get; }
    public float Energy { get; }

    public abstract bool HasRequisits(Storage feedstock, float energy);

    public abstract float Consume(Storage feedstock);
}

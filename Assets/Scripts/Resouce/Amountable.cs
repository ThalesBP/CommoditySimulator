using System.Collections;
using System.Collections.Generic;

public abstract class Amountable
{
    public string Name { get; }
    public int Amount { get; set; }

    public abstract Amountable Share(int amount);
    public abstract Amountable Clone();
    public override int GetHashCode()
    {
        return Name.GetHashCode();
    }
    public static Amountable operator +(Amountable a, Amountable b)
    {
        if (a.Name != b.Name)
        {
            throw new System.Exception(string.Format("They are not the same amountable: {0} =/= {0}", a.Name, b.Name));
        }
        Amountable clone = a.Clone();
        clone.Amount += b.Amount;
        return clone;
    }
    public static Amountable operator -(Amountable a, Amountable b)
    {
        if (a.Name != b.Name)
        {
            throw new System.Exception(string.Format("They are not the same amountable: {0} =/= {0}", a.Name, b.Name));
        }
        return a.Clone().Share(a.Amount - b.Amount);
    }
}

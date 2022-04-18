using System;
using System.Collections.Generic;
public class Product : Resource
{
    public Product(Product baseline) : base(baseline.Name, baseline.Energy, baseline.Requirements)
    {
        workSpent = baseline.WorkSpent;
    }
    // Create a new product in context
    public Product(string name, float energy, Storage requirements) : base(name, energy, requirements) { }

    public Product(Product baseline, Dictionary<string, Work> Works, int amount) : this (baseline)
    {
        workSpent = Works;
    }

    public Product(Product baseline, int amount) : this(baseline)
    {
        Amount = amount;
    }

    public Dictionary<string, Work> WorkSpent { get { return workSpent; } }
    private Dictionary<string, Work> workSpent { get; set; }

    public Amountable Produce(int amount, Dictionary<string, Work> works, Storage feedstock)
    {
        Consume(feedstock);
        return new Product(this, works, amount);
    }

    public override Amountable Share(int amount)
    {
        if (amount < Amount)
        {
            Amount -= amount;
            return new Product(this, amount);
        }
        throw new Exception("Amount insufficient"); //TODO: Create an custom exception
    }


    public override bool HasRequisits(Storage feedstock, float energy)
    {
        if (energy < Energy)
        {
            return false;
        }
        return feedstock.IsEnoughTo(Requirements);
    }

    // Returns the energy spent
    public override float Consume(Storage feedstock)
    {
        if (!HasRequisits(feedstock, Energy))
        {
            throw new Exception("Amount insufficient");
        }
        feedstock.Consume(Requirements);
        return Energy;
    }


    public override int GetHashCode()
    {
        return Name.GetHashCode();
    }

    public override Amountable Clone()
    {
        return new Product(this);
    }
}

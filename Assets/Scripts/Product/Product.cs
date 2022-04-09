using System;
using System.Collections.Generic;
public class Product : Resource
{
    // Create a new product in context
    public Product(string name, Dictionary<Amountable, int> requirements)
    {
        Name = name;
        Requirements = requirements;
        Amount = 1;
    }

    public Product(Product baseline, Dictionary<string, Work> Works, int amount)
    {
        Name = baseline.Name;
        Requirements = baseline.Requirements;
        workSpent = Works;
        Amount = amount;
    }

    public Product(Product baseline, int amount)
    {
        Name = baseline.Name;
        Requirements = baseline.Requirements;
        workSpent = baseline.workSpent;
        Amount = amount;
    }

    public string Name { get; }
    public int Amount { get; set; }
    public float Energy { get; }
    public Dictionary<string, Work> WorkSpent { get { return workSpent; } }
    private Dictionary<string, Work> workSpent { get; set; }
    public Dictionary<Amountable, int> Requirements { get; }

    public Amountable Produce(int amount, Dictionary<string, Work> works, Dictionary<Amountable, int> feedstock)
    {
        Consume(feedstock);
        return new Product(this, works, amount);
    }

    public Amountable Share(int amount)
    {
        if (amount < Amount)
        {
            Amount -= amount;
            return new Product(this, amount);
        }
        throw new Exception("Amount insufficient"); //TODO: Create an custom exception
    }


    public bool HasRequisits(Dictionary<Amountable, int> feedstock, float energy)
    {
        if (energy < Energy)
        {
            return false;
        }

        foreach (KeyValuePair<Amountable, int> element in feedstock)
        {
            if (Requirements.ContainsKey(element.Key))
            {
                if (element.Value < Requirements[element.Key])
                {
                    return false;
                }
            }
        }
        return true;
    }

    // Returns the energy spent
    public float Consume(Dictionary<Amountable, int> feedstock)
    {
        if (!HasRequisits(feedstock, Energy))
        {
            throw new Exception("Amount insufficient");
        }

        foreach (KeyValuePair<Amountable, int> element in Requirements)
        {
            feedstock[element.Key] -= Requirements[element.Key];
        }
        return Energy;
    }


    public override int GetHashCode()
    {
        return Name.GetHashCode();
    }
}

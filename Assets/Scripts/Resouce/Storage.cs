using System;
using System.Collections.Generic;

public class Storage
{
    private Dictionary<string, Amountable> storage = new Dictionary<string, Amountable>();
    public Amountable this[Amountable amountable]
    {
        get { return storage[amountable.Name]; }
        set { storage.Add(amountable.Name, amountable); }
    }

    public void Include(Amountable amountable)
    {
        if (storage.ContainsKey(amountable.Name))
        {
            storage[amountable.Name].Amount += amountable.Amount;
        } else
        {
            storage.Add(amountable.Name, amountable);
        }
    }

    public bool HasDebt
    {
        get
        {
            foreach (KeyValuePair<string, Amountable> amount in storage)
            {
                if (amount.Value.Amount < 0)
                {
                    return false;
                }
            }
            return true;
        }
    }
    public int Amount
    {
        get
        {
            int total = 0;
            foreach (KeyValuePair<string, Amountable> amount in storage)
            {
                total += amount.Value.Amount;
            }
            return total;
        }
    }
    public static Storage operator +(Storage a, Storage b)
    {
        Storage diff = new Storage();
        foreach (KeyValuePair<string, Amountable> amount_a in a.storage)
        {
            foreach (KeyValuePair<string, Amountable> amount_b in a.storage)
            {
                try
                {
                    diff.Include(amount_a.Value + amount_b.Value);
                }
                catch (Exception) { }
            }
        }
        return diff;
    }
    public static Storage operator -(Storage a, Storage b)
    {
        Storage diff = new Storage();
        foreach (KeyValuePair<string, Amountable> amountable in a.storage)
        {
            if (b.storage.ContainsKey(amountable.Value.Name))
            {
                diff.Include(amountable.Value - b.storage[amountable.Value.Name]);
            }
            else
            {
                diff.Include(amountable.Value);
            }
        }
        return diff;
    }
    public bool IsEnoughTo(Storage required)
    {
        return (this - required).HasDebt;
    }

    public static explicit operator Amountable[](Storage a)
    {
        Amountable[] b = new Amountable[a.storage.Count];
        a.storage.Values.CopyTo(b, 0);
        return b;
    }

    public void Consume(Storage feedstock)
    {
        if (this.IsEnoughTo(feedstock))
        {
            foreach (KeyValuePair<string, Amountable> material in feedstock.storage)
            {
                this[material.Value].Amount -= feedstock[material.Value].Amount;
            }
        } 
        else
        {
            throw new Exception("Material insufficient");
        }
    }
    public Storage Clone()
    {
        Storage clone = new Storage();
        foreach (KeyValuePair<string, Amountable> amountable in storage)
        {
            clone.Include(amountable.Value.Clone());
        }
        return clone;
    }
}

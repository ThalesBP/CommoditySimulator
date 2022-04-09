using System.Collections.Generic;

public class Work
{
    public static Work EMPTY = new Work(Power.EMPTY, Power.EMPTY, Power.EMPTY);

    public Work(Power extraction, Power circulation, Power production)
    {
        Extraction = extraction;
        Circulation = circulation;
        Production = production;
    }

    public Power Extraction { get; }

    public Power Circulation { get; }

    public Power Production { get; }

    public Power getPower()
    {
        return Extraction + Circulation + Production;
    }

    public static Work operator +(Work w1, Work w2)
        => new Work(w1.Extraction + w2.Extraction,
                    w1.Circulation + w2.Circulation,
                    w1.Production + w2.Production);
}

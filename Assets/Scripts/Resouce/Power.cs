public class Power
{
    public static Power EMPTY = new Power(0f, 1f);

    public Power(float energy, float time)
    {
        Energy = energy;
        Time = time;
    }

    public static implicit operator float(Power power)
    {
        return power.Energy / power.Time;
    }

    public float Energy { get; }
    public float Time { get; }

 //   public float getPower()
 //   {
 //       return Energy / Time;
 //   }

    public static Power operator +(Power p1, Power p2)
        => new Power(p1.Energy+p2.Energy, p1.Time+p2.Time);
    public static Power operator -(Power p1, Power p2)
        => new Power(p1.Energy-p2.Energy, p1.Time-p2.Time);
}

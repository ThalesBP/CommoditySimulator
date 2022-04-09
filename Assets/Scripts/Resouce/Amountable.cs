public interface Amountable
{
    public string Name { get; }
    public int Amount { get; set; }

    public abstract Amountable Share(int amount);

    public abstract int GetHashCode();
}

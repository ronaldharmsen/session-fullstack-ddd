public interface ICommand {}
public class BuyShipCommand : ICommand
{
    public string Name { get; set; }
    public string Price { get; set; }
}
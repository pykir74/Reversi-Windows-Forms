public enum ReversiState
{
    Empty,
    Black,
    White
}


public class ReversiButton : Button
{
    public ReversiState State { get; set; } = ReversiState.Empty;
    public int GridX { get; set; }
    public int GridY { get; set; }
}

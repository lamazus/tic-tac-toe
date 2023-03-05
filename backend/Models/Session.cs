namespace backend.Models;

public class Session 
{
    public int Id { get; set; }
    public List<Cell> Cells { get; set; } = new List<Cell>();
    public int ActivePlayer { get; set; } = 1;
    public bool isFinished { get; set; } = false;
    public int Winner {get; set;} = 0;
}
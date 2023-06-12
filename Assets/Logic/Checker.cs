using Assets.Logic;

public class Checker
{
    public int ID { get; private set; }
    public PlayerColor color { get; private set; }
    public FieldInBoard startingPlace { get; private set; }

    public Checker (int id, PlayerColor color)
    {
        ID = id;
        this.color = color;
    }

    public Checker(Checker oldChecker)
    {
        this.ID = oldChecker.ID;
        this.color = oldChecker.color;
        this.startingPlace = oldChecker.startingPlace;
    }

    public void SetStartingPosition (FieldInBoard newPlace)
    {
        startingPlace = newPlace;
    }
}
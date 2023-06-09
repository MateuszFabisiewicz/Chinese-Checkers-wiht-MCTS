using Assets.Logic;

public class Checker
{
    public int ID { get; private set; }
    public PlayerColor color { get; private set; }
    public FieldInBoard placement { get; private set; }
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
        this.placement = oldChecker.placement;
        this.startingPlace = oldChecker.startingPlace;
    }

    public void SetPosition (FieldInBoard newPlace)
    {
        placement = newPlace;
    }

    public void SetStartingPosition (FieldInBoard newPlace)
    {
        startingPlace = newPlace;
    }
}
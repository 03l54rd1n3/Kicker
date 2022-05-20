namespace Kicker.Shared.Game;

public struct Ball
{
    public PointS Position { get; set; }

    public PointS PreviousPosition { get; set; }

    public float PixelSpeed { get; set; }

    public float Direction { get; set; }
}
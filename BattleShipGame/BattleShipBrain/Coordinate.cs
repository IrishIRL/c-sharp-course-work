namespace BattleShipBrain
{
    public struct Coordinate
    {
        public Coordinate(int y, int x)
        {
            Y = y;
            X = x;
        }

        public int X { get; }
        public int Y { get; }

        public override string ToString() => $"X: {X}, Y: {Y}";
    }
}
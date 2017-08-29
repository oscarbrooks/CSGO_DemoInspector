public struct Round {
    public int Number { get; set; }
    public int StartTick { get; set; }

    public Round(int number, int startTick)
    {
        Number = number;
        StartTick = startTick;
    }
}

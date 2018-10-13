[System.Serializable]
public struct Round {
    public int Number;
    public int StartTick;

    public Round(int number, int startTick)
    {
        Number = number;
        StartTick = startTick;
    }
}

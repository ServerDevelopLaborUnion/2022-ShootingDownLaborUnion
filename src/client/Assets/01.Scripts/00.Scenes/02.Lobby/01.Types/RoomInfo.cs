public class RoomInfo
{
    public string UUID { get; private set; }
    public string Name { get; private set; }
    public bool IsPrivate { get; private set; }
    public int PlayerCount { get; private set; }
    public const int MaxPlayers = 4;

    public override string ToString()
    {
        return string.Format("{0} ({1}/{2})", Name, PlayerCount, MaxPlayers);
    }

    public RoomInfo(string name, bool isPrivate, int playerCount)
    {
        Name = name;
        IsPrivate = isPrivate;
        PlayerCount = playerCount;
    }
}

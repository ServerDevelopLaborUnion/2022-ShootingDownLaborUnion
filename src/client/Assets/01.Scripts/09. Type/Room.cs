using System.Collections.Generic;

public class Room
{
    public RoomInfo Info { get; private set; }
    public List<User> Players { get; private set; }

    public void AddUser(User user)
    {
        Players.Add(user);
    }

    public void ExitUser(User user)
    {
        Players.Remove(user);
    }
}

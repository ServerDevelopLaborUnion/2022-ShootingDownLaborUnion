using System.Collections.Generic;

public class Room
{
    public RoomInfo Info { get; private set; }
    public List<User> Users { get; private set; }

    public void AddUser(User user)
    {
        Users.Add(user);
    }

    public void ExitUser(User user)
    {
        Users.Remove(user);
    }

    public Room(RoomInfo info, List<User> users)
    {
        Info = info;
        Users = users;
    }
}

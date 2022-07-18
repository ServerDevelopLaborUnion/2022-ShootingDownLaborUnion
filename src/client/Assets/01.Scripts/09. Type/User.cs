public class User
{
    public string UUID { get; private set; }
    public string Name { get; private set; }
    public WeaponType Weapon { get; private set; }
    public bool IsReady { get; private set; }
    public bool IsMaster { get; private set; }

    public new string ToString()
    {
        return string.Format("{0} ({1})", Name, Weapon);
    }

    public User(string uuid, string name, WeaponType weapon)
    {
        UUID = uuid;
        Name = name;
        Weapon = weapon;
        IsReady = false;
    }
}

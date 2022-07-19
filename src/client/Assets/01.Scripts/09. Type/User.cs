public class User
{
    public string UUID { get; private set; }
    public string Name { get; private set; }
    public WeaponType Weapon { get; set; }
    public RoleType Role{ get; set; }
    public bool IsReady { get; set; }
    public bool IsMaster { get; set; }

    public new string ToString()
    {
        return string.Format("{0} ({1})", Name, Weapon);
    }

    public User(string uuid, string name)
    {
        UUID = uuid;
        Name = name;
        Weapon = WeaponType.None;
        IsReady = false;
        IsMaster = false;
    }
}

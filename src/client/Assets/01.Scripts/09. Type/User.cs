[System.Serializable]
public class User
{
    public string UUID { get; set; }
    public string Name { get; set; }
    public WeaponType Weapon { get; set; }
    public RoleType Role{ get; set; }
    public bool IsReady { get; set; }
    public bool IsMaster { get; set; }

    public override string ToString()
    {
        return string.Format("{0} ({1})", Name, Weapon);
    }

    public void SetUserName(string name){
        Name = name;
    }

    public User()
    {
        UUID = string.Empty;
        Name = string.Empty;
        Weapon = WeaponType.None;
        Role = RoleType.NONE;
        IsReady = false;
        IsMaster = false;
    }

    public User(string uuid, string name)
    {
        UUID = uuid;
        Name = name;
        Weapon = WeaponType.None;
        IsReady = false;
        IsMaster = false;
    }

    public User(string uuid, string name, WeaponType weapon, RoleType role, bool isReady, bool isMaster)
    {
        UUID = uuid;
        Name = name;
        Weapon = weapon;
        Role = role;
        IsReady = isReady;
        IsMaster = isMaster;
    }
}

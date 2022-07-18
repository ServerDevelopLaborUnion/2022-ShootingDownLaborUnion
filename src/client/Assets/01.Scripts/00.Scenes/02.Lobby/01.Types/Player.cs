public class Player
{
    public string Name { get; private set; }
    public WeaponType Weapon { get; private set; }
    public bool IsReady { get; private set; }

    public new string ToString()
    {
        return string.Format("{0} ({1})", Name, Weapon);
    }

    public Player(string name, WeaponType weapon)
    {
        Name = name;
        Weapon = weapon;
        IsReady = false;
    }
}

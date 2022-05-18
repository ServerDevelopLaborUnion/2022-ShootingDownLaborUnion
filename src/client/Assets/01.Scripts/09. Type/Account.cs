public class Account
{
    public string UserUUID { get; set; }
    public string Username { get; set; }
    public string Token { get; set; }

    public Account(string userUUID, string username, string token)
    {
        UserUUID = userUUID;
        Username = username;
        Token = token;
    }
}
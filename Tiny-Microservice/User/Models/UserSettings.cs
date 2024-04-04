namespace User.Models;

public class UserSettings
{
    public string id { get; set; }
    public string userId { get; set; }
    
    public User User { get; set; }
    public string theme { get; set; }
    public string language { get; set; }
}
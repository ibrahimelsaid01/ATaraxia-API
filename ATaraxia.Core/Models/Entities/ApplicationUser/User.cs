namespace ATaraxia.Core.Models;

public class User
{
    public Guid UserId { get; set; }
    public bool Gender { get; set; }
    public string? NickName { get; set; }
    public string? LoginStatus { get; set; }
    
    public List<Device>? DeviceIdList { get; set; }
    public List<Question>? Recomendation { get; set; }


}

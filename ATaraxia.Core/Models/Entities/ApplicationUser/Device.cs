namespace ATaraxia.Core.Models;

public class Device
{
    public Guid DeviceId { get; set; }

    public Guid UserId { get; set; }

    public virtual User? Users { get; set; }
}

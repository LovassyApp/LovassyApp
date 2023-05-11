namespace WebApi.Core.Lolo.Services.Options;

public class LoloOptions
{
    public int FiveThreshold { get; set; } = 3;
    public int FourThreshold { get; set; } = 5;
    public string FiveReason { get; set; } = "Ötösökből automatikusan generálva";
    public string FourReason { get; set; } = "Négyesekből automatikusan generálva";
    public string RequestReason { get; set; } = "Kérvényből automatikusan generálva";
    public string ManagerLockPrefix { get; set; } = "lolo_manager";
}
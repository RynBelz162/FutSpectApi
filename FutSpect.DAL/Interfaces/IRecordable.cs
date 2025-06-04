namespace FutSpect.Dal.Interfaces;

public interface IRecordable
{
    public DateTime CreatedOn { get; }
    public DateTime ModifiedOn { get; }
}
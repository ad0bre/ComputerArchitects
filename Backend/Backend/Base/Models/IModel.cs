namespace Backend.Base.Models;

public interface IModel
{
    public string Id { get; set; }

    public DateTime Created { get; set; }
    
    public DateTime Updated { get; set; }
}
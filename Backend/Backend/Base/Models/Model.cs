namespace Backend.Base.Models;

public abstract class Model : IModel
{
    public string Id { get; set; }
    
    public DateTime Created { get; set; }
    
    public DateTime Updated { get; set; }

    public Model()
    {
        Id = Guid.NewGuid().ToString();
        Created = Updated = DateTime.UtcNow;
    }
}
namespace Apps.Asana.Dtos.Base;

public interface ITaskWithParent
{
    AsanaEntity? Parent { get; }
}

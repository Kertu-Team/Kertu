namespace Kertu.InteractiveServer.Data.Models.Elements
{
    /// <summary>
    /// Represents a task card with completion status.
    /// </summary>
    public class TaskCard : Card
    {
        public bool IsCompleted { get; set; }
    }
}

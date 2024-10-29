namespace Kertu.InteractiveServer.Data.Models.Elements
{
    /// <summary>
    /// Base card
    /// </summary>
    /// <remarks>
    /// todo: all
    /// </remarks>
    public class Card : Element
    {
        public bool IsCompleted { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}

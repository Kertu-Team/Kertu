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
        public string Description { get; set; } = string.Empty;
        public bool IsTask { get; set; }
        public bool IsCompleted { get; set; }
    }
}

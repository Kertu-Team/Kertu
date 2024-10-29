namespace Kertu.InteractiveServer.Data.Models.Elements
{
    /// <summary>
    /// Board containing cards, lists, and other boards
    /// </summary>
    /// <remarks>
    /// todo: all 
    /// </remarks>
    public class Board : Element
    {
        public List<Element> Children { get; set; } = [];
    }
}

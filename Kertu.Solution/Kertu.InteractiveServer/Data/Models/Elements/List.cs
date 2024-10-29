namespace Kertu.InteractiveServer.Data.Models.Elements
{
    /// <summary>
    /// List containing cards
    /// </summary>
    /// <remarks>
    /// todo: all 
    /// </remarks>
    public class List : Element
    {
        public List<Card> Children { get; set; } = [];
    }
}

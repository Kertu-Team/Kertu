namespace Kertu.InteractiveServer.Data.KertuElements
{
    /// <summary>
    /// Kertu List containing cards
    /// </summary>
    /// <remarks>
    /// todo: all 
    /// </remarks>
    public class KertuList : KertuElement
    {
        public List<KertuCard> ChildCards { get; set; } = new List<KertuCard>();
    }
}

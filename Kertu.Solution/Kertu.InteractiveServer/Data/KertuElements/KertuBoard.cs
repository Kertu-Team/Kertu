namespace Kertu.InteractiveServer.Data.KertuElements
{
    /// <summary>
    /// Kertu Board containing cards, lists, and other boards
    /// </summary>
    /// <remarks>
    /// todo: all 
    /// </remarks>
    public class KertuBoard : KertuElement
    {
        public List<KertuElement> ChildKertuElements { get; set; } = new List<KertuElement>();
    }
}

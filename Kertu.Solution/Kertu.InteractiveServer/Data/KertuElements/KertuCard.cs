namespace Kertu.InteractiveServer.Data.KertuElements
{
    /// <summary>
    /// Base Kertu card
    /// </summary>
    /// <remarks>
    /// todo: all
    /// </remarks>
    public class KertuCard : KertuElement
    {
        public bool IsCompleted { get; set; }
        public string Description { get; set; } = string.Empty;
    }
}

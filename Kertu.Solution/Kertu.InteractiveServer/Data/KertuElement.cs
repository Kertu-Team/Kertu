using System.ComponentModel.DataAnnotations;

namespace Kertu.InteractiveServer.Data
{
    /// <summary>
    /// Base class for future Kertu elements
    /// </summary>
    /// <remarks>
    /// All Kertu elements such as lists or cards should inherit this class
    /// </remarks>
    public class KertuElement
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}

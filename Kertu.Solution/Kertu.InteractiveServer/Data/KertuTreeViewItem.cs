using Kertu.InteractiveServer.Data.KertuElements;

namespace Kertu.InteractiveServer.Data
{
    /// <summary>
    /// Class made for converting different kertu elements into common tree view representation
    /// </summary>
    public class KertuTreeViewItem
    {
        public KertuElement KertuElement { get; set; }
        public List<KertuTreeViewItem> Children { get; set; } = new List<KertuTreeViewItem>();

        public string Name { get { return KertuElement.Name; } set { KertuElement.Name = value; } }

        public KertuTreeViewItem(KertuElement element)
        {
            KertuElement = element;
        }

        public string GetIcon()
        {
            if(KertuElement is KertuCard)
            {
                return "article";
            }
            else if (KertuElement is KertuList)
            {
                return "splitscreen";
            }
            else if (KertuElement is KertuBoard)
            {
                return "space_dashboard";
            }
            return "question_mark";
        }

    }
}

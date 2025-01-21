using Kertu.InteractiveServer.Data.Models.Elements;

namespace Kertu.InteractiveServer.Data.Models
{
    /// <summary>
    /// Class made for converting different elements into common tree view representation
    /// </summary>
    public class TreeViewItem(Element element)
    {
        public Element Element { get; set; } = element;

        public List<TreeViewItem> Children { get; set; } = [];

        public string Name
        {
            get => Element != null ? Element.Name : specialName;
            set { if(Element != null) Element.Name = value; specialName = value; } 
        }

        string specialName;

        public string GetIcon()
        {
            if (Element is Card)
            {
                return "article";
            }
            else if (Element is List)
            {
                return "splitscreen";
            }
            else if (Element is Board)
            {
                return "space_dashboard";
            }
            else if (Element == null)
            {
                return "account_circle";
            }

            return "question_mark";
        }
    }
}

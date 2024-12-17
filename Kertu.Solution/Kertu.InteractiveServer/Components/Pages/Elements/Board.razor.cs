using Microsoft.AspNetCore.Components;
using Models = Kertu.InteractiveServer.Data.Models.Elements;

namespace Kertu.InteractiveServer.Components.Pages.Elements
{
    public partial class Board : ComponentBase
    {
        private IList<Models.Card>? _data;

        [Parameter]
        public required string Id { get; set; }

        private int IdValue => int.Parse(Id);

        protected override void OnInitialized() { }

        private void CreateItem() { }
    }
}

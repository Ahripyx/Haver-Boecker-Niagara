using Microsoft.AspNetCore.Mvc.Rendering;

namespace Haver_Boecker_Niagara.ViewModels
{
    public class EditUsersViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }

        public List<SelectListItem> AvailableRoles { get; set; } = new();
        public List<string> SelectedRoles { get; set; } = new();
    }
}

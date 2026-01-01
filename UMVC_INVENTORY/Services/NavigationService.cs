using System.Windows.Forms;
using UMVC_INVENTORY.Interfaces;

namespace UMVC_INVENTORY.Services
{
    public class NavigationService
    {
        public void NavigateToForm(Form currentForm, Form targetForm)
        {
            NavigationManager.NavigateTo(currentForm, targetForm);
        }

        public bool NavigateBackFromForm(Form currentForm)
        {
            return NavigationManager.NavigateBack(currentForm);
        }

        public bool CanNavigateBack()
        {
            return NavigationManager.CanNavigateBack();
        }
    }
}

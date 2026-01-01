using System.Windows.Forms;

namespace UMVC_INVENTORY.Interfaces
{
    public interface INavigableForm
    {
        void NavigateTo(Form targetForm);
        bool NavigateBack();
    }
}

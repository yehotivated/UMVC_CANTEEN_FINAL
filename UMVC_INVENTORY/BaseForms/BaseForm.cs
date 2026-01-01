using System.Windows.Forms;
using UMVC_INVENTORY.Interfaces;
using UMVC_INVENTORY.Services;

namespace UMVC_INVENTORY.BaseForms
{
    public class BaseForm : Form, INavigableForm
    {
        #region Protected Fields

        protected readonly NavigationService NavigationService;

        #endregion

        #region Constructor

        protected BaseForm()
        {
            NavigationService = new NavigationService();
        }

        #endregion

        #region INavigableForm Implementation

        public virtual void NavigateTo(Form targetForm)
        {
            NavigationService.NavigateToForm(this, targetForm);
        }

        public virtual bool NavigateBack()
        {
            bool success = NavigationService.NavigateBackFromForm(this);
            
            if (!success)
            {
                ShowNoPreviousPageMessage();
            }

            return success;
        }

        #endregion

        #region Protected Methods

        protected virtual void ShowNoPreviousPageMessage()
        {
            MessageBox.Show("No previous page to go back to.", "Navigation",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        protected virtual void OpenFormAndHideCurrent(Form form)
        {
            NavigateTo(form);
        }

        #endregion

    }
}

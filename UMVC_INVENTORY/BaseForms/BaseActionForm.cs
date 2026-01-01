using System.Windows.Forms;

namespace UMVC_INVENTORY.BaseForms
{
    public abstract class BaseActionForm : BaseForm
    {
        #region Protected Methods

        protected virtual void OnCancelButtonClick(object sender, System.EventArgs e)
        {
            NavigateBack();
        }

        protected abstract void OnSaveButtonClick(object sender, System.EventArgs e);

        #endregion
    }
}

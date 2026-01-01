using System.Windows.Forms;

namespace UMVC_INVENTORY.BaseForms
{
    /// <summary>
    /// Design-time only dummy class to allow the Visual Studio designer to instantiate forms
    /// that inherit from BaseActionForm. This class is always compiled to ensure the designer can find it.
    /// </summary>
    public class DesignTimeBaseActionForm : BaseActionForm
    {
        #region Protected Override Methods

        protected override void OnSaveButtonClick(object sender, System.EventArgs e)
        {
            // Empty implementation for design-time only
            // This allows the designer to instantiate the form without errors
        }

        #endregion
    }
}

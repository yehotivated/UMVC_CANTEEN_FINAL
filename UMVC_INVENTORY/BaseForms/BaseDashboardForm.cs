using System.Windows.Forms;
using UMVC_INVENTORY.Interfaces;
using UMVC_INVENTORY.Services;

namespace UMVC_INVENTORY.BaseForms
{
    public class BaseDashboardForm : BaseForm, ILogoutHandler, IRefreshable
    {
        #region Protected Fields

        protected readonly LogoutService LogoutService;

        #endregion

        #region Constructor

        protected BaseDashboardForm()
        {
            LogoutService = new LogoutService();
        }

        #endregion

        #region ILogoutHandler Implementation

        public virtual void PerformLogout()
        {
            LogoutService.RequestLogout(this);
        }

        #endregion

        #region IRefreshable Implementation

        public virtual void RefreshForm()
        {
            this.Refresh();
        }

        #endregion

        #region Protected Methods

        protected virtual void OnDashboardButtonClick(object sender, System.EventArgs e)
        {
            RefreshForm();
        }

        protected virtual void OnLogoutButtonClick(object sender, System.EventArgs e)
        {
            PerformLogout();
        }

        protected virtual void OnBackButtonClick(object sender, System.EventArgs e)
        {
            NavigateBack();
        }

        #endregion
    }
}

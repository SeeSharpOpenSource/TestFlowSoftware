using System;
using System.Windows.Forms;
using Testflow.Utility.I18nUtil;

namespace TestFlow.DevSoftware.Authentication
{
    public static class AuthenticationManage
    {
        internal static DataBaseProxy DataAccessor { get; }

        static AuthenticationManage()
        {
            I18NOption i18NOption = new I18NOption(typeof (AuthenticationManage).Assembly, "i18n_authentication_zh", "i18n_authentication_en")
            {
                Name = Constants.I18nName
            };
            I18N.InitInstance(i18NOption);
            DataAccessor = new DataBaseProxy();
        }

        public static AuthenticationSession GetLoginSession(AuthenticationSession session)
        {
            ScanLoginForm loginForm = new ScanLoginForm(session);
            Application.Run(loginForm);
            return loginForm.Session;
        }

        public static AuthenticationSession Relogin(AuthenticationSession session)
        {
            ScanLoginForm loginForm = new ScanLoginForm(session);
            loginForm.ShowDialog();
            return loginForm.Session;
        }

        public static AuthenticationSession ShowUserManageForm(AuthenticationSession session, IWin32Window owner)
        {
            UserManageForm userManageForm = new UserManageForm(session);
            userManageForm.ShowDialog(owner);
            return userManageForm.Session;
        }

        public static void Logout(AuthenticationSession session)
        {
            DataAccessor.LogOut(session);
        }
    }
}
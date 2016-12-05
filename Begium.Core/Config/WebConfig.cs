using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Begium.Core.Config
{
    public static class WebConfig
    {
        public static string BuyPackagesEmail = ConfigurationManager.AppSettings["buy_packages_email"];

        public static string SmtpHost = ConfigurationManager.AppSettings["SmtpHost"];
        public static string EmailUserTemplateFile = SafeGetServerPath() + "\\templates\\superadmin_welcome_letter_template.html";
        public static string EmailAgentlUserTemplateFile = SafeGetServerPath() + "\\templates\\agent_welcome_letter_template.html";
        public static string ForgotPwdEmailTemplateFile = SafeGetServerPath() + "\\templates\\forgot_pwd_template.html";

        public static int SmtpPort = SafeInteger(ConfigurationManager.AppSettings["SmtpPort"]);
        public static bool EnableSSL = SafeBoolean(ConfigurationManager.AppSettings["EnableSSL"]);
        public static string ProtoolSupportEmail = ConfigurationManager.AppSettings["proool_support_email"];
        public static string ActivateBranchNotificationEmail = ConfigurationManager.AppSettings["activate_branch_notification_email"];
        public static string FeaturePackageIsAddedManuallyEmail = SafeString(ConfigurationManager.AppSettings["feature_package_is_added_manually_email"]);
        public static string BusinessSignUpEmail = SafeString(ConfigurationManager.AppSettings["BUSINESS_SIGNUP_EMAIL"]);
        public static string GoodsSignUpEmail = SafeString(ConfigurationManager.AppSettings["GOODS_SIGNUP_EMAIL"]);

        public static string SmtpSendFrom = ConfigurationManager.AppSettings["SmtpSendFrom"];
        public static string SmtpUsername = ConfigurationManager.AppSettings["SmtpUsername"];
        public static string SmtpPassword = ConfigurationManager.AppSettings["SmtpPassword"];

        public static bool UseDefaultCredentials = SafeBoolean(ConfigurationManager.AppSettings["UseDefaultCredentials"]);
        public static string WelcomeEmailSubject = SafeString(ConfigurationManager.AppSettings["WelcomeEmailSubject"]);
        public static string ForgotPwdEmailSubject = SafeString(ConfigurationManager.AppSettings["ForgotPwdEmailSubject"]);
        public static int ForgotPwdExpiry = SafeInteger(ConfigurationManager.AppSettings["ForgotPwdExpiry"]);

        public static string SetPwdEmailTemplateFile = SafeGetServerPath() + "\\templates\\set_pwd_template.html";
        public static string SetPwdEmailSubject = SafeString(ConfigurationManager.AppSettings["SetPwdEmailSubject"]);
        public static int SetPwdExpiry
        {
            get
            {
                int rt = SafeInteger(ConfigurationManager.AppSettings["SetPwdExpiry"]);
                if (rt == 0)
                    return 24;
                return rt;
            }
        }

        public static bool IsLocal = SafeBoolean(ConfigurationManager.AppSettings["IsLocal"]);

        public static string GoogleMapApiKey = ConfigurationManager.AppSettings["GoogleMapApiKey"];

        public static string ErrorSystemFromAddress = ConfigurationManager.AppSettings["EMAIL_FROM_ADDRESS_ERROR"];
        public static string ErrorSystemToAddress = ConfigurationManager.AppSettings["EMAIL_SYSTEM_ERROR"];
        public static string PAGE_SIZES = ConfigurationManager.AppSettings["PAGE_SIZES"];
        public static string GetThumbnailWidth = System.Configuration.ConfigurationManager.AppSettings["ThumbnailWidth"];
        public static string GetThumbnailHeight = System.Configuration.ConfigurationManager.AppSettings["ThumbnailHeight"];

        public static string OverlayWorkingPathBase = ConfigurationManager.AppSettings["OverlayWorkingPath"];
    }
}

using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using MX.Core;
using MX.Core.Entity;
using MX.Core.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using System.Xml.Xsl;
using System.Xml.XPath;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Net.Mail;
using SD = System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace MX.BL.Util
{
    public static class Helper
    {
        #region Fields

        private static Random _Random = new Random();

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

        public static Rectangle PDFPageSize { get { return PageSize.A4.Rotate(); } }
        public static float PDFPageWidth { get { return PDFPageSize.Width; } }

        public static string GetThumbnailWidth = System.Configuration.ConfigurationManager.AppSettings["ThumbnailWidth"];
        public static string GetThumbnailHeight = System.Configuration.ConfigurationManager.AppSettings["ThumbnailHeight"];

        public static string OverlayWorkingPathBase = ConfigurationManager.AppSettings["OverlayWorkingPath"];
        public const string PropertyImageKeyLocal = "{0}";

        /// <summary>
        /// Generate S3Key for User's profile image.
        /// {AgencyId}/{BranchId}/User/{UserId}/Images
        /// </summary>
        public const string UserProfileImageKey = "{0}/{1}/User/{2}/Images";

        /// <summary>
        /// Generate S3Key for Property's image.
        /// {AgencyId}/{BranchId}/Property/{PropertyId}/Images
        /// </summary>
        public const string PropertyImageKey = "{0}/{1}/Property/{2}/Images";

        /// <summary>
        /// Generate S3Key for Lead's voice recording.
        /// {AgencyId}/{BranchId}/Lead/{LeadId}/VoiceRecording
        /// </summary>
        public const string LeadVoiceRecordingKey = "{0}/{1}/Lead/{2}/VoiceRecording";

        /// <summary>
        /// Generate S3Key for Agency's logo image.
        /// {AgencyId}/Logo/Images
        /// </summary>
        public const string AgencyLogoImageKey = "{0}/Logo";

        public const string AgencyEmptyLogo = "data:image/jpg;base64,/9j/4QAYRXhpZgAASUkqAAgAAAAAAAAAAAAAAP/sABFEdWNreQABAAQAAAA8AAD/4QMsaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wLwA8P3hwYWNrZXQgYmVnaW49Iu+7vyIgaWQ9Ilc1TTBNcENlaGlIenJlU3pOVGN6a2M5ZCI/PiA8eDp4bXBtZXRhIHhtbG5zOng9ImFkb2JlOm5zOm1ldGEvIiB4OnhtcHRrPSJBZG9iZSBYTVAgQ29yZSA1LjUtYzAxNCA3OS4xNTE0ODEsIDIwMTMvMDMvMTMtMTI6MDk6MTUgICAgICAgICI+IDxyZGY6UkRGIHhtbG5zOnJkZj0iaHR0cDovL3d3dy53My5vcmcvMTk5OS8wMi8yMi1yZGYtc3ludGF4LW5zIyI+IDxyZGY6RGVzY3JpcHRpb24gcmRmOmFib3V0PSIiIHhtbG5zOnhtcD0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wLyIgeG1sbnM6eG1wTU09Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9tbS8iIHhtbG5zOnN0UmVmPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvc1R5cGUvUmVzb3VyY2VSZWYjIiB4bXA6Q3JlYXRvclRvb2w9IkFkb2JlIFBob3Rvc2hvcCBDQyAoTWFjaW50b3NoKSIgeG1wTU06SW5zdGFuY2VJRD0ieG1wLmlpZDpCMTQzREUwMzQ4MUIxMUU1OUIxMkNBODcxRkU2OTAzNCIgeG1wTU06RG9jdW1lbnRJRD0ieG1wLmRpZDpCMTQzREUwNDQ4MUIxMUU1OUIxMkNBODcxRkU2OTAzNCI+IDx4bXBNTTpEZXJpdmVkRnJvbSBzdFJlZjppbnN0YW5jZUlEPSJ4bXAuaWlkOkIxNDNERTAxNDgxQjExRTU5QjEyQ0E4NzFGRTY5MDM0IiBzdFJlZjpkb2N1bWVudElEPSJ4bXAuZGlkOkIxNDNERTAyNDgxQjExRTU5QjEyQ0E4NzFGRTY5MDM0Ii8+IDwvcmRmOkRlc2NyaXB0aW9uPiA8L3JkZjpSREY+IDwveDp4bXBtZXRhPiA8P3hwYWNrZXQgZW5kPSJyIj8+/+4ADkFkb2JlAGTAAAAAAf/bAIQABgQEBAUEBgUFBgkGBQYJCwgGBggLDAoKCwoKDBAMDAwMDAwQDA4PEA8ODBMTFBQTExwbGxscHx8fHx8fHx8fHwEHBwcNDA0YEBAYGhURFRofHx8fHx8fHx8fHx8fHx8fHx8fHx8fHx8fHx8fHx8fHx8fHx8fHx8fHx8fHx8fHx8f/8AAEQgAUABQAwERAAIRAQMRAf/EAEsAAQEAAAAAAAAAAAAAAAAAAAAIAQEAAAAAAAAAAAAAAAAAAAAAEAEAAAAAAAAAAAAAAAAAAAAAEQEAAAAAAAAAAAAAAAAAAAAA/9oADAMBAAIRAxEAPwCqQAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAf//Z";

        /// <summary>
        /// Generate S3Key for Agency's logo image.
        /// {AgencyId}/Logo/Images
        /// </summary>
        public const string BranchLogoImageKey = "{0}/bLogo";

        public const string BranchEmptyLogo = "data:image/jpg;base64,/9j/4QAYRXhpZgAASUkqAAgAAAAAAAAAAAAAAP/sABFEdWNreQABAAQAAAA8AAD/4QMsaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wLwA8P3hwYWNrZXQgYmVnaW49Iu+7vyIgaWQ9Ilc1TTBNcENlaGlIenJlU3pOVGN6a2M5ZCI/PiA8eDp4bXBtZXRhIHhtbG5zOng9ImFkb2JlOm5zOm1ldGEvIiB4OnhtcHRrPSJBZG9iZSBYTVAgQ29yZSA1LjUtYzAxNCA3OS4xNTE0ODEsIDIwMTMvMDMvMTMtMTI6MDk6MTUgICAgICAgICI+IDxyZGY6UkRGIHhtbG5zOnJkZj0iaHR0cDovL3d3dy53My5vcmcvMTk5OS8wMi8yMi1yZGYtc3ludGF4LW5zIyI+IDxyZGY6RGVzY3JpcHRpb24gcmRmOmFib3V0PSIiIHhtbG5zOnhtcD0iaHR0cDovL25zLmFkb2JlLmNvbS94YXAvMS4wLyIgeG1sbnM6eG1wTU09Imh0dHA6Ly9ucy5hZG9iZS5jb20veGFwLzEuMC9tbS8iIHhtbG5zOnN0UmVmPSJodHRwOi8vbnMuYWRvYmUuY29tL3hhcC8xLjAvc1R5cGUvUmVzb3VyY2VSZWYjIiB4bXA6Q3JlYXRvclRvb2w9IkFkb2JlIFBob3Rvc2hvcCBDQyAoTWFjaW50b3NoKSIgeG1wTU06SW5zdGFuY2VJRD0ieG1wLmlpZDpCMTQzREUwMzQ4MUIxMUU1OUIxMkNBODcxRkU2OTAzNCIgeG1wTU06RG9jdW1lbnRJRD0ieG1wLmRpZDpCMTQzREUwNDQ4MUIxMUU1OUIxMkNBODcxRkU2OTAzNCI+IDx4bXBNTTpEZXJpdmVkRnJvbSBzdFJlZjppbnN0YW5jZUlEPSJ4bXAuaWlkOkIxNDNERTAxNDgxQjExRTU5QjEyQ0E4NzFGRTY5MDM0IiBzdFJlZjpkb2N1bWVudElEPSJ4bXAuZGlkOkIxNDNERTAyNDgxQjExRTU5QjEyQ0E4NzFGRTY5MDM0Ii8+IDwvcmRmOkRlc2NyaXB0aW9uPiA8L3JkZjpSREY+IDwveDp4bXBtZXRhPiA8P3hwYWNrZXQgZW5kPSJyIj8+/+4ADkFkb2JlAGTAAAAAAf/bAIQABgQEBAUEBgUFBgkGBQYJCwgGBggLDAoKCwoKDBAMDAwMDAwQDA4PEA8ODBMTFBQTExwbGxscHx8fHx8fHx8fHwEHBwcNDA0YEBAYGhURFRofHx8fHx8fHx8fHx8fHx8fHx8fHx8fHx8fHx8fHx8fHx8fHx8fHx8fHx8fHx8fHx8f/8AAEQgAUABQAwERAAIRAQMRAf/EAEsAAQEAAAAAAAAAAAAAAAAAAAAIAQEAAAAAAAAAAAAAAAAAAAAAEAEAAAAAAAAAAAAAAAAAAAAAEQEAAAAAAAAAAAAAAAAAAAAA/9oADAMBAAIRAxEAPwCqQAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAf//Z";

        /// <summary>
        /// Get AWS.ImageBucketUrl from config file
        /// </summary>
        public static string AWS_ImageBucketUrl = ConfigurationManager.AppSettings["AWS.ImageBucketUrl"];
        public static string ScaleImageMaxHeight = ConfigurationManager.AppSettings["ScaleImage.MaxHeight"];

        public static string CurrencyCode = ConfigurationManager.AppSettings["CurrencyCode"];
        public static string CountryCode = ConfigurationManager.AppSettings["CountryCode"];

        public static string LoginPage
        {
            get
            {
                string _LoginPage = GetServerRoot(HttpContext.Current.Request) + "/mxlg.aspx";
                if (Helper.CountryCode.ToUpper() == "AR")
                    _LoginPage = GetServerRoot(HttpContext.Current.Request) + "/arlg.aspx";

                return _LoginPage;
            }
        }

        #endregion

        #region Encrypt | Decrypt
        public static string Encrypt(string toEncrypt)
        {
            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

            // Get the key from config file
            string key = "DENOVU";

            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
            //Always release the resources and flush data
            // of the Cryptographic service provide. Best Practice

            hashmd5.Clear();

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            //set the secret key for the tripleDES algorithm
            tdes.Key = keyArray;
            //mode of operation. there are other 4 modes.
            //We choose ECB(Electronic code Book)
            tdes.Mode = CipherMode.ECB;
            //padding mode(if any extra byte added)
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateEncryptor();
            //transform the specified region of bytes array to resultArray
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            //Release resources held by TripleDes Encryptor
            tdes.Clear();
            //Return the encrypted data into unreadable string format
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        public static string Decrypt(string cipherString)
        {
            byte[] keyArray;
            //get the byte code of the string

            byte[] toEncryptArray = Convert.FromBase64String(cipherString);

            //Get your key from config file to open the lock!
            string key = "DENOVU";

            //if hashing was used get the hash code with regards to your key
            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
            //release any resource held by the MD5CryptoServiceProvider
            hashmd5.Clear();

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            //set the secret key for the tripleDES algorithm
            tdes.Key = keyArray;
            //mode of operation. there are other 4 modes. 
            //We choose ECB(Electronic code Book)
            tdes.Mode = CipherMode.ECB;
            //padding mode(if any extra byte added)
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            //Release resources held by TripleDes Encryptor                
            tdes.Clear();
            //return the Clear decrypted TEXT
            return UTF8Encoding.UTF8.GetString(resultArray);
        }
        #endregion

        #region Hash

        public static string GetSHA256Hash(string plainText, string salt)
        {
            string hashText = string.Format("{0}{1}", plainText, salt);

            HashAlgorithm hash = new SHA256Managed();

            // Compute hash value of our plain text with appended salt.
            byte[] hashBytes = hash.ComputeHash(Encoding.UTF8.GetBytes(hashText));

            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sBuilder.Append(hashBytes[i].ToString("X2"));
            }

            return sBuilder.ToString();
        }

        public static string CalculateMD5Hash(string input)
        {
            // step 1, calculate MD5 hash from input
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }

        #endregion

        #region BrowserAgent

        public static string GetRequestIP(HttpContext ctx)
        {
            return ctx.Request.UserHostAddress;
        }

        public static string GetRequestAgent(HttpContext ctx)
        {
            return ctx.Request.UserAgent;
        }

        public static string GetRequestDomain(HttpContext ctx)
        {
            if (ctx.Request.Url != null)
            {
                return ctx.Request.Url.Host;
            }
            return string.Empty;
        }

        public static string GetBrowserName(HttpContext ctx)
        {
            string userAgent = ctx.Request.UserAgent ?? string.Empty;

            if (userAgent.Contains("MSIE 5.0"))
            {
                return "Internet Explorer 5";
            }
            else if (userAgent.Contains("MSIE 6.0"))
            {
                return "Internet Explorer 6";
            }
            else if (userAgent.Contains("MSIE 7.0"))
            {
                return "Internet Explorer 7";
            }
            else if (userAgent.Contains("MSIE 8.0"))
            {
                return "Internet Explorer 8";
            }
            else if (userAgent.Contains("Firefox"))
            {
                return userAgent.Substring(userAgent.IndexOf("Firefox")).Replace("/", " ");
            }
            else if (userAgent.Contains("Opera"))
            {
                return userAgent.Substring(userAgent.IndexOf("Opera"));
            }
            else if (userAgent.Contains("Chrome"))
            {
                string agentPart = userAgent.Substring(userAgent.IndexOf("Chrome"));
                return agentPart.Substring(0, agentPart.IndexOf("Safari") - 1).Replace("/", " ");
            }
            else if (userAgent.Contains("Safari"))
            {
                string agentPart = userAgent.Substring(userAgent.IndexOf("Version"));
                string version = agentPart.Substring(0, agentPart.IndexOf("Safari") - 1).Replace("Version/", "");
                return "Safari " + version;
            }
            else if (userAgent.Contains("Konqueror"))
            {
                string agentPart = userAgent.Substring(userAgent.IndexOf("Konqueror"));
                return agentPart.Substring(0, agentPart.IndexOf(";")).Replace("/", " ");
            }
            else if (userAgent.ToLower().Contains("bot") | userAgent.ToLower().Contains("search"))
            {
                return "Search Bot";
            }
            return "Unknown or Bot";
        }

        #endregion

        #region Helper Methods

        public static bool IsValidEmail(string InputEmail)
        {
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(InputEmail);
            return match.Success;
        }

        public static Guid SafeGuid(object value)
        {
            if (value == null || value is DBNull)
                return Guid.Empty;
            else if (value is Guid)
                return (Guid)value;
            else if (value.ToString() == string.Empty)
                return Guid.Empty;
            else
            {
                try
                {
                    string tempGuid = value.ToString().Trim().Replace("-", "").Replace("{", "").Replace("}", "");
                    if (tempGuid.Length == 32)
                        return new Guid(tempGuid);
                    else
                        return Guid.Empty;
                }
                catch
                {
                    return Guid.Empty;
                }
            }

        }

        public static bool IsNumeric(string strToCheck)
        {
            return Regex.IsMatch(strToCheck, "^\\d+(\\.\\d+)?$");
        }

        public static string Left(string str, int count)
        {
            if (str == null)
            {
                return "";
            }
            if (str.Length >= count)
            {
                return str.Substring(0, count);
            }
            return str;
        }

        public static DateTime? SafeDate(object s)
        {
            if (s.GetType() == typeof(DateTime))
                return Convert.ToDateTime(s);
            else
                return null;
        }

        public static DateTime SafeDate(string date)
        {
            DateTime tmp = new DateTime(1900, 1, 1);
            if (string.IsNullOrEmpty(date) == true) return tmp;
            DateTime.TryParse(date, out tmp);
            return tmp;
        }

        public static DateTime MXSafeDate(string date)
        {
            string[] arr = date.Split("/".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            DateTime tmp = new DateTime(1900, 1, 1);
            if (arr.Length != 3)
                return tmp;

            date = string.Format("{0:00}/{1:00}/{2:0000}", int.Parse(arr[0]), int.Parse(arr[1]), int.Parse(arr[2]));

            if (string.IsNullOrEmpty(date) == true) return tmp;
            if (!DateTime.TryParseExact(date, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out tmp))
            {
                if (!DateTime.TryParseExact(date, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out tmp))
                    tmp = new DateTime(1900, 1, 1);
            }

            return tmp;
        }

        public static DateTime SafeFromDate(DateTime date)
        {
            try
            {
                return DateTime.Parse(date.Month + "/" + date.Day + "/" + date.Year + " 12:00:00 AM");
            }
            catch
            {
            }

            return date;
        }

        public static DateTime SafeToDate(DateTime date)
        {
            try
            {
                return DateTime.Parse(date.Month + "/" + date.Day + "/" + date.Year + " 11:59:59 PM");
            }
            catch
            {
            }

            return date;
        }

        public static string SafeStringNoScript(object s)
        {
            var str = SafeString(s);
            return Regex.Replace(str, "[^0-9a-zA-Z_]+", "");
        }

        public static string SafeString(object s)
        {
            if (s == null || s.GetType() == typeof(DBNull))
            {
                return "";
            }
            else if (s.GetType() == typeof(DateTime) && Convert.ToDateTime(s) == DateTime.MinValue)
            {
                return "";
            }
            else if (s.GetType() == typeof(Guid))
            {
                return s.ToString();
            }
            else
                return Convert.ToString(s).Trim();
        }

        public static bool SafeBoolean(object s)
        {
            if (s == null) return false;
            if (s.GetType() == typeof(Boolean))
            {
                return Convert.ToBoolean(s);
            }
            string s2;
            s2 = SafeString(s);
            if (s2 == "") return false;
            if (IsNumeric(s2))
                return SafeInteger(s2) == 1 ? true : false;
            else
                return s2.ToLower() == "true" || s2.ToLower() == "yes" || s2.ToLower() == "y" ? true : false;
        }

        public static int SafeInteger(object s)
        {
            int result = 0;
            int.TryParse(SafeString(s), out result);
            return result;
        }

        public static short SafeShort(object s)
        {
            short result = 0;
            short.TryParse(SafeString(s), out result);
            return result;
        }

        public static int SafeIntegerV2(object s)
        {
            if (string.IsNullOrEmpty(SafeString(s))) return -1;
            int result = -1;
            int.TryParse(SafeString(s), out result);
            return result;
        }

        public static float SafeSingle(object s)
        {
            string s2 = SafeString(s);
            float f = 0;
            float.TryParse(s2, out f);
            return f;
        }

        public static Decimal SafeDecimal(object s)
        {
            string s2;
            s2 = SafeString(s);
            if (s2 == "") return 0;

            decimal result = 0;
            decimal.TryParse(s2, out result);
            return result;
        }

        public static double SafeDouble(object s)
        {
            string s2;
            s2 = SafeString(s);
            if (s2 == "") return 0;

            double result = 0;
            double.TryParse(s2, out result);
            return result;
        }

        public static string FormatPhoneNumber(string psUnformattedPhone)
        {
            if (psUnformattedPhone.Length == 9)
                psUnformattedPhone = "0" + psUnformattedPhone;

            if (psUnformattedPhone.Length > 10)
                return psUnformattedPhone;

            return Regex.Replace(SafePhone(psUnformattedPhone), "(\\d{3})(\\d{3})(\\d{4})", "$1-$2-$3");
        }

        public static string NormalizePhoneNumberMexico(string phone)
        {
            if (string.IsNullOrEmpty(phone))
                return phone;

            // remove all non-digits
            Regex reg = new Regex("\\D");
            phone = reg.Replace(phone, "");

            if (phone.StartsWith("52"))
                phone = phone.Substring(2);

            if (phone.Length == 10)
            {
                if (phone.StartsWith("33") || phone.StartsWith("55") || phone.StartsWith("81"))
                    return Regex.Replace(phone, "(\\d{2})(\\d{4})([\\d]+)", "($1) $2-$3");

                return Regex.Replace(phone, "(\\d{3})(\\d{3})([\\d]+)", "($1) $2-$3");
            }

            return phone;
        }

        public static string SafePhone(string psPhone)
        {
            if (string.IsNullOrEmpty(psPhone)) return "";
            string sPhone = Regex.Replace(psPhone.ToUpper(), "[^0-9A-Z]", "");

            //800-NEW-HONDA -> 800-639-46632: by keyboard mobile phone
            sPhone = Regex.Replace(sPhone, "[ABC]", "2");
            sPhone = Regex.Replace(sPhone, "[DEF]", "3");
            sPhone = Regex.Replace(sPhone, "[GHI]", "4");
            sPhone = Regex.Replace(sPhone, "[JKL]", "5");
            sPhone = Regex.Replace(sPhone, "[MNO]", "6");
            sPhone = Regex.Replace(sPhone, "[PQRS]", "7");
            sPhone = Regex.Replace(sPhone, "[TVU]", "8");
            sPhone = Regex.Replace(sPhone, "[WXYZ]", "9");

            if (sPhone.StartsWith("1") && sPhone.Length > 10)
                sPhone = sPhone.Substring(1);

            if (sPhone.Length > 10)
                sPhone = sPhone.Substring(0, 10);

            return sPhone;
        }

        public static string SafeDirectory(string filePath)
        {
            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);
            return filePath;
        }

        public static string StripTagsCharArray(string source)
        {
            char[] array = new char[source.Length];
            int arrayIndex = 0;
            bool inside = false;

            for (int i = 0; i < source.Length; i++)
            {
                char let = source[i];
                if (let == '<')
                {
                    inside = true;
                    continue;
                }
                if (let == '>')
                {
                    inside = false;
                    continue;
                }
                if (!inside)
                {
                    array[arrayIndex] = let;
                    arrayIndex++;
                }
            }
            return new string(array, 0, arrayIndex);
        }

        public static string FormatDecimal(object price)
        {
            decimal dPrice = SafeDecimal(price);
            return string.Format("{0:C0}", dPrice).Replace("$", "");
        }

        public static string SafeGetServerPath()
        {
            string path = "";

            try
            {
                path = HttpContext.Current.Server.MapPath("~");
            }
            catch { }

            return path;
        }

        public static string RenderLogDescription(string description)
        {
            StringBuilder builder = new StringBuilder();
            string[] oDesc = description.Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            int count = 0;
            string title = "";

            foreach (string item in oDesc)
            {
                string sortItem = item.Replace(" 12:00:00 AM", "");
                if (sortItem.Length > 60)
                    sortItem = sortItem.Substring(0, 60) + "...";
                title += item + "\r";

                if (count < 4)
                {
                    builder.AppendFormat("<div style='white-space: nowrap;'>{0}</div>", HttpContext.Current.Server.HtmlEncode(sortItem));
                }

                count++;
            }

            return string.Format("<div data-toggle='tooltip' title=\"{1}\">{0}</div>", builder.ToString(), HttpContext.Current.Server.HtmlEncode(title));
        }

        public static bool IsUrlValid(string url)
        {

            string pattern = @"^(https?:\/\/(?:www\.|(?!www))[^\s\.\/\,]+\.[^\s\/\,]{2,}(\/[^\s\/]+)*|www\.[^\s\/\,]+\.[^\s\/\,]{2,}(\/[^\s\/\,]+)*)$";
            System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(pattern, System.Text.RegularExpressions.RegexOptions.Compiled | System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            return reg.IsMatch(url);
        }

        public static bool IsPhoneNume(string phoneNum)
        {
            if (string.IsNullOrEmpty(phoneNum))
                return false;

            Regex regex = new Regex(@"^\+?(\d[\d-. ]+)?(\([\d-. ]+\))?[\d-. ]+\d$");
            return regex.IsMatch(phoneNum);
        }

        public static string TrimImageUrls(string imageUrl, int maxLength)
        {
            if (string.IsNullOrEmpty(imageUrl))
                return string.Empty;

            if (imageUrl.Length > maxLength)
            {
                int pos = imageUrl.LastIndexOf("|", maxLength);
                imageUrl = imageUrl.Substring(0, pos);
            }

            return imageUrl;
        }

        public static string EncodeHTMLString(string input)
        {
            return HttpContext.Current.Server.HtmlEncode(input);
        }

        public static string ToCamelCase(string input)
        {
            input = System.Text.RegularExpressions.Regex.Replace(input, "[A-Z]", " $0");
            return input;
        }

        public static string GetSortString(string input, int len)
        {
            if (input == null)
                return "";

            if (input.Length > len)
                return input.Substring(0, len - 4) + " ...";
            else
                return input;
        }

        public static string ToTitleCase(string str)
        {
            string result = str;
            if (!string.IsNullOrEmpty(str))
            {
                var words = str.Split(' ');
                for (int index = 0; index < words.Length; index++)
                {
                    var s = words[index];
                    if (s.Length > 0)
                    {
                        words[index] = s[0].ToString().ToUpper() + s.Substring(1);
                    }
                }
                result = string.Join(" ", words);
            }
            return result;
        }

        public static string GetFileNameFromUrl(string url)
        {
            Uri uri = new Uri(url);
            try
            {
                return System.IO.Path.GetFileName(uri.LocalPath);
            }
            catch (Exception) { }

            return "";
        }

        public static string GetExentsionFileNameFromUrl(string url)
        {
            Uri uri = new Uri(url);
            try
            {
                return System.IO.Path.GetExtension(uri.LocalPath);
            }
            catch (Exception) { }

            return "";
        }

        public static string SecondsToMinute(double seconds)
        {
            TimeSpan t = TimeSpan.FromSeconds(seconds);
            string minute = string.Format("{0:D2}:{1:D2} (mm:ss)", t.Minutes, t.Seconds);
            return minute;
        }

        public static string TranslateCallStatus(string callStatus)
        {
            if (CombineUserSession.CurrentCulture == MXCultures.es_MX)
            {
                switch (callStatus.ToLower())
                {
                    case "answered":
                        return LoadString("Answered");

                    case "missed call":
                        return LoadString("MissCall");

                    default:
                        return callStatus;
                }
            }
            else
            {
                return callStatus;
            }
        }

        #endregion

        #region Others

        public static string GetStateName(string state)
        {
            Dictionary<string, string> states = new Dictionary<string, string>();

            states.Add("AL", "Alabama");
            states.Add("AK", "Alaska");
            states.Add("AZ", "Arizona");
            states.Add("AR", "Arkansas");
            states.Add("CA", "California");
            states.Add("CO", "Colorado");
            states.Add("CT", "Connecticut");
            states.Add("DE", "Delaware");
            states.Add("DC", "District of Columbia");
            states.Add("FL", "Florida");
            states.Add("GA", "Georgia");
            states.Add("HI", "Hawaii");
            states.Add("ID", "Idaho");
            states.Add("IL", "Illinois");
            states.Add("IN", "Indiana");
            states.Add("IA", "Iowa");
            states.Add("KS", "Kansas");
            states.Add("KY", "Kentucky");
            states.Add("LA", "Louisiana");
            states.Add("ME", "Maine");
            states.Add("MD", "Maryland");
            states.Add("MA", "Massachusetts");
            states.Add("MI", "Michigan");
            states.Add("MN", "Minnesota");
            states.Add("MS", "Mississippi");
            states.Add("MO", "Missouri");
            states.Add("MT", "Montana");
            states.Add("NE", "Nebraska");
            states.Add("NV", "Nevada");
            states.Add("NH", "New Hampshire");
            states.Add("NJ", "New Jersey");
            states.Add("NM", "New Mexico");
            states.Add("NY", "New York");
            states.Add("NC", "North Carolina");
            states.Add("ND", "North Dakota");
            states.Add("OH", "Ohio");
            states.Add("OK", "Oklahoma");
            states.Add("OR", "Oregon");
            states.Add("PA", "Pennsylvania");
            states.Add("RI", "Rhode Island");
            states.Add("SC", "South Carolina");
            states.Add("SD", "South Dakota");
            states.Add("TN", "Tennessee");
            states.Add("TX", "Texas");
            states.Add("UT", "Utah");
            states.Add("VT", "Vermont");
            states.Add("VA", "Virginia");
            states.Add("WA", "Washington");
            states.Add("WV", "West Virginia");
            states.Add("WI", "Wisconsin");
            states.Add("WY", "Wyoming");

            if (states.ContainsKey(state))
                return (states[state]);
            else
                return "";
        }

        static int _MediaVersionn = 0;
        public static int MediaVersion
        {
            get
            {
                if (_MediaVersionn > 0)
                    return _MediaVersionn;

                using (MXUnit mx = new MXUnit())
                {
                    var kv = mx.ServiceUnit.GetKeyValue("MX", "Version");
                    if (kv != null)
                    {
                        string version = string.Format("{0:yyMMddHHmm}", kv.DateUpdated);
                        long lVersion = 0;
                        long.TryParse(version, out lVersion);

                        _MediaVersionn = (int)(lVersion % Int32.MaxValue);
                    }
                }
                return _MediaVersionn;
            }
        }

        public static string GetServerRoot(HttpRequest Request)
        {
            string strResult = "";
            string strHostName = Request.Url.Host;
            string strRoot = Request.ApplicationPath;
            string strPort = Request.Url.Port.ToString();
            strPort = strPort == "80" ? "" : ":" + strPort;
            strRoot = strRoot == "/" ? "" : strRoot;
            strResult = Request.Url.Scheme + "://" + strHostName + strPort + strRoot;
            return strResult;
        }

        public static string GetHostName(HttpRequest Request)
        {
            string strResult = "";
            string strHostName = Request.Url.Host;
            string strPort = Request.Url.Port.ToString();
            strPort = strPort == "80" ? "" : ":" + strPort;
            strResult = Request.Url.Scheme + "://" + strHostName + strPort;
            return strResult;
        }

        public static string RenderUserRoleDDL(string userType, int branchID)
        {
            string selectedValue = "";
            if (userType == "Admin" && branchID == 0)
                selectedValue = "HA";
            if (userType == "Admin" && branchID > 0)
                selectedValue = "BA";
            if (userType == "Agent")
                selectedValue = "BU";

            StringBuilder builder = new StringBuilder();
            builder.AppendLine("<option " + (selectedValue == "HA" ? "selected='selected'" : "") + " value='HA'>" + LoadString("AgencyAdmin") + "</option>");
            builder.AppendLine("<option " + (selectedValue == "BA" ? "selected='selected'" : "") + " value='BA'>" + LoadString("BranchAdmin") + "</option>");
            builder.AppendLine("<option " + (selectedValue == "BU" ? "selected='selected'" : "") + " value='BU'>" + LoadString("BranchAgent") + "</option>");

            return builder.ToString();
        }

        public static string RenderUserRoleDDLV2(string userType, int branchID, bool ShowHAInRole)
        {
            string selectedValue = "";
            if (userType == "Admin" && branchID == 0)
                selectedValue = "HA";
            if (userType == "Admin" && branchID > 0)
                selectedValue = "BA";
            if (userType == "Agent")
                selectedValue = "BU";

            StringBuilder builder = new StringBuilder();
            if (ShowHAInRole)
                builder.AppendLine("<option " + (selectedValue == "HA" ? "selected='selected'" : "") + " value='HA'>" + LoadString("AgencyAdmin") + "</option>");

            builder.AppendLine("<option " + (selectedValue == "BA" ? "selected='selected'" : "") + " value='BA'>" + LoadString("BranchAdmin") + "</option>");
            builder.AppendLine("<option " + (selectedValue == "BU" ? "selected='selected'" : "") + " value='BU'>" + LoadString("BranchAgent") + "</option>");

            return builder.ToString();
        }

        public static string UserRoleToString(string userType, int branchID)
        {
            string result = "";

            if (userType == "Admin" && branchID == 0)
                result = "Agency Admin";

            if (userType == "Admin" && branchID != 0)
                result = "Branch Admin";

            if (userType == "Agent")
                result = "Branch Agent";

            return result;
        }

        public static string MXCultureToString(MXCultures culture)
        {
            string result = "";
            switch (culture)
            {
                case MXCultures.en_US: result = "English"; break;
                case MXCultures.af_ZA: result = "Afrikaans"; break;
                case MXCultures.es_MX: result = "Spanish (Mexico)"; break;
                case MXCultures.es_AR: result = "Spanish (Argentina)"; break;
            }

            return result;
        }

        public static string MXCultureToValue(MXCultures culture)
        {

            switch (culture)
            {
                case MXCultures.en_US: return "en-US";
                case MXCultures.af_ZA: return "af-ZA";
                case MXCultures.es_MX: return "es-MX";
                case MXCultures.es_AR: return "es-AR";
                default: return "en-US";
            }
        }

        public static string AutomaticallySubmitToString(AutomaticallySubmit status)
        {
            string result = "";

            switch (status)
            {
                case AutomaticallySubmit.None: result = LoadString("None"); break;
                case AutomaticallySubmit.NewestProperty: result = LoadString("NewestProperties"); break;
                case AutomaticallySubmit.OlddestProperties: result = LoadString("OldestProperties"); break;
            }

            return result;
        }

        public static string RenderBranchAutomaticallySubmitDDL(AutomaticallySubmit selectedValue)
        {
            StringBuilder b = new StringBuilder();

            foreach (AutomaticallySubmit status in Enum.GetValues(typeof(AutomaticallySubmit)))
            {
                string selected = status == selectedValue ? "selected='selected'" : "";
                b.AppendLine("<option value='" + (byte)status + "' " + selected + ">" + AutomaticallySubmitToString(status) + "</option>");
            }

            return b.ToString();
        }

        public static bool IsAjaxRequest(System.Web.HttpRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }
            var context = System.Web.HttpContext.Current;
            var isCallbackRequest = false;
            // callback requests are ajax requests
            if (context != null && context.CurrentHandler != null && context.CurrentHandler is System.Web.UI.Page)
            {
                isCallbackRequest = ((System.Web.UI.Page)context.CurrentHandler).IsCallback;
            }
            return isCallbackRequest || (request["X-Requested-With"] == "XMLHttpRequest") || (request.Headers["X-Requested-With"] == "XMLHttpRequest");
        }

        public static string EncodeJsString(object obj)
        {
            if (obj == null)
                return string.Empty;
            string s = obj.ToString();
            StringBuilder sb = new StringBuilder();
            foreach (char c in s)
            {
                switch (c)
                {
                    case '\'':
                        sb.Append("\\\'");
                        break;
                    case '\"':
                        sb.Append("\\\"");
                        break;
                    case '\\':
                        sb.Append("\\\\");
                        break;
                    case '\b':
                        sb.Append("\\b");
                        break;
                    case '\f':
                        sb.Append("\\f");
                        break;
                    case '\n':
                        sb.Append("\\n");
                        break;
                    case '\r':
                        sb.Append("\\r");
                        break;
                    case '\t':
                        sb.Append("\\t");
                        break;
                    default:
                        int i = (int)c;
                        if (i < 32 || i > 127)
                        {
                            sb.AppendFormat("\\u{0:X04}", i);
                        }
                        else
                        {
                            sb.Append(c);
                        }
                        break;
                }
            }
            return sb.ToString();
        }

        public static string ListingTypeARToString(int type)
        {
            CategoryArgentina cat = (CategoryArgentina)type;
            switch (cat)
            {
                case CategoryArgentina.VentaDeDepartamentos:
                    return "Venta De Departamentos";
                case CategoryArgentina.AlquilerDeDepartamentos:
                    return "Alquiler De Departamentos";
                case CategoryArgentina.AlquileresTemporariosDeDepartamentos:
                    return "Alquileres Temporarios De Departamentos";
                case CategoryArgentina.VentaDeCasas:
                    return "Venta De Casas";
                case CategoryArgentina.AlquilerDeCasas:
                    return "Alquiler De Casas";
                case CategoryArgentina.AlquileresTemporariosDeCasas:
                    return "Alquileres Temporarios De Casas";
                case CategoryArgentina.VentaDeLocalesYOficinas:
                    return "Venta De Locales Y Oficinas";
                case CategoryArgentina.AlquilerDeLocalesYOficinas:
                    return "Alquiler De Locales Y Oficinas";
                case CategoryArgentina.VentaDeCocheras:
                    return "Venta De Cocheras";
                case CategoryArgentina.AlquilerDeCocheras:
                    return "Alquiler De Cocheras";
                case CategoryArgentina.VentaDeLotesYTerrenos:
                    return "Venta DeLotes Y Terrenos";
                case CategoryArgentina.AlquilerDeLotesYTerrenos:
                    return "Alquiler De Lotes Y Terrenos";
                case CategoryArgentina.VentaDeCampos:
                    return "Venta De Campos";
                case CategoryArgentina.AlquilerDeCampos:
                    return "Alquiler De Campos";
                case CategoryArgentina.VentaDeDepositosYGalpones:
                    return "Venta de Depósitos y Galpones";
                case CategoryArgentina.AlquilerDeDepositosYGalpones:
                    return "Alquiler de Depósitos y Galpones";
                case CategoryArgentina.VentaDeBovedasNichosYParcelas:
                    return "Venta de Bóvedas,Nichos y Parcelas";
                case CategoryArgentina.AlquilerDeBovedasNichosYParcelas:
                    return "Alquiler de Bóvedas,Nichos y Parcelas";
                default:
                    return "";
            }
        }

        public static string ListingTypeToString(int type)
        {
            if (Helper.CountryCode == "AR")
                return ListingTypeARToString(type);

            CategoryMexico cat = (CategoryMexico)type;
            switch (cat)
            {
                case CategoryMexico.Bodegas:
                    return "Bodegas";
                case CategoryMexico.CuartosEnRenta:
                    return "Cuartos en renta";
                case CategoryMexico.InmueblesEnRenta:
                    return "Inmuebles en renta";
                case CategoryMexico.InmueblesEnVenta:
                    return "Inmuebles en venta";
                case CategoryMexico.PropiedadesComerciales_LocalesComerciales:
                    return "Propiedades comerciales - Locales comerciales";
                case CategoryMexico.PropiedadesComerciales_OficinasYconsultorios:
                    return "Propiedades comerciales - Oficinas y consultorios";
                case CategoryMexico.PropiedadesComerciales_Traspasos:
                    return "Propiedades comerciales - Traspasos";
                case CategoryMexico.RentasVacacionales:
                    return "Rentas vacacionales";
                case CategoryMexico.TerrenosEnVenta:
                    return "Terrenos en venta";
                default:
                    return "";
            }
        }

        public static string PropertyTypeToString(short listingType, string propertyType)
        {
            CategoryMexico cat = (CategoryMexico)listingType;
            switch (cat)
            {
                case CategoryMexico.InmueblesEnVenta:
                    if (propertyType == "house")
                        return "Casa";
                    else if (propertyType == "apartment")
                        return "Departamento";
                    else if (propertyType == "development")
                        return "Desarrollo";
                    else if (propertyType == "foreclosure")
                        return "Remate Hipotecario";
                    break;
                case CategoryMexico.InmueblesEnRenta:
                    if (propertyType == "house")
                        return "Casa";
                    else if (propertyType == "apartment")
                        return "Departamento";
                    break;
                case CategoryMexico.RentasVacacionales:
                    if (propertyType == "house")
                        return "Casa";
                    else if (propertyType == "apartment")
                        return "Departamento";
                    break;
                default:
                    return "";
            }
            return "";
        }

        public static string RenderPageSizeDDL(int selectedValue)
        {
            int[] pageSizes = PAGE_SIZES.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Select(x => SafeInteger(x)).ToArray();
            StringBuilder builder = new StringBuilder();

            foreach (int size in pageSizes)
            {
                string selected = size == selectedValue ? "selected='selected'" : "";
                builder.AppendFormat("<option value='{0}' {1}>{0}</option>", size, selected);
            }

            return builder.ToString();
        }

        public static DateTime FromMXTime(DateTime dateObject)
        {
            if (dateObject == null)
                return new DateTime(1900, 1, 1);

            var saTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time (Mexico)");
            return TimeZoneInfo.ConvertTime(dateObject, saTimeZone, TimeZoneInfo.Local);
        }

        public static DateTime ToMXTime(DateTime dateObject)
        {
            if (dateObject == null)
                return new DateTime(1900, 1, 1);

            var saTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time (Mexico)");
            return TimeZoneInfo.ConvertTime(dateObject, saTimeZone);
        }

        public static string FormatCurrency4WP(object obj)
        {
            double value = 0;
            if (obj == null)
                return "";
            double.TryParse(obj.ToString(), out value);
            CultureInfo cultureInfo = new CultureInfo("es-MX");

            return value.ToString("C0", cultureInfo);
        }

        public static string RemoveTail(string src, string remove)
        {
            if (src.EndsWith(remove))
            {
                int lastIndex = src.LastIndexOf(remove);
                if (lastIndex >= 0)
                {
                    return src.Substring(0, lastIndex);
                }
            }

            return src;
        }

        public static string MXDisplayDateTimeFromUTC(object obj)
        {
            if (obj == null)
                return "";

            var dt = SafeDate(obj.ToString());

            if (dt.Year <= 1900)
                return "";

            return UtcToMXTime(dt).ToString("dd/MM/yy h:mm tt");
        }

        public static string MXDisplayDateTimeFromUTC(DateTime utcTime)
        {
            if (utcTime == null || utcTime.Year <= 1900)
                return "";

            return UtcToMXTime(utcTime).ToString("dd/MM/yy h:mm tt");

        }

        public static DateTime FromMXTimeToUtc(DateTime dateObject)
        {
            if (dateObject == null)
                return new DateTime(1900, 1, 1);

            var saTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time (Mexico)");
            return TimeZoneInfo.ConvertTime(dateObject, saTimeZone, TimeZoneInfo.Utc);
        }

        public static DateTime UtcToMXTime(DateTime dateObject)
        {
            if (dateObject == null)
                return new DateTime(1900, 1, 1);

            var saTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Central Standard Time (Mexico)");
            return TimeZoneInfo.ConvertTime(dateObject, TimeZoneInfo.Utc, saTimeZone);
        }

        public static void LoadDatesFromDateString(string selectedDate, string leadDay, ref DateTime fromDate, ref DateTime toDate)
        {
            string[] oDates = selectedDate.Split("-".ToCharArray());
            if (oDates.Length == 2)
            {
                fromDate = MXSafeDate(SafeString(oDates[0]).Trim());
                toDate = MXSafeDate(SafeString(oDates[1]).Trim());

                if (!string.IsNullOrEmpty(leadDay))
                {
                    // leadDay is MM/dd
                    fromDate = Helper.MXSafeDate(leadDay + "/" + fromDate.Year);
                    toDate = Helper.MXSafeDate(leadDay + "/" + toDate.Year);
                }

                fromDate = fromDate.Date;
                toDate = toDate.Date.AddDays(1).AddMilliseconds(-1);

                // conver to UTC
                fromDate = FromMXTimeToUtc(fromDate);
                toDate = FromMXTimeToUtc(toDate);
            }
        }

        public static void LoadLeadTypeFromLeadTypeString(string leadType, string excepLeadType, ref List<LeadType> leadTypes, ref List<LeadType> exceptLeadTypes)
        {
            #region Initial LeadTypes for filter

            if (!string.IsNullOrEmpty(leadType))
            {
                if (leadType == "Email")
                    leadTypes.Add(LeadType.Email);
                if (leadType == "Phone")
                    leadTypes.Add(LeadType.Phone);
            }

            //=====================================================================//

            exceptLeadTypes.Add(LeadType.SMS);

            if (!string.IsNullOrEmpty(excepLeadType))
            {
                string[] oExceptLeadTypes = excepLeadType.Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                foreach (string type in oExceptLeadTypes)
                {
                    if (type == "Email")
                        exceptLeadTypes.Add(LeadType.Email);
                    else if (type == "Phone")
                        exceptLeadTypes.Add(LeadType.Phone);
                }
            }

            #endregion
        }

        public static string AdActionStatusTypeTostring(AdActionStatusType type)
        {
            string result = "";

            switch (type)
            {
                case AdActionStatusType.APIError: result = "API Error"; break;
                case AdActionStatusType.MaxAdsReached: result = "Max Ads Reached"; break;
                case AdActionStatusType.Pending: result = "Pending"; break;
                case AdActionStatusType.Success: result = "Success"; break;
                case AdActionStatusType.TimeOut: result = "TimeOut"; break;
            }

            return result;
        }

        public static string AdActionTypeToString(AdActionType type)
        {
            string result = "";

            switch (type)
            {
                case AdActionType.BumpUp: result = LoadString("BumpUp"); break;
                case AdActionType.Highlight: result = LoadString("Highlight"); break;
                case AdActionType.HomepageGallery: result = LoadString("HomepageGallery"); break;
                case AdActionType.TopAd31: result = LoadString("TopAd31"); break;
                case AdActionType.TopAd7: result = LoadString("TopAd7"); break;
                case AdActionType.Urgent: result = LoadString("Urgent"); break;
                case AdActionType.srpgallery31: result = LoadString("srpgallery31"); break;

                case AdActionType.CreateAd: result = "Create Ad"; break;
                case AdActionType.DeleteAd: result = "Delete Ad"; break;
                case AdActionType.EditAd: result = "Edit Ad"; break;
                case AdActionType.Unknown: result = "Unknown"; break;
            }

            return result;
        }

        public static string RenderPropertyStatus(string listingStatus, string adID, bool isShowListingStatusText)
        {
            string status = "";
            string color = "";
            string loc = "";

            if (!string.IsNullOrEmpty(listingStatus))
            {

                switch (listingStatus.ToLower())
                {
                    case "active":
                        loc = LoadString("Active");
                        if (!string.IsNullOrEmpty(adID))
                            color = "green";
                        else
                            color = "yellow";
                        break;
                    case "cancelled":
                        loc = LoadString("Cancelled");
                        color = "gray";
                        break;
                    case "sold":
                        loc = LoadString("Sold");
                        color = "gray";
                        break;
                    default:
                        loc = listingStatus;
                        color = "orange";
                        break;
                }
            }

            if (isShowListingStatusText)
            {
                status = string.Format("<span><i class='fa fa-circle {0}'></i>&nbsp;&nbsp;{1}</span>", color, GetSortString(ToTitleCase(loc), 14));
            }
            else
            {
                status = string.Format("<div class='rectangle {0}'></div>", color);
            }

            return status;
        }

        public static string PhoneTrackingLogActionToString(object obj)
        {
            string result = "";
            PhoneTrackingLogAction action = (PhoneTrackingLogAction)Helper.SafeInteger(obj);

            switch (action)
            {
                case PhoneTrackingLogAction.Add: result = "Add"; break;
                case PhoneTrackingLogAction.Delete: result = "Delete"; break;
                case PhoneTrackingLogAction.Update: result = "Update"; break;
            }

            return result;
        }

        public static string GetSafeSubString(string input, int len)
        {
            if (input.Length > len)
                return input.Substring(0, len);
            else
                return input;
        }

        #endregion

        #region Agency Status

        public static string AgencyStatusToString(BillableStatus status)
        {
            string result = "";

            switch (status)
            {
                case BillableStatus.JustArrived:
                    result = LoadString("JustArrived");
                    break;
                case BillableStatus.Active:
                    result = LoadString("Active");
                    break;
                case BillableStatus.Suspended:
                    result = LoadString("Suspended");
                    break;
                case BillableStatus.Cancelled:
                    result = LoadString("Cancelled");
                    break;
                case BillableStatus.DataFeed:
                    result = LoadString("DataFeed");
                    break;

            }

            return result;
        }

        public static string AgencyStatusToString(object obj)
        {
            BillableStatus status = BillableStatus.Active;

            if (obj.GetType() == typeof(Int32))
            {
                Int32 bStatus = (Int32)obj;
                status = (BillableStatus)bStatus;
            }
            else
            {
                status = (BillableStatus)obj;
            }
            string result = "";

            switch (status)
            {
                case BillableStatus.JustArrived:
                    result = LoadString("JustArrived");
                    break;
                case BillableStatus.Active:
                    result = LoadString("Active");
                    break;
                case BillableStatus.Suspended:
                    result = LoadString("Suspended");
                    break;
                case BillableStatus.Cancelled:
                    result = LoadString("Cancelled");
                    break;
                case BillableStatus.DataFeed:
                    result = LoadString("DataFeed");
                    break;

            }

            return result;
        }

        public static string RenderAgencyStatusDDL(int selectedValue)
        {
            StringBuilder b = new StringBuilder();

            foreach (BillableStatus status in Enum.GetValues(typeof(BillableStatus)))
            {
                string selected = status == (BillableStatus)selectedValue ? "selected='selected'" : "";
                b.AppendLine("<option value='" + (int)status + "' " + selected + ">" + AgencyStatusToString(status) + "</option>");
            }

            return b.ToString();
        }

        #endregion

        #region Email

        public static string RandomString(int intSize)
        {
            StringBuilder objStringBuilder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < intSize; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                objStringBuilder.Append(ch);
            }

            return objStringBuilder.ToString();
        }

        public static string GenerateSuperAdminEmailTemplate(string emailTo, string newPassword, string strServerRoot)
        {
            String strBody = string.Empty;
            EmailEngine objEmailEngine = new EmailEngine();
            strBody = objEmailEngine.ReadTemplate(EmailUserTemplateFile);

            strBody = strBody.Replace("#link#", strServerRoot);
            strBody = strBody.Replace("#Email#", emailTo);
            strBody = strBody.Replace("#Password#", newPassword);

            return strBody;
        }

        public static string GenerateAgentEmailTemplate(string emailTo, string newPassword, string strServerRoot, string fullName)
        {
            String strBody = string.Empty;
            EmailEngine objEmailEngine = new EmailEngine();
            strBody = objEmailEngine.ReadTemplate(EmailAgentlUserTemplateFile);

            strBody = strBody.Replace("#link#", strServerRoot);
            strBody = strBody.Replace("#Email#", emailTo);
            strBody = strBody.Replace("#Password#", newPassword);
            strBody = strBody.Replace("#FullName#", fullName);

            return strBody;
        }

        public static string GenerateForgotPwdEmailTemplate(string resetPwdUrl, int expiry)
        {
            String strBody = string.Empty;
            EmailEngine objEmailEngine = new EmailEngine();
            strBody = objEmailEngine.ReadTemplate(ForgotPwdEmailTemplateFile);

            strBody = strBody.Replace("#link#", resetPwdUrl);
            strBody = strBody.Replace("#expiry#", expiry.ToString());
            return strBody;
        }

        public static string GenerateForgotPwdEmailTemplate(string userName, string resetPwdUrl, int expiry)
        {
            String strBody = string.Empty;
            EmailEngine objEmailEngine = new EmailEngine();
            strBody = objEmailEngine.ReadTemplate(ForgotPwdEmailTemplateFile);

            strBody = strBody.Replace("#USERNAME#", userName);
            strBody = strBody.Replace("#link#", resetPwdUrl);
            strBody = strBody.Replace("#expiry#", expiry.ToString());
            return strBody;
        }

        public static string GenerateSetPwdEmailTemplate(string userName, string resetPwdUrl, int expiry)
        {
            String strBody = string.Empty;
            EmailEngine objEmailEngine = new EmailEngine();
            strBody = objEmailEngine.ReadTemplate(SetPwdEmailTemplateFile);

            strBody = strBody.Replace("#link#", resetPwdUrl);
            strBody = strBody.Replace("#USERNAME#", userName);

            if (expiry != 0)
                strBody = strBody.Replace("#expiry#", expiry.ToString());
            return strBody;
        }

        public static void SendEmailWhenActiveBranch(string activateBy, Branch model)
        {
            try
            {
                if (model == null || IsLocal)
                    return;

                string adPackage = "";
                using (MXUnit DbUnit = new MXUnit())
                {
                    adPackage = DbUnit.PaymentUnit.GetCurrentBranchPackageNameForBranch(model.BranchID);
                }

                string ipAddress = GetIpAddress(HttpContext.Current.Request);
                string subject = string.Format("ProTool MX branch activated: {0}", model.BranchName);
                string body = "<table>" +
                              "     <tr>" +
                              "         <td style='padding: 5px;'>Branch ID: </td><td style='padding: 5px;'>" + model.BranchID + "</td>" +
                              "     </tr>" +
                              "     <tr>" +
                              "         <td style='padding: 5px;'>Branch Name: </td><td style='padding: 5px;'>" + model.BranchName + "</td>" +
                              "     </tr>" +
                              "     <tr>" +
                              "         <td style='padding: 5px;'>Branch Number: </td><td style='padding: 5px;'>" + model.BranchNumber + "</td>" +
                              "     </tr>" +
                              "     <tr>" +
                              "         <td style='padding: 5px;'>&nbsp;</td><td style='padding: 5px;'>&nbsp;</td>" +
                              "     </tr>" +
                              "     <tr>" +
                              "         <td style='padding: 5px;'>Activate by: </td><td style='padding: 5px;'>" + activateBy + "</td>" +
                              "     </tr>" +
                              "     <tr>" +
                              "         <td style='padding: 5px;'>IP Address: </td><td style='padding: 5px;'>" + ipAddress + "</td>" +
                              "     </tr>" +
                              "     <tr>" +
                              "         <td style='padding: 5px;'>Ad Package: </td><td style='padding: 5px;'>" + adPackage + "</td>" +
                              "     </tr>" +

                              "     <tr>" +
                              "         <td style='padding: 5px;'>Date Activated: </td><td style='padding: 5px;'>" + DateTime.Now.ToString() + "</td>" +
                              "     </tr>" +
                              "     <tr>" +
                              "         <td style='padding: 5px;'>Last Month: </td><td style='padding: 5px;'>" + DateTime.Now.AddMonths(-1).ToString("MMMM, yyyy") + "</td>" +
                              "     </tr>" +

                              "</table>";

                MailMessage oMail = new MailMessage();
                if (!string.IsNullOrEmpty(Helper.ActivateBranchNotificationEmail))
                {
                    var listSupport = Helper.ActivateBranchNotificationEmail.Split(";,|".ToArray(), StringSplitOptions.RemoveEmptyEntries);
                    foreach (var add in listSupport)
                    {
                        try
                        {
                            oMail.To.Add(add);
                        }
                        catch (Exception) { }
                    }
                }

                oMail.From = new MailAddress(Helper.SmtpSendFrom);
                oMail.Subject = subject;
                oMail.Body = body;
                oMail.IsBodyHtml = true;
                EmailEngine.SendMailToCustomer(oMail);
            }
            catch (Exception ex)
            {
                ErrorHandler.AddToLogFile("Error while sendEmailWhenActiveBranch", ex);
            }
        }

        public static string GetIpAddress(HttpRequest Request)
        {
            string strResult = string.Empty;
            strResult = Request.UserHostAddress;
            return strResult;
        }

        #endregion

        #region Render Property DDLs

        public static string RenderListingAgenDDL(int? selectedValue)
        {
            int sv = selectedValue == null ? 0 : selectedValue.Value;
            StringBuilder builder = new StringBuilder();

            builder.AppendLine("<option " + (sv == 0 ? "selected='selected'" : "") + " value='0'>-- Select --</option>");
            builder.AppendLine("<option " + (sv == 1 ? "selected='selected'" : "") + " value='1'>Jose</option>");
            builder.AppendLine("<option " + (sv == 2 ? "selected='selected'" : "") + " value='2'>Ricky</option>");
            builder.AppendLine("<option " + (sv == 3 ? "selected='selected'" : "") + " value='3'>Vincent</option>");

            return builder.ToString();
        }

        public static string RenderListingTypeDDL(int selectedValue)
        {
            StringBuilder builder = new StringBuilder();

            if (CountryCode == "MX")
            {

                CategoryMexico[] listCats = new CategoryMexico[]{
                    CategoryMexico.InmueblesEnVenta,
                    CategoryMexico.TerrenosEnVenta,
                    CategoryMexico.InmueblesEnRenta,
                    CategoryMexico.CuartosEnRenta,
                    CategoryMexico.RentasVacacionales,
                    CategoryMexico.PropiedadesComerciales_OficinasYconsultorios,
                    CategoryMexico.PropiedadesComerciales_LocalesComerciales,
                    CategoryMexico.PropiedadesComerciales_Traspasos,
                    CategoryMexico.Bodegas
                };

                foreach (var cat in listCats)
                {
                    builder.AppendFormat("<option value='{0}' {1}>{2}</option>", (short)cat, selectedValue == (short)cat ? "selected='selected'" : "", ListingTypeToString((short)cat));
                }
            }
            else
            {
                CategoryArgentina[] listCats = new CategoryArgentina[]{
                        CategoryArgentina.VentaDeDepartamentos,
                        CategoryArgentina.AlquilerDeDepartamentos,
                        CategoryArgentina.AlquileresTemporariosDeDepartamentos,
                        CategoryArgentina.VentaDeCasas,
                        CategoryArgentina.AlquilerDeCasas,
                        CategoryArgentina.AlquileresTemporariosDeCasas,
                        CategoryArgentina.VentaDeLocalesYOficinas,
                        CategoryArgentina.AlquilerDeLocalesYOficinas,
                        CategoryArgentina.VentaDeCocheras,
                        CategoryArgentina.AlquilerDeCocheras,
                        CategoryArgentina.VentaDeLotesYTerrenos,
                        CategoryArgentina.AlquilerDeLotesYTerrenos,
                        CategoryArgentina.VentaDeCampos,
                        CategoryArgentina.AlquilerDeCampos,
                        CategoryArgentina.VentaDeDepositosYGalpones,
                        CategoryArgentina.AlquilerDeDepositosYGalpones,
                        CategoryArgentina.VentaDeBovedasNichosYParcelas,
                        CategoryArgentina.AlquilerDeBovedasNichosYParcelas,
                };

                //foreach (CategoryArgentina cat in Enum.GetValues(typeof(CategoryArgentina)))
                //{
                //    builder.AppendFormat("<option value='{0}' {1}>{2}</option>", (short)cat, selectedValue == (short)cat ? "selected='selected'" : "", ListingTypeARToString((short)cat));
                //}

                foreach (CategoryArgentina cat in listCats)
                {
                    builder.AppendFormat("<option value='{0}' {1}>{2}</option>", (short)cat, selectedValue == (short)cat ? "selected='selected'" : "", ListingTypeARToString((short)cat));
                }
            }

            return builder.ToString();
        }

        public static string RenderListingStatusDDL(string selectedValue)
        {
            StringBuilder builder = new StringBuilder();
            string[] arrays = new string[]{
                "active", "cancelled", "contingent-under-contract",
                "expired", "pending", "sold", "withdrawn-temporarily-withdrawn-temporarily-off-market"
            };

            string[] arrayLocals = new string[]{
                "Active", "Cancelled", "ContingentOrUnderContract",
                "Expired", "Pending", "Sold", "WithdrawnTemporarilyWithdrawnTemporarilyOffMarket"
            };

            // builder.AppendLine("<option " + (selectedValue == "" ? "selected='selected'" : "") + " value=''>-- Select --</option>");
            for (int i = 0; i < arrays.Length; i++)
            {
                string val = arrays[i];
                string text = LoadString(arrayLocals[i]);
                builder.AppendFormat("<option value='{0}' {2}>{1}</option>", val, HttpUtility.HtmlEncode(text),
                    val.Equals(selectedValue, StringComparison.OrdinalIgnoreCase) ? "selected='selected'" : "");
            }

            return builder.ToString();
        }

        public static string normalizeString(string s)
        {
            return Regex.Replace(s.ToLower().Trim(), @"[^\d\w]+", "-");
        }

        public static string RenderDwellingTypeDDL(string selectedValue)
        {
            Dictionary<string, string> dwellingTypes = new Dictionary<string, string>();
            dwellingTypes.Add("house", "Casa");
            dwellingTypes.Add("apartment", "Departamento");
            dwellingTypes.Add("development", "Desarrollo");
            dwellingTypes.Add("foreclosure", "Remate Hipotecario");

            StringBuilder builder = new StringBuilder();
            // builder.AppendFormat("<option value=''>-- {0} --</option>", LoadString("AllPropertyTypes"));

            foreach (KeyValuePair<string, string> dic in dwellingTypes)
            {
                string selected = dic.Key == selectedValue ? "selected='selected'" : "";
                builder.AppendFormat("<option {2} value='{0}'>{1}</option>", dic.Key, dic.Value, selected);
            }

            // builder.AppendFormat("<option value='Other'>{0}</option>", LoadString("Other"));

            return builder.ToString();
        }

        public static string RenderNumBedRoomDDL(byte? selectedValue)
        {
            int sv = selectedValue == null ? 0 : selectedValue.Value;
            StringBuilder builder = new StringBuilder();
            if (Helper.CountryCode == "MX")
            {
                builder.AppendLine("<option " + (sv == 0 ? "selected='selected'" : "") + " value='0'>-- Select --</option>");
                builder.AppendLine("<option " + (sv == 10 ? "selected='selected'" : "") + " value='10'>Estudio</option>"); //Studio or Bachelor Pad
                builder.AppendLine("<option " + (sv == 1 ? "selected='selected'" : "") + " value='1'>1</option>");
                builder.AppendLine("<option " + (sv == 2 ? "selected='selected'" : "") + " value='2'>2</option>");
                builder.AppendLine("<option " + (sv == 3 ? "selected='selected'" : "") + " value='3'>3</option>");
                builder.AppendLine("<option " + (sv == 4 ? "selected='selected'" : "") + " value='4'>4</option>");
                builder.AppendLine("<option " + (sv == 5 ? "selected='selected'" : "") + " value='5'>5</option>");
                builder.AppendLine("<option " + (sv == 6 ? "selected='selected'" : "") + " value='6'>6 o más</option>");
            }
            else
            {
                sv = sv > 4 ? 4 : sv;

                builder.AppendLine("<option " + (sv == 1 ? "selected='selected'" : "") + " value='1'>1 Dormitorio</option>");
                builder.AppendLine("<option " + (sv == 2 ? "selected='selected'" : "") + " value='2'>2 Dormitorios</option>");
                builder.AppendLine("<option " + (sv == 3 ? "selected='selected'" : "") + " value='3'>3 Dormitorios</option>");
                builder.AppendLine("<option " + (sv == 4 ? "selected='selected'" : "") + " value='4'>Mas de 3 Dormitorios</option>");
            }

            return builder.ToString();
        }

        public static string RenderNumBathRoomDDL(byte? selectedValue)
        {
            byte sv = selectedValue == null ? (byte)0 : selectedValue.Value;
            if (sv > 4)
                sv = 4;

            StringBuilder builder = new StringBuilder();
            builder.AppendLine("<option " + (sv == 0 ? "selected='selected'" : "") + " value='0'>-- Select --</option>");
            builder.AppendLine("<option " + (sv == 1 ? "selected='selected'" : "") + " value='1'>1</option>");
            builder.AppendLine("<option " + (sv == 2 ? "selected='selected'" : "") + " value='2'>2</option>");
            builder.AppendLine("<option " + (sv == 3 ? "selected='selected'" : "") + " value='3'>3</option>");
            builder.AppendLine("<option " + (sv == 4 ? "selected='selected'" : "") + " value='4'>4 o más</option>");

            return builder.ToString();
        }

        public static string RenderNumEnsuiteDDL(byte? selectedValue)
        {
            byte sv = selectedValue == null ? (byte)0 : selectedValue.Value;

            StringBuilder builder = new StringBuilder();
            builder.AppendLine("<option " + (sv == 0 ? "selected='selected'" : "") + " value='0'>-- Select --</option>");
            builder.AppendLine("<option " + (sv == 1 ? "selected='selected'" : "") + " value='1'>1 ensuite</option>");
            builder.AppendLine("<option " + (sv == 2 ? "selected='selected'" : "") + " value='2'>2 ensuite</option>");
            builder.AppendLine("<option " + (sv == 3 ? "selected='selected'" : "") + " value='3'>3 ensuite</option>");
            builder.AppendLine("<option " + (sv == 4 ? "selected='selected'" : "") + " value='4'>4 or more Ensuite</option>");

            return builder.ToString();
        }

        public static string RenderNumKitchenDDL(byte? selectedValue)
        {
            byte sv = selectedValue == null ? (byte)0 : selectedValue.Value;

            StringBuilder builder = new StringBuilder();
            builder.AppendLine("<option " + (sv == 0 ? "selected='selected'" : "") + " value='0'>-- Select --</option>");
            builder.AppendLine("<option " + (sv == 1 ? "selected='selected'" : "") + " value='1'>1 kitchen</option>");
            builder.AppendLine("<option " + (sv == 2 ? "selected='selected'" : "") + " value='2'>2 kitchens</option>");
            builder.AppendLine("<option " + (sv == 3 ? "selected='selected'" : "") + " value='3'>3 or more kitchens</option>");

            return builder.ToString();
        }

        public static string RenderNumReceptionRoom(byte? selectedValue)
        {
            byte sv = selectedValue == null ? (byte)0 : selectedValue.Value;

            StringBuilder builder = new StringBuilder();
            builder.AppendLine("<option " + (sv == 0 ? "selected='selected'" : "") + " value='0'>-- Select --</option>");
            builder.AppendLine("<option " + (sv == 1 ? "selected='selected'" : "") + " value='1'>1 reception room</option>");
            builder.AppendLine("<option " + (sv == 2 ? "selected='selected'" : "") + " value='2'>2 reception rooms</option>");
            builder.AppendLine("<option " + (sv == 3 ? "selected='selected'" : "") + " value='3'>3 or more reception rooms</option>");

            return builder.ToString();
        }

        public static string RenderNumOfficeStudyDDL(byte? selectedValue)
        {
            byte sv = selectedValue == null ? (byte)0 : selectedValue.Value;

            StringBuilder builder = new StringBuilder();
            builder.AppendLine("<option " + (sv == 0 ? "selected='selected'" : "") + " value='0'>-- Select --</option>");
            builder.AppendLine("<option " + (sv == 1 ? "selected='selected'" : "") + " value='1'>1</option>");
            builder.AppendLine("<option " + (sv == 2 ? "selected='selected'" : "") + " value='2'>2</option>");
            builder.AppendLine("<option " + (sv == 3 ? "selected='selected'" : "") + " value='3'>3 or more</option>");

            return builder.ToString();
        }

        public static string RenderNumEntranceHallDDL(byte? selectedValue)
        {
            byte sv = selectedValue == null ? (byte)0 : selectedValue.Value;

            StringBuilder builder = new StringBuilder();
            builder.AppendLine("<option " + (sv == 0 ? "selected='selected'" : "") + " value='0'>-- Select --</option>");
            builder.AppendLine("<option " + (sv == 1 ? "selected='selected'" : "") + " value='1'>1</option>");
            builder.AppendLine("<option " + (sv == 2 ? "selected='selected'" : "") + " value='2'>2</option>");
            builder.AppendLine("<option " + (sv == 3 ? "selected='selected'" : "") + " value='3'>3 or more</option>");

            return builder.ToString();
        }

        public static string RenderNumPackingDDL(string selectedValue)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("<option " + (selectedValue == "" ? "selected='selected'" : "") + " value=''>-- Select --</option>");
            builder.AppendLine("<option " + (selectedValue == "grage" ? "selected='selected'" : "") + " value='grage'>Garage</option>");
            builder.AppendLine("<option " + (selectedValue == "covrd" ? "selected='selected'" : "") + " value='covrd'>Covered</option>");
            builder.AppendLine("<option " + (selectedValue == "stret" ? "selected='selected'" : "") + " value='stret'>Street</option>");
            builder.AppendLine("<option " + (selectedValue == "none" ? "selected='selected'" : "") + " value='none'>None</option>");

            return builder.ToString();
        }

        public static string RenderVacationLocationTypeDDL(string selectedValue)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("<option " + (selectedValue == "" ? "selected='selected'" : "") + " value=''>-- Select --</option>");
            builder.AppendFormat("<option " + (selectedValue == "campo" ? "selected='selected'" : "") + " value='campo'>{0}</option>", LoadString("country"));
            builder.AppendFormat("<option " + (selectedValue == "ciudad" ? "selected='selected'" : "") + " value='ciudad'>{0}</option>", LoadString("city").ToLower());
            builder.AppendFormat("<option " + (selectedValue == "montana" ? "selected='selected'" : "") + " value='montana'>{0}</option>", LoadString("mountain"));
            builder.AppendFormat("<option " + (selectedValue == "playa" ? "selected='selected'" : "") + " value='playa'>{0}</option>", LoadString("beach"));
            builder.AppendFormat("<option " + (selectedValue == "suburbios" ? "selected='selected'" : "") + " value='suburbios'>{0}</option>", LoadString("suburb"));

            return builder.ToString();
        }

        public static string RenderNumSleepDDL(int? selectedValue)
        {
            int sv = selectedValue == null ? 0 : selectedValue.Value;

            StringBuilder builder = new StringBuilder();
            builder.AppendLine("<option " + (sv == 0 ? "selected='selected'" : "") + " value='0'>-- Select --</option>");
            builder.AppendLine("<option " + (sv == 1 ? "selected='selected'" : "") + " value='1'>1</option>");
            builder.AppendLine("<option " + (sv == 2 ? "selected='selected'" : "") + " value='2'>2</option>");
            builder.AppendLine("<option " + (sv == 3 ? "selected='selected'" : "") + " value='3'>3</option>");
            builder.AppendLine("<option " + (sv == 4 ? "selected='selected'" : "") + " value='4'>4</option>");
            builder.AppendLine("<option " + (sv == 5 ? "selected='selected'" : "") + " value='5'>5</option>");
            builder.AppendLine("<option " + (sv == 6 ? "selected='selected'" : "") + " value='6'>6</option>");
            builder.AppendLine("<option " + (sv == 7 ? "selected='selected'" : "") + " value='7'>7</option>");
            builder.AppendLine("<option " + (sv == 8 ? "selected='selected'" : "") + " value='8'>8</option>");
            builder.AppendLine("<option " + (sv == 9 ? "selected='selected'" : "") + " value='9'>9</option>");
            builder.AppendLine("<option " + (sv == 10 ? "selected='selected'" : "") + " value='10'>10</option>");
            builder.AppendLine("<option " + (sv == 11 ? "selected='selected'" : "") + " value='11'>11</option>");
            builder.AppendLine("<option " + (sv == 12 ? "selected='selected'" : "") + " value='12'>12</option>");
            builder.AppendLine("<option " + (sv == 13 ? "selected='selected'" : "") + " value='13'>13</option>");
            builder.AppendLine("<option " + (sv == 14 ? "selected='selected'" : "") + " value='14'>14</option>");
            builder.AppendLine("<option " + (sv == 15 ? "selected='selected'" : "") + " value='15'>(+)15</option>");

            return builder.ToString();
        }

        public static string RenderForSaleByDDL(string selectedValue)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("<option " + (selectedValue == "" ? "selected='selected'" : "") + " value=''>-- Select --</option>");
            builder.AppendLine("<option " + (selectedValue == "ownr" ? "selected='selected'" : "") + " value='ownr'>Particular</option>");
            builder.AppendLine("<option " + (selectedValue == "agncy" ? "selected='selected'" : "") + " value='agncy'>Inmobiliaria</option>");

            return builder.ToString();
        }

        public static string RenderForSaleRentDDL(string selectedValue)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("<option " + (selectedValue == "" ? "selected='selected'" : "") + " value=''>-- Select --</option>");
            builder.AppendLine("<option " + (selectedValue == "sale" ? "selected='selected'" : "") + " value='sale'>Vender</option>");
            builder.AppendLine("<option " + (selectedValue == "rent" ? "selected='selected'" : "") + " value='rent'>Rentar </option>");

            return builder.ToString();
        }

        public static string RenderShareBasisDDL(string selectedValue)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("<option " + (selectedValue == "" ? "selected='selected'" : "") + " value=''>-- Select --</option>");
            builder.AppendLine("<option " + (selectedValue == "Room Share" ? "selected='selected'" : "") + " value='Room Share'>Room Share</option>");
            builder.AppendLine("<option " + (selectedValue == "Flat or House Share" ? "selected='selected'" : "") + " value='Flat or House Share'>Flat or House Share</option>");

            return builder.ToString();
        }

        public static string RenderTemperatureControlDDL(string selectedValue)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("<option " + (selectedValue == "" ? "selected='selected'" : "") + " value=''>-- Select --</option>");
            builder.AppendLine("<option " + (selectedValue == "A/C" ? "selected='selected'" : "") + " value='A/C'>A/C</option>");
            builder.AppendLine("<option " + (selectedValue == "Central Air" ? "selected='selected'" : "") + " value='Central Air'>Central Air</option>");
            builder.AppendLine("<option " + (selectedValue == "Other" ? "selected='selected'" : "") + " value='Other'>Other</option>");

            return builder.ToString();
        }

        public static string RenderSecurityFeaturesDDL(string selectedValue)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("<option " + (selectedValue == "" ? "selected='selected'" : "") + " value=''>-- Select --</option>");
            builder.AppendLine("<option " + (selectedValue == "Electric fencing" ? "selected='selected'" : "") + " value='Electric fencing'>Electric fencing</option>");
            builder.AppendLine("<option " + (selectedValue == "Burglar Bars" ? "selected='selected'" : "") + " value='Burglar Bars'>Burglar Bars</option>");
            builder.AppendLine("<option " + (selectedValue == "Alarm System" ? "selected='selected'" : "") + " value='Alarm System'>Alarm System</option>");
            builder.AppendLine("<option " + (selectedValue == "Electric Gate" ? "selected='selected'" : "") + " value='Electric Gate'>Electric Gate</option>");
            builder.AppendLine("<option " + (selectedValue == "Other" ? "selected='selected'" : "") + " value='Other'>Other</option>");

            return builder.ToString();
        }

        public static string RenderNumGaragesDDL(byte? selectedValue)
        {
            byte sv = selectedValue == null ? (byte)0 : selectedValue.Value;

            StringBuilder builder = new StringBuilder();
            builder.AppendLine("<option " + (sv == 0 ? "selected='selected'" : "") + " value='0'>-- Select --</option>");
            builder.AppendLine("<option " + (sv == 1 ? "selected='selected'" : "") + " value='1'>1 garage</option>");
            builder.AppendLine("<option " + (sv == 2 ? "selected='selected'" : "") + " value='2'>2 garages</option>");
            builder.AppendLine("<option " + (sv == 3 ? "selected='selected'" : "") + " value='3'>3 or more garages</option>");

            return builder.ToString();
        }

        public static string RenderProvinceDDL(string selectedValue)
        {
            string[] provinces = { "Aguascalientes", "Baja California", "Baja California Sur", "Campeche", "Chiapas", "Chihuahua", "Coahuila", "Colima", "Distrito Federal", "Durango",
                                   "Guanajuato", "Guerrero","Hidalgo", "Jalisco","Mexico", "Michoacán","Morelos", "Nayarit","Nuevo León", "Oaxaca",
                                   "Puebla", "Querétaro","Quintana Roo", "San Luis Potosi","Sinaloa", "Sonora","Tabasco", "Tamaulipas","Tlaxcala", "Veracruz", "Yucatan", "Zacatecas"};

            StringBuilder builder = new StringBuilder();

            foreach (string province in provinces)
            {
                string selected = province == selectedValue ? "selected='selected'" : "";
                builder.AppendLine("<option value='" + province + "' " + selected + ">" + province + "</option>");
            }

            return builder.ToString();
        }

        public static string RenderPreferredGenderDDL(string selectedValue)
        {
            string sv = selectedValue;

            StringBuilder builder = new StringBuilder();
            builder.AppendLine("<option " + (sv == "Po Preference" ? "selected='selected'" : "") + " value='Po Preference'>No Preference</option>");
            builder.AppendLine("<option " + (sv == "Female" ? "selected='selected'" : "") + " value='Female'>Female</option>");
            builder.AppendLine("<option " + (sv == "Male" ? "selected='selected'" : "") + " value='Male'>Male</option>");

            return builder.ToString();
        }

        public static string RenderPropertyLocation(Property prop)
        {
            bool hasTextBefore = false;
            string location = "";
            if (prop == null) return location;

            if (!string.IsNullOrEmpty(prop.Address)) { location += prop.Address; hasTextBefore = true; }
            if (!string.IsNullOrEmpty(prop.City)) { location += (hasTextBefore ? " " : "") + prop.City + ","; hasTextBefore = true; }
            if (!string.IsNullOrEmpty(prop.Province)) { location += (hasTextBefore ? " " : "") + prop.Province; hasTextBefore = true; }
            if (!string.IsNullOrEmpty(prop.PostalCode)) { location += (hasTextBefore ? " " : "") + prop.PostalCode; hasTextBefore = true; }
            if (!string.IsNullOrEmpty(prop.Neighborhood)) { location += (hasTextBefore ? " " : "") + prop.Neighborhood; }
            return location;
        }

        public static string RenderPriceTypeDDL(string selectedValue)
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendFormat("<option {2} value='{0}'>{1}</option>", "Fixed", "Fijo", (selectedValue == "Fixed" ? "selected='selected'" : ""));
            builder.AppendFormat("<option {2} value='{0}'>{1}</option>", "Negotiable", "Negociable", (selectedValue == "Negotiable" ? "selected='selected'" : ""));
            builder.AppendFormat("<option {2} value='{0}'>{1}</option>", "Contact for Price", "Contactar", (selectedValue == "Contact for Price" ? "selected='selected'" : ""));

            return builder.ToString();
        }

        public static string RenderCurrencyTypeDDL(string selectedValue)
        {
            StringBuilder builder = new StringBuilder();

            builder.AppendFormat("<option {2} value='{0}'>{1}</option>", Helper.CurrencyCode, Helper.CurrencyCode, (selectedValue == Helper.CurrencyCode ? "selected='selected'" : ""));
            builder.AppendFormat("<option {2} value='{0}'>{1}</option>", "USD", "USD", (selectedValue == "USD" ? "selected='selected'" : ""));

            return builder.ToString();
        }

        #endregion

        #region Scale image

        /// <summary>
        /// Scale By Height Size
        /// </summary>
        /// <param name="imgPhoto"></param>
        /// <param name="Width"></param>
        /// <param name="Height"></param>
        /// <returns></returns>
        public static System.Drawing.Image ScaleByHeightSize(System.Drawing.Image imgPhoto, int Height)
        {
            const int OrientationKey = 0x0112;
            const int NormalOrientation = 1;
            const int MirrorHorizontal = 2;
            const int UpsideDown = 3;
            const int MirrorVertical = 4;
            const int MirrorHorizontalAndRotateRight = 5;
            const int RotateLeft = 6;
            const int MirorHorizontalAndRotateLeft = 7;
            const int RotateRight = 8;

            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;
            int sourceX = 0;
            int sourceY = 0;
            int destX = 0;
            int destY = 0;

            float nPercent = 0;

            int Width = 0;
            nPercent = ((float)Height / (float)sourceHeight);

            int destHeight = Height;// (int)(sourceWidth * nPercent);
            int destWidth = (int)(Math.Round(sourceWidth * nPercent, 0));

            //Width =(int) (sourceWidth * nPercent);
            Width = (int)(Math.Round(sourceWidth * nPercent, 0));

            System.Drawing.Bitmap bmPhoto = new System.Drawing.Bitmap(Width, Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

            //bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

            bmPhoto.SetResolution(300, 300);

            using (System.Drawing.Graphics grPhoto = System.Drawing.Graphics.FromImage(bmPhoto))
            {
                //Very Good
                grPhoto.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                grPhoto.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                grPhoto.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                grPhoto.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                grPhoto.Clear(System.Drawing.Color.White);

                grPhoto.DrawImage(imgPhoto,
                    new System.Drawing.Rectangle(destX, destY, destWidth, destHeight),
                    new System.Drawing.Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                    System.Drawing.GraphicsUnit.Pixel);

                //grPhoto.DrawImage(imgPhoto, 0, 0, destWidth, destHeight);
            }

            if (imgPhoto.PropertyIdList.Contains(OrientationKey))
            {
                var orientation = (int)imgPhoto.GetPropertyItem(OrientationKey).Value[0];
                switch (orientation)
                {
                    case NormalOrientation:
                        // No rotation required.
                        break;
                    case MirrorHorizontal:
                        bmPhoto.RotateFlip(System.Drawing.RotateFlipType.RotateNoneFlipX);
                        break;
                    case UpsideDown:
                        bmPhoto.RotateFlip(System.Drawing.RotateFlipType.Rotate180FlipNone);
                        break;
                    case MirrorVertical:
                        bmPhoto.RotateFlip(System.Drawing.RotateFlipType.Rotate180FlipX);
                        break;
                    case MirrorHorizontalAndRotateRight:
                        bmPhoto.RotateFlip(System.Drawing.RotateFlipType.Rotate90FlipX);
                        break;
                    case RotateLeft:
                        bmPhoto.RotateFlip(System.Drawing.RotateFlipType.Rotate90FlipNone);
                        break;
                    case MirorHorizontalAndRotateLeft:
                        bmPhoto.RotateFlip(System.Drawing.RotateFlipType.Rotate270FlipX);
                        break;
                    case RotateRight:
                        bmPhoto.RotateFlip(System.Drawing.RotateFlipType.Rotate270FlipNone);
                        break;
                    default:
                        break;
                }
            }

            return bmPhoto;
        }


        public static bool MoveFileFollowRatio(string strSource, string strDest)
        {
            bool blnResult = false;
            FileInfo objFileInfo = new FileInfo(strSource);
            if (objFileInfo.Exists)
            {
                //Last Version
                using (SD.Image imgOrg = SD.Image.FromFile(strSource))
                {
                    SD.Image imgOrgNew = ScaleByFixedSize(imgOrg, imgOrg.Width, imgOrg.Height);

                    using (SD.Bitmap bmp = new SD.Bitmap(imgOrgNew))
                    {
                        MemoryStream ms = new MemoryStream();
                        bmp.Save(ms, imgOrg.RawFormat);
                        byte[] bImage = ms.GetBuffer();
                        using (MemoryStream msa = new MemoryStream(bImage, 0, bImage.Length))
                        {
                            msa.Write(bImage, 0, bImage.Length);
                            using (SD.Image ResizeImage = SD.Image.FromStream(msa, true))
                            {
                                string SaveTo = strDest;
                                ResizeImage.Save(SaveTo, ResizeImage.RawFormat);
                                ResizeImage.Dispose();
                            }
                            msa.Dispose();
                            msa.Close();
                        }
                        ms.Dispose();
                        ms.Close();
                        bmp.Dispose();
                    }
                    imgOrgNew.Dispose();
                    imgOrg.Dispose();
                }

                objFileInfo.Delete();

                blnResult = true;
            }
            return blnResult;
        }

        static SD.Image ScaleByFixedSize(SD.Image imgPhoto, int Width, int Height)
        {
            // Fix orientation if needed. - 1
            const int OrientationKey = 0x0112;
            const int NormalOrientation = 1;
            const int MirrorHorizontal = 2;
            const int UpsideDown = 3;
            const int MirrorVertical = 4;
            const int MirrorHorizontalAndRotateRight = 5;
            const int RotateLeft = 6;
            const int MirorHorizontalAndRotateLeft = 7;
            const int RotateRight = 8;

            int sourceWidth = imgPhoto.Width;
            int sourceHeight = imgPhoto.Height;
            int sourceX = 0;
            int sourceY = 0;
            int destX = 0;
            int destY = 0;

            float nPercent = 0;
            float nPercentW = 0;
            float nPercentH = 0;

            nPercentW = ((float)Width / (float)sourceWidth);
            nPercentH = ((float)Height / (float)sourceHeight);

            if (nPercentH < nPercentW)
            {
                nPercent = nPercentH;

            }
            else
            {
                nPercent = nPercentW;

            }

            destX = System.Convert.ToInt16((Width -
                              (sourceWidth * nPercentW)) / 2);
            destY = System.Convert.ToInt16((Height -
                              (sourceHeight * nPercentH)) / 2);

            int destWidth = (int)(sourceWidth * nPercentW);
            int destHeight = (int)(sourceHeight * nPercentH);

            SD.Bitmap bmPhoto = new SD.Bitmap(Width, Height, PixelFormat.Format32bppArgb);
            bmPhoto.SetResolution(imgPhoto.HorizontalResolution, imgPhoto.VerticalResolution);

            SD.Graphics grPhoto = SD.Graphics.FromImage(bmPhoto);
            grPhoto.Clear(SD.Color.White);
            grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;

            grPhoto.DrawImage(imgPhoto,
                new SD.Rectangle(destX, destY, destWidth, destHeight),
                new SD.Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                SD.GraphicsUnit.Pixel);

            grPhoto.Dispose();

            // Fix orientation if needed. - 2
            if (imgPhoto.PropertyIdList.Contains(OrientationKey))
            {
                var orientation = (int)imgPhoto.GetPropertyItem(OrientationKey).Value[0];
                switch (orientation)
                {
                    case NormalOrientation:
                        // No rotation required.
                        break;
                    case MirrorHorizontal:
                        bmPhoto.RotateFlip(SD.RotateFlipType.RotateNoneFlipX);
                        break;
                    case UpsideDown:
                        bmPhoto.RotateFlip(SD.RotateFlipType.Rotate180FlipNone);
                        break;
                    case MirrorVertical:
                        bmPhoto.RotateFlip(SD.RotateFlipType.Rotate180FlipX);
                        break;
                    case MirrorHorizontalAndRotateRight:
                        bmPhoto.RotateFlip(SD.RotateFlipType.Rotate90FlipX);
                        break;
                    case RotateLeft:
                        bmPhoto.RotateFlip(SD.RotateFlipType.Rotate90FlipNone);
                        break;
                    case MirorHorizontalAndRotateLeft:
                        bmPhoto.RotateFlip(SD.RotateFlipType.Rotate270FlipX);
                        break;
                    case RotateRight:
                        bmPhoto.RotateFlip(SD.RotateFlipType.Rotate270FlipNone);
                        break;
                    default:
                        break;
                }
            }

            return bmPhoto;
        }

        #endregion

        #region Exports

        public static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

        public static void ExportToPDF(HttpResponse Response, DataTable dt, string fileName, string headerTitle, float[] tableWidths)
        {
            Response.ContentType = "application/pdf";
            Response.AddHeader("Content-Disposition", "attachment;filename=" + fileName);

            System.Text.StringBuilder builder = new System.Text.StringBuilder();

            builder.Append("<html><head></head><body>");
            builder.Append("<br />");
            builder.Append("<table border=1 class=\"tbl\">");
            builder.Append("<tr>");

            foreach (DataColumn dc in dt.Columns)
            {
                builder.AppendFormat("<th align=\"center\"><b>{0}</b></th>", dc.ColumnName);
            }

            builder.Append("</tr>");

            foreach (DataRow dr in dt.Rows)
            {
                builder.Append("<tr>");

                foreach (DataColumn col in dt.Columns)
                {
                    string cellValue = Helper.SafeString(dr[col.ColumnName]);
                    string cssStyle = "";

                    if (col.ColumnName.Contains("#"))
                        cssStyle = "style='text-align: center'";

                    // 1 cell is 1 table
                    builder.AppendFormat("<td>");
                    builder.AppendFormat("    <table border='0' cellpadding='0' cellspacing='0'>");
                    builder.AppendFormat("      <tr><td {1}><b class='title'>  {0}</b></td></tr>", cellValue.Trim(), cssStyle);
                    builder.AppendFormat("    </table>");
                    builder.AppendFormat("</td>");
                }

                builder.Append("</tr>");
            }

            builder.AppendFormat("</table>");
            builder.Append("</body></html>");

            #region PDF Create

            StyleSheet style = new StyleSheet();
            style.LoadStyle("tbl", iTextSharp.text.html.HtmlTags.SIZE, "1.9");
            style.LoadStyle("tbl", "font-family", "times");
            style.LoadStyle("title", "color", "#4276a7");

            // need the Rectangle for later when we set the column widths            
            Document document = new Document(PDFPageSize);
            PdfWriter pdfWriter = PdfWriter.GetInstance(document, Response.OutputStream);
            //create an instance of your PDFpage class. This is the class we generated below.
            pdfPage page = new pdfPage();

            // set the PageEvent of the pdfWriter instance to the instance of our PDFPage class
            pdfWriter.PageEvent = page;

            document.Open();

            // use a PdfPtable with 1 column to position my header
            PdfPTable headerTbl = new PdfPTable(1);

            //set the width of the table to be the same as the document
            headerTbl.TotalWidth = document.PageSize.Width;

            //create instance of a table cell to contain the header
            PdfPCell cell = new PdfPCell(new Paragraph(headerTitle, new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.COURIER, 22, iTextSharp.text.Font.BOLD)));

            //align the logo to the center of the cell
            cell.HorizontalAlignment = Element.ALIGN_CENTER;

            //remove the border
            cell.Border = 0;

            //Add the cell to the table
            headerTbl.AddCell(cell);

            PdfPCell cell1 = new PdfPCell(new Paragraph(String.Format("{0:f}\n\n", DateTime.Now),
                new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.COURIER, 13, iTextSharp.text.Font.BOLD)));
            cell1.HorizontalAlignment = Element.ALIGN_CENTER;
            cell1.Border = 0;
            headerTbl.AddCell(cell1);

            document.Add(headerTbl);

            // iterate over iText elements
            List<IElement> ie = HTMLWorker.ParseToList(
              new StringReader(builder.ToString()), style
            );

            foreach (IElement element in ie)
            {
                PdfPTable table = element as PdfPTable;

                // set the column widths
                if (table != null)
                {
                    table.SetWidthPercentage(tableWidths, PDFPageSize);
                }

                document.Add(element);
            }

            document.Close();

            #endregion
        }

        public static void ExportToExcel(HttpResponse Response, DataTable dt, string fileName)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "Application/x-msexcel";
            Response.AddHeader("Content-Disposition", "attachment;filename=" + fileName);
            Response.Charset = "";

            string sTab = "";
            foreach (DataColumn dc in dt.Columns)
            {
                HttpContext.Current.Response.Write(sTab + dc.ColumnName);
                sTab = "\t";
            }
            HttpContext.Current.Response.Write("\n");

            foreach (DataRow dr in dt.Rows)
            {
                sTab = "";
                foreach (DataColumn col in dt.Columns)
                {
                    string cellValue = SafeString(dr[col.ColumnName]).Replace("\t", "");
                    HttpContext.Current.Response.Write(sTab + cellValue);
                    sTab = "\t";
                }
                HttpContext.Current.Response.Write("\n");
            }
        }

        public static void ExportToExcelV2(HttpResponse Response, DataTable dt, string fileName)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "Application/x-msexcel";
            Response.AddHeader("Content-Disposition", "attachment;filename=" + fileName);
            Response.Charset = "";

            if (dt != null && dt.Rows.Count > 0)
            {
                //Create a dummy GridView
                GridView GridView1 = new GridView();
                GridView1.AllowPaging = false;
                GridView1.DataSource = dt;
                GridView1.DataBind();

                StringWriter sw = new StringWriter();
                HtmlTextWriter hw = new HtmlTextWriter(sw);

                for (int i = 0; i < GridView1.Rows.Count; i++)
                {
                    //Apply text style to each Row
                    GridView1.Rows[i].Attributes.Add("class", "textmode");
                }
                GridView1.RenderControl(hw);

                //style to format numbers to string
                string style = @"<style> .textmode { mso-number-format:\@; } </style>";
                Response.Write(style);
                Response.Output.Write(sw.ToString());
            }
            Response.Flush();
            Response.End();
        }

        public static void ExportToText(HttpResponse Response, DataTable dt, string fileName)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "text/plain";
            Response.AddHeader("Content-Disposition", "attachment;filename=" + fileName);
            Response.Charset = "";

            string sTab = "";
            foreach (DataColumn dc in dt.Columns)
            {
                HttpContext.Current.Response.Write(sTab + dc.ColumnName);
                sTab = "\t\t";
            }
            HttpContext.Current.Response.Write("\r\n");

            foreach (DataRow dr in dt.Rows)
            {
                sTab = "";
                foreach (DataColumn col in dt.Columns)
                {
                    string cellValue = cellValue = Helper.SafeString(dr[col.ColumnName]);
                    HttpContext.Current.Response.Write(sTab + cellValue);
                    sTab = "\t\t";
                }
                HttpContext.Current.Response.Write("\r\n");
            }
            HttpContext.Current.Response.End();
        }

        #endregion

        #region Date Time And Currency: We can change format in the future

        public static string FormatCurrency(object price)
        {
            price = SafeDecimal(price);
            try
            {
                return string.Format("{0:C0}", price);
            }
            catch
            {
                return SafeString(price);
            }
        }

        public static string ShortDateFormat(DateTime? obj)
        {
            if (!obj.HasValue || obj.Value.Year <= 1900)
                return "";

            return obj.Value.ToLocalTime().ToString("dd/MM/yyyy");
        }

        public static string BreakLineDateFormat(string date)
        {
            DateTime logDate = UtcToMXTime(Helper.SafeDate(date));
            string day = logDate.ToString("dd/MM/yyyy");
            string hour = logDate.ToString("hh:mm:ss tt");
            string result = string.Format("{0}<br />{1}", day, hour);

            return result;
        }

        #endregion

        #region Map And GetLog From Object

        public static T DeserializeObject<T>(string jsonInput)
        {
            T objectFromDB = Activator.CreateInstance<T>();
            return DeserializeObject<T>(jsonInput, objectFromDB);
        }

        public static T DeserializeObject<T>(string jsonInput, T objectFromDB)
        {
            return JsonConvert.DeserializeObject<T>(jsonInput, new ApplyDateTimeConverter<T>(objectFromDB, new CultureInfo("fr-FR")));
        }

        public static void MapModelValues<T>(ref T source, T dest) where T : class
        {
            try
            {
                if (source != null && dest != null)
                {
                    Type type = typeof(T);

                    foreach (PropertyInfo pi in type.GetProperties())
                    {
                        object destValue = type.GetProperty(pi.Name).GetValue(dest, null);

                        if (destValue != null)
                        {
                            if (destValue.GetType() == typeof(DateTime) && destValue.ToString().Contains("1/1/0001"))
                            {
                                continue;
                            }
                            
                            try  // Because some property do not have set method
                            {
                                type.GetProperty(pi.Name).SetValue(source, destValue);
                            }
                            catch
                            {
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.AddToLogFile("Error while MapModelValues", ex);
            }
        }

        public static void StripTagsObject<T>(ref T source) where T : class
        {
            try
            {
                if (source != null)
                {
                    Type type = typeof(T);

                    foreach (PropertyInfo pi in type.GetProperties())
                    {
                        if (pi.PropertyType == typeof(string))
                        {
                            string value = SafeString(type.GetProperty(pi.Name).GetValue(source, null));
                            try  // Because some property do not have set method
                            {
                                type.GetProperty(pi.Name).SetValue(source, value.StripTagsCharArray());
                            }
                            catch
                            {
                            }
                        }                        
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.AddToLogFile("Error while StripTagsObject", ex);
            }
        }

        public static string GetLogDescription<T>(T source, T dest) where T : class
        {
            List<string> changedList = new List<string>();
            try
            {
                if (source != null && dest != null)
                {
                    Type type = typeof(T);

                    foreach (PropertyInfo pi in type.GetProperties())
                    {
                        object sourceValue = type.GetProperty(pi.Name).GetValue(source, null);
                        object destValue = type.GetProperty(pi.Name).GetValue(dest, null);

                        if (destValue != null)
                        {
                            if ((destValue.GetType() == typeof(DateTime) && destValue.ToString().Contains("1/1/0001"))
                                || pi.Name == "HashedPassword" || pi.Name == "BranchID"
                                || pi.Name == "UserType" || pi.Name == "DateCreated" || pi.Name == "PropertyFeatures")
                            {
                                continue;
                            }

                            if (!Object.Equals(sourceValue, destValue))
                            {
                                string dValue = string.IsNullOrEmpty(SafeString(destValue)) ? "[empty]" : SafeString(destValue);
                                string sValue = string.IsNullOrEmpty(SafeString(sourceValue)) ? "[empty]" : SafeString(sourceValue);
                                string sName = pi.Name;

                                #region Replace in special case

                                if (pi.Name == "DwellingType")
                                {
                                    sName = "PropertyType";
                                }

                                if (pi.Name == "ListingType")
                                {
                                    short sTemp = SafeShort(sValue);
                                    sValue = ListingTypeToString(sTemp);
                                    short dTemp = SafeShort(dValue);
                                    dValue = ListingTypeToString(dTemp);
                                }

                                if (pi.Name == "Role")
                                {
                                    sValue = UserRoleToString(sValue);
                                    dValue = UserRoleToString(dValue);
                                }

                                sName = ToCamelCase(sName);

                                #endregion

                                string desc = string.Format("{0} from {1} to {2}", sName, sValue, dValue);
                                changedList.Add(desc);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHandler.AddToLogFile("Error while GetLogDescription", ex);
            }

            return string.Join("|", changedList.ToArray());
        }

        public static string UserRoleToString(string role)
        {
            string result = "";
            if (role == "HeadOfficeAdmin")
                result = "Agency Admin";
            else if (role == "BranchAdmin")
                result = "Branch Admin";
            else if (role == "BranchAgent")
                result = "Branch Agent";

            return result;
        }

        #endregion

        #region XML Stuffs

        public static string SerializeToXml(this object o)
        {
            return serializeToXml(o);
        }

        private static string serializeToXml(object o)
        {
            if (o == null)
            {
                return "";
            }
            XmlWriterSettings settings = new XmlWriterSettings
            {
                OmitXmlDeclaration = true,
                CheckCharacters = false
            };
            StringWriter output = new StringWriter();
            using (XmlWriter writer2 = XmlWriter.Create(output, settings))
            {
                XmlSerializer serializer = new XmlSerializer(o.GetType());
                XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
                namespaces.Add("", "");
                serializer.Serialize(writer2, o, namespaces);
                writer2.Close();
                return output.ToString();
            }
        }

        public static string TransformDoc<T>(string xslDocPath, T objectInfo) where T : class
        {
            string html = string.Empty;

            if (File.Exists(xslDocPath))
            {
                Logger.LogBL.Info("Loaded template: " + xslDocPath);
                XDocument xmlDoc = XDocument.Parse(objectInfo.SerializeToXml());
                XslCompiledTransform xslTrans = new XslCompiledTransform();
                xslTrans.Load(xslDocPath);

                using (MemoryStream stream = new MemoryStream())
                {
                    xslTrans.Transform(xmlDoc.CreateNavigator(), null, stream);
                    html = Encoding.UTF8.GetString(stream.ToArray());
                }
            }
            else
            {
                Logger.LogBL.Warn("No template file found!");
                throw new FileNotFoundException("No template file found!");
            }

            return html;
        }


        #endregion

        #region Image Stuffs
        /// <summary>
        /// Returns the base64 encoded string representation of the given image.
        /// </summary>
        /// <param name="image">A System.Drawing.Image to encode as a string.</param>
        static string ImageToBase64String(System.Drawing.Image image)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                image.Save(stream, image.RawFormat);
                return Convert.ToBase64String(stream.ToArray());
            }
        }

        /// <summary>
        /// Creates a new image from the given base64 encoded string.
        /// </summary>
        /// <param name="base64String">The encoded image data as a string.</param>
        static System.Drawing.Image ImageFromBase64String(string base64String)
        {
            using (MemoryStream stream = new MemoryStream(
                Convert.FromBase64String(base64String)))
            using (System.Drawing.Image sourceImage = System.Drawing.Image.FromStream(stream))
            {
                return new System.Drawing.Bitmap(sourceImage);
            }
        }
        #endregion

        #region General Base Page

        private static GeneralUserExtModel _CombineUserSession = null;

        /// <summary>
        /// Combine UserInfo and SAInfo
        /// </summary>        
        public static GeneralUserExtModel CombineUserSession
        {
            get
            {
                // if (_CombineUserSession == null) -- BUG
                // {
                HttpContext current = HttpContext.Current;
                GeneralUserExtModel model = new GeneralUserExtModel();
                model.FirstName = "";
                model.LastName = "";
                model.CurrentCulture = MXCultures.es_MX;

                if (current.Session["CURRENT_USER_INFO"] != null)
                {
                    object obj = current.Session["CURRENT_USER_INFO"];

                    if (obj.GetType() == typeof(User))
                    {
                        User userInfo = (User)current.Session["CURRENT_USER_INFO"];
                        if (userInfo != null)
                        {
                            model.UserID = userInfo.UserID;
                            model.AgencyID = userInfo.AgencyID;
                            model.FirstName = userInfo.FirstName;
                            model.LastName = userInfo.LastName;
                            model.CurrentCulture = (MXCultures)userInfo.Culture;
                            model.Role = userInfo.Role;
                            model.NumberLoginFailed = userInfo.NumLoginAttemptFail;
                            model.IsLocked = userInfo.IsLocked;
                        }
                    }
                    else if (obj.GetType() == typeof(SuperAdmin))
                    {
                        SuperAdmin saInfo = (SuperAdmin)current.Session["CURRENT_USER_INFO"];
                        if (saInfo != null)
                        {
                            model.UserID = saInfo.SuperAdminID;
                            model.AgencyID = 0;
                            model.FirstName = saInfo.FirstName;
                            model.LastName = saInfo.LastName;
                            model.CurrentCulture = (MXCultures)saInfo.Culture;
                            model.Role = UserRole.SuperAdmin;
                            model.NumberLoginFailed = saInfo.NumLoginAttemptFail;
                            model.IsLocked = saInfo.IsLocked;
                        }
                    }
                }

                _CombineUserSession = model;
                // } -- BUG

                return _CombineUserSession;
            }
            set
            {
                _CombineUserSession = value;
            }
        }

        public static bool IsEspanolLanguage { get { return CombineUserSession.CurrentCulture != MXCultures.en_US; } }

        public static string LoadString(string name)
        {
            string result = "";

            using (MXUnit DbUnit = new MXUnit())
            {
                GeneralUserExtModel userSession = Helper.CombineUserSession;

                if (userSession != null)
                {
                    if (userSession.CurrentCulture == MXCultures.en_US)
                        DbUnit.LocalizationStringUnit.LocalizationDicEn.TryGetValue(name.ToLower(), out result);
                    else
                        DbUnit.LocalizationStringUnit.LocalizationDicSp.TryGetValue(name.ToLower(), out result);
                }

            }
            return Helper.SafeString(result);
        }

        #endregion

        #region Action Buttons Info

        public static RemainInfoModel GetActionButtonInfo(User u, MXUnit dbUnit)
        {
            // var branch = dbUnit.BranchUnit.GetByID(u.BranchID);

            return new RemainInfoModel()
            {
                // IsManualBumpUp = branch != null && branch.BumpUpMode == "Manual",
                Highlight = " - " + (u.HighlightCount),
                HomepageGallery = " - " + (u.HpgCount),
                TopAd31 = " - " + (u.TopAd31Count),
                TopAd7 = " - " + (u.TopAd7Count),
                Urgent = " - " + (u.UrgentCount),
                srpgallery31 = " - " + (u.SrpGallery31Count),
            };
        }


        #endregion

        #region Browser Detection

        public static bool IsMacintosh(HttpRequest request)
        {
            if (request.UserAgent == null) return false;
            // http://www.useragentstring.com/pages/useragentstring.php
            // http://stackoverflow.com/questions/10960551/how-to-detect-apple-computers-using-user-agent
            // http://stackoverflow.com/questions/5155479/how-do-i-check-if-the-useragent-is-an-ipad-or-iphone
            return request.UserAgent.Contains("Macintosh") || request.UserAgent.Contains("iPad") ||
                   request.UserAgent.Contains("iPhone");
        }

        #endregion

        #region left menu caching
        static string LEFT_MENU_SESSION_KEY = "LEFT_MENU_SESSION_KEY";
        static string LEFT_MENU_SESSION_TIME_KEY = "LEFT_MENU_SESSION_TIME_KEY";
        public static string GetLeftMenuSessionValue()
        {
            if (HttpContext.Current != null)
            {
                var dt = new DateTime(1900, 1, 1);
                if (HttpContext.Current.Session[LEFT_MENU_SESSION_TIME_KEY] != null)
                {
                    dt = (DateTime)(HttpContext.Current.Session[LEFT_MENU_SESSION_TIME_KEY]);

                }
                if ((dt - DateTime.Now).TotalMinutes > 5)
                {
                    HttpContext.Current.Session[LEFT_MENU_SESSION_KEY] = "";
                    return string.Empty;
                }

                if (HttpContext.Current.Session[LEFT_MENU_SESSION_KEY] != null)
                    return HttpContext.Current.Session[LEFT_MENU_SESSION_KEY].ToString();
            }
            return string.Empty;
        }

        public static void SetLeftMenuSessionValue(string value)
        {
            if (HttpContext.Current != null)
            {
                HttpContext.Current.Session[LEFT_MENU_SESSION_KEY] = value;
                HttpContext.Current.Session[LEFT_MENU_SESSION_TIME_KEY] = DateTime.Now;
            }
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;

namespace Begium.Core.Security
{
    public class LoginManager : BaseObject
    {
        private string ServerRoot { get { return Helper.GetServerRoot(HttpContext.Current.Request); } }

        private UserLoginLog prepareLog(HttpContext ctx, string email)
        {
            string browserAgent = Helper.GetRequestAgent(ctx);
            int browserAgentID = 0;
            try
            {
                string friendlyName = Helper.GetBrowserName(ctx);
                using (var _lu = new BegiumLogUnit())
                {
                    browserAgentID = _lu.BrowserAgentUnit.GetBrowserAgentID(browserAgent, friendlyName);
                }
            }
            catch (Exception ex)
            {
                _log.Error("Unable to prepare UserLoginLog", ex);
            }

            UserLoginLog log = new UserLoginLog()
            {
                Login = email,
                IsSuccess = false,
                DateLogin = DateTime.Now.ToUniversalTime(),
                RemoteIP = Helper.GetRequestIP(ctx),
                Domain = Helper.GetRequestDomain(ctx),
                BrowserAgentID = short.Parse(browserAgentID.ToString())
            };

            return log;
        }

        /// <summary>
        /// Impersonate user
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="timeToLive">Specify how many MINUTES should the key live</param>
        /// <returns></returns>
        public LoginResult impersonate(HttpContext ctx, int timeToLive = 15)
        {
            LoginResult res = new LoginResult();

            // IMPORTANT!!! clear all session first
            ctx.Session.Clear();

            using (var _u = new BegiumUnit())
            {
                // Reset Language
                _u.LocalizationStringUnit.LocalizationDicEn = null;
                _u.LocalizationStringUnit.LocalizationDicSp = null;
                Helper.CombineUserSession = null;

                string token = ctx.Request.Params["token"];

                string key = "";
                try
                {
                    key = LoginManager.DecryptImpersonateKey(token);
                }
                catch (Exception ex)
                {
                    _log.Error("Could not DecryptImpersonateKey: " + token, ex);
                    res.Message = "Token is invalid.";
                    return res;
                }

                KeyValuePair<int, DateTime> kv = new KeyValuePair<int, DateTime>();
                try
                {
                    kv = LoginManager.ValidateImpersonateKey(key, 15);
                }
                catch (Exception ex)
                {
                    _log.Error("Could not ValidateImpersonateKey: " + token, ex);
                    res.Message = ex.Message;
                    return res;
                }

                var uInfo = _u.UserUnit.GetByID(kv.Key);
                if (uInfo == null)
                {
                    res.Message = "User not found.";
                    return res;
                }
                else if (!uInfo.IsActive)
                {
                    res.Message = "User is inactive.";
                    return res;
                }

                // everything looks fine now ...

                try
                {
                    // write log
                    UserLoginLog log = prepareLog(ctx, uInfo.Email);
                    // append SSO info
                    log.SSOToken = token;
                    log.SSOTimestamp = kv.Value;
                    log.IsSuccess = true;
                    log.UserID = uInfo.UserID;
                    using (var _lu = new BegiumLogUnit())
                    {
                        _lu.UserLoginLogUnit.Insert(log, true);
                    }

                    // update last login
                    _u.UserUnit.UpdateEach(x => x.UserID == uInfo.UserID, x => x.DateLastLogin = DateTime.Now.ToUniversalTime(), true);

                    // get user profile pic
                    var img = _u.ImageUnit.GetUniqueImage(uInfo.UserID, Core.ImageType.User);
                    if (img != null)
                    {
                        uInfo.ProfileImgURL = img.URL;
                    }

                    // get agencyName and branchName
                    string agencyName = "";
                    string branchName = "";
                    _u.UserUnit.GetBranchNameAgencyNameByUserID(uInfo.AgencyID, uInfo.UserID, ref agencyName, ref branchName);
                    uInfo.AgencyName = agencyName;
                    uInfo.BranchName = branchName;

                    // set user session
                    ctx.Session["CURRENT_USER_INFO"] = uInfo;
                    // set cookie
                    FormsAuthentication.SetAuthCookie(uInfo.Email, false);

                    // make response successful
                    res.IsSuccess = true;
                    res.Url = ServerRoot + "/Agency/Index.aspx";
                }
                catch (Exception ex)
                {
                    _log.Error("Could not finish impersonate process.", ex);
                    res.Message = "Unable to process request. Please try again later.";
                }
            }
            return res;
        }

        public LoginResult impersonateForSA(HttpContext ctx, int timeToLive = 15)
        {
            LoginResult res = new LoginResult();

            // IMPORTANT!!! clear all session first
            ctx.Session.Clear();

            using (var _u = new BegiumUnit())
            {
                // Reset Language
                _u.LocalizationStringUnit.LocalizationDicEn = null;
                _u.LocalizationStringUnit.LocalizationDicSp = null;
                Helper.CombineUserSession = null;

                string token = ctx.Request.Params["token"];

                string key = "";
                try
                {
                    key = LoginManager.DecryptImpersonateKey(token);
                }
                catch (Exception ex)
                {
                    _log.Error("Could not DecryptImpersonateKey: " + token, ex);
                    res.Message = "Token is invalid.";
                    return res;
                }

                KeyValuePair<int, DateTime> kv = new KeyValuePair<int, DateTime>();
                try
                {
                    kv = LoginManager.ValidateImpersonateKey(key, timeToLive);
                }
                catch (Exception ex)
                {
                    _log.Error("Could not ValidateImpersonateKey: " + token, ex);
                    res.Message = ex.Message;
                    return res;
                }

                SuperAdmin uInfo = _u.SuperAdminUnit.GetByID(kv.Key);
                if (uInfo == null)
                {
                    res.Message = "User not found.";
                    return res;
                }
                else if (!uInfo.IsActive)
                {
                    res.Message = "User is inactive.";
                    return res;
                }

                // everything looks fine now ...

                try
                {
                    // write log
                    UserLoginLog log = prepareLog(ctx, uInfo.Email);
                    // append SSO info
                    log.SSOToken = token;
                    log.SSOTimestamp = kv.Value;
                    log.IsSuccess = true;
                    log.UserID = uInfo.SuperAdminID;
                    using (var _lu = new BegiumLogUnit())
                    {
                        _lu.UserLoginLogUnit.Insert(log, true);
                    }

                    // update last login
                    _u.SuperAdminUnit.UpdateEach(x => x.SuperAdminID == uInfo.SuperAdminID, x => x.DateLastLogin = DateTime.Now.ToUniversalTime(), true);

                    // set user session
                    ctx.Session["CURRENT_USER_INFO"] = uInfo;
                    // set cookie
                    FormsAuthentication.SetAuthCookie(uInfo.Email, false);

                    // make response successful
                    res.IsSuccess = true;
                    res.Url = "~/SuperAdmin/Agency/AgenciesManager.aspx";
                }
                catch (Exception ex)
                {
                    _log.Error("Could not finish impersonate process.", ex);
                    res.Message = "Unable to process request. Please try again later.";
                }
            }

            return res;
        }

        public LoginResult impersonateSetPW(HttpContext ctx)
        {
            LoginResult res = new LoginResult();

            // IMPORTANT!!! clear all session first
            ctx.Session.Clear();

            using (var _u = new BegiumUnit())
            {
                using (var _ul = new BegiumLogUnit())
                {
                    // Reset Language
                    _u.LocalizationStringUnit.LocalizationDicEn = null;
                    _u.LocalizationStringUnit.LocalizationDicSp = null;
                    Helper.CombineUserSession = null;

                    string token = ctx.Request.Params["token"];

                    string key = "";
                    try
                    {
                        key = LoginManager.DecryptImpersonateKey(token);
                    }
                    catch (Exception ex)
                    {
                        _log.Error("Could not DecryptImpersonateKey: " + token, ex);
                        res.Message = "Token is invalid.";
                        return res;
                    }

                    KeyValuePair<int, Guid> kv = new KeyValuePair<int, Guid>();
                    try
                    {
                        kv = LoginManager.ValidateImpersonateKeySetPW(key);
                    }
                    catch (Exception ex)
                    {
                        _log.Error("Could not ValidateImpersonateKey: " + token, ex);
                        res.Message = ex.Message;
                        return res;
                    }

                    //valid OTP
                    var otp = _ul.OneTimePWUnit.GetOTPByOTPKey(kv.Key, kv.Value);
                    if (otp == null)
                    {
                        res.Message = "not valid";
                        return res;
                    }
                    else if (otp.ExpirationHours == 0)
                    {
                        if (otp.DateFirstLogin.Year > 1900)
                        {
                            res.Message = "Este enlace ya no es válido";
                            return res;
                        }
                    }
                    else if (otp.ExpirationHours > 0)
                    {
                        if (otp.DateCreated.AddHours(otp.ExpirationHours) < DateTime.UtcNow)
                        {
                            res.Message = "Este enlace ya no es válido";
                            return res;
                        }
                    }

                    var uInfo = _u.UserUnit.GetByID(kv.Key);
                    if (uInfo == null)
                    {
                        res.Message = "User not found.";
                        return res;
                    }
                    else if (!uInfo.IsActive)
                    {
                        res.Message = "User is inactive.";
                        return res;
                    }

                    // everything looks fine now ...
                    otp.DateFirstLogin = DateTime.UtcNow;
                    _ul.OneTimePWUnit.Update(otp, true);

                    try
                    {
                        // write log
                        UserLoginLog log = prepareLog(ctx, uInfo.Email);
                        // append SSO info
                        log.SSOToken = token;
                        log.SSOTimestamp = DateTime.UtcNow;
                        log.IsSuccess = true;
                        log.UserID = uInfo.UserID;
                        using (var _lu = new BegiumLogUnit())
                        {
                            _lu.UserLoginLogUnit.Insert(log, true);
                        }

                        // update last login
                        _u.UserUnit.UpdateEach(x => x.UserID == uInfo.UserID, x => x.DateLastLogin = DateTime.Now.ToUniversalTime(), true);

                        // get user profile pic
                        var img = _u.ImageUnit.GetUniqueImage(uInfo.UserID, Core.ImageType.User);
                        if (img != null && !img.IsDeleted)
                        {
                            uInfo.ProfileImgURL = img.URL;
                        }

                        // get agencyName and branchName
                        string agencyName = "";
                        string branchName = "";
                        _u.UserUnit.GetBranchNameAgencyNameByUserID(uInfo.AgencyID, uInfo.UserID, ref agencyName, ref branchName);
                        uInfo.AgencyName = agencyName;
                        uInfo.BranchName = branchName;

                        // set user session
                        ctx.Session["CURRENT_USER_INFO"] = uInfo;
                        // set cookie
                        FormsAuthentication.SetAuthCookie(uInfo.Email, false);

                        // make response successful
                        res.IsSuccess = true;
                        res.Url = ServerRoot + "/Agency/Index.aspx";
                    }
                    catch (Exception ex)
                    {
                        _log.Error("Could not finish impersonate process.", ex);
                        res.Message = "Unable to process request. Please try again later.";
                    }
                }
            }

            return res;
        }

        public LoginResult impersonateSetPWForSA(HttpContext ctx)
        {
            LoginResult res = new LoginResult();

            // IMPORTANT!!! clear all session first
            ctx.Session.Clear();

            using (var _u = new BegiumUnit())
            {
                using (var _ul = new BegiumLogUnit())
                {
                    // Reset Language
                    _u.LocalizationStringUnit.LocalizationDicEn = null;
                    _u.LocalizationStringUnit.LocalizationDicSp = null;
                    Helper.CombineUserSession = null;

                    string token = ctx.Request.Params["token"];

                    string key = "";
                    try
                    {
                        key = LoginManager.DecryptImpersonateKey(token);
                    }
                    catch (Exception ex)
                    {
                        _log.Error("Could not DecryptImpersonateKey: " + token, ex);
                        res.Message = "Token is invalid.";
                        return res;
                    }

                    KeyValuePair<int, Guid> kv = new KeyValuePair<int, Guid>();
                    try
                    {
                        kv = LoginManager.ValidateImpersonateKeySetPW(key);
                    }
                    catch (Exception ex)
                    {
                        _log.Error("Could not ValidateImpersonateKey: " + token, ex);
                        res.Message = ex.Message;
                        return res;
                    }

                    //valid OTP
                    var otp = _ul.OneTimePWUnit.GetOTPByOTPKey(kv.Key, kv.Value);
                    if (otp == null)
                    {
                        res.Message = "not valid";
                        return res;
                    }
                    else if (otp.DateFirstLogin.Year > 1900)
                    {
                        res.Message = "Este enlace ya no es válido";
                        return res;
                    }

                    SuperAdmin uInfo = _u.SuperAdminUnit.GetByID(kv.Key);
                    if (uInfo == null)
                    {
                        res.Message = "User not found.";
                        return res;
                    }
                    else if (!uInfo.IsActive)
                    {
                        res.Message = "User is inactive.";
                        return res;
                    }

                    // everything looks fine now ...
                    otp.DateFirstLogin = DateTime.UtcNow;
                    _ul.OneTimePWUnit.Update(otp, true);

                    try
                    {
                        // write log
                        UserLoginLog log = prepareLog(ctx, uInfo.Email);
                        // append SSO info
                        log.SSOToken = token;
                        log.SSOTimestamp = DateTime.UtcNow;
                        log.IsSuccess = true;
                        log.UserID = uInfo.SuperAdminID;
                        using (var _lu = new BegiumLogUnit())
                        {
                            _lu.UserLoginLogUnit.Insert(log, true);
                        }

                        // update last login
                        _u.UserUnit.UpdateEach(x => x.UserID == uInfo.SuperAdminID, x => x.DateLastLogin = DateTime.Now.ToUniversalTime(), true);

                        // set user session
                        ctx.Session["CURRENT_USER_INFO"] = uInfo;
                        // set cookie
                        FormsAuthentication.SetAuthCookie(uInfo.Email, false);

                        // make response successful
                        res.IsSuccess = true;
                        res.Url = "~/SuperAdmin/Agency/AgenciesManager.aspx";
                    }
                    catch (Exception ex)
                    {
                        _log.Error("Could not finish impersonate process.", ex);
                        res.Message = "Unable to process request. Please try again later.";
                    }
                }

            }

            return res;
        }

        private void IncreaseFailedTime(string email)
        {
            using (var _u = new BegiumUnit())
            {
                _u.UserUnit.UpdateEach(x => x.Email == email && x.NumLoginAttemptFail < 120, x => x.NumLoginAttemptFail += 1, true);
                _u.SuperAdminUnit.UpdateEach(x => x.Email == email && x.NumLoginAttemptFail < 120, x => x.NumLoginAttemptFail += 1, true);
            }
        }

        private void ResetFailedTime(string email)
        {
            using (var _u = new BegiumUnit())
            {
                _u.UserUnit.UpdateEach(x => x.Email == email, x => x.NumLoginAttemptFail = 0, true);
                _u.SuperAdminUnit.UpdateEach(x => x.Email == email, x => x.NumLoginAttemptFail = 0, true);
            }
        }

        private void LockUser(string email)
        {
            using (var _u = new BegiumUnit())
            {
                _u.UserUnit.UpdateEach(x => x.Email == email && !x.IsLocked, x =>
                {
                    x.IsLocked = true;
                    x.DateLocked = DateTime.UtcNow;
                }, true);
                _u.SuperAdminUnit.UpdateEach(x => x.Email == email && !x.IsLocked, x =>
                {
                    x.IsLocked = true;
                    x.DateLocked = DateTime.UtcNow;
                }, true);
            }
        }

        private bool loginNormalUser(HttpContext ctx, string email, string pwd)
        {
            UserLoginLog log = prepareLog(ctx, email);

            bool result = false;
            using (var _u = new BegiumUnit())
            {
                User uModel = _u.UserUnit.GetByLogin(email);
                if (uModel == null)
                    return result;

                string hashedPwd = Util.Helper.GetSHA256Hash(pwd, uModel.SaltPassword);
                User uInfo = _u.UserUnit.GetByLogin(email, hashedPwd);
                using (var _lu = new BegiumLogUnit())
                {
                    if (uInfo != null)
                    {
                        // write log
                        log.IsSuccess = true;
                        log.UserID = uInfo.UserID;
                        _lu.UserLoginLogUnit.Insert(log, true);

                        // update last login
                        _u.UserUnit.UpdateEach(x => x.UserID == uInfo.UserID, x => x.DateLastLogin = DateTime.Now.ToUniversalTime(), true);

                        // get user profile pic
                        var img = _u.ImageUnit.GetUniqueImage(uInfo.UserID, Core.ImageType.User);
                        if (img != null)
                            uInfo.ProfileImgURL = img.URL;

                        // get agencyName and branchName
                        string agencyName = "";
                        string branchName = "";
                        _u.UserUnit.GetBranchNameAgencyNameByUserID(uInfo.AgencyID, uInfo.UserID, ref agencyName, ref branchName);
                        uInfo.AgencyName = agencyName;
                        uInfo.BranchName = branchName;

                        // set user session
                        ctx.Session["CURRENT_USER_INFO"] = uInfo;
                        result = true;
                    }
                    else
                    {
                        // write log
                        log.UserID = 0;
                        _lu.UserLoginLogUnit.Insert(log, true);
                    }
                }
            }

            return result;
        }

        private bool loginSuperAdmin(HttpContext ctx, string email, string pwd)
        {
            UserLoginLog log = prepareLog(ctx, email);

            bool result = false;
            using (var _u = new BegiumUnit())
            {
                SuperAdmin sModel = _u.SuperAdminUnit.GetByLogin(email.Trim());
                if (sModel == null)
                    return result;

                string hashedPwd = Util.Helper.GetSHA256Hash(pwd, sModel.SaltPassword);
                SuperAdmin sInfo = _u.SuperAdminUnit.GetByLogin(email.Trim(), hashedPwd);
                using (var _lu = new BegiumLogUnit())
                {
                    if (sInfo != null)
                    {
                        // write log
                        log.IsSuccess = true;
                        log.UserID = sInfo.SuperAdminID;
                        _lu.UserLoginLogUnit.Insert(log, true);
                        // update last login
                        _u.SuperAdminUnit.UpdateEach(x => x.SuperAdminID == sInfo.SuperAdminID, x => x.DateLastLogin = DateTime.Now.ToUniversalTime(), true);
                        // set user session
                        ctx.Session["CURRENT_USER_INFO"] = sInfo;
                        result = true;
                    }
                    else
                    {
                        // write log
                        log.UserID = 0;
                        _lu.UserLoginLogUnit.Insert(log, true);
                    }
                }
            }
            return result;
        }

        public static string[] GetRoles(string login)
        {
            var currentCache = HttpContext.Current.Cache;
            string key = "ROLE_" + login.ToUpper();
            List<string> roles = new List<string>();
            if (currentCache[key] != null)
            {
                return (string[])currentCache[key];
            }

            using (var u = new BegiumUnit())
            {
                User normalUser = u.UserUnit.GetByLogin(login);
                if (normalUser != null)
                {
                    roles.Add("AGENCYUSER");
                }
                else
                {
                    SuperAdmin sa = u.SuperAdminUnit.GetByLogin(login);
                    if (sa != null)
                    {
                        roles.Add("SUPERADMIN");
                    }
                }
            }

            currentCache.Add(key,
                            roles.ToArray(),
                            null,
                            DateTime.Now.AddMinutes(60),
                            TimeSpan.Zero,
                            System.Web.Caching.CacheItemPriority.Default,
                            (x, y, z) => _log.WarnFormat("Cache {0} is expired!", key));

            return roles.ToArray();
        }

        static int MAX_ALLOW_LOGIN_FAILED = 5;
        public LoginResult ExecLogin(HttpContext ctx, string email, string pwd)
        {
            LoginResult result = new LoginResult();
            if (loginNormalUser(ctx, email, pwd))
            {
                result.Url = ServerRoot + "/Agency/Index.aspx";
                result.IsSuccess = true;
            }
            else if (loginSuperAdmin(ctx, email, pwd))
            {
                result.Url = ServerRoot + "/SuperAdmin/Agency/AgenciesManager.aspx";
                result.IsSuccess = true;
            }
            else
            {
                result.IsSuccess = false;
                result.Message = "Your login is invalid";
                IncreaseFailedTime(email);
            }

            if (result.IsSuccess)
            {
                if (Helper.CombineUserSession.NumberLoginFailed >= MAX_ALLOW_LOGIN_FAILED || Helper.CombineUserSession.IsLocked)
                {
                    Helper.CombineUserSession.IsLocked = true;
                    LockUser(email);

                    ctx.Session.Clear();

                    result.IsSuccess = false;
                    result.Message = "Your account is locked. Please contact administrator.";
                }
            }

            if (result.IsSuccess && Helper.CombineUserSession.NumberLoginFailed > 0)
                ResetFailedTime(email);


            return result;
        }

        public void LogOut(HttpContext ctx, bool goToLoginPage = true)
        {
            System.Web.Security.FormsAuthentication.SignOut();
            ctx.Session.Clear();
            ctx.Session.Abandon();

            using (var _u = new BegiumUnit())
            {
                // Reset Language
                _u.LocalizationStringUnit.LocalizationDicEn = null;
                _u.LocalizationStringUnit.LocalizationDicSp = null;
            }
            Helper.CombineUserSession = null;

            if (goToLoginPage)
            {
                ctx.Response.Redirect(Helper.LoginPage);
            }
        }

        /// <summary>
        /// Generate encrypted impersonate key using format: user id + ":" + GUID
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static string EncryptImpersonateKeySetPW(int userID, Guid otpKey)
        {
            return Helper.Encrypt(string.Format("{0}:{1}", userID, otpKey.ToString()));
        }


        /// <summary>
        /// Generate encrypted impersonate key using format: user id + "_" + current timestamp in UTC
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        public static string EncryptImpersonateKey(int userID)
        {
            return Helper.Encrypt(string.Format("{0}_{1}", userID, DateTime.Now.ToUniversalTime()));
        }

        /// <summary>
        /// Decrypt impersonate key
        /// </summary>
        /// <param name="encryptedKey"></param>        
        /// <returns></returns>
        public static string DecryptImpersonateKey(string encryptedKey)
        {
            return Helper.Decrypt(encryptedKey);
        }

        /// <summary>
        /// Validate impersonate key then return KeyValuePair(Of UserID, Encrypt time)
        /// </summary>
        /// <param name="key"></param>
        /// <param name="timeToLive">Specify how many MINUTES should the key live</param>
        /// <returns></returns>
        public static KeyValuePair<int, DateTime> ValidateImpersonateKey(string key, int? timeToLive)
        {
            string[] pieces = key.Split("_".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            int userID = int.Parse(pieces[0]);
            DateTime encryptTime = DateTime.Parse(pieces[1]);
            if (timeToLive.HasValue && timeToLive.Value > 0)
            {
                if (DateTime.Now.ToUniversalTime() > encryptTime.AddMinutes(timeToLive.Value))
                {
                    throw new Exception("Token has expired");
                }
            }
            return new KeyValuePair<int, DateTime>(userID, encryptTime);
        }

        public static KeyValuePair<int, Guid> ValidateImpersonateKeySetPW(string key)
        {
            string[] pieces = key.Split(":".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            int userID = int.Parse(pieces[0]);
            Guid encryptGuid = Guid.Parse(pieces[1]);
            return new KeyValuePair<int, Guid>(userID, encryptGuid);
        }

    }
}

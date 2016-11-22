using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCTutorial.WebAPI
{
    internal class Constants
    {
        internal static int TokenExpiryHours = 2;
        internal static string Success = "Success";
        internal static int CountLimit = 3;

        internal static class Error
        {
            internal static class Login
            {
                internal const string InValid = "UserInValid";
                internal const string WrongPassword = "WrongPassword";
                internal const string Exception = "LoginException";
                internal const string NoUser = "UserNotExist";
                public const string InActive = "InActive";
                public const string IsLocked = "IsLocked";
                public const string IsDeleted = "IsDeleted";
                //public const string NoUser = "NoUser";
                public const string IsValid = "IsValid";
                //public const string Exception = "LoginException";
            }

            internal static class Password
            {
                internal const string NoData = "userNoData";
                internal const string Exception = "userException";
                internal const string SaveFailed = "resetPasswordSaveFailed";
            }

            internal static class Token
            {
                internal const string InValid = "invalidToken";
            }

            internal static class SecurityQuestions
            {
                internal const string NoData_User = "securityQuestionNoDataForUser";
                internal const string NoData = "securityQuestionNoData";
                internal const string NoSource = "securityQuestionNoSource";
                internal const string Exception = "securityQuestionException";
                internal const string NotAdded = "securityQuestionNotAdded";
                internal const string OK = "securityQuestionAdded";
            }

            internal static class User
            {
                internal const string Exception = "userException";
                internal const string InValid = "invalidUser";
                internal const string NoData = "userNoData";
                internal const string NoSource = "userNoSource";
                internal const string NoPassword = "PasswordMismatch";
                internal const string NotDeleted = "userNotDeleted";
                internal const string NoPasswordReset = "unsuccessfulResetPassword";
                internal const string UpdatePasswordError = "UpdatePasswordError";
                internal const string AnswerNotVerified="ErrorInAnswerVerification";
            }

        }

        internal static class PasswordCreation
        {
            internal const string PasswordReset = "passwordResetedOK";
            
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVCTutorial.DAL.ADO
{
    public class MSSQLConstants
    {
        public const string Login_Get_Email = "login_get_email";

        public const string Staff_Get_Email_AllStatus = "Get the user by email whether the user is active or not.";
        public const string Staff_Get_Email = "staff_get_email";
        public const string Staff_Get_UserName = "staff_get_username";
        public const string Staff_Get_Peers_Email = "Gets all my peer staff except me.";
        public const string Staff_Add = "staff_add";
        public const string Staff_Update = "staff_update";
        public const string Staff_Activate = "staff_activate";
        public const string Staff_Deactivate = "staff_deactivate";
        public const string Staff_Lock = "staff_lock";
        public const string Staff_Unlock = "staff_unlock";
        public const string Staff_Delete = "staff_delete";

        public const string Team_Get_Email = "Get the teams by requested user's email & to which organization the user belong to.";
        public const string Team_Get_TeamName_OrgID = "get the team by teamname & organization.";
        public const string Team_Add = "team_add";
        public const string Team_Update = "team_update";
        public const string Team_Delete = "team_delete";

        public const string Team_Assign_Add = "Assign staff user to a team while adding staff.";
        public const string Team_Assign_Update = "Update staff user to a team.";

        public const string Role_Get_Email = "Get the roles by requested user's email & to which organization the user belong to.";
        public const string Role_Get_TeamName_OrgID = "Get the role by rolename & organization.";
        public const string Role_Add = "Add role.";
        public const string Role_Update = "Update Role";
        public const string Role_Delete = "Delete Role";

        public const string Role_Assign_Add = "Assign staff user to a role while adding staff.";
        public const string Role_Assign_Update = "Update staff user to a role.";

        //Timesheet
        public const string User_Get = "Get user";
        public const string User_Add = "Add user";
        public const string UserRole_Add = "Add userRole";
        public const string User_Update = "Update user";
        public const string User_Delete = "Delete user";

        //Account-Credentials
        //public const string Account_Validate = "Get account";
        public const string CheckValidUser = "Check valid user";
        public const string ValidateUser = "Validate User";
        public const string GetEmployeeByUsername = "Get EmployeeInfo";
        public const string Get_User_Roles = "Get user roles";
        public const string TemporaryPassword_Add = "Add temporary password";
        public const string CheckTempPassword = "Check TempPassword";
        public const string TemporaryPassword_Update = "Update TempPassword";
        public const string Clear_UserPassword = "Clear UserPassword";

        //Registration
        public const string SecurityQuestion_Get = "Get SecurityQuestions";
        public const string UserSecurityQuestion_Get = "Get UserSecurityQuestions";
        public const string VerifyUserAnswers = "Verify User Answers";
        public const string RegistrationQuestion_Get = "Get QuestionInRegistration";
        public const string SecurityQuestion_Add = "Add Security Question";
        public const string SecurityQuestion_Update = "Update Security Question";
        public const string RegisterNewUser = "Register new user";
        public const string UserPassword_Update = "Update user password";
        public const string ClearTemporaryPassword_Update= "Clear TemporaryPassword";
        public const string Insert_WrongPassword = "Insert WrongPassword";
        public const string Count_WrongPassword = "Count WrongPassword";
        public const string Update_UserLocked = "Update user locked";
        public const string Delete_WrongPassword = "Delete wrong password";





    }
}


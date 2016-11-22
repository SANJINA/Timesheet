using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVCTutorial.DAL.ADO
{
    public class MSSQLStatements
    {
        public static string GetStatement(string name)
        {
            switch(name)
            {
                case MSSQLConstants.Login_Get_Email:
                    return @"SELECT OrgId, UserName, FirstName, LastName, Email, Password, Photo, DOB, Gender, MobileNumber, OfficeNumber, AboutMe, 
                        Expertixe, ResumeContent, ResumeName, IsLocked, IsActive, WrongPassword, CreatedDate, CreatedBy, LastModifiedDate, LastModifiedBy FROM User_Staff
                        where Email = @email";

                case MSSQLConstants.Staff_Get_Email_AllStatus:
                    return @"SELECT Id, OrgId, UserName, FirstName, LastName, Email, Photo, DOB, Gender, MobileNumber, OfficeNumber, AboutMe, 
                        Expertixe, ResumeContent, ResumeName, IsLocked, IsActive, WrongPassword, CreatedDate, CreatedBy, LastModifiedDate, LastModifiedBy FROM User_Staff
                        where Email = @email";
                case MSSQLConstants.Staff_Get_Email:
                    return @"SELECT Id, OrgId, UserName, FirstName, LastName, Email, Photo, DOB, Gender, MobileNumber, OfficeNumber, AboutMe, 
                        Expertixe, ResumeContent, ResumeName, IsLocked, IsActive, WrongPassword, CreatedDate, CreatedBy, LastModifiedDate, LastModifiedBy FROM User_Staff
                        where IsActive = 1 AND Email = @email";
                case MSSQLConstants.Staff_Get_UserName:
                    return @"SELECT Id, OrgId, UserName, FirstName, LastName, Email, Photo, DOB, Gender, MobileNumber, OfficeNumber, AboutMe, 
                        Expertixe, ResumeContent, ResumeName, IsLocked, IsActive, WrongPassword, CreatedDate, CreatedBy, LastModifiedDate, LastModifiedBy FROM User_Staff
                        where IsActive = 1 AND UserName = @username";
                case MSSQLConstants.Staff_Get_Peers_Email:
                    return @"SELECT u.Id, u.OrgId, u.UserName, u.FirstName, u.LastName, u.Email, u.Photo, u.DOB, u.Gender, u.MobileNumber, u.OfficeNumber, u.AboutMe, 
                        u.Expertixe, u.ResumeContent, u.ResumeName, u.IsLocked, u.IsActive, u.WrongPassword, u.CreatedDate, u.CreatedBy, u.LastModifiedDate, u.LastModifiedBy 
                        FROM User_Staff me INNER JOIN User_Staff u ON u.OrgId = me.OrgId WHERE me.Email = @email AND u.Email != @email";
                case MSSQLConstants.Staff_Add :
                    return @"DECLARE @userid INT ; SELECT @userid = id FROM User_Staff WHERE Email = @byuseremail;
                        INSERT INTO User_Staff(OrgId, UserName, FirstName, LastName, [Password], Email, Photo, DOB, Gender, MobileNumber, OfficeNumber, AboutMe, 
                        Expertixe, ResumeContent, ResumeName, IsLocked, IsActive, WrongPassword, CreatedDate, CreatedBy, LastModifiedDate, LastModifiedBy)
                        SELECT @OrgId, @UserName, @FirstName, @LastName, @Password, @Email, @Photo, @DOB, @Gender, @MobileNumber, @OfficeNumber, @AboutMe, 
                        @Expertixe, @ResumeContent, @ResumeName, 0, 1, 0, GetDate(), @userid, GetDate(), @userid";
                case MSSQLConstants.Staff_Update:
                    return @"DECLARE @userid INT ; SELECT @userid = id FROM User_Staff WHERE Email = @byuseremail;
                        UPDATE User_Staff SET FirstName = @FirstName, LastName = @LastName, Email = @Email, Photo = @Photo, DOB = @DOB, Gender = @Gender, 
                        MobileNumber = @MobileNumber, OfficeNumber = @OfficeNumber, AboutMe = @AboutMe, Expertixe = @Expertixe, ResumeContent = @ResumeContent, 
                        ResumeName = @ResumeName, LastModifiedDate = GETDATE(), LastModifiedBy = @userid WHERE id = @id";
                case MSSQLConstants.Staff_Activate:
                    return @"DECLARE @userid INT ; SELECT @userid = id FROM User_Staff WHERE Email = @byuseremail;
                        UPDATE User_Staff SET IsActive = 1, LastModifiedDate = GETDATE(), LastModifiedBy = @userid WHERE id = @id";
                case MSSQLConstants.Staff_Deactivate:
                    return @"DECLARE @userid INT ; SELECT @userid = id FROM User_Staff WHERE Email = @byuseremail;
                        UPDATE User_Staff SET IsActive = 0, LastModifiedDate = GETDATE(), LastModifiedBy = @userid WHERE id = @id";
                case MSSQLConstants.Staff_Lock:
                    return @"DECLARE @userid INT ; SELECT @userid = id FROM User_Staff WHERE Email = @byuseremail;
                        UPDATE User_Staff SET IsLocked = 1, LastModifiedDate = GETDATE(), LastModifiedBy = @userid WHERE id = @id";
                case MSSQLConstants.Staff_Unlock:
                    return @"DECLARE @userid INT ; SELECT @userid = id FROM User_Staff WHERE Email = @byuseremail;
                        UPDATE User_Staff SET IsLocked = 0, LastModifiedDate = GETDATE(), LastModifiedBy = @userid WHERE id = @id";
                case MSSQLConstants.Staff_Delete:
                    return @"DELETE User_Staff WHERE id = @id";

                case MSSQLConstants.Team_Get_Email:
                    return @"SELECT t.* FROM team t INNER JOIN User_Staff us ON us.OrgId = t.OrgId WHERE us.Email = @email";
                case MSSQLConstants.Team_Get_TeamName_OrgID:
                    return @"SELECT * FROM Team WHERE teamname = @teamname AND OrgId = @orgid";
                case MSSQLConstants.Team_Add:
                    return @"DECLARE @userid INT ; SELECT @userid = id FROM User_Staff WHERE Email = @byuseremail;
                        INSERT INTO Team(OrgId, TeamName, CreatedDate, CreatedBy, LastModifiedDate, LastModifiedBy)
                        SELECT @orgid, @teamname, GetDate(), @userid, GetDate(), @userid";
                case MSSQLConstants.Team_Update:
                    return @"DECLARE @userid INT ; SELECT @userid = id FROM User_Staff WHERE Email = @byuseremail;
                        UPDATE Team SET TeamName = @teamname, LastModifiedDate = GETDATE(), LastModifiedBy = @userid WHERE id = @id";
                case MSSQLConstants.Team_Delete:
                    return @"DELETE Team WHERE id = @id";

                case MSSQLConstants.Team_Assign_Add:
                    return @"DECLARE @userid INT ; SELECT @userid = id FROM User_Staff WHERE Email = @byuseremail;
                        INSERT INTO Team_Assign(StaffId, TeamId, CreatedDate, CreatedBy, LastModifiedDate, LastModifiedBy)
                        SELECT @staffid, @teamid, GetDate(), @userid, GetDate(), @userid";
                case MSSQLConstants.Team_Assign_Update:
                    return @"DECLARE @userid INT ; SELECT @userid = id FROM User_Staff WHERE Email = @byuseremail;
                        UPDATE Team_Assign SET TeamId = @teamid, LastModifiedDate = GETDATE(), LastModifiedBy = @userid WHERE staffid = @staffid AND Id = @id";

                case MSSQLConstants.Role_Get_Email:
                    return @"SELECT r.* FROM Roles r INNER JOIN User_Staff us ON us.OrgId = r.OrgId WHERE us.Email = @byuseremail AND RoleName != 'super admin' ORDER BY RoleName";
                case MSSQLConstants.Role_Get_TeamName_OrgID:
                    return @"SELECT * FROM Roles WHERE rolename = @rolename AND OrgId = @orgid";
                case MSSQLConstants.Role_Add:
                    return @"DECLARE @userid INT ; SELECT @userid = id FROM User_Staff WHERE Email = @byuseremail;
                        INSERT INTO Roles(OrgId, RoleName, CreatedDate, CreatedBy, LastModifiedDate, LastModifiedBy)
                        SELECT @orgid, @rolename, GetDate(), @userid, GetDate(), @userid";
                case MSSQLConstants.Role_Update:
                    return @"DECLARE @userid INT ; SELECT @userid = id FROM User_Staff WHERE Email = @byuseremail;
                        UPDATE Roles SET RoleName = @rolename, LastModifiedDate = GETDATE(), LastModifiedBy = @userid WHERE id = @id";
                case MSSQLConstants.Role_Delete:
                    return @"DELETE Roles WHERE id = @id";
                case MSSQLConstants.Role_Assign_Add:
                    return @"DECLARE @userid INT ; SELECT @userid = id FROM User_Staff WHERE Email = @byuseremail;
                        INSERT INTO Role_Assign(StaffId, RoleId, CreatedDate, CreatedBy, LastModifiedDate, LastModifiedBy)
                        SELECT @staffid, @roleid, GetDate(), @userid, GetDate(), @userid";
                case MSSQLConstants.Role_Assign_Update:
                    return @"DECLARE @userid INT ; SELECT @userid = id FROM User_Staff WHERE Email = @byuseremail;
                        UPDATE Role_Assign SET RoleId = @roleid, LastModifiedDate = GETDATE(), LastModifiedBy = @userid WHERE staffid = @staffid AND Id = @id";

                
                case MSSQLConstants.User_Get:
                    return @"select * from [User]";
                case MSSQLConstants.User_Add:
                    return @"Insert into [User] (FirstName, LastName,Email, DOB, Location,IsActive,IsDeleted, IsLocked,LastUpdatedBy,LastUpdatedDate) 
                    Values(@firstname, @lastname,@email,@DOB,@location,@isActive,@isDeleted,@isLocked,@lastUpdatedBy, @lastUpdatedDate); select SCOPE_IDENTITY();";

                //case MSSQLConstants.UserRole_Add:
                //    return @"Insert into RoleAssignment (EmployeeId,RoleId,LastUpdatedBy,LastUpdatedDate) values ();";

                case MSSQLConstants.User_Delete:
                    return @"delete from TemporaryPassword where EmployeeId = @EmployeeId;
                            delete from Registration where EmployeeId = @EmployeeId;
                            delete from WrongPassword where EmployeeId = @EmployeeId;
                             delete from [User] where EmployeeId= @EmployeeId;";
                case MSSQLConstants.User_Update:
                    return @"UPDATE [User] SET FirstName=@firstname,LastName=@lastname,Email=@email,Location=@location
                             WHERE EmployeeId=@employeeId;";



                case MSSQLConstants.GetEmployeeByUsername:
                    return @"select * from [User] 
                            where Email= @username";
                case MSSQLConstants.Get_User_Roles:
                    return @"select u.Email, r.Description
                            from [User] As u
                            join RoleAssignment As ra ON u.EmployeeId=ra.EmployeeId
                            join Roles As r ON ra.RoleId=r.RoleId Where u.Email = @username ";

                case MSSQLConstants.CheckValidUser:
                    return @"select IsActive, IsLocked, IsDeleted from [User] where Email = @UserName;";

                case MSSQLConstants.ValidateUser:
                    return @"declare @empId int;
                             declare @isReg bit; 
                            declare @shouldChangePassword bit;
                            select @shouldChangePassword = (SELECT CASE WHEN ISNULL(EncryptedPassword, '') = '' THEN 1 ELSE 0 END FROM [User] WHERE Email = @UserName);                            
                            select @empId = EmployeeId, @isReg = IsRegistered from [User] where Email = @UserName;
                            IF (SELECT COUNT(*) FROM [User] WHERE Email = @UserName AND @isReg= 0) = 1 OR
	                           (SELECT COUNT(*)  FROM [User] WHERE Email = @UserName AND IsRegistered = 1 AND EncryptedPassword = '') = 1
		                        SELECT @shouldChangePassword AS ShouldChangePassword,
                                       t.EmployeeId, u.IsRegistered, u.FirstName, u.LastName 
                                FROM TemporaryPassword t INNER JOIN [User] u on t.EmployeeId = u.EmployeeId
		                        WHERE t.EncryptedPassword = @Password AND t.EmployeeId= @empId;
                            ELSE
	                            SELECT @shouldChangePassword AS ShouldChangePassword,
                                       EmployeeId, IsRegistered, FirstName, LastName 
	                            FROM [User] 
                                WHERE Email = @UserName AND EncryptedPassword = @Password;";

                case MSSQLConstants.TemporaryPassword_Add:
                    return @"Insert into TemporaryPassword (EmployeeId,EncryptedPassword,LastUpdatedBy,LastUpdatedDate) 
                    Values(@EmployeeId, @EncryptedPassword,@LastUpdatedBy, @LastUpdatedDate)";
                case MSSQLConstants.CheckTempPassword:
                    return @"select EmployeeId from TemporaryPassword 
                            where EmployeeId= @EmployeeId AND EncryptedPassword= @EncryptedPassword";

                case MSSQLConstants.Clear_UserPassword:
                    return @"UPDATE [User] SET EncryptedPassword ='', LastUpdatedBy = @LastUpdatedBy WHERE EmployeeId = @EmployeeId;";

                case MSSQLConstants.TemporaryPassword_Update:
                    return @"UPDATE [TemporaryPassword] SET EncryptedPassword =@EncryptedPassword, LastUpdatedBy = @LastUpdatedBy WHERE EmployeeId = @EmployeeId;";


                //case MSSQLConstants.SecurityQuestion_Get1:
                //    return @"select * from SecurityQuestion";
                case MSSQLConstants.SecurityQuestion_Get:
                    return @"SELECT sq.QuestionId, sq.Question,
                             CASE WHEN ((SELECT  COUNT(*) FROM    Registration WHERE   QuestionId= sq.QuestionId) > 0) THEN cast(0 as bit) ELSE cast(1 as bit) END AS CanUpdate
                             FROM SecurityQuestion sq";   

                case MSSQLConstants.SecurityQuestion_Add:
                    return @"   IF NOT EXISTS 
                                (   SELECT  1
                                    FROM    SecurityQuestion 
                                    WHERE   Question=@Question        
                                )
                                BEGIN
                                   INSERT INTO SecurityQuestion (Question,LastUpdatedBy,LastUpdatedDate) VALUES (@Question,@LastUpdatedBy,@LastUpdatedDate) 
                                END;";

                case MSSQLConstants.SecurityQuestion_Update:
                    return @" UPDATE [SecurityQuestion] SET Question=@Question,LastUpdatedBy=@LastUpdatedBy,LastUpdatedDate=@LastUpdatedDate
		                      WHERE QuestionId= @QuestionId;";

                case MSSQLConstants.UserSecurityQuestion_Get:
                    return @" SELECT r.QuestionId,s.Question
                              FROM [User] as u JOIN Registration AS r ON r.EmployeeId= u.EmployeeId
				                               JOIN SecurityQuestion AS s ON s.QuestionId= r.QuestionId
                              WHERE u.Email= @Email;";

                case MSSQLConstants.VerifyUserAnswers:
                    return @"declare @a1 nvarchar(50)
                             declare @a2 nvarchar(50)
                            declare @a3 nvarchar(50)
                            select @a1 = Answer from Registration where QuestionId= @QuestionId1 and LastUpdatedBy = @Email;
                            select @a2 = Answer from Registration where QuestionId= @QuestionId2 and LastUpdatedBy = @Email;
                            select @a3 = Answer from Registration where QuestionId= @QuestionId3 and LastUpdatedBy = @Email;
                            select EmployeeId 
                            from Registration 
                            where @a1=@Answer1 and @a2=@Answer2 and @a3=@Answer3 and LastUpdatedBy =@Email;
                            ";


                case MSSQLConstants.RegisterNewUser:
                    return @"DECLARE @eId INT;
                             SET @eId = (SELECT EmployeeId FROM [User] WHERE Email = @email);
                             INSERT INTO Registration (EmployeeId,QuestionId,Answer,LastUpdatedBy,LastUpdatedDate) 
                             VALUES (@eId,@QuestionId,@Answer,@LastUpdatedBy,@LastUpdatedDate)";

                case MSSQLConstants.UserPassword_Update:
                    return @"update [User] set EncryptedPassword = @EncryptedPassword, IsRegistered = 1 where EmployeeId = @EmployeeId;";

                case MSSQLConstants.ClearTemporaryPassword_Update:
                    return @"update TemporaryPassword set EncryptedPassword='' where EmployeeId = @EmployeeId";

                case MSSQLConstants.Insert_WrongPassword:
                    return @"DECLARE @empId int;
                             SELECT @empId = EmployeeId from [User] 
                             WHERE Email= @UserName;
                             INSERT into WrongPassword (EmployeeId,LastUpdatedBy,LastUpdatedDate) VALUES (@empId,@lastUpdatedBy,@lastUpdatedDate)";

                case MSSQLConstants.Count_WrongPassword:
                    return @"DECLARE @empId int;
                             SELECT @empId = EmployeeId FROM [User] WHERE Email= @UserName;
                             SELECT COUNT(EmployeeId) as IdCount FROM WrongPassword WHERE EmployeeId = @empId";

                case MSSQLConstants.Update_UserLocked:
                    return @"DECLARE @empId int;
                             SELECT @empId = EmployeeId FROM [User] WHERE Email= @UserName;
                             UPDATE [User] SET IsLocked=1 where EmployeeId = @empId";

                case MSSQLConstants.Delete_WrongPassword:
                    return @"DELETE from WrongPassword where EmployeeId= @EmployeeId;";

                default :
                    return "";
            }
        }
    }
}

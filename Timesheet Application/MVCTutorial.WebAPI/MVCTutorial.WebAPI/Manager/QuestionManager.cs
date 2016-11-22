using MVCTutorial.DAL.ADO;
using MVCTutorial.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Web;

namespace MVCTutorial.WebAPI.Manager
{
    public class QuestionManager
    {
        private static IDatabase _db = new Database(); 

        internal static List<SecurityQuestion> GetQuestions()
        {
            try
            {
                string sql = _db.getStatement(MSSQLConstants.SecurityQuestion_Get);
                System.Data.Common.DbParameter[] param = { };
                DataSet ds = _db.ExecuteDataSet(sql, param, MSSQLConstants.SecurityQuestion_Get);
                List<SecurityQuestion> sqlist = ProcessInfo(ds);
                return sqlist;
                //return ProcessInfo(ds);
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        internal static List<SecurityQuestion> GetQuestions(string email)
        {
            try
            {
                string sql = _db.getStatement(MSSQLConstants.UserSecurityQuestion_Get);
                System.Data.Common.DbParameter[] param = { _db.getDbParameter("?Email", email) };
                DataSet ds = _db.ExecuteDataSet(sql, param, MSSQLConstants.UserSecurityQuestion_Get);

                if (ds != null)
                {
                    return ds.Tables[0].AsEnumerable().Select(x => new SecurityQuestion
                    {
                        QuestionId = x.Field<int>("QuestionId"),
                        Question = x.Field<string>("Question")
                        //Answer = x.Field<string>("Answer")
                    }).ToList();
                }
                else
                {
                    return null;
                }              
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        internal static int VerifyUserAnswers(ForgotPasswordInfo userInfo)
        {
            try
            {
                string sql = _db.getStatement(MSSQLConstants.VerifyUserAnswers);

                DbParameter[] param = { _db.getDbParameter("?Email", userInfo.Email),
                                        _db.getDbParameter("?QuestionId1",userInfo.SecurityQuestions[0].QuestionId),
                                        _db.getDbParameter("?QuestionId2",userInfo.SecurityQuestions[1].QuestionId),
                                        _db.getDbParameter("?QuestionId3",userInfo.SecurityQuestions[2].QuestionId),
                                        _db.getDbParameter("?Answer1",userInfo.SecurityQuestions[0].Answer),
                                        _db.getDbParameter("?Answer2",userInfo.SecurityQuestions[1].Answer),
                                        _db.getDbParameter("?Answer3",userInfo.SecurityQuestions[2].Answer) };

                int result = _db.ExecuteScalarInt(sql, param);

                return result;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }


        internal static int AddQuestion(SecurityQuestion securityQuestion, string email)
        {
            try
            {
                string sql = _db.getStatement(MSSQLConstants.SecurityQuestion_Add);
             
                DbParameter[] param = { _db.getDbParameter("?Question", securityQuestion.Question),                   
                    _db.getDbParameter("?LastUpdatedBy",email),
                    _db.getDbParameter("?LastUpdatedDate", DateTime.Now)};
               
                int result = _db.ExecuteNonQuery(sql, param);

                return result;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }

        internal static int UpdateQuestion(SecurityQuestion sq, string email)
        {
            try
            {
                string sql = _db.getStatement(MSSQLConstants.SecurityQuestion_Update);

                DbParameter[] param = { _db.getDbParameter("?Question", sq.Question),
                                        _db.getDbParameter("?QuestionId", sq.QuestionId),
                                        _db.getDbParameter("?LastUpdatedBy", email),
                                        _db.getDbParameter("?LastUpdatedDate", DateTime.Now)};
                int result = (int)_db.ExecuteNonQuery(sql, param);
                return result;
            }
            catch (Exception ex)
            {
                return -1;
            }
        }      

        private static List<SecurityQuestion> ProcessInfo(DataSet ds)
        {
            return ds.Tables[0].AsEnumerable().Select(x => new SecurityQuestion
            {
                QuestionId = x.Field<int>("QuestionId"),
                Question = x.Field<string>("Question"),
                CanUpdate = x.Field<bool>("CanUpdate")              
            }).ToList();
        }

    }
}
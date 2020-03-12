using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using TestStation.Authentication.Data;

namespace TestStation.Authentication
{
    internal class DataBaseProxy : IDisposable
    {
        const string TimeFormat = @"yyyy-MM-dd hh:mm:ss";
        private DbConnection _connection;
        private Dictionary<UserGroup, string> _groupToNameRegex;

        public DataBaseProxy()
        {
            InitializeDatabaseAndConnection();
            _groupToNameRegex = new Dictionary<UserGroup, string>(4)
            {
                {UserGroup.Administrator, "^10(S[a-zA-Z0-9]{5}|A[a-zA-Z0-9]{5}S)$" },
                {UserGroup.Configurator,  "^10(S[a-zA-Z0-9]{5}C|A[a-zA-Z0-9]{5}C)$"},
                {UserGroup.Adjustor,  "^10(S[a-zA-Z0-9]{5}D|A[a-zA-Z0-9]{5}D)$"},
                {UserGroup.Operator,  "^10(A[a-zA-Z0-9]{5}|S[a-zA-Z0-9]{5}A)$"}
            };
        }

        public void Dispose()
        {
            _connection?.Dispose();
        }

        private void InitializeDatabaseAndConnection()
        {
            string testflowHome = Environment.GetEnvironmentVariable("TESTFLOW_HOME");
            if (string.IsNullOrWhiteSpace(testflowHome))
            {
                throw new ApplicationException("Testflow home directory not exist.");
            }
            if (!testflowHome.EndsWith(Path.DirectorySeparatorChar.ToString()))
            {
                testflowHome += Path.DirectorySeparatorChar;
            }
            string databaseFilePath = testflowHome + Constants.DatabaseName;
            // 使用DbProviderFactory方式连接需要在App.Config文件中定义DbProviderFactories节点
            // 但是App.Config文件只在入口Assembly中时才会被默认加载，所以目前写死为SqlConnection
            //            Connection.ConnectionString = $"Data Source={databaseFilePath}";

            // 如果已经存在则直接跳出
            if (File.Exists(databaseFilePath))
            {
                _connection = new SQLiteConnection($"Data Source={databaseFilePath}");
                _connection.Open();
                return;
            }
            _connection = new SQLiteConnection($"Data Source={databaseFilePath}");
            DbTransaction transaction = null;
            try
            {
                const string endDelim = ";";
                const string commentPrefix = "--";
                _connection.Open();
                string sqlCmds = Properties.Resources.Authentication;
                string[] cmdLines = sqlCmds.Split(Environment.NewLine.ToCharArray());
                StringBuilder createTableCmd = new StringBuilder(500);
                transaction = _connection.BeginTransaction(IsolationLevel.Serializable);
                foreach (string cmdLine in cmdLines)
                {
                    string lineData = cmdLine.Trim();
                    if (lineData.StartsWith(commentPrefix))
                    {
                        continue;
                    }
                    createTableCmd.Append(lineData);
                    if (lineData.EndsWith(endDelim))
                    {
                        DbCommand dbCommand = _connection.CreateCommand();
                        dbCommand.CommandText = createTableCmd.ToString();
                        dbCommand.Transaction = transaction;
                        dbCommand.ExecuteNonQuery();
                        createTableCmd.Clear();
                    }
                }
                InsertDefaultData(transaction);
                transaction.Commit();
                transaction.Dispose();
            }
            catch (Exception ex)
            {
                transaction?.Rollback();
                transaction?.Dispose();
                _connection?.Dispose();
                // 如果失败则删除文件
                File.Delete(databaseFilePath);
                throw;
            }
        }

        private void InsertDefaultData(DbTransaction transaction)
        {
            InsertUserGroupInfo(transaction);
            InsertDefaultUser(transaction);
            InsertAuthorityInfo(transaction);
        }

        private void InsertUserGroupInfo(DbTransaction transaction)
        {
            // 插入账户类型信息
            string cmd =
                $"INSERT INTO TestStation_UserGroup (GroupName, Priority, Description) VALUES ('{UserGroup.SuperAdmin}', {(int)UserGroup.SuperAdmin}, '超级管理员')";
            ExecuteWriteCmd(cmd, transaction);

            cmd =
                $"INSERT INTO TestStation_UserGroup (GroupName, Priority, Description) VALUES ('{UserGroup.Administrator}', {(int)UserGroup.Administrator}, '管理员')";
            ExecuteWriteCmd(cmd, transaction);

            cmd = $"INSERT INTO TestStation_UserGroup (GroupName, Priority, Description) VALUES ('{UserGroup.Configurator}', {(int)UserGroup.Configurator}, '配置员')";
            ExecuteWriteCmd(cmd, transaction);

            cmd = $"INSERT INTO TestStation_UserGroup (GroupName, Priority, Description) VALUES ('{UserGroup.Adjustor}', {(int)UserGroup.Adjustor}, '调试员')";
            ExecuteWriteCmd(cmd, transaction);

            cmd = $"INSERT INTO TestStation_UserGroup (GroupName, Priority, Description) VALUES ('{UserGroup.Operator}', {(int)UserGroup.Operator}, '操作员')";
            ExecuteWriteCmd(cmd, transaction);
        }

        private void InsertDefaultUser(DbTransaction transaction)
        {
            const string adminName = "admin";
            AuthenticationSession session = new AuthenticationSession(adminName, UserGroup.SuperAdmin,
                DateTime.Now.ToString(TimeFormat));

            // 插入默认管理员账户
            string password = EncryptionHandler.Encrypt(session, Constants.DefaultPassword);
            string cmd =
                $"INSERT INTO TestStation_UserInfo (Name, Password, GroupName, CreationTime) VALUES ('{adminName}', '{password}', '{UserGroup.SuperAdmin}', '{session.CreationTime}')";
            ExecuteWriteCmd(cmd, transaction);
        }

        private void InsertAuthorityInfo(DbTransaction transaction)
        {
            List<string> authorities = new List<string>(10)
            {
                AuthorityDefinition.CheckUserLoginInfo,
                AuthorityDefinition.CheckAdminLoginInfo,
                AuthorityDefinition.DeleteUser,
                AuthorityDefinition.DeleteSelf,
                AuthorityDefinition.ModifyUserGroup,
                AuthorityDefinition.ResetPassword,
                AuthorityDefinition.AddUser,
                AuthorityDefinition.EditSequence,
                AuthorityDefinition.DebugSequence,
                AuthorityDefinition.CreateSequence,
                AuthorityDefinition.SaveSequence,
                AuthorityDefinition.RunSequence
            };

            string cmdFormat = "INSERT INTO TestStation_Authorities (Authority) VALUES('{0}')";
            string cmd;
            foreach (string authority in authorities)
            {
                cmd = string.Format(cmdFormat, authority);
                ExecuteWriteCmd(cmd, transaction);
            }

            Dictionary<string, List<string>> certifications = new Dictionary<string, List<string>>(5);
            certifications.Add(UserGroup.SuperAdmin.ToString(), new List<string>()
            {
                AuthorityDefinition.CheckUserLoginInfo,
                AuthorityDefinition.CheckAdminLoginInfo,
                AuthorityDefinition.DeleteUser,
                AuthorityDefinition.ModifyUserGroup,
                AuthorityDefinition.ResetPassword,
                AuthorityDefinition.AddUser,
                AuthorityDefinition.EditSequence,
                AuthorityDefinition.DebugSequence,
                AuthorityDefinition.CreateSequence,
                AuthorityDefinition.SaveSequence,
                AuthorityDefinition.RunSequence
            });

            certifications.Add(UserGroup.Administrator.ToString(), new List<string>()
            {
                AuthorityDefinition.CheckUserLoginInfo,
                AuthorityDefinition.ResetPassword,
                AuthorityDefinition.AddUser,
                AuthorityDefinition.EditSequence,
                AuthorityDefinition.DebugSequence,
                AuthorityDefinition.CreateSequence,
                AuthorityDefinition.SaveSequence,
                AuthorityDefinition.RunSequence
            });

            certifications.Add(UserGroup.Configurator.ToString(), new List<string>()
            {
                AuthorityDefinition.EditSequence,
                AuthorityDefinition.DebugSequence,
                AuthorityDefinition.CreateSequence,
                AuthorityDefinition.SaveSequence,
                AuthorityDefinition.RunSequence
            });

            certifications.Add(UserGroup.Adjustor.ToString(), new List<string>()
            {
                AuthorityDefinition.DebugSequence,
                AuthorityDefinition.RunSequence
            });

            certifications.Add(UserGroup.Operator.ToString(), new List<string>()
            {
                AuthorityDefinition.RunSequence
            });

            cmdFormat =
                "INSERT INTO TestStation_AuthorityAvailable (Authority, GroupName, Available) VALUES('{0}', '{1}', {2})";
            foreach (string key in certifications.Keys)
            {
                foreach (string authority in certifications[key])
                {
                    cmd = string.Format(cmdFormat, authority, key, true);
                    ExecuteWriteCmd(cmd, transaction);
                }
            }
        }

        public IList<string> GetAuthorities(UserGroup userGroup)
        {
            string cmd = $"SELECT Authority FROM TestStation_AuthorityAvailable WHERE (GroupName='{userGroup}')";
            List<string> authorities = new List<string>(10);
            using (DbDataReader reader = ExecuteReadCommand(cmd))
            {
                while (reader.Read())
                {
                    authorities.Add(reader.GetString(0));
                }
            }
            return authorities;
        }

        public UserInfo GetUserInfo(string userName)
        {
            string cmd = $"SELECT Name,GroupName,CreationTime FROM TestStation_UserInfo WHERE (Name='{userName}')";
            List<string> authorities = new List<string>(10);
            using (DbDataReader reader = ExecuteReadCommand(cmd))
            {
                if (!reader.Read())
                {
                    throw new AuthenticationException("用户不存在。");
                }
                UserInfo userInfo = new UserInfo()
                {
                    Name = reader.GetString(0),
                    UserGroup = (UserGroup) Enum.Parse(typeof(UserGroup), reader.GetString(1)),
                    CreationTime = reader.GetString(2)
                };
                return userInfo;
            }
        }

        public UserGroupInfo GetUserGroupInfo(string groupName)
        {
            string cmd =
                $"SELECT GroupName, Priority, Description FROM TestStation_UserGroup WHERE GroupName='{groupName}'";
            using (DbDataReader dataReader = ExecuteReadCommand(cmd))
            {
                if (!dataReader.Read())
                {
                    throw new AuthenticationException("无法找到用户组信息");
                }
                UserGroupInfo userGroupInfo = new UserGroupInfo()
                {
                    GroupName = dataReader.GetString(0),
                    Priority = dataReader.GetInt32(1),
                    Description = dataReader.GetString(2)
                };
                return userGroupInfo;
            }
        }

        public IList<UserInfo> GetUserInfos(AuthenticationSession session)
        {
            int currentGroupId = (int)session.UserGroup;
            int[] groupIds = (int[])Enum.GetValues(typeof(UserGroup));
            IEnumerable<UserGroup> userGroups = from groupId in groupIds
                where groupId >= currentGroupId
                select (UserGroup) groupId;
            StringBuilder groupFilter = new StringBuilder(100);
            groupFilter.Append("(");
            foreach (UserGroup userGroup in userGroups)
            {
                groupFilter.Append("'").Append(userGroup).Append("'").Append(",");
            }
            groupFilter.Remove(groupFilter.Length - 1, 1);
            groupFilter.Append(")");
            string cmd = $"SELECT Name,GroupName,CreationTime FROM TestStation_UserInfo WHERE GroupName IN {groupFilter}";
            List<UserInfo> userInfos = new List<UserInfo>(50);
            using (DbDataReader dataReader = ExecuteReadCommand(cmd))
            {
                while (dataReader.Read())
                {
                    userInfos.Add(new UserInfo()
                    {
                        Name = dataReader.GetString(0),
                        UserGroup = (UserGroup) Enum.Parse(typeof(UserGroup), dataReader.GetString(1)),
                        CreationTime = dataReader.GetString(2)
                    });
                }
            }
            return userInfos;
        }

        public void AddUser(AuthenticationSession session, string userName, UserGroup userGroup, string password = Constants.DefaultPassword)
        {
            // 如果Session为null则是通过扫码注册，可以无需判断权限是否存在
            if (null != session && !session.Authorities.Contains(AuthorityDefinition.AddUser))
            {
                throw new AuthenticationException("当前账户未拥有创建用户的权限。");
            }
            string cmd = $"SELECT COUNT(Name) from TestStation_UserInfo WHERE Name='{userName}'";
            using (DbDataReader dataReader = ExecuteReadCommand(cmd))
            {
                if (dataReader.Read() && dataReader.GetInt32(0) > 0)
                {
                    throw new AuthenticationException("已经存在同名的用户。");   
                }
            }
            UserInfo userInfo = new UserInfo()
            {
                Name = userName,
                UserGroup = userGroup,
                CreationTime = DateTime.Now.ToString(TimeFormat)
            };
            string encryptPassword = EncryptionHandler.Encrypt(userInfo, password);
            cmd =
                $"INSERT INTO TestStation_UserInfo (Name, Password, GroupName, CreationTime) VALUES('{userName}', '{encryptPassword}', '{userGroup}', '{userInfo.CreationTime}')";
            ExecuteWriteCmd(cmd, null);
        }

        public void DeleteUser(AuthenticationSession session, UserInfo userInfo)
        {
            if (!session.Authorities.Contains(AuthorityDefinition.DeleteUser))
            {
                throw new AuthenticationException("当前会话未拥有删除用户的权限。");
            }
            using (DbTransaction transaction = _connection.BeginTransaction(IsolationLevel.Serializable))
            {
                string cmd = $"DELETE FROM TestStation_UserInfo WHERE Name='{userInfo.Name}'";
                ExecuteWriteCmd(cmd, transaction);
                cmd = $"DELETE FROM Testflow_LoginInfo WHERE UserName='{userInfo.Name}'";
                ExecuteWriteCmd(cmd, transaction);
                transaction.Commit();
            }
        }

        public IList<LoginInfo> GetLoginInfos(string userName)
        {
            string cmd =
                $"SELECT LoginTime,LogoutTime FROM Testflow_LoginInfo WHERE UserName='{userName}' ORDER BY ItemIndex desc";
            using (DbDataReader dataReader = ExecuteReadCommand(cmd))
            {
                List<LoginInfo> loginInfos = new List<LoginInfo>(100);
                while (dataReader.Read())
                {
                    LoginInfo loginInfo = new LoginInfo()
                    {
                        UserName = userName,
                        LoginTime = dataReader.GetString(0),
                        LogoutTime = dataReader.GetString(1)
                    };
                    loginInfos.Add(loginInfo);
                }
                return loginInfos;
            }
        }

        public AuthenticationSession ScanLoginIn(string userName)
        {
            UserGroup userGroup = GetUserGroup(userName);
            AuthenticationSession session = null;
            string cmd = $"SELECT * FROM TestStation_UserInfo WHERE (Name='{userName}')";
            using (DbDataReader reader = ExecuteReadCommand(cmd))
            {
                if (reader.Read())
                {
                    session = new AuthenticationSession(reader.GetString(1),
                    (UserGroup)Enum.Parse(typeof(UserGroup), reader.GetString(3)),
                    reader.GetString(4));
                    session.Authorities = GetAuthorities(session.UserGroup);
                    session.LogInTime = DateTime.Now;
                    session.LogOutTime = DateTime.MaxValue;

                    cmd =
                        $"INSERT INTO Testflow_LoginInfo (UserName, LoginTime, LogoutTime) VALUES('{session.UserName}', '{session.LogInTime.ToString(TimeFormat)}', '{string.Empty}')";
                    ExecuteWriteCmd(cmd, null);
                    return session;
                }
                
            }
            // 用户不存在，则先创建用户，然后登入
            AuthenticationManage.DataAccessor.AddUser(null, userName, userGroup, Constants.DefaultPassword);
            return LoginIn(userName, Constants.DefaultPassword);
        }

        public AuthenticationSession LoginIn(string userName, string password)
        {
            AuthenticationSession session = null;
            string cmd = $"SELECT * FROM TestStation_UserInfo WHERE (Name='{userName}')";
            using (DbDataReader reader = ExecuteReadCommand(cmd))
            {
                if (!reader.Read())
                {
                    throw new AuthenticationException("用户不存在。");
                }
                session = new AuthenticationSession(reader.GetString(1),
                    (UserGroup) Enum.Parse(typeof (UserGroup), reader.GetString(3)),
                    reader.GetString(4));
                string passwordData = reader.GetString(2);
                string encryptedPasswd = EncryptionHandler.Encrypt(session, password);
                if (!passwordData.Equals(encryptedPasswd))
                {
                    throw new AuthenticationException("密码错误。");
                }
                session.Authorities = GetAuthorities(session.UserGroup);
                session.LogInTime = DateTime.Now;
                session.LogOutTime = DateTime.MaxValue;
                
            }
            cmd =
                $"INSERT INTO Testflow_LoginInfo (UserName, LoginTime, LogoutTime) VALUES('{session.UserName}', '{session.LogInTime.ToString(TimeFormat)}', '{string.Empty}')";
            ExecuteWriteCmd(cmd, null);
            return session;
        }

        public IList<string> GetLoginList()
        {
            string cmd = "SELECT UserName FROM Testflow_LoginNames ORDER BY ItemIndex desc";
            using (DbDataReader dataReader = ExecuteReadCommand(cmd))
            {
                List<string> loginList = new List<string>(20);
                while (dataReader.Read())
                {
                    loginList.Add(dataReader.GetString(0));
                }
                return loginList;
            }
        }

        public void AddToLoginList(string name)
        {
            string cmd = $"DELETE FROM Testflow_LoginNames WHERE UserName='{name}'";
            ExecuteWriteCmd(cmd, null);
            cmd = $"INSERT INTO Testflow_LoginNames (UserName) VALUES ('{name}')";
            ExecuteWriteCmd(cmd, null);
        }

        public void ClearLoginList()
        {
            string cmd = "DELETE FROM Testflow_LoginNames WHERE 1=1";
            ExecuteWriteCmd(cmd, null);
        }

        public void ClearLoginInfoList(string userName)
        {
            string cmd = $"DELETE FROM Testflow_LoginInfo WHERE UserName='{userName}'";
            ExecuteWriteCmd(cmd, null);
        }

        public void LogOut(AuthenticationSession session)
        {
            session.LogOutTime = DateTime.Now;
            string cmd =
                $"UPDATE Testflow_LoginInfo SET LogoutTime = '{session.LogOutTime.ToString(TimeFormat)}' WHERE (UserName='{session.UserName}' AND LoginTime='{session.LogInTime.ToString(TimeFormat)}')";
            ExecuteWriteCmd(cmd, null);
        }

        public void FixPassword(AuthenticationSession session, string oldPassword, string newPassword)
        {
            string cmd = $"SELECT Password, CreationTime FROM TestStation_UserInfo WHERE Name='{session.UserName}'";
            using (DbDataReader reader = ExecuteReadCommand(cmd))
            {
                if (!reader.Read())
                {
                    return;
                }
                string password = reader.GetString(0);
                string creationTime = reader.GetString(1);
                string encryptOldPassword = EncryptionHandler.Encrypt(session, oldPassword);
                if (!encryptOldPassword.Equals(password))
                {
                    throw new AuthenticationException("旧密码输入错误。");
                }
                string encryptedPassword = EncryptionHandler.Encrypt(session, newPassword);
                cmd =
                    $"UPDATE TestStation_UserInfo SET Password = '{encryptedPassword}' WHERE (Name='{session.UserName}')";
                ExecuteWriteCmd(cmd, null);
            }
        }

        public void ModifyUserGroup(AuthenticationSession session, string userName, UserGroup userGroup)
        {
            if (session.UserGroup != UserGroup.SuperAdmin)
            {
                throw new AuthenticationException("非超级管理员不能修改用户组。");
            }
            string cmd = $"UPDATE TestStation_UserInfo SET GroupName = '{userGroup}' WHERE (Name='{userName}')";
            ExecuteWriteCmd(cmd, null);
        }

        public void ResetUserPassword(AuthenticationSession session, string userName)
        {
            if (session.UserGroup != UserGroup.SuperAdmin)
            {
                throw new AuthenticationException("非超级管理员不能修改用户组。");
            }
            UserInfo userInfo = GetUserInfo(userName);
            string encryptedPasswd = EncryptionHandler.Encrypt(userInfo, Constants.DefaultPassword);
            string cmd = $"UPDATE TestStation_UserInfo SET Password = '{encryptedPasswd}' WHERE (Name='{userName}')";
            ExecuteWriteCmd(cmd, null);
        }

        private void ExecuteWriteCmd(string cmd, DbTransaction transaction)
        {
            DbCommand dbCommand = _connection.CreateCommand();
            dbCommand.CommandText = cmd;
            dbCommand.Transaction = transaction;
            dbCommand.ExecuteNonQuery();
        }

        private DbDataReader ExecuteReadCommand(string command, DbTransaction transaction = null)
        {
            DbCommand dbCommand = _connection.CreateCommand();
            dbCommand.CommandText = command;
            dbCommand.CommandTimeout = 1000;
            if (null != transaction)
            {
                dbCommand.Transaction = transaction;
            }
            return dbCommand.ExecuteReader();
        }

        private UserGroup GetUserGroup(string userName)
        {
            foreach (UserGroup userGroup in _groupToNameRegex.Keys)
            {
                if (Regex.IsMatch(userName, _groupToNameRegex[userGroup]))
                {
                    return userGroup;
                }
            }
            throw new AuthenticationException($"无效的用户名:{userName}");
        }


    }
}
-- 用户组表
CREATE TABLE TestStation_UserGroup(
	ItemIndex INTEGER PRIMARY KEY AUTOINCREMENT,
	GroupName TEXT NOT NULL,
	Priority INTEGER NOT NULL,
	Description TEXT
);

-- 会话结果表
CREATE TABLE TestStation_UserInfo(
    UserId INTEGER PRIMARY KEY AUTOINCREMENT,
	Name TEXT NOT NULL,
	Password TEXT NOT NULL,
	GroupName TEXT NOT NULL,
	CreationTime TEXT NOT NULL
);

-- 级别权限表格
CREATE TABLE TestStation_AuthorityAvailable(
	ItemIndex INTEGER PRIMARY KEY AUTOINCREMENT,
	Authority TEXT NOT NULL,
	GroupName TEXT NOT NULL,
	Available BOOLEAN NOT NULL
);

CREATE TABLE TestStation_Authorities (
	ItemIndex INTEGER PRIMARY KEY AUTOINCREMENT,
	Authority TEXT NOT NULL
);

-- 登录记录
CREATE TABLE Testflow_LoginInfo(
	ItemIndex INTEGER PRIMARY KEY AUTOINCREMENT,
	UserName TEXT NOT NULL,
	LoginTime TEXT NOT NULL,
	LogoutTime TEXT NOT NULL
);

-- 登录名称
CREATE TABLE Testflow_LoginNames(
	ItemIndex INTEGER PRIMARY KEY AUTOINCREMENT,
	UserName TEXT NOT NULL
); 

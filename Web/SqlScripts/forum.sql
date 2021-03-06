IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[tb_ExceptionLogs]') AND type in (N'U'))
DROP TABLE [tb_ExceptionLogs]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[tb_Sections]') AND type in (N'U'))
DROP TABLE [tb_Sections]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[tb_Threads]') AND type in (N'U'))
DROP TABLE [tb_Threads]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[sp_GetThreads]') AND type in (N'P', N'PC'))
DROP PROCEDURE [sp_GetThreads]
GO
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[vw_RolePermissions]'))
DROP VIEW [vw_RolePermissions]
GO
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[vw_UserPermissions]'))
DROP VIEW [vw_UserPermissions]
GO
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[vw_PostWithUser]'))
DROP VIEW [vw_PostWithUser]
GO
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[vw_RoleUsers]'))
DROP VIEW [vw_RoleUsers]
GO
IF  EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[vw_SectionRoleUsers]'))
DROP VIEW [vw_SectionRoleUsers]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[tb_Members]') AND type in (N'U'))
DROP TABLE [tb_Members]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[tb_Groups]') AND type in (N'U'))
DROP TABLE [tb_Groups]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[sp_GetEntities]') AND type in (N'P', N'PC'))
DROP PROCEDURE [sp_GetEntities]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[tb_Permissions]') AND type in (N'U'))
DROP TABLE [tb_Permissions]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[tb_Roles]') AND type in (N'U'))
DROP TABLE [tb_Roles]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[tb_UserRoles]') AND type in (N'U'))
DROP TABLE [tb_UserRoles]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[tb_Posts]') AND type in (N'U'))
DROP TABLE [tb_Posts]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[tb_Users]') AND type in (N'U'))
DROP TABLE [tb_Users]
GO
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[tb_SectionRoleUsers]') AND type in (N'U'))
DROP TABLE [tb_SectionRoleUsers]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[tb_Permissions]') AND type in (N'U'))
BEGIN
CREATE TABLE [tb_Permissions](
	[EntityId] [int] IDENTITY(1,1) NOT NULL,
	[RoleId] [int] NOT NULL,
	[AllowMask] [bigint] NOT NULL,
	[DenyMask] [bigint] NOT NULL,
 CONSTRAINT [PK_tb_RolePermissions] PRIMARY KEY CLUSTERED 
(
	[EntityId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[tb_Posts]') AND type in (N'U'))
BEGIN
CREATE TABLE [tb_Posts](
	[EntityId] [int] IDENTITY(1,1) NOT NULL,
	[Body] [ntext] NULL,
	[GroupId] [int] NOT NULL,
	[SectionId] [int] NOT NULL,
	[ThreadId] [int] NOT NULL,
	[AuthorId] [int] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
 CONSTRAINT [PK_tb_Posts] PRIMARY KEY CLUSTERED 
(
	[EntityId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[tb_Posts]') AND name = N'IX_tb_Posts_ThreadIdAndCreateDate')
CREATE NONCLUSTERED INDEX [IX_tb_Posts_ThreadIdAndCreateDate] ON [tb_Posts] 
(
	[ThreadId] ASC,
	[CreateDate] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[tb_Roles]') AND type in (N'U'))
BEGIN
CREATE TABLE [tb_Roles](
	[EntityId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](256) NOT NULL,
	[Description] [nvarchar](256) NULL,
	[RoleType] [bigint] NOT NULL CONSTRAINT [DF_tb_Roles_RoleType]  DEFAULT ((0)),
 CONSTRAINT [PK_tb_Roles] PRIMARY KEY CLUSTERED 
(
	[EntityId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[tb_SectionRoleUsers]') AND type in (N'U'))
BEGIN
CREATE TABLE [tb_SectionRoleUsers](
	[EntityId] [int] IDENTITY(1,1) NOT NULL,
	[SectionId] [int] NOT NULL,
	[RoleId] [int] NOT NULL,
	[UserId] [int] NOT NULL,
 CONSTRAINT [PK_tb_SectionAdminUsers] PRIMARY KEY CLUSTERED 
(
	[EntityId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[tb_SectionRoleUsers]') AND name = N'IX_tb_SectionAdminUsers')
CREATE NONCLUSTERED INDEX [IX_tb_SectionAdminUsers] ON [tb_SectionRoleUsers] 
(
	[SectionId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[tb_SectionRoleUsers]') AND name = N'IX_tb_SectionAdminUsers_1')
CREATE NONCLUSTERED INDEX [IX_tb_SectionAdminUsers_1] ON [tb_SectionRoleUsers] 
(
	[UserId] ASC,
	[RoleId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[tb_ExceptionLogs]') AND type in (N'U'))
BEGIN
CREATE TABLE [tb_ExceptionLogs](
	[EntityId] [int] IDENTITY(1,1) NOT NULL,
	[Message] [varchar](512) NOT NULL,
	[ExceptionDetails] [varchar](2000) NOT NULL,
	[IPAddress] [varchar](15) NULL,
	[UserAgent] [varchar](256) NULL,
	[HttpReferrer] [varchar](256) NULL,
	[HttpVerb] [varchar](50) NULL,
	[PathAndQuery] [varchar](512) NULL,
	[DateCreated] [datetime] NOT NULL CONSTRAINT [DF_tb_ExceptionLogs_DateCreated]  DEFAULT (getdate()),
	[DateLastOccurred] [datetime] NOT NULL,
	[Frequency] [int] NOT NULL CONSTRAINT [DF_tb_ExceptionLogs_Frequency]  DEFAULT ((0)),
	[UserName] [varchar](64) NULL,
 CONSTRAINT [PK_tb_ExceptionLogs] PRIMARY KEY CLUSTERED 
(
	[EntityId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[tb_Sections]') AND type in (N'U'))
BEGIN
CREATE TABLE [tb_Sections](
	[EntityId] [int] IDENTITY(1,1) NOT NULL,
	[Subject] [varchar](128) NOT NULL,
	[Enabled] [int] NOT NULL,
	[GroupId] [int] NOT NULL,
	[TotalThreads] [int] NOT NULL,
 CONSTRAINT [PK_tb_Sections] PRIMARY KEY CLUSTERED 
(
	[EntityId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[tb_Sections]') AND name = N'IX_tb_Sections_GroupIdAndEnabled')
CREATE NONCLUSTERED INDEX [IX_tb_Sections_GroupIdAndEnabled] ON [tb_Sections] 
(
	[GroupId] ASC,
	[Enabled] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[tb_Sections]') AND name = N'IX_tb_Sections_Subject')
CREATE NONCLUSTERED INDEX [IX_tb_Sections_Subject] ON [tb_Sections] 
(
	[Subject] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[tb_Threads]') AND type in (N'U'))
BEGIN
CREATE TABLE [tb_Threads](
	[EntityId] [int] IDENTITY(1,1) NOT NULL,
	[Subject] [varchar](256) NOT NULL,
	[Body] [ntext] NULL,
	[CreateDate] [datetime] NOT NULL,
	[UpdateDate] [datetime] NOT NULL,
	[StickDate] [datetime] NOT NULL,
	[AuthorId] [int] NOT NULL,
	[Author] [varchar](128) NOT NULL,
	[GroupId] [int] NOT NULL,
	[SectionId] [int] NOT NULL,
	[TotalPosts] [int] NOT NULL,
	[TotalViews] [int] NOT NULL,
	[ThreadStatus] [int] NOT NULL,
	[MostRecentReplierId] [int] NULL,
	[MostRecentReplierName] [varchar](128) NULL,
	[ThreadMarks] [int] NOT NULL,
	[ThreadReleaseStatus] [int] NOT NULL,
 CONSTRAINT [PK_tb_Threads] PRIMARY KEY CLUSTERED 
(
	[EntityId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[tb_Threads]') AND name = N'IX_tb_Threads_AuthorId')
CREATE NONCLUSTERED INDEX [IX_tb_Threads_AuthorId] ON [tb_Threads] 
(
	[AuthorId] DESC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[tb_Threads]') AND name = N'IX_tb_Threads_SectionThreadIndex1')
CREATE NONCLUSTERED INDEX [IX_tb_Threads_SectionThreadIndex1] ON [tb_Threads] 
(
	[SectionId] ASC,
	[StickDate] DESC,
	[UpdateDate] DESC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[tb_Threads]') AND name = N'IX_tb_Threads_SectionThreadIndex2')
CREATE NONCLUSTERED INDEX [IX_tb_Threads_SectionThreadIndex2] ON [tb_Threads] 
(
	[SectionId] ASC,
	[StickDate] DESC,
	[TotalViews] DESC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[tb_Threads]') AND name = N'IX_tb_Threads_SectionThreadIndex3')
CREATE NONCLUSTERED INDEX [IX_tb_Threads_SectionThreadIndex3] ON [tb_Threads] 
(
	[SectionId] ASC,
	[StickDate] DESC,
	[TotalPosts] DESC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[tb_Threads]') AND name = N'IX_tb_Threads_SectionThreadStatusIndex')
CREATE NONCLUSTERED INDEX [IX_tb_Threads_SectionThreadStatusIndex] ON [tb_Threads] 
(
	[SectionId] ASC,
	[ThreadStatus] ASC,
	[ThreadReleaseStatus] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[tb_Threads]') AND name = N'IX_tb_Threads_ThreadIndex1')
CREATE NONCLUSTERED INDEX [IX_tb_Threads_ThreadIndex1] ON [tb_Threads] 
(
	[StickDate] DESC,
	[UpdateDate] DESC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[tb_Threads]') AND name = N'IX_tb_Threads_ThreadIndex2')
CREATE NONCLUSTERED INDEX [IX_tb_Threads_ThreadIndex2] ON [tb_Threads] 
(
	[StickDate] DESC,
	[TotalViews] DESC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[tb_Threads]') AND name = N'IX_tb_Threads_ThreadIndex3')
CREATE NONCLUSTERED INDEX [IX_tb_Threads_ThreadIndex3] ON [tb_Threads] 
(
	[StickDate] DESC,
	[TotalPosts] DESC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[tb_Threads]') AND name = N'IX_tb_Threads_ThreadStatusIndex')
CREATE NONCLUSTERED INDEX [IX_tb_Threads_ThreadStatusIndex] ON [tb_Threads] 
(
	[ThreadStatus] ASC,
	[ThreadReleaseStatus] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[tb_UserRoles]') AND type in (N'U'))
BEGIN
CREATE TABLE [tb_UserRoles](
	[EntityId] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [int] NOT NULL,
	[RoleId] [int] NOT NULL,
 CONSTRAINT [PK_tb_UserRoles] PRIMARY KEY CLUSTERED 
(
	[EntityId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[tb_UserRoles]') AND name = N'IX_tb_UserRoles_RoleId')
CREATE NONCLUSTERED INDEX [IX_tb_UserRoles_RoleId] ON [tb_UserRoles] 
(
	[RoleId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[tb_UserRoles]') AND name = N'IX_tb_UserRoles_UserId')
CREATE NONCLUSTERED INDEX [IX_tb_UserRoles_UserId] ON [tb_UserRoles] 
(
	[UserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[tb_Users]') AND type in (N'U'))
BEGIN
CREATE TABLE [tb_Users](
	[EntityId] [int] IDENTITY(10000,1) NOT NULL,
	[MemberId] [uniqueidentifier] NOT NULL CONSTRAINT [DF_tb_Users_MembershipID]  DEFAULT (newid()),
	[NickName] [varchar](64) NULL,
	[AvatarFileName] [varchar](128) NULL,
	[AvatarContent] [image] NULL,
	[UserStatus] [int] NULL CONSTRAINT [DF_tb_Users_UserStatus]  DEFAULT ((0)),
	[TotalMarks] [int] NULL CONSTRAINT [DF_tb_Users_TotalMarks]  DEFAULT ((0)),
	[Language] [varchar](128) NULL,
	[SiteTheme] [varchar](128) NULL,
 CONSTRAINT [PK_tb_Users] PRIMARY KEY CLUSTERED 
(
	[EntityId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[tb_Users]') AND name = N'IX_tb_Users_MemberId')
CREATE UNIQUE NONCLUSTERED INDEX [IX_tb_Users_MemberId] ON [tb_Users] 
(
	[MemberId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[sp_GetEntities]') AND type in (N'P', N'PC'))
BEGIN
EXEC sp_executesql @statement = N'


CREATE PROCEDURE [sp_GetEntities]
	@TableName NVARCHAR(128),
	@ReturnFields NVARCHAR(2000),
	@TopCount int,
    @SqlFullPopulate NVARCHAR(4000),
    @SqlPopulate NVARCHAR(4000),
	@VariableDecalrations NVARCHAR(1024),
    @ParameterString1 NVARCHAR(512),
    @ParameterString2 NVARCHAR(512),
    @ParameterString3 NVARCHAR(512),
    @ParameterString4 NVARCHAR(512),
	@DisOrderSql NVARCHAR(256),
	@TotalRecords int OUTPUT
AS

DECLARE @totalSQL NVARCHAR(4000)
DECLARE @sqlBegin NVARCHAR(1024)
DECLARE @sqlPart1 NVARCHAR(4000)
DECLARE @sqlPart2 NVARCHAR(4000)
DECLARE @sqlPart3 NVARCHAR(4000)
DECLARE @sqlEnd NVARCHAR(1024)

-- Set the begin SQL
set @sqlBegin = N''
SET NOCOUNT ON

CREATE TABLE #t
(
    IndexId INT IDENTITY (1, 1) NOT NULL,
    EntityId INT
)
CREATE TABLE #EntityIdTable
(
    EntityId INT
)

DECLARE @TotalRecords int
set @TotalRecords = 0

''

-- Set the end SQL
set @sqlEnd = N''
DROP TABLE #t
DROP TABLE #EntityIdTable

set @totalCount = @TotalRecords

SET NOCOUNT OFF

''

-- Set the SQL to get the total records count.
set @sqlPart1 = N''EXECUTE sp_executesql N''''SELECT @TotalRecords = COUNT(*) FROM ('' + @SqlFullPopulate + N'') a'''''' + '', N'''''' + @ParameterString1 + N''@TotalRecords INT OUTPUT'''', '' + @ParameterString2 + N''@TotalRecords OUTPUT''

-- Set the SQL to get the record keys of the current page.
set @sqlPart2 = N''EXECUTE sp_executesql N'''''' + N''INSERT INTO #t(EntityId) SELECT TOP '' + CAST(@TopCount AS NVARCHAR(20)) + N'' EntityId FROM ('' + @SqlPopulate + N'') t ORDER BY '' + @DisOrderSql + N'''''''' + @ParameterString3 + @ParameterString4 + N''; INSERT INTO #EntityIdTable SELECT EntityId FROM #t ORDER BY IndexId DESC''

-- Set the SQL to get the record entities of the current page.
set @sqlPart3 = N''EXECUTE sp_executesql N''''SELECT '' + @ReturnFields + N'' FROM #EntityIdTable dt INNER JOIN ['' + @TableName + N''] t ON dt.EntityId = t.EntityId''''''

-- Set the total whole SQL
set @totalSQL = @sqlBegin + @VariableDecalrations
+ N''
''
+ @sqlPart1
+ N''

''
+ @sqlPart2
+ N''

''
+ @sqlPart3
+ N''

''
+ @sqlEnd

-- Execute the total whole SQL
EXECUTE sp_executesql @totalSQL, N''@totalCount INT OUTPUT'', @TotalRecords OUTPUT


' 
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[tb_Members]') AND type in (N'U'))
BEGIN
CREATE TABLE [tb_Members](
	[EntityId] [int] IDENTITY(1,1) NOT NULL,
	[MemberId] [uniqueidentifier] NOT NULL,
	[MemberName] [varchar](256) NOT NULL,
	[Password] [nvarchar](128) NOT NULL,
	[PasswordFormat] [int] NOT NULL,
	[PasswordSalt] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_tb_Membership] PRIMARY KEY CLUSTERED 
(
	[EntityId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[tb_Members]') AND name = N'IX_tb_Members_MemberId')
CREATE NONCLUSTERED INDEX [IX_tb_Members_MemberId] ON [tb_Members] 
(
	[MemberId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[tb_Members]') AND name = N'IX_tb_Members_MemberName')
CREATE UNIQUE NONCLUSTERED INDEX [IX_tb_Members_MemberName] ON [tb_Members] 
(
	[MemberName] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[tb_Groups]') AND type in (N'U'))
BEGIN
CREATE TABLE [tb_Groups](
	[EntityId] [int] IDENTITY(1,1) NOT NULL,
	[Subject] [varchar](128) NOT NULL,
	[Enabled] [int] NOT NULL,
 CONSTRAINT [PK_tb_Groups] PRIMARY KEY CLUSTERED 
(
	[EntityId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_PADDING OFF
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[tb_Groups]') AND name = N'IX_tb_Groups_Enabled')
CREATE NONCLUSTERED INDEX [IX_tb_Groups_Enabled] ON [tb_Groups] 
(
	[Enabled] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[tb_Groups]') AND name = N'IX_tb_Groups_Subject')
CREATE NONCLUSTERED INDEX [IX_tb_Groups_Subject] ON [tb_Groups] 
(
	[Subject] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[vw_RolePermissions]'))
EXEC sp_executesql @statement = N'
CREATE VIEW [vw_RolePermissions]
AS
SELECT tb_Roles.EntityId, tb_Roles.EntityId AS RoleId, 
      tb_Roles.Name AS RoleName, tb_Roles.Description AS RoleDescription, 
      tb_Roles.RoleType, tb_Permissions.EntityId AS PermissionId, 
      tb_Permissions.AllowMask, tb_Permissions.DenyMask
FROM tb_Permissions INNER JOIN
      tb_Roles ON tb_Permissions.RoleId = tb_Roles.EntityId

'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[vw_UserPermissions]'))
EXEC sp_executesql @statement = N'
CREATE VIEW [vw_UserPermissions]
AS
SELECT tb_UserRoles.UserId AS EntityId, tb_UserRoles.UserId, 
      tb_Roles.EntityId AS RoleId, tb_Permissions.EntityId AS PermissionId, 
      tb_Roles.Name AS RoleName, tb_Roles.RoleType, 
      tb_Roles.Description AS RoleDescription, tb_Permissions.AllowMask, 
      tb_Permissions.DenyMask
FROM tb_UserRoles INNER JOIN
      tb_Roles ON tb_UserRoles.RoleId = tb_Roles.EntityId INNER JOIN
      tb_Permissions ON tb_Roles.EntityId = tb_Permissions.RoleId

'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[vw_PostWithUser]'))
EXEC sp_executesql @statement = N'CREATE VIEW [vw_PostWithUser]
AS
SELECT     tb_Posts.EntityId, tb_Posts.Body, tb_Posts.GroupId, tb_Posts.SectionId, tb_Posts.ThreadId, tb_Posts.AuthorId, tb_Posts.CreateDate, 
                      tb_Users.MemberId, tb_Users.NickName, tb_Users.AvatarFileName, tb_Users.TotalMarks
FROM         tb_Posts INNER JOIN
                      tb_Users ON tb_Posts.AuthorId = tb_Users.EntityId
'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[vw_RoleUsers]'))
EXEC sp_executesql @statement = N'CREATE VIEW [vw_RoleUsers]
AS
SELECT     tb_Users.EntityId, tb_Users.EntityId AS UserId, tb_Roles.EntityId AS RoleId, tb_Roles.Name AS RoleName, 
                      tb_Roles.Description AS RoleDescription, tb_Roles.RoleType, tb_Users.MemberId, tb_Users.NickName, tb_Users.AvatarFileName, 
                      tb_Users.AvatarContent, tb_Users.UserStatus, tb_Users.TotalMarks, tb_Users.Language, tb_Users.SiteTheme
FROM         tb_Roles INNER JOIN
                      tb_UserRoles ON tb_Roles.EntityId = tb_UserRoles.RoleId INNER JOIN
                      tb_Users ON tb_UserRoles.UserId = tb_Users.EntityId
'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[vw_SectionRoleUsers]'))
EXEC sp_executesql @statement = N'CREATE VIEW [vw_SectionRoleUsers]
AS
SELECT     tb_SectionRoleUsers.EntityId, tb_SectionRoleUsers.SectionId, tb_SectionRoleUsers.RoleId, tb_SectionRoleUsers.UserId, tb_Users.MemberId, 
                      tb_Users.NickName
FROM         tb_SectionRoleUsers INNER JOIN
                      tb_Users ON tb_SectionRoleUsers.UserId = tb_Users.EntityId
'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[sp_GetThreads]') AND type in (N'P', N'PC'))
BEGIN
EXEC sp_executesql @statement = N'


CREATE PROCEDURE [sp_GetThreads]
    @SectionId int,
    @AuthorId int,
    @ReplierId int,
    @ThreadStatus int,
    @ThreadReleaseStatus int,
    @OrderField int,
    @PageIndex int,
    @PageSize int,
    @TotalRecords int output
AS

DECLARE @tableName NVARCHAR(128)
DECLARE @topCount int
DECLARE @returnFields NVARCHAR(256)
DECLARE @selectTopFields NVARCHAR(128)
DECLARE @dynamicConditions NVARCHAR(256)
DECLARE @selectCountSql NVARCHAR(1024) 
DECLARE @selectSql NVARCHAR(1024)
DECLARE @orderFields NVARCHAR(128)
DECLARE @disOrderFields NVARCHAR(128)
DECLARE @variableDeclarations NVARCHAR(2000)
DECLARE @parameterString1 NVARCHAR(2000)
DECLARE @parameterString2 NVARCHAR(2000)
DECLARE @parameterString3 NVARCHAR(2000)
DECLARE @parameterString4 NVARCHAR(2000)

DECLARE @orderFieldName NVARCHAR(32)

-- set @tableName.
set @tableName = N''tb_Threads''

-- set @returnFields.
set @returnFields = N''
t.EntityId,
t.Subject,
t.CreateDate,
t.UpdateDate,
t.StickDate,
t.AuthorId,
t.Author,
t.GroupId,
t.SectionId,
t.TotalPosts,
t.TotalViews,
t.ThreadStatus,
t.MostRecentReplierId,
t.MostRecentreplierName,
t.ThreadMarks,
t.ThreadReleaseStatus
''

-- init the local variables.
set @dynamicConditions = N''''
set @variableDeclarations = N''''
set @parameterString1 = N''''
set @parameterString2 = N''''
set @parameterString3 = N''''
set @parameterString4 = N''''

-- set @topCount.
set @topCount = @pageIndex * @pageSize

-- set @orderFieldName.
if @OrderField = 1
	set @orderFieldName = N''t.TotalViews''
else if @OrderField = 2
	set @orderFieldName = N''t.TotalPosts''
else
	set @orderFieldName = N''t.UpdateDate''

-- set @selectTopFields. 
-- Notes: The select top fields must include the entityId and the order fields.
set @selectTopFields = N''t.EntityId,'' + N''t.StickDate,'' + @orderFieldName

-- set @orderFields and @disOrderFields.
set @orderFields = N''t.StickDate desc,'' + @orderFieldName + N'' desc''
set @disOrderFields = N''t.StickDate asc,'' + @orderFieldName + N'' asc''

-- set the other variables according with the business logic.
if not @AuthorId is null and @AuthorId > 0
begin
	set @variableDeclarations = @variableDeclarations + N''DECLARE @AuthorId int; set @AuthorId = '' + CAST(@AuthorId AS NVARCHAR(32)) + N'';''
	set @dynamicConditions = @dynamicConditions + N'' AND AuthorId = @AuthorId''
	set @parameterString1 = @parameterString1 + N''@AuthorId int, ''
	set @parameterString2 = @parameterString2 + N''@AuthorId, ''
	set @parameterString3 = @parameterString3 + N'',@AuthorId int''
	set @parameterString4 = @parameterString4 + N'',@AuthorId''
end
if not @ThreadStatus is null and @ThreadStatus > 0
begin
	set @variableDeclarations = @variableDeclarations + N''DECLARE @ThreadStatus int; set @ThreadStatus = '' + CAST(@ThreadStatus AS NVARCHAR(32)) + N'';''
	set @dynamicConditions = @dynamicConditions + N'' AND ThreadStatus = @ThreadStatus''
	set @parameterString1 = @parameterString1 + N''@ThreadStatus int, ''
	set @parameterString2 = @parameterString2 + N''@ThreadStatus, ''
	set @parameterString3 = @parameterString3 + N'',@ThreadStatus int''
	set @parameterString4 = @parameterString4 + N'',@ThreadStatus''
end
if not @ThreadReleaseStatus is null and @ThreadReleaseStatus > 0
begin
	set @variableDeclarations = @variableDeclarations + N''DECLARE @ThreadReleaseStatus int; set @ThreadReleaseStatus = '' + CAST(@ThreadReleaseStatus AS NVARCHAR(32)) + N'';''
	set @dynamicConditions = @dynamicConditions + N'' AND ThreadReleaseStatus = @ThreadReleaseStatus''
	set @parameterString1 = @parameterString1 + N''@ThreadReleaseStatus int, ''
	set @parameterString2 = @parameterString2 + N''@ThreadReleaseStatus, ''
	set @parameterString3 = @parameterString3 + N'',@ThreadReleaseStatus int''
	set @parameterString4 = @parameterString4 + N'',@ThreadReleaseStatus''
end

-- set @selectSql and @selectCountSql.
if @SectionId is null or @SectionId <= 0
begin
	if @ReplierId is null or @ReplierId <= 0
	begin
		set @selectSql = N''SELECT TOP '' + CAST(@topCount AS NVARCHAR(32)) + '' '' + @selectTopFields + N'' FROM tb_Threads t INNER JOIN tb_Sections s ON t.SectionId = s.EntityId INNER JOIN tb_Groups g ON t.GroupId = g.EntityId WHERE s.Enabled = 1 AND g.Enabled = 1'' + @dynamicConditions + '' ORDER BY '' + @orderFields
		set @selectCountSql = N''SELECT t.EntityId FROM tb_Threads t INNER JOIN tb_Sections s ON t.SectionId = s.EntityId INNER JOIN tb_Groups g ON t.GroupId = g.EntityId WHERE s.Enabled = 1 AND g.Enabled = 1'' + @dynamicConditions
	end
	else
	begin
		set @selectSql = N''SELECT TOP '' + CAST(@topCount AS NVARCHAR(32)) + '' '' + @selectTopFields + N'' FROM tb_Threads t INNER JOIN tb_Sections s ON t.SectionId = s.EntityId INNER JOIN tb_Groups g ON t.GroupId = g.EntityId WHERE s.Enabled = 1 AND g.Enabled = 1 AND t.EntityId IN (SELECT p.ThreadId FROM tb_Posts p WHERE p.AuthorId = @ReplierId)'' + @dynamicConditions + '' ORDER BY '' + @orderFields
		set @selectCountSql = N''SELECT t.EntityId FROM tb_Threads t INNER JOIN tb_Sections s ON t.SectionId = s.EntityId INNER JOIN tb_Groups g ON t.GroupId = g.EntityId WHERE s.Enabled = 1 AND g.Enabled = 1 AND t.EntityId IN (SELECT p.ThreadId FROM tb_Posts p WHERE p.AuthorId = @ReplierId)'' + @dynamicConditions
		set @variableDeclarations = @variableDeclarations + N''DECLARE @ReplierId int; set @ReplierId = '' + CAST(@ReplierId AS NVARCHAR(32)) + N'';''
		set @parameterString1 = @parameterString1 + N''@ReplierId int, ''
		set @parameterString2 = @parameterString2 + N''@ReplierId, ''
		set @parameterString3 = @parameterString3 + N'',@ReplierId int''
		set @parameterString4 = @parameterString4 + N'',@ReplierId''
	end
end
else
begin
	if @ReplierId is null or @ReplierId <= 0
	begin
		set @selectSql = N''SELECT TOP '' + CAST(@topCount AS NVARCHAR(32)) + '' '' + @selectTopFields + N'' FROM tb_Threads t WHERE SectionId = @SectionId'' + @dynamicConditions + '' ORDER BY '' + @orderFields
		set @selectCountSql = N''SELECT t.EntityId FROM tb_Threads t WHERE SectionId = @SectionId'' + @dynamicConditions
		set @variableDeclarations = @variableDeclarations + N''DECLARE @SectionId int; set @SectionId = '' + CAST(@SectionId AS NVARCHAR(32)) + N'';''		
		set @parameterString1 = @parameterString1 + N''@SectionId int, ''
		set @parameterString2 = @parameterString2 + N''@SectionId, ''
		set @parameterString3 = @parameterString3 + N'',@SectionId int''
		set @parameterString4 = @parameterString4 + N'',@SectionId''
	end
	else
	begin
		set @selectSql = N''SELECT TOP '' + CAST(@topCount AS NVARCHAR(32)) + '' '' + @selectTopFields + N'' FROM tb_Threads t WHERE SectionId = @SectionId and t.EntityId IN (SELECT p.ThreadId FROM tb_Posts p WHERE p.AuthorId = @ReplierId)'' + @dynamicConditions + '' ORDER BY '' + @orderFields
		set @selectCountSql = N''SELECT t.EntityId FROM tb_Threads t WHERE SectionId = @SectionId and t.EntityId IN (SELECT p.ThreadId FROM tb_Posts p WHERE p.AuthorId = @ReplierId)'' + @dynamicConditions
		set @variableDeclarations = @variableDeclarations + N''DECLARE @SectionId int; set @SectionId = '' + CAST(@SectionId AS NVARCHAR(32)) + N'';''
		set @parameterString1 = @parameterString1 + N''@SectionId int, ''
		set @parameterString2 = @parameterString2 + N''@SectionId, ''
		set @parameterString3 = @parameterString3 + N'',@SectionId int''
		set @parameterString4 = @parameterString4 + N'',@SectionId''
		set @variableDeclarations = @variableDeclarations + N''DECLARE @ReplierId int; set @ReplierId = '' + CAST(@ReplierId AS NVARCHAR(32)) + N'';''
		set @parameterString1 = @parameterString1 + N''@ReplierId int, ''
		set @parameterString2 = @parameterString2 + N''@ReplierId, ''
		set @parameterString3 = @parameterString3 + N'',@ReplierId int''
		set @parameterString4 = @parameterString4 + N'',@ReplierId''
	end
end

-- Format @parameterString3 and @parameterString4
if @parameterString3 != N''''
    set @parameterString3 = N'', N'''''' + substring(@parameterString3, 2, len(@parameterString3) - 1) + N'''''', ''
if @parameterString4 != N''''
    set @parameterString4 = substring(@parameterString4, 2, len(@parameterString4) - 1)

-- Call sp_GetEntities to get the records of the current page and the total records count.
exec sp_GetEntities
@tableName,
@returnFields,
@PageSize,
@selectCountSql,
@selectSql,
@variableDeclarations,
@parameterString1,
@parameterString2,
@parameterString3,
@parameterString4,
@disOrderFields,
@TotalRecords output
' 
END
GO

---------------------------------------------------------------------------------------------------------------
---------------------------------------------------------------------------------------------------------------

-- Create core and default data

-- Create admin member
declare @AdminMemberId int
INSERT INTO [tb_members] (
    MemberId,
    MemberName,
    [Password],
    PasswordFormat,
    PasswordSalt)
VALUES
(
    '654CABD1-D88B-4BDE-BF30-3EF058BF995A',
    'admin',
    'E10ADC3949BA59ABBE56E057F20F883E', -- 系统管理员密码默认为123456
    1,
    ''
)

-- Create admin user
INSERT INTO [tb_users] (MemberId, NickName, UserStatus, TotalMarks)
VALUES
(
    '654CABD1-D88B-4BDE-BF30-3EF058BF995A',
    'admin',
    1,
    10000
)

-- Create roles
declare @EveryOneRoleID int
declare @RegisteredUserRoleID int
declare @OwnerRoleID int
declare @AdministratorRoleID int
declare @UserManageRoleID int
declare @RoleManageRoleID int
declare @RolePermissionRoleID int
declare @SectionManageRoleID int
declare @SectionAdminsManageRoleID int
declare @SectionAdministratorRoleID int
declare @ExceptionLogManageRoleID int

insert into tb_Roles values ('所有人', '', 0)
SELECT @EveryOneRoleID = SCOPE_IDENTITY()
insert into tb_Roles values ('注册用户', '',4)
SELECT @RegisteredUserRoleID = SCOPE_IDENTITY()
insert into tb_Roles values ('所有者', '', 4)
SELECT @OwnerRoleID = SCOPE_IDENTITY()
insert into tb_Roles values ('系统管理员', '', 0)
SELECT @AdministratorRoleID = SCOPE_IDENTITY()
insert into tb_Roles values ('用户管理员', '', 7)
SELECT @UserManageRoleID = SCOPE_IDENTITY()
insert into tb_Roles values ('角色管理员', '', 7)
SELECT @RoleManageRoleID = SCOPE_IDENTITY()
insert into tb_Roles values ('角色授权管理员', '', 7)
SELECT @RolePermissionRoleID = SCOPE_IDENTITY()
insert into tb_Roles values ('版块管理员', '', 7)
SELECT @SectionManageRoleID = SCOPE_IDENTITY()
insert into tb_Roles values ('版主管理员', '', 7)
SELECT @SectionAdminsManageRoleID = SCOPE_IDENTITY()
insert into tb_Roles values ('错误日志管理员', '', 7)
SELECT @ExceptionLogManageRoleID = SCOPE_IDENTITY()
insert into tb_Roles values ('版主', '', 5)
SELECT @SectionAdministratorRoleID = SCOPE_IDENTITY()

-- Get admin userId
declare @AdminUserId int
SELECT @AdminUserId = EntityId FROM tb_Users

-- Grant admin user roles
INSERT INTO tb_UserRoles ([UserId], [RoleId]) VALUES (@AdminUserId, @AdministratorRoleID)

-- Set permissions for roles
INSERT INTO tb_Permissions ([RoleId], [AllowMask], [DenyMask] )
VALUES (@AdministratorRoleID,32767,0)
INSERT INTO tb_Permissions ([RoleId], [AllowMask], [DenyMask] )
VALUES (@OwnerRoleID,8195,0)
INSERT INTO tb_Permissions ([RoleId], [AllowMask], [DenyMask] )
VALUES (@RegisteredUserRoleID,3,0)
INSERT INTO tb_Permissions ([RoleId], [AllowMask], [DenyMask] )
VALUES (@EveryOneRoleID,1,0)
INSERT INTO tb_Permissions ([RoleId], [AllowMask], [DenyMask] )
VALUES (@UserManageRoleID,256,0)
INSERT INTO tb_Permissions ([RoleId], [AllowMask], [DenyMask] )
VALUES (@RoleManageRoleID,512,0)
INSERT INTO tb_Permissions ([RoleId], [AllowMask], [DenyMask] )
VALUES (@RolePermissionRoleID,1024,0)
INSERT INTO tb_Permissions ([RoleId], [AllowMask], [DenyMask] )
VALUES (@SectionManageRoleID,2048,0)
INSERT INTO tb_Permissions ([RoleId], [AllowMask], [DenyMask] )
VALUES (@SectionAdminsManageRoleID,4096,0)
INSERT INTO tb_Permissions ([RoleId], [AllowMask], [DenyMask] )
VALUES (@SectionAdministratorRoleID,8447,0)
INSERT INTO tb_Permissions ([RoleId], [AllowMask], [DenyMask] )
VALUES (@ExceptionLogManageRoleID,16384,0)
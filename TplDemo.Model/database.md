# 安装依赖
> Common

```
Microsoft.EntityFrameworkCore
Microsoft.EntityFrameworkCore.Tools
```

> MSSQL

```
Microsoft.EntityFrameworkCore.SqlServer
```

> MYSQL

```
Pomelo.EntityFrameworkCore.MySql
```

# 程序包管理器控制台
> 设置 TplDemo 为启动项目

* 在 TplDemo 中需安装 `Microsoft.EntityFrameworkCore.Design`

> 在程序包管理器控制台的默认项目中选择 TplDemo.Model

* 1、迁移文件
多个 DbContext,则需指定 
`add-migration init -Context MSSQLContext`
单个 DBContext
`add-migration init`

* 2、删除不必要的迁移文件
多个 DBContext
`Remove-Migration -Context MSSQLContext`
单个 DBContext
`Remove-Migration`

* 3、生成数据库
多个 DbContext,则需指定 
`update-database -Context MSSQLContext`
单个 DContext
`update-database`


# 异常情况
## 问题一
> More than one DbContext was found. Specify which one to use. Use the '-Context' parameter for PowerShell commands and the '--context' parameter for dotnet commands.
> 解析：存在多个DBContext
> 解决方案：指定DBContext
`add-migration init -Context MSSQLContext`
`update-database -Context MSSQLContext`
`Remove-Migration -Context MSSQLContext`
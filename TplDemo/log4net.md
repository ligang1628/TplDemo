﻿# 1、NLog
> NLog、NLog.Web.AspNetCore

# 2、Log4net
> Microsoft.Extensions.Logging.Log4Net.AspNetCore

## 2.1  简单配置

`Program.cs` 中 CreateHostBuilder
```
.ConfigureLogging((context, logbuild) =>
{
    logbuild.AddFilter("System", LogLevel.Warning);
    logbuild.AddFilter("Microsoft", LogLevel.Warning);
    logbuild.AddLog4Net();
})
```

> log4net.config 配置

```
<?xml version="1.0" encoding="utf-8"?>
<log4net>
  <!-- Define some output appenders -->
  <appender name="rollingAppender" type="log4net.Appender.RollingFileAppender">
    <file value="log\log.txt" />
    <!--追加日志内容-->
    <appendToFile value="true" />
    <!--防止多线程时不能写Log,官方说线程非安全-->
    <lockingModel type="log4net.Appender.FileAppender+MinimalLock" />
    <!--可以为:Once|Size|Date|Composite-->
    <!--Composite为Size和Date的组合-->
    <rollingStyle value="Composite" />
    <!--当备份文件时,为文件名加的后缀-->
    <datePattern value="yyyyMMdd.txt" />
    <!--日志最大个数,都是最新的-->
    <!--rollingStyle节点为Size时,只能有value个日志-->
    <!--rollingStyle节点为Composite时,每天有value个日志-->
    <maxSizeRollBackups value="20" />
    <!--可用的单位:KB|MB|GB-->
    <maximumFileSize value="5MB" />
    <!--置为true,当前最新日志文件名永远为file节中的名字-->
    <staticLogFileName value="true" />
    <!--输出级别在INFO和ERROR之间的日志-->
    <!--过滤级别 FATAL > ERROR > WARN > INFO > DEBUG-->
    <filter type="log4net.Filter.LevelRangeFilter">
      <param name="LevelMin" value="WARN" />
      <param name="LevelMax" value="FATAL" />
    </filter>
    <layout type="log4net.Layout.PatternLayout">
      <conversionPattern value="%date [%thread] %-5level %logger -  %message%newline"/>
    </layout>
  </appender>
  <root>
    <priority value="ALL"/>
    <level value="ALL"/>
    <appender-ref ref="rollingAppender" />
  </root>
</log4net>
```
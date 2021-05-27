说明：
从铂金人力资源管理系统同步数据到 UUM 的计划任务

部署：
将此 dll 配置到 ServiceHost 服务下运行

配置实例：
<ServiceHost>
  <Task ID="ClearOnlineFileCache" Name="清除在线预览缓存">
    <Action Type="Indigox.ServiceHost.Actions.CQRSCommandAction, Indigox.ServiceHost">
      <Property Name="SiteUrl" Value="http://uum.dev.indigox.net" />
      <Property Name="CommandName" Value="SyncFromHRCommand" />
    </Action>
    <Schedules>
      <Schedule Type="Indigox.ServiceHost.Schedules.DaliySchedule, Indigox.ServiceHost">
        <Property Name="StartBoundary" Value="05:00" />
        <Property Name="EndBoundary" Value="23:00" />
        <Property Name="Interval" Value="00:05" />
      </Schedule>
    </Schedules>
  </Task>
</ServiceHost>

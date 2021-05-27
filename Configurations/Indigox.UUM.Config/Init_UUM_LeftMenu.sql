--select * from navigationnode
--select * from navigation
delete  navigationnode
delete  navigation


insert into navigation (id,name) values(1,'Detault')
insert into navigation (id,name) values(2,'UUM')

--HR同步管理
insert into navigationnode (ID,ParentType,Parent,Title,Description,Url,Target,Icon,CreateTime,ModifyTime,OrderNum,Root)
values(100,'Navigation',2,'HR同步管理',NULL,'javascript:void(0)','_self','',getdate(),getdate(),1.001,2)
insert into navigationnode (ID,ParentType,Parent,Title,Description,Url,Target,Icon,CreateTime,ModifyTime,OrderNum,Root)
values(101,'NavigationNode',100,'命名规则管理',NULL,'#/NameStrategyDescriptor/List.htm','_blank','',getdate(),getdate(),1.001,2)
insert into navigationnode (ID,ParentType,Parent,Title,Description,Url,Target,Icon,CreateTime,ModifyTime,OrderNum,Root)
values(102,'NavigationNode',100,'HR同步确认',NULL,'#/HR/HRPrincipalList.htm','_blank','',getdate(),getdate(),1.002,2)
--系统同步管理
insert into navigationnode (ID,ParentType,Parent,Title,Description,Url,Target,Icon,CreateTime,ModifyTime,OrderNum,Root)
values(103,'Navigation',2,'系统同步管理',NULL,'javascript:void(0)','_self','',getdate(),getdate(),1.002,2)
insert into navigationnode (ID,ParentType,Parent,Title,Description,Url,Target,Icon,CreateTime,ModifyTime,OrderNum,Root)
values(104,'NavigationNode',103,'同步系统配置',NULL,'#/SysConfiguration/List.htm','_blank','',getdate(),getdate(),1.001,2)
insert into navigationnode (ID,ParentType,Parent,Title,Description,Url,Target,Icon,CreateTime,ModifyTime,OrderNum,Root)
values(105,'NavigationNode',103,'同步队列管理',NULL,'#/SysConfiguration/State.htm','_blank','',getdate(),getdate(),1.002,2)
insert into navigationnode (ID,ParentType,Parent,Title,Description,Url,Target,Icon,CreateTime,ModifyTime,OrderNum,Root)
values(106,'NavigationNode',103,'同步任务管理',NULL,'#/SyncTask/List.htm','_blank','',getdate(),getdate(),1.003,2)
--个人自助管理
insert into navigationnode (ID,ParentType,Parent,Title,Description,Url,Target,Icon,CreateTime,ModifyTime,OrderNum,Root)
values(107,'Navigation',2,'个人自助管理',NULL,'javascript:void(0)','_self','',getdate(),getdate(),1.003,2)
insert into navigationnode (ID,ParentType,Parent,Title,Description,Url,Target,Icon,CreateTime,ModifyTime,OrderNum,Root)
values(108,'NavigationNode',107,'个人设置',NULL,'#/Profile/Edit.htm','_blank','',getdate(),getdate(),1.001,2)
insert into navigationnode (ID,ParentType,Parent,Title,Description,Url,Target,Icon,CreateTime,ModifyTime,OrderNum,Root)
values(109,'NavigationNode',107,'密码修改',NULL,'#/HR/HRPrincipalList.htm','_blank','',getdate(),getdate(),1.002,2)
--工作区
insert into navigationnode (ID,ParentType,Parent,Title,Description,Url,Target,Icon,CreateTime,ModifyTime,OrderNum,Root)
values(110,'Navigation',2,'工作区',NULL,'javascript:void(0)','_self','',getdate(),getdate(),1.004,2)
insert into navigationnode (ID,ParentType,Parent,Title,Description,Url,Target,Icon,CreateTime,ModifyTime,OrderNum,Root)
values(111,'NavigationNode',110,'角色管理',NULL,'#/Role/List.htm','_blank','',getdate(),getdate(),1.001,2)
insert into navigationnode (ID,ParentType,Parent,Title,Description,Url,Target,Icon,CreateTime,ModifyTime,OrderNum,Root)
values(112,'NavigationNode',110,'群组管理',NULL,'#/Group/List.htm','_blank','',getdate(),getdate(),1.002,2)
insert into navigationnode (ID,ParentType,Parent,Title,Description,Url,Target,Icon,CreateTime,ModifyTime,OrderNum,Root)
values(113,'NavigationNode',110,'禁用用户管理',NULL,'#/OrganizationalPerson/DisabledUserList.htm','_blank','',getdate(),getdate(),1.003,2)
--组织结构
insert into navigationnode (ID,ParentType,Parent,Title,Description,Url,Target,Icon,CreateTime,ModifyTime,OrderNum,Root)
values(114,'Navigation',2,'组织结构',NULL,'javascript:void(0)','_self','',getdate(),getdate(),1.005,2)
--通讯录
insert into navigationnode (ID,ParentType,Parent,Title,Description,Url,Target,Icon,CreateTime,ModifyTime,OrderNum,Root)
values(115,'Navigation',2,'通讯录',NULL,'javascript:void(0)','_self','',getdate(),getdate(),1.006,2)


--select * from dbo.serial
update dbo.serial set serial_seed=200 where serial_name='NavigationNode'
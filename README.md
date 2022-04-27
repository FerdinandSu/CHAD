# CHAD-课程资源管理

HIT(SZ) 数据库系统实验3

## Environment

- Windows x64
- .NET 5.0.104
- Node 14.8.0
- Yarn 1.22.5
- Angular CLI: 11.0.1
- MySQL 8.0.22

### 其他工具

- PowerDesigner 16.7.1（试用）

## 目录结构

- client前端
- server后端
  - src/Server服务器
  - src/Core服务、模型
- models模型(pdm,ldm,cdm)
- scripts脚本

## 复现

### 数据库

使用`scripts/chad.sql`建库。

### 后端

全部代码位于`server/`，在`server/src/Server/`添加`appsettings.json`，内容为：

```json
{
  "AllowedHosts": "*",
  "ConnectionStrings": {
    "CHAD": "连接字符串"

  },
  "JWTSettings": {
    "securityKey": "随机字符串",
    "validIssuer": "ChadApi",
    "validAudience": "后端服务运行的URL，如https://localhost:5000"
  },
  "Initialization": {
    "Includes": [
      {
        "Type": "User",
        "File": "assets/.users.csv"
      }
    ]
  }
}
```

可以向`server/src/Server/assets/.users.csv`添加更多内容，或者在JSON的`Includes`里增加其他项目，来增加初始化用户。
系统默认管理员两个，账号分别为`root`和`root_backup`，默认密码均为MAR5_admin。

在`server/src/Server/`下`dotnet run`即可运行服务器。服务器默认运行在`localhost:5000`，可更改`server/src/Server/Properties/launchSettings.json`来变更端口。

### 前端

全部代码位于`client/`，运行`yarn install && yarn run start`来运行测试服务器。测试服务器包含了反向代理`localhost:5000`到`/api`的功能。服务器默认运行在`localhost:4200`.

## 部署

本系统为数据库系统的课程实验，因此没有进行部署，文档中也不会提供部署方案。但是Angular和.NET的部署方案都很完善，因此部署是一定可行的。

## WebAPIs

### 账户

- account/login post(username,pwd,rememberme)=>User
- account/logout get
- account/chpwd post(str[])
- account/register post(ManagedGeneratingUser)
- account/managed get()=>ManagedUser[]
- account get(UserRole?)=UserSummary[]
- account delete(#id)

### 资源

- res get()=>Resource[]
- res get(#id)=>_file
- res post(_file)=>Resource
- res delete(#id)

### 元素

- course 
  - get()=>ElementSummary[]
  - get(#id)=>Course
  - post(DES)=>ES
  - delete(#id)
- course/%id/lesson/%i post(DES)=>ES
- course/%id/class post(ES)
- course/%id/class/%classId delete
- lesson
  - get(#id)=>Lesson
  - delete(#id)
- lesson/%id/res 
  - post(ElementSummary)
  - delete
- class
  - get()=>ElementSummary[]
  - post(ES)=>ES
  - get(#id)=>Class
  - delete(#id)
- class/%id/student post(UserSummary),delete

### 聊天

- chat
  - get()=>ChatMessage[]
  - post(ChatMessage)

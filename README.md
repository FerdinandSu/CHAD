# CHAD

## APIs

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

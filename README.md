## 💻 Pré-requisitos

* Instalar `.NET 7.0 sdk`, você pode baixar [aqui](https://dotnet.microsoft.com/en-us/download)

## ☕ Iniciando o projeto

Para iniciar o projeto, siga estas etapas:

* Navegue até a pasta do projeto;

```
dotnet run
```


## 🐛 Problemas?

Para tentar corrigir tente executar essas linhas na pasta do projeto:

```
dotnet add package Microsoft.AspNetCore.StaticFiles --version 6.0

dotnet add package Microsoft.AspNetCore.Mvc --version 6.0

dotnet add package Microsoft.EntityFrameworkCore.Design --version 6.0

dotnet add package Microsoft.EntityFrameworkCore.Sqlite --version 6.0

dotnet tool install --global dotnet-ef

dotnet ef migrations add InitialCreate

dotnet ef database update
```

* Tente iniciar o projeto novamente com:

```
dotnet run
```
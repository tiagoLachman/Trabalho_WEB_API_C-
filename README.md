## 💻 Pré-requisitos

* Instalar `.NET 7.0 sdk`, você pode baixar [aqui](https://dotnet.microsoft.com/en-us/download)
* Instalar `Postman`, que pode ser baixado [aqui](https://www.postman.com/downloads/)

## 🚀 Configurando o Postman

* Para importar arquivos no `Postman`, vá até `☰ -> File -> Import` e selecione os arquivos.

* Na pasta do projeto [postman](/postman), importe o arquivo [collection.json](/postman/Trabalho%20C#%20WEB_API.postman_collection.json) no `Postman`.

* Na mesma pasta, importe o arquivo [environment.json](/postman/Trabalho%20C#%20WEB_API_ENV.postman_environment.json) no `Postman`, este arquivo corresponde as configurações do projeto.

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
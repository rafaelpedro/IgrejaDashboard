# Igreja Dashboard

Aplicação web de controle de membros de uma igreja, com **backend em .NET 8** e **frontend em Angular 18**.  
O sistema permite **listar, cadastrar, editar e remover pessoas**, além de exibir um **dashboard com indicadores** de total de membros, homens e mulheres.

---

## Tecnologias Utilizadas

### 🔹 Backend (.NET)
- ASP.NET Core 8 Web API  
- Entity Framework Core (Code First + Migrations)  
- SQL Server (LocalDB)  
- Swagger UI  
- CORS habilitado para comunicação com o frontend  

### 🔹 Frontend (Angular)
- Angular 18  
- TypeScript  
- Bootstrap 5  
- RxJS e HttpClient  
---

## Como Executar o Projeto

###  1. Clonar o repositório
git clone https://github.com/seuusuario/IgrejaDashboard.git
cd IgrejaDashboard.Api

###  2. Configurar e rodar o backend (.NET)
 Passo 1: Verifique o arquivo de conexão
No arquivo appsettings.json:
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=IgrejaDashboardDB;Trusted_Connection=True;"
}

 Passo 2: Aplicar as migrations
No Package Manager Console do Visual Studio:
Update-Database

 Passo 3: Executar a API
Execute o projeto (Ctrl + F5) e acesse o Swagger:
📍 https://localhost:7240/swagger

###  3. Rodar o frontend (Angular)
Acesse a pasta do projeto Angular:
cd igreja-dashboard

Instale as dependências:
npm install

Inicie o servidor:
ng serve

Acesse no navegador:
 http://localhost:4200

---

###  Comunicação Front ↔ Back

O Angular consome os endpoints do backend via serviço (dashboard.service.ts):
private apiUrl = 'https://localhost:7240/api/pessoas';

Ajuste a porta caso o backend rode em outra URL.
O CORS está habilitado no backend:

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod());
});
app.UseCors("AllowAll");

---

👤 Autor
Rafael Alencar Pedro
Desenvolvedor Full Stack Jr
rafael.apedro95@gmail.com

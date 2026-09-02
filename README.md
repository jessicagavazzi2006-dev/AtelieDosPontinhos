# Atelie dos Pontinhos

Aplicação ASP.NET Core em arquitetura em camadas (Domain, Application, Infrastructure, API, UI e Desktop) — projeto didático para ensino de EF Core, Identity, APIs e Razor MVC.

Resumo rápido
- Estrutura em camadas com Repositórios, Services e DbContext.
- Seed automático que cria dados iniciais e um usuário administrador.

Projetos principais
- AtelieDosPontinhos.Domain — Entidades e interfaces
- AtelieDosPontinhos.Application — Serviços, DTOs e lógica de aplicação
- AtelieDosPontinhos.Infrastructure — DbContext, Repositórios, Migrations e Seed
- AtelieDosPontinhos.API — Endpoints REST (Swagger)
- AtelieDosPontinhos.UI — Interface web (MVC/Razor)
- AtelieDosPontinhos.Desktop — aplicação desktop (opcional)

Target framework
- Todos os projetos principais neste repositório usam .NET 10.0 (TargetFramework: net10.0).

Tecnologias
- .NET 10
- ASP.NET Core (Web API + MVC)
- Entity Framework Core 10.x (SQL Server)
- ASP.NET Core Identity
- Swagger (Swashbuckle)
- Bootstrap 5 (UI)

Pré-requisitos
- .NET 10 SDK instalado
- SQL Server LocalDB (vem com o Visual Studio) ou outro SQL Server

Instalação e execução
1. Clonar o repositório
```bash
git clone https://github.com/jessicagavazzi2006-dev/AtelieDosPontinhos.git
cd AtelieDosPontinhos
```

2. Restaurar pacotes
```bash
dotnet restore
```

3. Criar/aplicar o banco de dados (migrations)

Opção A — Package Manager Console (Visual Studio)
```powershell
Update-Database -Project AtelieDosPontinhos.Infrastructure -StartupProject AtelieDosPontinhos.API
```

Opção B — CLI
```bash
dotnet ef database update --project AtelieDosPontinhos.Infrastructure --startup-project AtelieDosPontinhos.API
```

Observação: Em ambiente de Development os projetos aplicam migrations/seed automaticamente durante a inicialização.

4. Executar a aplicação

API (Swagger)
```bash
dotnet run --project AtelieDosPontinhos.API
```
URLs (padrão de launchSettings):
- HTTP: http://localhost:5006
- HTTPS: https://localhost:7033
Swagger: http://localhost:5006/swagger (ou https://localhost:7033/swagger)

UI (MVC)
```bash
dotnet run --project AtelieDosPontinhos.UI
```
URLs (padrão de launchSettings):
- HTTP: http://localhost:5012
- HTTPS: https://localhost:7049

Credenciais administrativas (seed)
- Email: admin@site.com
- Senha: Admin@123
- Role: Admin

Connection string (padrão)
Arquivo: AtelieDosPontinhos.API/appsettings.json
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=AtelieDB;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
  }
}
```

API — endpoints principais

Produtos (controller: api/Product)
- GET /api/Product — lista produtos
- GET /api/Product/{id} — busca por id
- GET /api/Product/search?term={term} — busca por termo
- POST /api/Product — cria produto
- PUT /api/Product/{id} — atualiza produto
- DELETE /api/Product/{id} — remove produto

Categorias (controller: api/Category)
- GET /api/Category — lista categorias
- GET /api/Category/{id} — busca categoria por id
- GET /api/Category/search?term={term} — busca por termo
- POST /api/Category — cria categoria (Admin)
- PUT /api/Category/{id} — atualiza categoria (Admin)
- DELETE /api/Category/{id} — remove categoria (Admin)

Autenticação / Conta (controller: api/Account)
- POST /api/Account/register — registra usuário (cadastro)
- POST /api/Account/login — login (retorna roles)
- GET /api/Account/user-data?email={email} — retorna endereço/pagamento associado ao e-mail

Pedidos (controller: api/Orders)
- POST /api/Orders — criar pedido (Authorize)
- GET /api/Orders/my — listar pedidos do usuário atual (Authorize)
- GET /api/Orders — listar todos (Admin)
- GET /api/Orders/{id} — obter pedido por id (dono ou Admin)
- PUT /api/Orders/{id}/status — atualizar status (Admin)

Observações sobre API
- Há implementação de repositórios com métodos adicionais (CountAsync, GetFeaturedAsync, SearchAsync) na camada Infrastructure; nem todos estão expostos diretamente como endpoints REST no controller atual.
- O CartController existe no código, mas está comentado no repositório atual.

Dicas de desenvolvimento
- Se alterar entidades, crie uma nova migration e aplique-a conforme comandos acima.
- Use o perfil de launchSettings indicado para obter as mesmas URLs locais mostradas aqui.

Licença
Projeto didático desenvolvido para fins educacionais.

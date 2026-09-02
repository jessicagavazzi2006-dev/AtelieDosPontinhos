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

Desenvolvidos pelos alunos:
- Lucas Richard - Domain, Application, Desktop
- Jessica Alves - Domain, Infrastructure, Desktop
- Elizabeth Dorigon - UI, API, Cart(função de carrinho)

## Tecnologias Utilizadas

| Tecnologia | Versão / Observação | Uso |
|------------|---------------------|-----|
| .NET | API: 10.0, Demais projetos: 8.0 | Framework principal |
| ASP.NET Core MVC / Razor Pages | 8.0 / 10.0 | Aplicação web / API |
| Entity Framework Core | 8.x / 10.x | ORM / Acesso a dados |
| SQL Server LocalDB | — | Banco de dados |
| ASP.NET Core Identity | 8.x / 10.x | Autenticação |
| Bootstrap | 5.3 | Framework CSS |
| Bootstrap Icons | 1.11 | Ícones |
| Swagger | 6.x / 10.x | Documentação da API |

> Observação: o projeto contém partes alvo em .NET 10 (API) e outros projetos em .NET 8 — ajuste o SDK instalado conforme necessário.

## Estrutura das Camadas


```
////!!!!!!!!!!!!necessario atulizar quando tiver terminado!!!!!!!!!!!!!!!!!!!

AtelieDosPontinhos/
├── AtelieDosPontinhos.Domain     → Entidades, Interfaces
├── AtelieDosPontinhos.Application   → Services, DTOs, ViewModels
├── AtelieDosPontinhos.Infrastructure → DbContext, Repositories, Identity, Migrations, Configurations
├── AtelieDosPontinhos.API       → Controllers REST, Swagger
├── AtelieDosPontinhos.Desktop       → DTOs, Forms, Helpers, Resource, Services, Themes, userControls
└── AtelieDosPontinhos.UI       → Controllers MVC, Views Razor, Bootstrap
```


### Responsabilidade de cada camada:

- **Domain**: Entidades (Product, Pagamento, Endereco, Category, Material, Pedido, etc.) e interfaces dos repositórios.
- **Application**: Serviços que orquestram operações, DTOs e ViewModels.
- **Infrastructure**: Implementação do acesso a dados com EF Core, repositórios, Identity e Seed Data.
- **API**: Endpoints REST com Swagger.
- **Desktop**: Interface local/gerenciamento (opcional).
- **UI**: Interface web (MVC/Razor) com Bootstrap.

## Novas funcionalidades adicionadas (detectadas)

- CORS: política `PermitirTudo` registrada e aplicada em `Program.cs` para permitir que a UI (front-end) consulte a API via JavaScript.
- Autenticação em API: comportamento do cookie configurado para que chamadas a rotas que começam com `/api` retornem `401` (quando não autenticado) ou `403` (when access denied) em vez de redirecionamentos HTML.
- Serviços e repositórios de produto:
  - `IProductService` e `IProductRepository` registrados via DI (`AddScoped`) no startup.
  - Métodos detectados no serviço/repositório:
    - `SearchAsync(term)` — busca produtos por termo.
    - `CountAsync()` — retorna total de produtos.
    - `GetFeaturedAsync()` — retorna produtos em destaque.
- DbContext:
  - `CoverImageUrl` do `Product` mapeado como `nvarchar(max)` para suportar imagens longas em Base64.
  - Novas entidades observadas: `Material`, `Product_Material`, `Pedido`, `PedidoItem`, `Endereco`, `Pagamento`.
- Seed Data: execução automática do seed durante a inicialização da aplicação (aplica dados iniciais, inclusive usuário admin).
- Debug: impressão da connection string no console ao iniciar para facilitar verificação.
- Swagger habilitado em ambiente de desenvolvimento.

## Como Executar

### Pré-requisitos
- [.NET 10 SDK] (para a API) e [.NET 8 SDK] (para outros projetos) conforme presente no repositório.
- [SQL Server LocalDB] (vem com o Visual Studio)

### Passo 1: Clonar o repositório
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

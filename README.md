# Atelie do Pontinhos

> Aplicação completa ASP.NET Core em arquitetura de camadas para ensino.

## Sobre o Projeto

O **AtelieDosPontinhos** é uma loja virtual que vende produtos e materiais de artesanato focado apenas em ponto cruz. O site foi desenvolvido de forma didática para ensino de:

- ASP.NET Core (API e MVC/Razor Pages)
- Arquitetura em camadas
- Entity Framework Core
- ASP.NET Core Identity
- API REST
- CRUD completo
- Razor Views
- Bootstrap 5

## Integrantes do grupo

Desenvolvidos pelos alunos:
- Lucas Richard - Domain, Application
- Jessica Alves - Domain, Infrastructure
- Elizabeth Dorigon - UI, API

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

SenacGames/
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

### Passo 2: Restaurar pacotes
```bash
dotnet restore
```

### Passo 3: Criar o banco de dados

#### Opção 1 — Package Manager Console (Visual Studio)
```powershell
Update-Database -Project AtelieDosPontinhos.Infrastructure -StartupProject AtelieDosPontinhos.API
```

#### Opção 2 — PowerShell / CMD
```bash
dotnet ef database update --project AtelieDosPontinhos.Infrastructure --startup-project AtelieDosPontinhos.API
```

> **Nota:** O banco é criado automaticamente na primeira execução (o Seed Data aplica as migrations).

### Passo 4: Executar a aplicação

#### Rodar a API (Swagger):
```bash
dotnet run --project AtelieDosPontinhos.API
```
Acesse: `https://localhost:5001/swagger`

#### Rodar a UI (MVC):
```bash
dotnet run --project AtelieDosPontinhos..UI
```
Acesse: `https://localhost:5002` (ou a porta indicada no terminal)

## Usuário Administrador

O sistema cria automaticamente um usuário admin:

| Campo | Valor |
|-------|-------|
| Email | admin@site.com |
| Senha | Admin@123 |
| Role  | Admin |

## Endpoints da API (principais)

### Produtos (novos endpoints detectados)
| Método | Endpoint | Descrição |
|--------|----------|-----------|
| GET | `/api/Products` | Lista todos os produtos |
| GET | `/api/Products/{id}` | Busca produto por ID |
| GET | `/api/Products/search?term={term}` | Busca produtos por termo (novo) |
| GET | `/api/Products/featured` | Produtos em destaque (novo) |
| GET | `/api/Products/count` | Total de produtos (novo) |
| POST | `/api/Products` | Cria novo produto (Admin) |
| PUT | `/api/Products/{id}` | Atualiza produto (Admin) |
| DELETE | `/api/Products/{id}` | Remove produto (Admin) |

### Categorias
| Método | Endpoint | Descrição |
|--------|----------|-----------|
| GET | `/api/categories` | Lista todas as categorias |
| POST | `/api/categories` | Cria categoria (Admin) |
| PUT | `/api/categories/{id}` | Atualiza categoria (Admin) |
| DELETE | `/api/categories/{id}` | Remove categoria (Admin) |

### Materiais
| Método | Endpoint | Descrição |
|--------|----------|-----------|
| GET | `/api/Material` | Lista todos os materiais |
| POST | `/api/Material` | Cria material (Admin) |
| PUT | `/api/Material/{id}` | Atualiza material (Admin) |
| DELETE | `/api/Material/{id}` | Remove material (Admin) |

### Autenticação
| Método | Endpoint | Descrição |
|--------|----------|-----------|
| POST | `/api/auth/register` | Registra usuário |
| POST | `/api/auth/login` | Faz login |
| POST | `/api/auth/logout` | Faz logout |
| GET | `/api/auth/me` | Dados do usuário |

## Configuração do Banco

A connection string está em `appsettings.json`:

```json
{
 "ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=MaterialDb;Trusted_Connection=True;MultipleActiveResultSets=true"
 }
}
```

Para usar outro servidor SQL Server, altere a connection string nos projetos **API** e **UI**.

## Migrations

### Criar nova migration:

#### Package Manager Console:
```powershell
Add-Migration NomeDaMigration -Project AtelieDosPontinhos.Infrastructure -StartupProject AtelieDosPontinhos.API
```

#### PowerShell:
```bash
dotnet ef migrations add NomeDaMigration --project AtelieDosPontinhos.Infrastructure --startup-project AtelieDosPontinhos.API
```

### Aplicar migrations:

#### Package Manager Console:
```powershell
Update-Database -Project AtelieDosPontinhos.Infrastructure -StartupProject AtelieDosPontinhos.API
```

#### PowerShell:
```bash
dotnet ef database update --project AtelieDosPontinhos.Infrastructure --startup-project AtelieDosPontinhos.API
```

## Licença

Projeto didático desenvolvido para o Senac — uso educacional.

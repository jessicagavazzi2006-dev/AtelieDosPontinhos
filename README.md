# Atelie do Pontinhos

> Aplicação completa ASP.NET Core MVC em arquitetura de camadas para ensino.

## Sobre o Projeto

O **AtelieDosPontinhos** é uma loja virtual que vende Products de artesanato, o site foi desenvolvido de forma de didatico :

desenvolvidos pelos alunos:
Lucar Richard - Domain, Aplication
Jessica Alves - Domain, Infrastucture
Elizabeth Dorigon - UI, API

- ASP.NET Core MVC
- Arquitetura em camadas
- Entity Framework Core
- ASP.NET Core Identity
- API REST
- CRUD completo
- Razor Views
- Bootstrap 5

## Tecnologias Utilizadas

| Tecnologia | Versão | Uso |
|------------|--------|-----|
| .NET | 8.0 | Framework principal |
| ASP.NET Core MVC | 8.0 | Aplicação web |
| Entity Framework Core | 8.0.11 | ORM / Acesso a dados |
| SQL Server LocalDB | — | Banco de dados |
| ASP.NET Core Identity | 8.0 | Autenticação |
| Bootstrap | 5.3 | Framework CSS |
| Bootstrap Icons | 1.11 | Ícones |
| Swagger | 6.5 | Documentação da API |

## Estrutura das Camadas

```
////!!!!!!!!!!!!necessario atulizar quando tiver terminado!!!!!!!!!!!!!!!!!!!

SenacGames/
├── AtelieDosPontinhos.Domain     → Entidades, Interfaces
├── SenacGames.Application   → Services, DTOs, ViewModels
├── SenacGames.Infrastructure → DbContext, Repositories, Identity, Migrations
├── SenacGames.API       → Controllers REST, Swagger
└── SenacGames.UI       → Controllers MVC, Views Razor, Bootstrap
```

### Responsabilidade de cada camada:

- **Domain**: Define as entidades (Product, Material e Category ) e as interfaces dos repositórios. Não depende de nenhuma outra camada.
- **Application**: Contém os serviços que orquestram as operações, DTOs para transferência de dados e ViewModels para as Views.
- **Infrastructure**: Implementa o acesso a dados com Entity Framework Core, os repositórios, o Identity e o Seed Data.
- **API**: Expõe os endpoints REST com Swagger para testes.
- **UI**: Aplicação MVC com Razor Views e Bootstrap para a interface do usuário.

## Como Executar

### Pré-requisitos
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server LocalDB](https://docs.microsoft.com/sql/database-engine/configure-windows/sql-server-express-localdb) (vem com o Visual Studio)

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
| Role | Admin |

## Endpoints da API

### Produtos
| Método | Endpoint | Descrição |
|--------|----------|-----------|
| GET | `/api/Products` | Lista todos os produtos |
| GET | `/api/Products/{id}` | Busca produto por ID |
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
Add-Migration NomeDaMigration -Project Material.Infrastructure -StartupProject Material.API
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

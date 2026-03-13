# 🛒 OrderFlow API

![.NET](https://img.shields.io/badge/.NET-8.0-512BD4?style=flat-square&logo=dotnet)
![C#](https://img.shields.io/badge/C%23-12.0-239120?style=flat-square&logo=csharp)
![SQL Server](https://img.shields.io/badge/SQL_Server-2022-CC2927?style=flat-square&logo=microsoftsqlserver)
![EF Core](https://img.shields.io/badge/EF_Core-8.0-512BD4?style=flat-square&logo=dotnet)
![JWT](https://img.shields.io/badge/JWT-Bearer-000000?style=flat-square&logo=jsonwebtokens)
![Docker](https://img.shields.io/badge/Docker-ready-2496ED?style=flat-square&logo=docker)
![Swagger](https://img.shields.io/badge/Swagger-OpenAPI-85EA2D?style=flat-square&logo=swagger)
![License](https://img.shields.io/badge/license-MIT-green?style=flat-square)

API RESTful para **gerenciamento comercial completo** — clientes, fornecedores, produtos, pedidos, pagamentos e controle de estoque. Desenvolvida com **ASP.NET Core 8** seguindo os princípios de **Clean Architecture**.

---

## 🚀 Funcionalidades

- ✅ **Autenticação JWT** — Login seguro com controle de perfis (Admin, Manager, Seller)
- ✅ **Clientes** — CRUD completo com paginação, filtros por nome e cidade
- ✅ **Fornecedores** — CRUD com controle de acesso por perfil
- ✅ **Categorias** — Gerenciamento de categorias de produtos
- ✅ **Produtos** — CRUD com SKU único, margem de lucro e alerta de estoque mínimo
- ✅ **Pedidos** — Criação com baixa automática de estoque e controle de status
- ✅ **Pagamentos** — Registro com validação de valor e confirmação automática do pedido
- ✅ **Estoque** — Movimentações de entrada, saída e ajuste com histórico completo
- ✅ **Dashboard** — Resumo gerencial com totais, pedidos pendentes e vendas por período
- ✅ **Soft Delete** — Registros nunca são deletados fisicamente
- ✅ **Paginação** — Todos os endpoints de listagem são paginados
- ✅ **Validação** — Regras de negócio com FluentValidation
- ✅ **Seed automático** — Banco populado automaticamente na primeira execução

---

## 📸 Screenshots

### Autenticação JWT
![Auth](docs/screenshots/01-auth.png)

### Categorias
![Category Create](docs/screenshots/02-category-create.png)
![Category Create](docs/screenshots/03-category-create-2.png)
![Category Create](docs/screenshots/04-category-create-3.png)
![Categories List](docs/screenshots/05-categories-list.png)

### Fornecedores
![Suppliers List](docs/screenshots/06-suppliers-list.png)

### Produtos
![Products List](docs/screenshots/07-products-list.png)

### Clientes
![Customers List](docs/screenshots/08-customers-list.png)

### Pedidos
![Order Create](docs/screenshots/09-order-create.png)
![Orders List](docs/screenshots/10-orders-list.png)

### Pagamentos
![Payment Create](docs/screenshots/11-payment-create.png)

### Estoque
![Stock Movement](docs/screenshots/12-stock-movement.png)

### Dashboard
![Dashboard](docs/screenshots/13-dashboard.png)

## 🏗️ Arquitetura

Projeto estruturado em **4 camadas** seguindo os princípios de **Clean Architecture**:
```
OrderFlow/
├── src/
│   ├── OrderFlow.API/           # Controllers, Middlewares, Configurações
│   ├── OrderFlow.Application/   # DTOs, Services, Validators, Interfaces
│   ├── OrderFlow.Domain/        # Entidades, Enums, Exceções, Interfaces
│   └── OrderFlow.Infrastructure/# DbContext, Mappings, Repositórios, JWT
└── tests/
    └── OrderFlow.Tests/         # Testes unitários
```

**Fluxo:** Controller → Service → Repository → DbContext

---

## 🛠️ Tecnologias

| Tecnologia | Uso |
|---|---|
| ASP.NET Core 8 | Framework web |
| Entity Framework Core 8 | ORM com Code First |
| SQL Server | Banco de dados relacional |
| JWT Bearer | Autenticação e autorização |
| AutoMapper 12 | Mapeamento Entity ↔ DTO |
| FluentValidation | Validação de dados |
| Serilog | Logging estruturado |
| Swagger/OpenAPI | Documentação interativa |
| xUnit + Moq | Testes unitários |
| Docker | Containerização |

---

## ⚙️ Como Rodar Localmente

### Pré-requisitos
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8)
- [SQL Server](https://www.microsoft.com/sql-server) ou [Docker](https://www.docker.com)

### Com Docker (recomendado)
```bash
docker-compose up -d
```

A API estará disponível em `http://localhost:5230`

### Sem Docker
```bash
# 1. Clone o repositório
git clone https://github.com/degasdegani/OrderFlow.git
cd OrderFlow

# 2. Configure a connection string em src/OrderFlow.API/appsettings.json

# 3. Execute a aplicação
dotnet run --project src/OrderFlow.API
```

> A migration e o seed são executados **automaticamente** na primeira execução.

---

## 🔐 Autenticação

A API utiliza **JWT Bearer**. Para acessar endpoints protegidos:

1. Faça login em `POST /api/Auth/login`
2. Copie o token retornado
3. No Swagger, clique em **Authorize** e informe: `Bearer {seu_token}`

### Usuários padrão (seed)

| E-mail | Senha | Perfil |
|--------|-------|--------|
| admin@orderflow.com | Admin@123 | Admin |
| gerente@orderflow.com | Gerente@123 | Manager |
| vendedor@orderflow.com | Vendedor@123 | Seller |

---

## 📁 Estrutura do Banco de Dados
```
Users
├── Orders
│   ├── OrderItems
│   │   └── Products
│   │       ├── Categories
│   │       └── Suppliers
│   └── Payments
├── Customers
│   └── Orders
└── StockMovements
    └── Products
```

---

## 🧪 Testes
```bash
dotnet test
```

11 testes unitários cobrindo os principais services da aplicação.

---

## 📐 Padrões utilizados

- **Repository Pattern** — Abstração do acesso a dados
- **Unit of Work** — Gerenciamento de transações
- **DTO Pattern** — Transferência de dados entre camadas
- **Global Exception Middleware** — Tratamento centralizado de erros
- **Soft Delete** — Deleção lógica de registros
- **Fluent API** — Mapeamento do banco de dados sem Data Annotations

---

## 👨‍💻 Autor

**Eduardo Degani** — Desenvolvedor .NET em transição de carreira

[![LinkedIn](https://img.shields.io/badge/LinkedIn-Eduardo%20Degani-0077B5?style=flat-square&logo=linkedin)](https://www.linkedin.com/in/eduardo-degani/)
[![GitHub](https://img.shields.io/badge/GitHub-degasdegani-181717?style=flat-square&logo=github)](https://github.com/degasdegani)
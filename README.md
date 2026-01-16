# API Valheim - Documentação Completa

## 📋 Índice
1. [Visão Geral do Projeto](#visão-geral-do-projeto)
2. [Arquitetura](#arquitetura)
3. [Tecnologias e Dependências](#tecnologias-e-dependências)
4. [Estrutura do Projeto](#estrutura-do-projeto)
5. [Configuração e Instalação](#configuração-e-instalação)
6. [Autenticação e Autorização](#autenticação-e-autorização)
7. [Rotas da API](#rotas-da-api)
8. [Modelos de Dados](#modelos-de-dados)
9. [Como Utilizar as Rotas](#como-utilizar-as-rotas)
10. [Desenvolvimento e Execução](#desenvolvimento-e-execução)

---

## Visão Geral do Projeto

A **API Valheim** é uma API REST desenvolvida em **ASP.NET Core 6** que gerencia dados relacionados ao jogo Valheim. O projeto implementa um sistema de gerenciamento de:

- **Usuários**: Autenticação e autorização com JWT
- **Jogadores**: Perfis de jogadores do jogo Valheim
- **Personagens**: Personagens criados por cada jogador
- **Itens**: Itens de inventário associados aos personagens

A API utiliza **Entity Framework Core 7** para persistência de dados em um banco de dados SQL Server, segue o padrão **Repository Pattern** para acesso a dados e implementa autenticação segura via **JWT (JSON Web Tokens)**.

---

## Arquitetura

O projeto segue uma arquitetura em camadas com separação clara de responsabilidades:

```
API Valheim
│
├── Controllers          (Rotas e endpoints da API)
├── Models              (Entidades do banco de dados)
├── Repository          (Padrão Repository - acesso a dados)
│   ├── Interfaces      (Contratos das operações)
│   └── Context         (DbContext do Entity Framework)
├── Services            (Lógica de negócio - ex: geração de tokens)
├── DTO                 (Data Transfer Objects)
└── Migrations          (Controle de versão do banco de dados)
```

### Fluxo de Requisição
```
Requisição HTTP
    ↓
Controller (valida e extrai dados)
    ↓
Repository (acessa dados no banco)
    ↓
Database (SQL Server)
    ↓
Resposta JSON
```

---

## Tecnologias e Dependências

### Framework Principal
- **ASP.NET Core 6.0** - Framework web moderno
- **.NET 6.0** - Runtime

### Pacotes NuGet Instalados
| Pacote | Versão | Propósito |
|--------|--------|----------|
| Microsoft.AspNetCore.Authentication | 2.0 | Autenticação básica |
| Microsoft.AspNetCore.Authentication.JwtBearer | 6.0 | Autenticação JWT |
| Microsoft.EntityFrameworkCore | 7.0.4 | ORM para acesso a dados |
| Microsoft.EntityFrameworkCore.Design | 7.0.4 | Ferramentas de design |
| Microsoft.EntityFrameworkCore.SqlServer | 7.0.4 | Provider SQL Server |
| Swashbuckle.AspNetCore | 6.2.3 | Swagger/OpenAPI UI |

---

## Estrutura do Projeto

```
api-valheim/
├── Controllers/
│   ├── UserController.cs          # Autenticação (Signup/Login)
│   ├── PlayerController.cs        # Gerenciamento de jogadores
│   ├── CharacterController.cs     # Gerenciamento de personagens
│   └── ItemController.cs          # Gerenciamento de itens
│
├── Models/
│   ├── User.cs                    # Usuário (admin/auth)
│   ├── Player.cs                  # Jogador
│   ├── Character.cs               # Personagem
│   └── Item.cs                    # Item de inventário
│
├── Repository/
│   ├── Interfaces/
│   │   ├── IUserRepository.cs
│   │   ├── IPlayerRepository.cs
│   │   ├── ICharacterRepository.cs
│   │   └── IItemRepository.cs
│   ├── Context/
│   │   ├── IValheimContext.cs     # Interface do DbContext
│   │   └── ValheimContext.cs      # DbContext principal
│   ├── UserRepository.cs
│   ├── PlayerRepository.cs
│   ├── CharacterRepository.cs
│   └── ItemRepository.cs
│
├── Services/
│   └── TokenGenerator.cs           # Geração de tokens JWT
│
├── DTO/
│   └── TokenDTO.cs                 # DTOs para login/auth
│
├── Migrations/
│   ├── 20240606142002_InitialCreate.cs
│   ├── 20240606142002_InitialCreate.Designer.cs
│   └── ValheimContextModelSnapshot.cs
│
├── Properties/
│   └── launchSettings.json         # Configurações de execução
│
├── Program.cs                      # Configuração principal
├── appsettings.json                # Configurações gerais
├── appsettings.Development.json    # Configurações de desenvolvimento
├── api-valheim.csproj              # Arquivo de projeto
├── api-valheim.sln                 # Solution do Visual Studio
└── docker-compose.yml              # Configuração Docker
```

---

## Configuração e Instalação

### Pré-requisitos
- .NET 6.0 SDK instalado
- SQL Server instalado ou disponível
- Visual Studio Code ou Visual Studio

### Passos de Instalação

1. **Clone ou abra o projeto**
   ```bash
   cd c:\Users\avner\OneDrive\Área de Trabalho\api_valheim\valheim_api
   ```

2. **Restaure as dependências**
   ```bash
   dotnet restore
   ```

3. **Configure a string de conexão** (se necessário em `appsettings.json`)
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=SEU_SERVIDOR;Database=ValheimDb;..."
   }
   ```

4. **Execute as migrações do banco de dados**
   ```bash
   dotnet ef database update
   ```

5. **Execute o projeto**
   ```bash
   dotnet run
   ```

6. **Acesse o Swagger UI** (documentação interativa)
   ```
   https://localhost:5001/swagger/index.html
   ```

---

## Autenticação e Autorização

### Mecanismo de Segurança

A API utiliza **JWT (JSON Web Tokens)** com os seguintes componentes:

#### JWT Configuration
- **Algoritmo**: HMAC SHA-256
- **Chave Secreta**: `4d82a63bbdc67c1e4784edd6587f3730c`
- **Validade**: 4 dias após a emissão

#### Claims no Token
Cada token JWT contém:
- `Email` - Email do usuário
- `Name` - Nome do usuário
- `Role` - Papel/nível de acesso (Admin, User, etc)

### Políticas de Autorização

#### Policy: "levelA" (Admin)
```csharp
Requer: Email + Role = "Admin"
Acesso: Criar itens
```

#### Policy: "levelB" (User)
```csharp
Requer: Email + Role = "User"
Acesso: Criar personagens
```

### Fluxo de Autenticação

```
1. Usuário faz signup ou login
2. TokenGenerator.cs cria um JWT
3. JWT é retornado ao cliente
4. Cliente inclui token no header: Authorization: Bearer <token>
5. API valida o token
6. Se válido, requisição é processada com dados do token
```

---

## Rotas da API

### 📌 BASE URL
```
http://localhost:5000 (produção)
https://localhost:5001 (desenvolvimento)
```

---

### 👤 USER ENDPOINTS - Autenticação

#### 1. **Signup - Criar Novo Usuário**
```
POST /user/signup
Content-Type: application/json
Authorization: Nenhuma (público)
```

**Request Body:**
```json
{
  "name": "João Silva",
  "email": "joao@example.com",
  "password": "senha123",
  "access": "User"
}
```

**Response (201 Created):**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
```

**Status Codes:**
- `201 Created` - Usuário criado com sucesso
- `400 Bad Request` - Dados inválidos

---

#### 2. **Login - Autenticar Usuário**
```
POST /user/login
Content-Type: application/json
Authorization: Nenhuma (público)
```

**Request Body:**
```json
{
  "email": "joao@example.com",
  "password": "senha123"
}
```

**Response (200 OK):**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "email": "joao@example.com"
}
```

**Status Codes:**
- `200 OK` - Login bem-sucedido
- `401 Unauthorized` - Email ou senha incorretos

---

### 🎮 PLAYER ENDPOINTS - Gerenciamento de Jogadores

#### 1. **Criar Novo Jogador**
```
POST /players
Content-Type: application/json
Authorization: Bearer <token>
Requer Policy: "levelA" (Admin)
```

**Request Body:**
```json
{
  "username": "player_valheim",
  "email": "player@example.com",
  "password": "senha123",
  "dateJoined": "2024-06-06T14:20:02"
}
```

**Response (201 Created):**
```json
{
  "name": "João Silva",
  "email": "joao@example.com",
  "newPlayer": {
    "playerId": 1,
    "username": "player_valheim",
    "email": "player@example.com",
    "password": "senha123",
    "dateJoined": "2024-06-06T14:20:02"
  }
}
```

**Status Codes:**
- `201 Created` - Jogador criado
- `400 Bad Request` - Erro na criação
- `401 Unauthorized` - Token inválido/expirado
- `403 Forbidden` - Sem permissão (não é Admin)

---

#### 2. **Listar Todos os Jogadores**
```
GET /players
Content-Type: application/json
Authorization: Nenhuma (público)
```

**Response (200 OK):**
```json
[
  {
    "playerId": 1,
    "username": "player_valheim",
    "email": "player@example.com",
    "password": "senha123",
    "dateJoined": "2024-06-06T14:20:02",
    "characters": [
      {
        "characterId": 1,
        "name": "Lendário",
        "level": 50,
        "playerId": 1
      }
    ]
  }
]
```

**Status Codes:**
- `200 OK` - Lista retornada com sucesso

---

#### 3. **Deletar Jogador**
```
DELETE /players/{PlayerId}
Authorization: Nenhuma (público)
```

**Parameters:**
- `PlayerId` (path, required) - ID do jogador a deletar

**Response (204 No Content):**
```
(sem corpo)
```

**Status Codes:**
- `204 No Content` - Jogador deletado
- `400 Bad Request` - Erro ao deletar

---

### ⚔️ CHARACTER ENDPOINTS - Gerenciamento de Personagens

#### 1. **Criar Novo Personagem**
```
POST /characters
Content-Type: application/json
Authorization: Bearer <token>
Requer Policy: "levelB" (User)
```

**Request Body:**
```json
{
  "name": "Guerreiro Lendário",
  "level": 25,
  "playerId": 1
}
```

**Response (201 Created):**
```json
{
  "name": "João Silva",
  "character": {
    "characterId": 1,
    "name": "Guerreiro Lendário",
    "level": 25,
    "playerId": 1
  }
}
```

**Status Codes:**
- `201 Created` - Personagem criado
- `400 Bad Request` - Erro na criação
- `401 Unauthorized` - Token inválido/expirado
- `403 Forbidden` - Sem permissão (não é User)

---

#### 2. **Listar Todos os Personagens**
```
GET /characters
Authorization: Nenhuma (público)
```

**Response (200 OK):**
```json
[
  {
    "characterId": 1,
    "name": "Guerreiro Lendário",
    "level": 25,
    "playerId": 1,
    "player": {
      "playerId": 1,
      "username": "player_valheim",
      "email": "player@example.com"
    },
    "items": [
      {
        "itemId": 1,
        "itemName": "Espada Lendária",
        "itemDescription": "Uma espada poderosa",
        "itemType": "Weapon"
      }
    ]
  }
]
```

**Status Codes:**
- `200 OK` - Lista retornada

---

#### 3. **Deletar Personagem**
```
DELETE /characters/{CharacterId}
Authorization: Nenhuma (público)
```

**Parameters:**
- `CharacterId` (path, required) - ID do personagem a deletar

**Response (204 No Content):**
```
(sem corpo)
```

**Status Codes:**
- `204 No Content` - Personagem deletado
- `404 Not Found` - Personagem não encontrado

---

### 🎁 ITEM ENDPOINTS - Gerenciamento de Itens

#### 1. **Criar Novo Item**
```
POST /items
Content-Type: application/json
Authorization: Bearer <token>
Requer Policy: "levelA" (Admin)
```

**Request Body:**
```json
{
  "itemName": "Espada Lendária",
  "itemDescription": "Uma espada forjada pelos deuses",
  "itemType": "Weapon",
  "characterId": 1
}
```

**Response (201 Created):**
```json
{
  "item": {
    "itemId": 1,
    "itemName": "Espada Lendária",
    "itemDescription": "Uma espada forjada pelos deuses",
    "itemType": "Weapon",
    "characterId": 1
  },
  "name": "João Silva",
  "email": "joao@example.com"
}
```

**Status Codes:**
- `201 Created` - Item criado
- `400 Bad Request` - Erro na criação
- `401 Unauthorized` - Token inválido/expirado
- `403 Forbidden` - Sem permissão (não é Admin)

---

#### 2. **Listar Todos os Itens**
```
GET /items
Authorization: Nenhuma (público)
```

**Response (200 OK):**
```json
[
  {
    "itemId": 1,
    "itemName": "Espada Lendária",
    "itemDescription": "Uma espada forjada pelos deuses",
    "itemType": "Weapon",
    "characterId": 1,
    "character": {
      "characterId": 1,
      "name": "Guerreiro Lendário",
      "level": 25,
      "playerId": 1
    }
  }
]
```

**Status Codes:**
- `200 OK` - Lista retornada

---

#### 3. **Deletar Item**
```
DELETE /items/{ItemId}
Authorization: Nenhuma (público)
```

**Parameters:**
- `ItemId` (path, required) - ID do item a deletar

**Response (204 No Content):**
```
(sem corpo)
```

**Status Codes:**
- `204 No Content` - Item deletado
- `400 Bad Request` - Erro ao deletar

---

## Modelos de Dados

### 📊 Relacionamentos

```
User (Autenticação)
  ↓
Player (Jogador)
  ├─→ Character (Personagem)
        ├─→ Item (Item de Inventário)
```

---

### User (Autenticação)
```csharp
{
  Id: int (PK),
  Name: string,
  Email: string (único),
  Password: string (hash recomendado),
  Access: string (Admin, User, etc)
}
```

**Propósito**: Armazenar usuários para autenticação e geração de tokens JWT.

---

### Player (Jogador)
```csharp
{
  PlayerId: int (PK),
  Username: string,
  Email: string,
  Password: string,
  DateJoined: DateTime,
  Characters: ICollection<Character> (relação 1:N)
}
```

**Propósito**: Representar um jogador de Valheim com seus dados básicos.

---

### Character (Personagem)
```csharp
{
  CharacterId: int (PK),
  Name: string,
  Level: int,
  PlayerId: int (FK),
  Player: Player (relação N:1),
  Items: ICollection<Item> (relação 1:N)
}
```

**Propósito**: Representar um personagem criado por um jogador (um jogador pode ter múltiplos personagens).

---

### Item (Item de Inventário)
```csharp
{
  ItemId: int (PK),
  ItemName: string,
  ItemDescription: string,
  ItemType: string (Weapon, Armor, Consumable, etc),
  CharacterId: int (FK),
  Character: Character (relação N:1)
}
```

**Propósito**: Representar itens de inventário de um personagem.

---

## Como Utilizar as Rotas

### Exemplo Completo: Fluxo Básico

#### Passo 1: Criar um Usuário (Admin)
```bash
curl -X POST http://localhost:5000/user/signup \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Admin User",
    "email": "admin@example.com",
    "password": "admin123",
    "access": "Admin"
  }'
```

**Resposta:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
}
```

---

#### Passo 2: Fazer Login
```bash
curl -X POST http://localhost:5000/user/login \
  -H "Content-Type: application/json" \
  -d '{
    "email": "admin@example.com",
    "password": "admin123"
  }'
```

**Resposta:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "email": "admin@example.com"
}
```

---

#### Passo 3: Criar um Jogador (requer Admin)
```bash
curl -X POST http://localhost:5000/players \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..." \
  -d '{
    "username": "player1",
    "email": "player1@example.com",
    "password": "pass123",
    "dateJoined": "2024-06-06T14:20:02"
  }'
```

---

#### Passo 4: Criar um Usuário (User)
```bash
curl -X POST http://localhost:5000/user/signup \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Regular User",
    "email": "user@example.com",
    "password": "user123",
    "access": "User"
  }'
```

---

#### Passo 5: Criar um Personagem (requer User)
```bash
curl -X POST http://localhost:5000/characters \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <token_user>" \
  -d '{
    "name": "Meu Herói",
    "level": 1,
    "playerId": 1
  }'
```

---

#### Passo 6: Criar um Item (requer Admin)
```bash
curl -X POST http://localhost:5000/items \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer <token_admin>" \
  -d '{
    "itemName": "Espada de Bronze",
    "itemDescription": "Uma espada feita de bronze",
    "itemType": "Weapon",
    "characterId": 1
  }'
```

---

#### Passo 7: Listar Personagens
```bash
curl -X GET http://localhost:5000/characters
```

---

### Exemplo em Postman/Insomnia

1. **Abra Postman/Insomnia**

2. **Crie uma request POST** para `http://localhost:5000/user/login`
   - Headers: `Content-Type: application/json`
   - Body (raw):
   ```json
   {
     "email": "admin@example.com",
     "password": "admin123"
   }
   ```

3. **Copie o token** da resposta

4. **Crie uma request GET** para `http://localhost:5000/characters`
   - Headers:
     - `Content-Type: application/json`
     - `Authorization: Bearer <seu_token>`

---

## Desenvolvimento e Execução

### Executar o Projeto

```bash
# Restaurar dependências
dotnet restore

# Executar
dotnet run
```

A API estará disponível em:
- HTTP: `http://localhost:5000`
- HTTPS: `https://localhost:5001`
- Swagger UI: `https://localhost:5001/swagger/index.html`

---

### Migrações de Banco de Dados

```bash
# Ver migrações pendentes
dotnet ef migrations list

# Criar nova migração
dotnet ef migrations add "NomeDaMigracao"

# Atualizar banco de dados
dotnet ef database update

# Reverter para migração anterior
dotnet ef database update <NomeDaMigracao>

# Remover última migração
dotnet ef migrations remove
```

---

### Variáveis de Ambiente

Configure no `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=ValheimDB;..."
  },
  "Jwt": {
    "Secret": "sua-chave-secreta-aqui",
    "ExpiresInDays": 4
  }
}
```

---

### Docker

Para executar com Docker:

```bash
# Build da imagem
docker-compose build

# Iniciar containers
docker-compose up

# Parar containers
docker-compose down
```

---

## Dicas de Segurança

⚠️ **Importante para Produção**:

1. **Altere a chave JWT**: A chave atual (`4d82a63bbdc67c1e4784edd6587f3730c`) é apenas para desenvolvimento

2. **Use HTTPS**: Em produção, sempre use HTTPS

3. **Hash de Senhas**: Implemente bcrypt ou PBKDF2 para armazenar senhas com hash, não em texto plano

4. **CORS**: Configure CORS apropriadamente para seus clientes

5. **Rate Limiting**: Implemente rate limiting para proteção contra ataques

6. **Validação de Input**: Valide todos os dados de entrada

---

## Troubleshooting

### Erro: "Unable to resolve service"
**Solução**: Verifique se todos os Repositories estão registrados em `Program.cs`

### Erro: "The instance of entity type 'Player' cannot be tracked"
**Solução**: Isso geralmente ocorre ao tentar atualizar entidades. Use `.AsNoTracking()` ou `.Detach()`

### Erro: "Invalid token"
**Solução**: Certifique-se de:
- O token não está expirado (4 dias)
- A chave secreta está correta
- O formato é: `Authorization: Bearer <token>`

---

## Contato e Suporte

Para dúvidas sobre este projeto, consulte a documentação do ASP.NET Core ou as respectivas documentações dos pacotes utilizados.

---

**Última atualização**: Junho de 2024
**Versão da API**: 1.0.0
**Ambiente**: ASP.NET Core 6.0 | Entity Framework 7.0.4

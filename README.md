//Integrantes:
Guilherme Brasil - 22309720
Gabriel Rodrigues - 22310104


# 🧩 RPG_BD - API de Itens de RPG

API REST completa em *ASP.NET Core 9* com persistência *SQLite, validações automáticas, tratamento de erros e **interface gráfica interativa (Swagger)* para testar todos os endpoints.

---

## 🚀 Requisitos

Antes de rodar o projeto, verifique se possui instalado:

- *.NET 9 SDK*  
  Verifique com:
  bash
  dotnet --version
  
  Deve começar com 9.

- *(Opcional)* Ferramenta do Entity Framework CLI (para gerar migrations):
  bash
  dotnet tool install --global dotnet-ef
  

- *Editor*: Visual Studio Code, Visual Studio ou Rider.

- *(Opcional)* sqlite3 para visualizar o banco local items.db.

---

## ⚙️ Estrutura do Projeto

| Pasta / Arquivo | Descrição |
|------------------|------------|
| Models/Item.cs | Modelo com validações (Id, Name, Rarity, Price, CreatedAt) |
| Data/AppDbContext.cs | Configuração do banco SQLite e índice único em Name |
| Controllers/ItemsController.cs | CRUD completo com DTOs e respostas HTTP corretas |
| Middlewares/ErrorHandlingMiddleware.cs | Middleware global de erros (retorna JSON) |
| Filters/ValidationFilter.cs | Filtro de validação global do ModelState |
| Program.cs | Configuração principal, Swagger, HTTPS e abertura automática da interface |
| Properties/launchSettings.json | Define porta e URL padrão (https://localhost:5000) |
| Migrations/ | Pasta com migrations do banco de dados |
| appsettings.json | Configuração da conexão (Data Source=items.db) |

---

## 🧱 Instalação e Execução (passo a passo)

### 1️⃣ Abrir o terminal na pasta do projeto (onde está o .csproj)

### 2️⃣ Restaurar dependências
bash
dotnet restore


### 3️⃣ (Opcional) Instalar o Entity Framework CLI
bash
dotnet tool install --global dotnet-ef


### 4️⃣ Criar as migrations e atualizar o banco
bash
dotnet ef migrations add InitialCreate
dotnet ef database update


### 5️⃣ Executar o projeto
bash
dotnet run


---

## 🖥️ Interface Gráfica Automática (Swagger)

Quando você executa o comando:
bash
dotnet run


O terminal exibirá algo como:


info: Microsoft.Hosting.Lifetime[14]
      Now listening on: https://localhost:5000
Swagger disponível em: https://localhost:5000/swagger


### ✨ O que acontece:

- O projeto foi configurado para *abrir automaticamente o navegador* com a interface Swagger (https://localhost:5000/swagger).
- Essa interface exibe todos os endpoints da API e permite *testar requisições diretamente*, sem precisar do Postman.
- A documentação é gerada automaticamente pelo Swagger, com suporte a requisições GET, POST, PUT e DELETE.

Caso a aba não abra automaticamente, você pode acessar manualmente:

https://localhost:5000/swagger


---

## 🌐 Endpoints Principais

| Método | Rota | Descrição |
|--------|------|-----------|
| GET | /api/items | Lista todos os itens |
| GET | /api/items/{id} | Retorna um item específico |
| POST | /api/items | Cria um novo item |
| PUT | /api/items/{id} | Atualiza um item existente |
| DELETE | /api/items/{id} | Remove um item |

### 🧾 Exemplo de corpo (POST / PUT)
json
{
  "name": "Espada Curta",
  "rarity": "Common",
  "price": 12.5
}


---

## 💻 CLI Interno (modo terminal)

O projeto também possui um modo CLI simples. Para usá-lo:

bash
dotnet run -- --cli


### Comandos disponíveis:

list
add Name;Rarity;Price
get {id}
update {id}
delete {id}
exit


Exemplo:

add Espada Longa;Uncommon;50


---

## ⚡ Tratamento de Erros e Respostas HTTP

| Código | Descrição |
|--------|------------|
| *200* | Sucesso |
| *201* | Criado com sucesso |
| *204* | Atualizado / Deletado com sucesso (sem conteúdo) |
| *400* | Erro de validação (campos obrigatórios, formato incorreto) |
| *404* | Item não encontrado |
| *409* | Conflito (nome duplicado) |
| *500* | Erro interno (capturado pelo middleware de erro) |

O middleware ErrorHandlingMiddleware garante que *todas as exceções* sejam tratadas e exibidas em formato JSON, por exemplo:

json
{ "error": "Ocorreu um erro interno no servidor." }


---

## 🧠 Especificações Técnicas Implementadas

- Modelo Item com DataAnnotations ([Required], [StringLength], [Range]).
- Banco de dados *SQLite* integrado via *Entity Framework Core*.
- CRUD completo via ItemsController com DTOs e respostas adequadas.
- *Swagger* configurado e aberto automaticamente no navegador.
- *Middleware global de erros* retornando JSON.
- *Validação automática de modelos* via filtro global (ValidationFilter).
- *CORS e HTTPS* configurados.
- *Migrations* prontas para versionamento e entrega.

---

## 🧩 Problemas Comuns e Soluções

| Problema | Solução |
|-----------|----------|
| Failed to determine the https port for redirect | Verifique se launchSettings.json contém "https://localhost:5000" em applicationUrl |
| Swagger não abre automaticamente | Execute novamente dotnet run ou acesse manualmente https://localhost:5000/swagger |
| Banco não cria automaticamente | Execute dotnet ef database update |
| Nome duplicado retorna erro | Esperado — a API impede nomes repetidos (409 Conflict) |

---

## 📦 Entrega Recomendada

- Inclua este README.md no repositório.  
- Inclua a pasta Migrations/ gerada pelo EF.  
- Certifique-se de que appsettings.json possui:  
  json
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=items.db"
  }
  
- Rode dotnet run e verifique o funcionamento da interface Swagger.  
- (Opcional) Inclua prints ou vídeo mostrando o CRUD em funcionamento.

---

## 💬 Autor / Suporte

Projeto desenvolvido para a disciplina de *Desenvolvimento de Sistemas*.  
Em caso de dúvidas, revise este README ou consulte o Swagger em:

👉 *[https://localhost:5000/swagger](https://localhost:5000/swagger)*

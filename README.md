# 🎟️ Ticket API avec Authentification JWT

Ce projet est une **API REST** développée en **ASP.NET Core** avec **Entity Framework Core** et **SQL Server**.  
Elle permet de gérer des **tickets** attachés à des **utilisateurs**, avec une authentification sécurisée via **JWT**.

---

## 🛠️ Technologies utilisées

- .NET 8 ou .NET 9
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server (local ou distant)
- JWT Bearer Authentication
- Swagger / OpenAPI

---

## 📦 Fonctionnalités

- 🔐 Authentification via JWT (login/register)
- 👤 Gestion des utilisateurs (CRUD)
- 🎫 Gestion des tickets (CRUD)
- 🔗 Association utilisateur ↔ tickets
- 🧪 Swagger intégré pour tester les routes
- 🗂️ Architecture propre : séparation Controllers / Services / Models

---

## 🧱 Modèle de données

### 👤 User
```csharp
public class User {
    public int Id { get; set; }
    public string Username { get; set; }
    public string PasswordHash { get; set; }
    public ICollection<Ticket> Tickets { get; set; }
}
```

### 🎫 Ticket
```csharp
public class Ticket {
    public int Id { get; set; }
    public string Title { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
}
```

---

## 🚀 Démarrage rapide

### ✅ Prérequis

- [.NET SDK 8 ou 9](https://dotnet.microsoft.com/)
- SQL Server (localdb ou instance réelle)
- (Facultatif) [Postman](https://www.postman.com/) pour tester les endpoints

### ⚙️ Configuration

Dans `appsettings.json`, configure la chaîne de connexion :
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\mssqllocaldb;Database=TicketDb;Trusted_Connection=True;"
}
```

### ▶️ Lancer le projet

```bash
git clone https://github.com/PatoucheH/TicketApi.git
cd TicketApi
dotnet ef database update
dotnet run
```

---

## 🔐 Authentification JWT

### Enregistrement
```http
POST /auth/register
```
```json
{
  "username": "testuser",
  "password": "Test1234"
}
```

### Connexion
```http
POST /auth/login
```
```json
{
  "username": "testuser",
  "password": "Test1234"
}
```

Tu recevras un **JWT token** :
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR..."
}
```

Utilise ce token dans Swagger ou Postman :
```
Authorization: Bearer <votre_token>
```

---

## 📬 Endpoints principaux

### 🧾 Auth
- `POST /auth/register` — Créer un utilisateur
- `POST /auth/login` — Obtenir un JWT token

### 👤 Users
- `GET /users`
- `GET /users/{id}`
- `POST /users`
- `PUT /users/{id}`
- `DELETE /users/{id}`

### 🎫 Tickets
- `GET /tickets`
- `GET /tickets/{id}`
- `POST /tickets`
- `PUT /tickets/{id}`
- `DELETE /tickets/{id}`

---

## 📂 Architecture du projet

```
TicketApi/
│
├── Controllers/       → API endpoints
├── Models/            → Entités EF Core
├── Services/          → Logique métier (UserService, TicketService)
├── Data/              → ApplicationDbContext, migrations
├── Validators/        → Validateurs pour les DTOs
├── Program.cs         → Configuration Minimal API
└── appsettings.json   → Connexions et clés JWT
```

---

## 🧰 Outils de développement

- `dotnet ef migrations add InitialCreate` — Ajouter une migration
- `dotnet ef database update` — Appliquer les migrations
- `dotnet watch run` — Lancer en mode dev avec rafraîchissement

---

## 👨‍💻 Auteur

Développé par [PatoucheH](https://github.com/PatoucheH)

---

## 🔗 Lien du dépôt

[👉 https://github.com/PatoucheH/TicketApi](https://github.com/PatoucheH/TicketApi)

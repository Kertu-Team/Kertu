# Kertu – Project Management and Task Organization

Kertu is a modern web application designed for project management, task organization, and team collaboration. With its intuitive interface and rich functionality.

---

## 🔧 **Features**
- **Tree View Navigation:**
  - Add, edit, and delete elements such as cards, lists, or boards.
  - Dynamic mapping of child data for elements (Board Children Data Mapping).
  - Context-specific menu based on the type of element.
  - Open boards directly from the tree view.

- **User Account Management:**
  - Change email address.
  - Reset and change passwords.
  - Support for two-factor authentication (2FA).

- **Boards, Lists, and Cards:**
  - Create and edit boards, lists, and cards.
  - Drag-and-drop functionality for easy reorganization.
  - Ability to add comments and attachments to cards.

- **Search**

- **Scheduled Processes:**
  - Automatic branch merging with success or failure notifications.

---

## 🛠️ **Technologies**
- **App:** ASP.NET Core Blazor Server, Entity Framework Core
- **Database:** PostgreSQL
- **CI/CD:** Docker, Docker Compose, GitHub Actions
- **Additional Tools:** CSharpier, Radzen, Watchtower

---

## 🚀 **Getting Started**

### Requirements:
- Docker and Docker Compose
- .NET 8
- PostgreSQL

### Steps:
1. **Clone the repository:**
   ```bash
   git clone https://github.com/TwojProjekt/kertu.git
   cd kertu

2. **Run locally (Dev):**
```bash
  docker-compose up --build
```
3. **Run in production:**
```bash
  docker-compose -f docker-compose.prod.yml up --build -d
```
4. **Verify the application:**
- The application should be available at http://localhost:5000 (locally) or at the configured production URL.

🧪 **Testing**
**Automated QA tests:**

Tests/*

💡 **Additional Information**
- For any questions or bug reports, please use the Issues section.

🤝 **Contributors**
- This project is developed by the Kertu Team.
- We welcome collaboration and suggestions for improvements!

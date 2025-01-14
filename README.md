# Kertu – Project Management and Task Organization

Kertu is a modern web application designed for project management, task organization, and team collaboration. With its intuitive interface and rich functionality, Kertu supports both teams and individual users.

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

- **Search:**
  - Search bar integrated with Google Ads.
  - Dynamic results and ad suggestions.

- **Kubernetes Integration:**
  - Deployment to Azure servers using Kubernetes Service.

- **Scheduled Processes:**
  - Automatic branch merging with success or failure notifications.

---

## 🛠️ **Technologies**
- **Backend:** ASP.NET Core Blazor, Entity Framework Core
- **Frontend:** Blazor WebAssembly
- **Database:** PostgreSQL
- **CI/CD:** Docker, Docker Compose, Kubernetes, GitHub Actions
- **Additional Tools:** CSharpier, Radzen, Watchtower

---

## 📂 **Project Structure**
- `Frontend/`: Source code for the user interface.
- `Backend/`: Server-side logic and data models.
- `Database/`: SQL scripts for database management.
- `Deployments/`: YAML files for Kubernetes deployments.
- `Tests/`: Automated QA tests.

---

## 🚀 **Getting Started**

### Requirements:
- Docker and Docker Compose
- .NET 6 SDK
- PostgreSQL
- Kubernetes (optional)

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
- Aplikacja powinna być dostępna na http://localhost:5000 (lokalnie) lub na skonfigurowanym URL produkcyjnym.

🧪 Testing
**Automated QA tests:**
```bash
dotnet test Tests/
```
Manual Testing:
- Verify the operation of key features:
 - Tree view navigation.
 - User account management.
 - Search functionality.

🌐 Deployment on Azure Servers
**1.Prepare AKS (Azure Kubernetes Service):**
```bash
az aks create --resource-group RG_NAME --name CLUSTER_NAME --node-count 3
```
**2.Copy the YAML files from the Deployments/ folder.**
**3.Execute the deployment:**
```bash
kubectl apply -f deployments/
```
**4.Verify cluster status:**
```bash
kubectl get pods
```
💡 Additional Information
- For any questions or bug reports, please use the Issues section.
- Technical documentation can be found in the docs/ folder.

🤝 Contributors
This project is developed by the Kertu Team.
We welcome collaboration and suggestions for improvements!

# How to run project for development purposes

1. Create .env file in the project root directory with the following content:

```env
POSTGRES_DB=kertu
POSTGRES_USER=postgres
POSTGRES_PASSWORD=password
```

2. Open terminal in the project root directory
3. Run the following command:

```bash
docker-compose up --build postgres -d
```

4. In visual studio, run the project with debug mode and http profile

# How to run project for development purposes

1. Open terminal in the project root directory
2. Run the following command:

```bash
docker-compose up postgres
```

3. In visual studio, run the project with debug mode and http profile

## Issues

If you need to rebuild the container, run the following command:

```bash
docker-compose up --build postgres
```
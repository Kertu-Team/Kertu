import os
import subprocess
import requests

# Configuration
FILE_URLS = [
    "https://raw.githubusercontent.com/Kertu-Team/Kertu/main/docker-compose.yml",
    "https://raw.githubusercontent.com/Kertu-Team/Kertu/main/docker-compose.prod.yml",
]
ENV_FILE = ".env"
DEFAULT_ENV_VARS = {
    "POSTGRES_DB": "kertu",
    "POSTGRES_USER": "postgres",
    "POSTGRES_PASSWORD": "password",
}


def download_file(url):
    """Always download and overwrite the file."""
    filename = url.split("/")[-1]  # Extract file name from URL
    print(f"Downloading {filename}...")
    response = requests.get(url)
    response.raise_for_status()
    with open(filename, "wb") as f:
        f.write(response.content)
    print(f"{filename} downloaded.")


def configure_env():
    """Create or update the .env file with required variables."""
    print("Configuring environment variables...")
    existing_vars = {}
    if os.path.exists(ENV_FILE):
        with open(ENV_FILE, "r") as f:
            existing_vars = {
                line.split("=", 1)[0]: line.split("=", 1)[1].strip()
                for line in f
                if "=" in line and not line.startswith("#")
            }

    with open(ENV_FILE, "a") as f:
        for key, default in DEFAULT_ENV_VARS.items():
            if key not in existing_vars:
                value = (
                    input(f"Enter value for {key} [default: {default}]: ").strip()
                    or default
                )
                f.write(f"{key}={value}\n")
    print(".env file updated.")


def restart_docker():
    """Restart the application using Docker Compose."""
    print("Restarting application...")
    try:
        subprocess.run(["docker", "compose", "down"], check=True)
        subprocess.run(["docker", "compose", "up", "-d"], check=True)
        print("Application restarted successfully.")
    except subprocess.CalledProcessError as e:
        print(f"Error restarting application: {e}")
        raise


def main():
    print("=== Deployment Started ===")

    # Always overwrite Docker Compose files
    for url in FILE_URLS:
        download_file(url)

    # Configure environment variables
    configure_env()

    # Restart the application
    restart_docker()

    print("=== Deployment Completed ===")


if __name__ == "__main__":
    main()

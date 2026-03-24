# graphdb-linking-assets

ASP.NET Core Web API that manages domain asset linking using Azure Cosmos DB (Gremlin API). Secrets are resolved at startup from Azure Key Vault via `DefaultAzureCredential`.

---

## Build

### Prerequisites

- [Docker](https://docs.docker.com/get-docker/)
- [Azure CLI](https://learn.microsoft.com/en-us/cli/azure/install-azure-cli)
- Contributor access to an existing Azure Container Registry (ACR)

### Variables

```bash
ACR_NAME=<your-acr-name>        # e.g. myregistry
TAG=latest                       # or a specific version / git SHA
IMAGE=$ACR_NAME.azurecr.io/graphdb-linking-assets:$TAG
```

### Build and push

```bash
# Authenticate Docker to ACR
az acr login --name $ACR_NAME

# Build image (run from repo root)
docker build -f infra/Dockerfile -t $IMAGE .

# Push to ACR
docker push $IMAGE
```

> **ACR login fails with `docker-credential-secretservice not found`?**
> This happens in headless / dev container environments where no GUI secret store is available.
> Fix it by clearing the Docker credential helper config:
> ```bash
> echo '{}' > ~/.docker/config.json
> az acr login --name $ACR_NAME
> ```
> Docker will then store the token directly in `~/.docker/config.json` (safe for personal dev environments).

> The Dockerfile uses a multi-stage build. The final image is based on
> `mcr.microsoft.com/dotnet/aspnet:10.0` and listens on **port 80**.

---

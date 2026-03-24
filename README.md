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

> The Dockerfile uses a multi-stage build. The final image is based on
> `mcr.microsoft.com/dotnet/aspnet:10.0` and listens on **port 80**.

---

## Deploy to Azure Container Apps

```bash
az containerapp create \
  --name graphdb-linking-assets \
  --resource-group <resource-group> \
  --environment <container-app-environment> \
  --image $IMAGE \
  --registry-server $ACR_NAME.azurecr.io \
  --target-port 80 \
  --ingress external \
  --env-vars KeyVaultUri=https://<keyvault-name>.vault.azure.net/ \
             Gremlin__Hostname=<cosmos-account-name> \
             Gremlin__Database=<db-name> \
             Gremlin__Collection=<graph-name>
```

### Key Vault access via Managed Identity

The app uses `DefaultAzureCredential`, which automatically resolves to the
Container App's managed identity when running in Azure.

```bash
# 1. Enable system-assigned managed identity on the Container App
az containerapp identity assign \
  --system-assigned \
  --name graphdb-linking-assets \
  --resource-group <resource-group>

# 2. Capture the principal ID
PRINCIPAL_ID=$(az containerapp identity show \
  --name graphdb-linking-assets \
  --resource-group <resource-group> \
  --query principalId -o tsv)

# 3. Grant the identity read access to Key Vault secrets
az role assignment create \
  --role "Key Vault Secrets User" \
  --assignee $PRINCIPAL_ID \
  --scope $(az keyvault show --name <keyvault-name> --query id -o tsv)
```

No code changes are required — `DefaultAzureCredential` picks up the managed
identity automatically.

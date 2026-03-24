source source.sh

az group create --name "$rg_name" --location "$location"

az cosmosdb create --resource-group "$rg_name" --name "$db_account_name" --locations "regionName=$location" --capabilities "EnableGremlin"

az cosmosdb gremlin database create --resource-group "$rg_name" --account-name "$db_account_name" --name "$db_name"

# using tenant Id as partiion key allows for multi tennency but kepps data from tenenats together for faster traversals
az cosmosdb gremlin graph create --resource-group "$rg_name" --account-name "$db_account_name" --database-name "$db_name" --name "$graph_name" --partition-key-path "/tenantId"
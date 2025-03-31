npm install -g azure-functions-core-tools@4 --unsafe-perm true

func init --worker-runtime dotnet-isolated --target-framework net8.0

func new --name HelloWorld --template "HttpTrigger" --authlevel "anonymous"

http://localhost:5094/api/azure-functions
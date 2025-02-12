param location string

resource servicePlan 'Microsoft.Web/serverfarms@2024-04-01' = {
  location: location
  name: 'uowpln${uniqueString(resourceGroup().id)}'
  sku: {
    name: 'Y1'
    tier: 'dynamic'
  }
}

@description('function language')
param function_runtime string = 'dotnet-isolated'

param functionName string = 'uowfunc${uniqueString(resourceGroup().id)}'

resource function 'Microsoft.Web/sites@2024-04-01' = {
  location: location
  name: functionName

  kind: 'functionapp'
identity:{
  type: 'SystemAssigned'
}

  properties: {
    serverFarmId: servicePlan.id
    siteConfig: {
      alwaysOn: true
      
      appSettings: [
        {
          name: 'FUNCTIONS_WORKER_RUNTIME'
          value: function_runtime
        }

        {
          name: 'FUNCTIONS_EXTENSIONS_VERSION'
          value: '~4'
        }
      ]
    }
    httpsOnly: true
  }
}

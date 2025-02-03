param location string

param vaultName string = 'vault${uniqueString(resourceGroup().id)}'

resource keyVault 'Microsoft.KeyVault/vaults@2023-07-01' = {
  location: location
  name: vaultName
  properties: {
    sku: {
      name: 'standard'
      family: 'A'
    }

    tenantId: subscription().tenantId
    accessPolicies: []
  }
}

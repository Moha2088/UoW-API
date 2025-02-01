@description('Name of the storageaccount')
param storageName string = 'UoW-${uniqueString(resourceGroup().id)}'

@description('Storage redundancy type')
@allowed([
  'Standard_LRS'
])
param skuName string = 'Standard_LRS'

param location string

resource storageAcc 'Microsoft.Storage/storageAccounts@2023-05-01' = {
  kind: 'BlobStorage'
  location: location
  name: storageName
  
  sku: {
    name: skuName
  }
}

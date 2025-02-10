@description('Location of the resource group')
param location string

@description('Cache name')
param redisCacheName string = 'uow${uniqueString(resourceGroup().id)}'

@description('SKU name')
@allowed(['Standard'])
param skuName string = 'Standard'

@description('Cache capacity')
@allowed([0])
param capacity int = 0

@description('SKU family')
@allowed(['C'])
@maxLength(1)
param family string = 'C'

resource redisCache 'Microsoft.Cache/redis@2024-11-01' = {
  location: location
  name: redisCacheName
  properties: {
    sku: {
      capacity: capacity
      family: family
      name: skuName
    }

    redisConfiguration:{
      'aad-enabled': null
    }
  }
}

targetScope = 'resourceGroup'

@description('Location of the resourcegroup')
param location string = resourceGroup().location

module storage 'Modules/Storage.Bicep' = {
  name: 'Storage'
  params: {
    location: location
  }
}

module keyVault 'Modules/KeyVault.Bicep' = {
  name: 'KeyVault'
  params: {
    location: location
  }
}

module function 'Modules/Function.bicep' ={
  name: 'Function'
  params:{
    location: location
  }
}

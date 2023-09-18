param privateDnsZoneName string
param privateDnsZoneLinkName string
param vnetId string
param privateEndpointName string
param subnetId string
param resourceIds string[]
param groupIds string[]
param location string
param tags object

resource privateDnsZone 'Microsoft.Network/privateDnsZones@2020-06-01' = {
  name: privateDnsZoneName
  tags: tags
  location: 'global'

  resource link 'virtualNetworkLinks' = {
    name: privateDnsZoneLinkName
    tags: tags
    location: 'global'
    properties: {
      registrationEnabled: false
      virtualNetwork: { id: vnetId }
    }
  }
}

module privateEndpoints 'PrivateEndpoint.bicep' = [for (item, index) in resourceIds: {
  name: '${privateDnsZoneName}-links-${index}'
  params: {
    location: location
    tags: tags
    groupIds: groupIds
    privateDnsZoneId: privateDnsZone.id
    privateEndpointName: privateEndpointName
    resourceId: item
    subnetId: subnetId
  }
}]

param location string

param tags object

param privateEndpointName string

param subnetId string

param resourceId string

param privateDnsZoneId string

param groupIds string[]

var uniqueId = uniqueString(privateEndpointName, resourceId)

resource privateEndpoint 'Microsoft.Network/privateEndpoints@2023-05-01' = {
  name: '${privateEndpointName}-${uniqueId}'
  tags: tags
  location: location
  properties: {
    customNetworkInterfaceName: '${privateEndpointName}-${uniqueId}-nic'
     subnet: { id: subnetId }
     privateLinkServiceConnections: [
      {
        name: '${privateEndpointName}-${uniqueId}-link'
        properties: {
          groupIds: groupIds
          privateLinkServiceId: resourceId
        }
      }
     ]
  }
}

resource privateEndpointDns 'Microsoft.Network/privateEndpoints/privateDnsZoneGroups@2023-05-01' = {
  name: '${privateEndpointName}-${uniqueId}-dns'
  parent: privateEndpoint
  properties: {
    privateDnsZoneConfigs: [
      {
        name: '${privateEndpointName}-${uniqueId}-dns-config'
        properties: {
          privateDnsZoneId: privateDnsZoneId
        }
      }
    ]
  }
}

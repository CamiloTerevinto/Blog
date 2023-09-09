param location string = resourceGroup().location

param tags object

param vnetId string

param subnetId string

param privateEndpointPrefixName string

param privateEndpointsData array

module privateDnsZones 'Components/PrivateDnsZone.bicep' = [for item in privateEndpointsData: {
  name: item.privateDnsZoneName
  params: {
    tags: tags
    privateDnsZoneLinkName: item.privateDnsZoneLinkName
    privateDnsZoneName: item.privateDnsZoneName
    vnetId: vnetId
    groupIds: item.groupIds
    location: location
    privateEndpointName: privateEndpointPrefixName
    resourceIds: item.resourceIds
    subnetId: subnetId
  }
}]

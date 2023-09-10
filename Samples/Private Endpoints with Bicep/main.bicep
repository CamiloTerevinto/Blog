param vnetId string
param subnetId string
param privateEndpointPrefixName string
param privateEndpointsData array
param location string = resourceGroup().location
param tags object

module privateDnsZones 'Components/PrivateDnsZone.bicep' = [for item in privateEndpointsData: {
  name: item.privateDnsZoneName
  params: {
    privateDnsZoneLinkName: item.privateDnsZoneLinkName
    privateDnsZoneName: item.privateDnsZoneName
    vnetId: vnetId
    groupIds: item.groupIds
    privateEndpointName: privateEndpointPrefixName
    resourceIds: item.resourceIds
    subnetId: subnetId
    location: location
    tags: tags
  }
}]

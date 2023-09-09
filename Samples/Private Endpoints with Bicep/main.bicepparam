using './main.bicep'

param location = ''
param vnetId = ''
param subnetId = ''
param privateEndpointPrefixName = ''
param privateEndpointsData = [
  {
    privateDnsZoneName: 'privatelink.blob.core.windows.net'
    privateDnsZoneLinkName: 'ct-conn-uks-vnetlink-blob'
    resourceIds: [ 'YourBlobStorageAccountId' ]
    groupIds: [ 'blob' ]
  }
  {
    privateDnsZoneName: 'privatelink.vaultcore.azure.net'
    privateDnsZoneLinkName: 'ct-conn-uks-vnetlink-kv'
    resourceIds: [ 'YourKeyVaultId1', 'YourKeyVaultId2' ]
    groupIds: [ 'vault' ]
  }
]

param tags = {}

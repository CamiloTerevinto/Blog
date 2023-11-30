@description('Resource group location')
param rgLocation string

@description('VNET resource name')
param vnetName string

@description('VNET Address Space (CIDR notation, /23 or greater)')
param vnetAddressSpace string

@description('Subnet resource name')
param containerAppSubnetName string

@description('Subnet Address Prefix (CIDR notation, /23 or greater)')
param containerAppSubnetAddressPrefix string

resource vnet 'Microsoft.Network/virtualNetworks@2023-05-01' = {
  name: vnetName
  location: rgLocation
  properties: {
    addressSpace: {
      addressPrefixes: [ vnetAddressSpace ]
    }
  }

  resource subnet 'subnets' = {
    name: containerAppSubnetName
    properties: {
      addressPrefix: containerAppSubnetAddressPrefix
      serviceEndpoints: [
        {
          service: 'Microsoft.Storage'
          locations: [ rgLocation ]
        }
      ]
    }
  }
}

output vnetId string = vnet.id
output subnetId string = vnet.properties.subnets[0].id

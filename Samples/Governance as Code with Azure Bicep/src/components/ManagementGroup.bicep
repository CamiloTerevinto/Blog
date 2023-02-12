targetScope = 'tenant'

@description('The ID of the parent MG')
param parentManagementGroupId string

@description('The name of the MG')
param name string

resource managementGroup 'Microsoft.Management/managementGroups@2021-04-01' = {
  name: name
  scope: tenant()
  properties: {
    details: {
      parent: {
        id: parentManagementGroupId
      }
    }
    displayName: name
  }
}

output id string = managementGroup.id

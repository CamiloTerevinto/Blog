targetScope = 'managementGroup'

resource policySetDefinition 'Microsoft.Authorization/policyDefinitions@2021-06-01' existing = {
  name: '89c6cddc-1c73-4ac1-b19c-54d1a15a42f2'
}

output policyDefinitionId string = policySetDefinition.id

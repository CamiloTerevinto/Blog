targetScope = 'subscription'

@description('The ID of the policy definition to assign')
param policyDefinitionId string

@description('The name of the policy assignment')
param policyName string

@description('The description of the policy assignment')
param policyDescription string

resource policyAssignment 'Microsoft.Authorization/policyAssignments@2022-06-01' = {
  name: policyName
  properties: {
    policyDefinitionId: policyDefinitionId
    description: policyDescription
  }
}

targetScope = 'tenant'

@description('The billing scope for this subscription')
param billingScope string

@description('The ID of the management group that is the parent of the subscription')
param parentManagementGroupdId string

module productionMG '../components/ManagementGroup.bicep' = {
  name: 'ProductionMG'
  params: {
    name: 'Production'
    parentManagementGroupId: parentManagementGroupdId
  }
}

module iso27001Policy '../policies/iso27001.bicep' = {
  scope: managementGroup('Production')
  name: 'ISO27001-PolicyReference'
  dependsOn: [
    productionMG
  ]
}

module policyAssignment '../components/ManagementGroupPolicyAssignment.bicep' = {
  scope: managementGroup('Production')
  name: 'ISO27001-PolicyAssignment'
  params: {
    policyDefinitionId: iso27001Policy.outputs.policyDefinitionId
    policyDescription: 'Regulatory Compliance'
    policyName: 'ISO27001'
  }
  dependsOn: [
    productionMG
  ]
}

module externalMG 'Production/Production-external.bicep' = {
  name: 'ProductionExternalMG'
  params: {
    billingScope: billingScope
    parentManagementGroupdId: productionMG.outputs.id
  }
  dependsOn: [
    productionMG
  ]
}

module internalMG 'Production/Production-internal.bicep' = {
  name: 'ProductionInternalMG'
  params: {
    billingScope: billingScope
    parentManagementGroupdId: productionMG.outputs.id
  }
  dependsOn: [
    productionMG
  ]
}

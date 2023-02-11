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

module externalMG 'Production/Production-external.bicep' = {
  name: 'ProductionExternalMG'
  params: {
    billingScope: billingScope
    parentManagementGroupdId: productionMG.outputs.id
  }
}

module internalMG 'Production/Production-internal.bicep' = {
  name: 'ProductionInternalMG'
  params: {
    billingScope: billingScope
    parentManagementGroupdId: productionMG.outputs.id
  }
}

targetScope = 'tenant'

@description('The billing scope for this subscription')
param billingScope string

@description('The ID of the management group that is the parent of the subscription')
param parentManagementGroupdId string

module productionExternalMg '../../components/ManagementGroup.bicep' = {
  name: 'ProductionExternalMg'
  params: {
    name: 'ProductionExternal'
    parentManagementGroupId: parentManagementGroupdId
  }
}

module client1 '../../components/Subscription.bicep' = {
  name: 'Client1Sub'
  scope: managementGroup('ProductionExternal')
  params: {
    billingScope: billingScope
    parentManagementGroupdId: productionExternalMg.outputs.id
    subscriptionName: 'Client1Sub'
    subscriptionWorkload: 'Production'
  }
  dependsOn: [
    productionExternalMg
  ]
}

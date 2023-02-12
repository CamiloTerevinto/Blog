targetScope = 'tenant'

@description('The billing scope for this subscription')
param billingScope string

@description('The ID of the management group that is the parent of the subscription')
param parentManagementGroupdId string

module productionInternalMg '../../components/ManagementGroup.bicep' = {
  name: 'ProductionInternalMg'
  params: {
    name: 'ProductionInternal'
    parentManagementGroupId: parentManagementGroupdId
  }
}

module product1 '../../components/Subscription.bicep' = {
  name: 'Product1Sub'
  scope: managementGroup('ProductionInternal')
  params: {
    billingScope: billingScope
    parentManagementGroupdId: productionInternalMg.outputs.id
    subscriptionName: 'Product1Sub'
    subscriptionWorkload: 'Production'
  }
  dependsOn: [
    productionInternalMg
  ]
}

module product2 '../../components/Subscription.bicep' = {
  name: 'Product2Sub'
  scope: managementGroup('ProductionInternal')
  params: {
    billingScope: billingScope
    parentManagementGroupdId: productionInternalMg.outputs.id
    subscriptionName: 'Product2Sub'
    subscriptionWorkload: 'Production'
  }
  dependsOn: [
    productionInternalMg
  ]
}

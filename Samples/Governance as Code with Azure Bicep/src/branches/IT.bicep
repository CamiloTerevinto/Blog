targetScope = 'tenant'

@description('The billing scope for this subscription')
param billingScope string

@description('The ID of the management group that is the parent of the subscription')
param parentManagementGroupdId string

module itMG '../components/ManagementGroup.bicep' = {
  name: 'ITMG'
  params: {
    name: 'IT'
    parentManagementGroupId: parentManagementGroupdId
  }
}

module networkSub '../components/Subscription.bicep' = {
  name: 'NetworkSub'
  params: {
    billingScope: billingScope
    parentManagementGroupdId: itMG.outputs.id
    subscriptionName: 'NetworkSub'
    subscriptionWorkload: 'Production'
  }
}

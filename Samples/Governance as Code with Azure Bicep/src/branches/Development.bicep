targetScope = 'tenant'

@description('The billing scope for this subscription')
param billingScope string

@description('The ID of the management group that is the parent of the subscription')
param parentManagementGroupdId string

module developmentMG '../components/ManagementGroup.bicep' = {
  name: 'DevelopmentMG'
  params: {
    name: 'Development'
    parentManagementGroupId: parentManagementGroupdId
  }
}

module devSub1 '../components/Subscription.bicep' = {
  name: 'DevSub1'
  params: {
    billingScope: billingScope
    parentManagementGroupdId: developmentMG.outputs.id
    subscriptionName: 'DevSub1'
    subscriptionWorkload: 'DevTest'
  }
}

module devSub2 '../components/Subscription.bicep' = {
  name: 'DevSub2'
  params: {
    billingScope: billingScope
    parentManagementGroupdId: developmentMG.outputs.id
    subscriptionName: 'DevSub2'
    subscriptionWorkload: 'DevTest'
  }
}

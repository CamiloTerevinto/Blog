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

module europeNorthRegionLock '../policies/RegionLock.bicep' = {
  scope: managementGroup(developmentMG.name)
  name: 'DevSub1 Europe North lock'
  params: {
    allowedRegion: 'EuropeNorth'
  }
}

module ukSouthRegionLock '../policies/RegionLock.bicep' = {
  scope: managementGroup(developmentMG.name)
  name: 'DevSub2 UK South lock'
  params: {
    allowedRegion: 'UKSouth'
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

module devSub1Scoped 'Subscriptions/DevSub1.bicep' = {
  name: 'DevSub1ScopedResources'
  params: {
    subscriptionId: devSub1.outputs.subscriptionId
    policies: [
      {
        Name: ukSouthRegionLock.name
        DefinitionId: ukSouthRegionLock.outputs.policyDefinitionId
        Description: 'UK South regional lock'
      }
    ]
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

module devSub2Scoped 'Subscriptions/DevSub2.bicep' = {
  name: 'DevSub2ScopedResources'
  params: {
    subscriptionId: devSub2.outputs.subscriptionId
    policies: [
      {
        Name: europeNorthRegionLock.name
        DefinitionId: europeNorthRegionLock.outputs.policyDefinitionId
        Description: 'Europe North regional lock'
      }
    ]
  }
}

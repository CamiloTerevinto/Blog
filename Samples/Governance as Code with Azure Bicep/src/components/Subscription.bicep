targetScope = 'managementGroup'

@description('The billing scope for this subscription')
param billingScope string

@description('The ID of the management group that is the parent of the subscription')
param parentManagementGroupdId string

@description('Name of the subscription')
param subscriptionName string

@description('Workload type for the subscription')
@allowed([
    'Production'
    'DevTest'
])
param subscriptionWorkload string

resource subscription 'Microsoft.Subscription/aliases@2021-10-01' = {
  name: subscriptionName
  scope: tenant()
  properties: {
    workload: subscriptionWorkload
    displayName: subscriptionName
    billingScope: billingScope
    additionalProperties: {
      managementGroupId: parentManagementGroupdId
    }
  }
}

output subscriptionId string = subscription.id

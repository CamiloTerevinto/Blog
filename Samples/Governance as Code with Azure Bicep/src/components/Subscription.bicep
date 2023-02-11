targetScope = 'tenant'

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

resource subscription 'Microsoft.Subscription/aliases@2020-09-01' = {
  name: subscriptionName
  properties: {
    workload: subscriptionWorkload
    displayName: subscriptionName
    billingScope: billingScope
  }
}

resource subscriptionAssignment 'Microsoft.Management/managementGroups/subscriptions@2021-04-01' = {
  name: '${parentManagementGroupdId}/${subscription.id}'
}

output subscriptionId string = subscription.id

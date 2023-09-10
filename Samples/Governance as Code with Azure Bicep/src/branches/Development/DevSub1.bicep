targetScope = 'tenant'

@description('The ID of the subscription')
param subscriptionId string

@description('The list of policies to apply to the subscription')
param policies array

module policiesAssignmentsLoop '../../components/SubscriptionPolicyAssignment.bicep' = [for policy in policies: {
  name: '${policy.Name}-Assignment'
  scope: subscription(subscriptionId)
  params: {
    policyDefinitionId: policy.DefinitionId
    policyName: policy.Name
    policyDescription: policy.Description
  }
}]

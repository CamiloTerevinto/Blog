targetScope = 'tenant'

param rootManagementGroupName string

param billingAccount string

param enrollmentAccount string

var billingScope = tenantResourceId('Microsoft.Billing/billingAccounts/enrollmentAccounts', billingAccount, enrollmentAccount)

resource rootMG 'Microsoft.Management/managementGroups@2021-04-01' existing = {
  name: rootManagementGroupName
}

module itBranch 'branches/IT.bicep' = {
  name: 'IT'
  params: {
    billingScope: billingScope
    parentManagementGroupdId: rootMG.id
  }
}

module productionBranch 'branches/Production.bicep' = {
  name: 'Production'
  params: {
    billingScope: billingScope
    parentManagementGroupdId: rootMG.id
  }
}

module developmentBranch 'branches/Development.bicep' = {
  name: 'Development'
  params: {
    billingScope: billingScope
    parentManagementGroupdId: rootMG.id
  }
}

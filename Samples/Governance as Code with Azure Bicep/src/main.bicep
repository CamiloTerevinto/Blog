targetScope = 'tenant'

param rootManagementGroupName string

param billingAccountName string

param billingProfileName string

param invoiceSectionName string

var billingScope = '/billingAccounts/${billingAccountName}/billingProfiles/${billingProfileName}/invoiceSections/${invoiceSectionName}'

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

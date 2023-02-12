targetScope = 'managementGroup'

@description('The region to which resources can be deployed to')
@allowed([
  'UKSouth'
  'EuropeNorth'
])
param allowedRegion string

var policyDefinitionName = 'Region restriction'

resource policyDefinition 'Microsoft.Authorization/policyDefinitions@2021-06-01' = {
  name: policyDefinitionName  
  properties: {
    policyType: 'Custom'
    mode: 'All'
    parameters: {}
    policyRule: {
      if: {
        not: {
          field: 'location'
          in: [ allowedRegion ]
        }
      }
      then: {
        effect: 'deny'
      }
    }
  }
}

output policyDefinitionId string = policyDefinition.id

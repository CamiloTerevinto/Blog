@description('Resource group location')
param rgLocation string = resourceGroup().location

@description('VNET Address Space (CIDR notation, /23 or greater)')
param vnetAddressSpace string

@description('Subnet Address Prefix (CIDR notation, /23 or greater)')
param containerAppSubnetAddressPrefix string

@description('Docker Registry URL')
param dockerRegistryUrl string

@description('Docker Registry username')
param dockerRegistryUsername string

@secure()
@description('Docker Registry password')
param dockerRegistryPassword string

@description('Public API Docker image name')
param publicApiImageName string

@description('Query API Docker image name')
param queryApiImageName string

@description('Command API Docker image name')
param commandApiImageName string

@description('Processor Docker image name')
param processorImageName string

@description('The name of the company for resource names')
param companyName string

@description('The name of the product for resource names')
param productName string

@description('The name of the region for resource names')
param regionName string

@description('The name of the region for resource names')
param environmentName string

var deploymentPrefix = '[Company].[Product].Bicep-[version]'

// Naming convention: {company}-{product}-{environment}-{region}-{component}-[identifier]
// Example: ct-blog-prod-neu-subnet-app
// Identifier is optional, to disambiguate between components.
var componentsDashedPrefix = '${companyName}-${productName}-${environmentName}-${regionName}-'

// Naming convention for resources that don't support dashes
var componentsPrefix = '${companyName}${productName}${environmentName}${regionName}'

module vnet 'Components/Networking.bicep' = {
  name: '${deploymentPrefix}-Network'
  params: {
    rgLocation: rgLocation
    vnetName: '${componentsDashedPrefix}vnet'
    vnetAddressSpace: vnetAddressSpace
    containerAppSubnetName: '${componentsDashedPrefix}subnet-app'
    containerAppSubnetAddressPrefix: containerAppSubnetAddressPrefix
  }
}

resource logAnalytics 'Microsoft.OperationalInsights/workspaces@2022-10-01' = {
  name: '${componentsDashedPrefix}law'
  location: rgLocation
  properties: {
    sku: { name: 'PerGB2018' }
  }
}

module storageAccount 'Components/Storage.bicep' = {
  name: '${deploymentPrefix}Storage'
  params: {
    rgLocation: rgLocation
    storageAccountName: '${componentsPrefix}sa'
    subnetId: vnet.outputs.subnetId
  }
}

module containerApps 'Components/ContainerApps.bicep' = {
  name: '${deploymentPrefix}ContainerApps'
  params: {
    commandApiImageName: commandApiImageName
    componentsDashedPrefix: componentsDashedPrefix
    deploymentPrefix: deploymentPrefix
    dockerRegistryPassword: dockerRegistryPassword
    dockerRegistryUrl: dockerRegistryUrl
    dockerRegistryUsername: dockerRegistryUsername
    logAnalyticsWorkspaceName: '${componentsDashedPrefix}law'
    processorImageName: processorImageName
    publicApiImageName: publicApiImageName
    queryApiImageName: queryApiImageName
    rgLocation: rgLocation
    storageAccountName: '${componentsPrefix}sa'
    subnetId: vnet.outputs.subnetId
  }
}

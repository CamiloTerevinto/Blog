@description('Resource group location')
param rgLocation string

@description('Public API Docker image name')
param publicApiImageName string

@description('Query API Docker image name')
param queryApiImageName string

@description('Command API Docker image name')
param commandApiImageName string

@description('Processor Docker image name')
param processorImageName string

@description('Docker Registry Hostname')
param dockerRegistryUrl string

@description('Docker Registry username')
param dockerRegistryUsername string

@secure()
@description('Docker Registry password')
param dockerRegistryPassword string

@description('Resource ID of the subnet to which access should be allowed')
param subnetId string

@description('The name for the storage account')
param storageAccountName string

@description('The name for the Log Analytics Workspace')
param logAnalyticsWorkspaceName string

@description('The dashed prefix for component names')
param componentsDashedPrefix string

@description('The prefix for module deployments')
param deploymentPrefix string

resource logAnalytics 'Microsoft.OperationalInsights/workspaces@2022-10-01' existing = {
  name: logAnalyticsWorkspaceName
}

resource storageAccount 'Microsoft.Storage/storageAccounts@2023-01-01' existing = {
  name: storageAccountName
}

resource containerAppEnvironment 'Microsoft.App/managedEnvironments@2023-05-01' = {
  location: rgLocation
  name: '${componentsDashedPrefix}-cappenv'
  properties: {
    appLogsConfiguration: {
      destination: 'log-analytics'
      logAnalyticsConfiguration: {
        customerId: logAnalytics.properties.customerId
        sharedKey: logAnalytics.listKeys().primarySharedKey
      }
    }
    vnetConfiguration: {
      infrastructureSubnetId: subnetId
    }
  }
}

module publicApiApp 'ContainerApp.bicep' = {
  name: '${deploymentPrefix}-PublicApiApp'
  params: {
    rgLocation: rgLocation
    resourceName: '${componentsDashedPrefix}-capp-public'
    dockerImageName: publicApiImageName
    dockerRegistryFqdn: dockerRegistryUrl
    dockerRegistryPassword: dockerRegistryPassword
    dockerRegistryUsername: dockerRegistryUsername
    environmentId: containerAppEnvironment.id
    isPublicFacing: true
    environmentVariables: [
      {
        name: 'MICROSERVICES__QUERY_URL'
        value: queryApiApp.outputs.fqdn
      }
      {
        name: 'MICROSERVICES__COMMAND_URL'
        value: commandApiApp.outputs.fqdn
      }
    ]
  }
}

module queryApiApp 'ContainerApp.bicep' = {
  name: '${deploymentPrefix}-QueryApiApp'
  params: {
    rgLocation: rgLocation
    resourceName: '${componentsDashedPrefix}-capp-query'
    dockerImageName: queryApiImageName
    dockerRegistryFqdn: dockerRegistryUrl
    dockerRegistryPassword: dockerRegistryPassword
    dockerRegistryUsername: dockerRegistryUsername
    environmentId: containerAppEnvironment.id
    isPublicFacing: false
  }
}

var blobStorageConnectionString = 'DefaultEndpointsProtocol=https;AccountName=${storageAccountName};EndpointSuffix=${environment().suffixes.storage};AccountKey=${storageAccount.listKeys().keys[0].value}'

module commandApiApp 'ContainerApp.bicep' = {
  name: '${deploymentPrefix}-CommandApiApp'
  params: {
    rgLocation: rgLocation
    resourceName: '${componentsDashedPrefix}-capp-command'
    dockerImageName: commandApiImageName
    dockerRegistryFqdn: dockerRegistryUrl
    dockerRegistryPassword: dockerRegistryPassword
    dockerRegistryUsername: dockerRegistryUsername
    environmentId: containerAppEnvironment.id
    isPublicFacing: false
    environmentVariables: [
      {
        name: 'SERVICES__STORAGE_CONNECTION_STRING'
        secretRef: 'storage-connection-string'
      }
    ]
    secretVariables: [
      {
        name: 'storage-connection-string'
        value: blobStorageConnectionString
      }
    ]
  }
}

module processorApp 'ContainerApp.bicep' = {
  name: '${deploymentPrefix}-ProcessorApp'
  params: {
    rgLocation: rgLocation
    resourceName: '${componentsDashedPrefix}-capp-processor'
    dockerImageName: processorImageName
    dockerRegistryFqdn: dockerRegistryUrl
    dockerRegistryPassword: dockerRegistryPassword
    dockerRegistryUsername: dockerRegistryUsername
    environmentId: containerAppEnvironment.id
    isPublicFacing: false
  }
}

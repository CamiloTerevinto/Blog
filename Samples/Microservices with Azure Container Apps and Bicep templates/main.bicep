@description('Resource group location')
param rgLocation string

@description('VNET resource name')
param vnetName string

@description('VNET Address Space (CIDR notation, /23 or greater)')
param vnetAddressSpace string = '10.0.0.0/22'

@description('Subnet resource name')
param containerAppSubnetName string

@description('Subnet Address Prefix (CIDR notation, /23 or greater)')
param subnetAddressPrefix string = '10.0.0.0/23'

@description('Log Analytics resource name')
param cappLogAnalyticsName string

@description('Docker Registry URL')
param dockerRegistryUrl string

@description('Docker Registry username')
param dockerRegistryUsername string

@secure()
@description('Docker Registry password')
param dockerRegistryPassword string

@description('Container Apps Environment resource name')
param cappEnvName string

@description('Public API Container App resource name')
param publicApiCappName string

@description('Public API Docker image name')
param publicApiImageName string

@description('Query API Container App resource name')
param queryApiCappName string

@description('Query API Docker image name')
param queryApiImageName string

@description('Command API Container App resource name')
param commandApiCappName string

@description('Command API Docker image name')
param commandApiImageName string

@description('Processor Container App resource name')
param processorCappName string

@description('Processor Docker image name')
param processorImageName string

@description('Storage Account resource name')
param storageAccountName string

resource vnet 'Microsoft.Network/virtualNetworks@2022-01-01' = {
  name: vnetName
  location: rgLocation
  properties: {
    addressSpace: {
      addressPrefixes: [ vnetAddressSpace ]
    }
  }

  resource subnet 'subnets@2022-01-01' = {
    name: containerAppSubnetName
    properties: {
      addressPrefix: subnetAddressPrefix
      serviceEndpoints: [
        {
          service: 'Microsoft.Storage'
          locations: [ rgLocation ]
        }
      ]
    }
  }
}

resource logAnalytics 'Microsoft.OperationalInsights/workspaces@2021-06-01' = {
  name: cappLogAnalyticsName
  location: rgLocation
  properties: {
    sku: { name: 'PerGB2018' }
  }
}

resource containerAppEnvironment 'Microsoft.App/managedEnvironments@2022-03-01' = {
  location: rgLocation
  name: cappEnvName
  properties: {
    appLogsConfiguration: {
      destination: 'log-analytics'
      logAnalyticsConfiguration: {
        customerId: logAnalytics.properties.customerId
        sharedKey: logAnalytics.listKeys().primarySharedKey
      }
    }
    vnetConfiguration: {
      infrastructureSubnetId: vnet::subnet.id
    }
  }
}

resource publicApi 'Microsoft.App/containerApps@2022-03-01' = {
  name: publicApiCappName
  location: rgLocation
  properties: {
    managedEnvironmentId: containerAppEnvironment.id
    configuration: {
      secrets: [
        {
          name: 'registry-password'
          value: dockerRegistryPassword
        }
      ]
      registries: [
        {
          passwordSecretRef: 'registry-password'
          server: dockerRegistryUrl
          username: dockerRegistryUsername
        }
      ]
    }
    template: {
      containers: [
        {
          image: '${publicApiImageName}:latest'
          env: [
            {
              name: 'MICROSERVICES__QUERY_URL'
              value: queryApi.properties.configuration.ingress.fqdn
            }
            {
              name: 'MICROSERVICES__COMMAND_URL'
              value: commandApi.properties.configuration.ingress.fqdn
            }
          ]
        }
      ]
      scale: {
        maxReplicas: 1
        minReplicas: 1
      }
    }
  }
}

resource queryApi 'Microsoft.App/containerApps@2022-03-01' = {
  name: queryApiCappName
  location: rgLocation
  properties: {
    managedEnvironmentId: containerAppEnvironment.id
    configuration: {
      secrets: [
        {
          name: 'registry-password'
          value: dockerRegistryPassword
        }
      ]
      registries: [
        {
          passwordSecretRef: 'registry-password'
          server: dockerRegistryUrl
          username: dockerRegistryUsername
        }
      ]
      ingress: {
        external: true
        targetPort: 80
      }
    }
    template: {
      containers: [
        {
          image: '${queryApiImageName}:latest'
          env: [
          ]
        }
      ]
      scale: {
        maxReplicas: 1
        minReplicas: 1
      }
    }
  }
}

resource commandApi 'Microsoft.App/containerApps@2022-03-01' = {
  name: commandApiCappName
  location: rgLocation
  properties: {
    managedEnvironmentId: containerAppEnvironment.id
    configuration: {
      secrets: [
        {
          name: 'registry-password'
          value: dockerRegistryPassword
        }
        {
          name: 'storage-connection-string'
          value: blobStorageConnectionString
        }
      ]
      registries: [
        {
          passwordSecretRef: 'registry-password'
          server: dockerRegistryUrl
          username: dockerRegistryUsername
        }
      ]
    }
    template: {
      containers: [
        {
          image: '${commandApiImageName}:latest'
          env: [
            {
              name: 'SERVICES__STORAGE_CONNECTION_STRING'
              secretRef: 'storage-connection-string'
            }
          ]
        }
      ]
      scale: {
        maxReplicas: 1
        minReplicas: 1
      }
    }
  }
}

resource processor 'Microsoft.App/containerApps@2022-03-01' = {
  name: processorCappName
  location: rgLocation
  properties: {
    managedEnvironmentId: containerAppEnvironment.id
    configuration: {
      secrets: [
        {
          name: 'registry-password'
          value: dockerRegistryPassword
        }
      ]
      registries: [
        {
          passwordSecretRef: 'registry-password'
          server: dockerRegistryUrl
          username: dockerRegistryUsername
        }
      ]
    }
    template: {
      containers: [
        {
          image: '${processorImageName}:latest'
        }
      ]
      scale: {
        maxReplicas: 1
        minReplicas: 1
      }
    }
  }
}

resource storageAccount 'Microsoft.Storage/storageAccounts@2021-02-01' = {
  name: storageAccountName
  location: rgLocation
  kind: 'StorageV2'
  sku: {
    name: 'Standard_LRS'
  }
  properties: {
    networkAcls: {
      defaultAction: 'Deny'
      bypass: 'AzureServices'
      virtualNetworkRules: [
        {
          id: vnet::subnet.id
          action: 'Allow'
        }
      ]
    }
  }
}

var blobStorageConnectionString = 'DefaultEndpointsProtocol=https;AccountName=${storageAccountName};EndpointSuffix=${environment().suffixes.storage};AccountKey=${storageAccount.listKeys().keys[0].value}'

@description('Resource group location')
param rgLocation string

@description('The name of the resource')
param resourceName string

@description('The resource id for the Container Apps Environment')
param environmentId string

@description('Docker Registry Hostname')
param dockerRegistryFqdn string

@description('Docker Registry username')
param dockerRegistryUsername string

@secure()
@description('Docker Registry password')
param dockerRegistryPassword string

@description('The name of the docker image')
param dockerImageName string

@description('Whether internet access is allowed (false) or not (true)')
param isPublicFacing bool

@description('The environment variables to pass to the application')
param environmentVariables {
  name: string
  value: string?
  secretRef: string?
}[] = []

@description('The environment variables to pass to the application stored as secrets')
param secretVariables {
  name: string
  value: string
}[] = []

@description('Whether ingress is enabled for the app')
param ingressEnabled bool = true

var ingressSettings = ingressEnabled ?  {
  allowInsecure: false
  targetPort: 80
  transport: 'auto'
  external: isPublicFacing
} : null

resource app 'Microsoft.App/containerApps@2023-05-01' = {
  name: resourceName
  location: rgLocation
  properties: {
    managedEnvironmentId: environmentId
    configuration: {
      secrets: concat([
        {
          name: 'registry-password'
          value: dockerRegistryPassword
        }
      ], secretVariables)
      registries: [
        {
          passwordSecretRef: 'registry-password'
          server: dockerRegistryFqdn
          username: dockerRegistryUsername
        }
      ]
      ingress: ingressSettings
    }
    template: {
      containers: [
        {
          image: '${dockerRegistryFqdn}/${dockerImageName}:latest'
          env: environmentVariables          
        }
      ]
      scale: {
        maxReplicas: 1
        minReplicas: 1
      }
    }
  }
}

output fqdn string = app.properties.configuration.ingress.fqdn

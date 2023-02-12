# Introduction

This sample presents an **unfinished**/**untested** way of deploying Bicep templates for managing your cloud's governance.

## Prerequisites

1. Please note that you need to follow these steps before being able to use/test this sample: [ALZ Setup azure](https://github.com/Azure/Enterprise-Scale/wiki/ALZ-Setup-azure).

2. The sample has 2 parameters that need to be filled in. The first one (`rootManagementGroupName`) is simply the ID of the Azure Tenant where you want to deploy the services. The second (`billingScope`) is the resource ID of the billing scope to which subscriptions are assigned. You can find the details of this by following one of the Microsoft Learn guides [available here](https://learn.microsoft.com/en-us/azure/cost-management-billing/manage/programmatically-create-subscription).

## Feedback

If you'd like to provide feedback on this sample, please feel free to create an issue.  
I'd also be glad to accept Pull Requests to improve this sample.

## License

This sample is licensed through the MIT license, which can be found [here](/LICENSE).

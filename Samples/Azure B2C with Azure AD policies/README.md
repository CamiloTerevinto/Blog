# Introduction

This Azure B2C policy pack contains support for:

1. Local account sign in.
2. Local account sign up with a separated email verification step.
3. Local account password reset.
4. Azure Active Directory federated sign in.

While this pack is based on the "Social and local accounts" template pack, Facebook support was removed from the Extensions policy.

## Prerequisites

You are welcome to read my [blog post on using this sample pack](https://www.camiloterevinto.com/post/azure-b2c-with-azure-active-directory) or follow the [Microsoft Learn](https://learn.microsoft.com/en-us/azure/active-directory-b2c/tutorial-create-user-flows?pivots=b2c-custom-policy) tutorials.

## Variables

The following variables must be replaced for the policy to be usable:

* {YOUR_TENANT} => name of your B2C tenant
* {YOUR_APPLICATION_INSIGHTS_KEY} => application insights key (this entry is in the SignUpOrSignin policy and can be deleted)
* {YOUR_AAD_TENANT_ID} => ID of your base AAD tenant
* {YOUR_AAD_CLIENT_ID} => ID of the app registration on the base tenant
* {YOUR_IEF_PROXY_CLIENT_ID} => ID of the app registration on the B2C tenant for the proxy login
* {YOUR_IEF_CLIENT_ID} => ID of the app registration on the B2C tenant for the login

You can easily replace these by downloading the code and, from an editor such as Visual Studio Code, using Search and Replace.

# CT.Examples.B2cClientCredentials

This sample ASP.NET Core 6 application implements the Client Credentials OAuth 2.0 flow to obtain an access token from Azure B2C.

The sample then attempts to call 2 separate APIs using the configured `ConfidentialClientApplication` to show how to overcome the issue of B2C not supporting tokens for multiple APIs.
using System.Security.Cryptography;
using System;

namespace CT.Examples.CustomApiKeys.Services
{
    public interface IApiKeyService
    {
        string GenerateApiKey();
    }

    internal class ApiKeyService : IApiKeyService
    {
        private const string _prefix = "CT-";
        private const int _bytesCountToGenerate = 32;
        private const int _lengthOfKey = 36;

        public string GenerateApiKey()
        {
            var bytes = RandomNumberGenerator.GetBytes(_bytesCountToGenerate);

            return string.Concat(_prefix, Convert.ToBase64String(bytes)
                .Replace("/", "")
                .Replace("+", "")
                .Replace("=", "")
                .AsSpan(0, _lengthOfKey - _prefix.Length));
        }
    }
}

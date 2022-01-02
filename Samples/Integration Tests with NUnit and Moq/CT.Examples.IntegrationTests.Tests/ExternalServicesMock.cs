using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CT.Examples.IntegrationTests.Tests
{
    public class ExternalServicesMock
    {
        public Mock<ITemperatureApiClient> TemperatureApiClient { get; }

        public ExternalServicesMock()
        {
            TemperatureApiClient = new Mock<ITemperatureApiClient>();
        }

        public IEnumerable<(Type, object)> GetMocks()
        {
            return GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Select(x =>
                {
                    var underlyingType = x.PropertyType.GetGenericArguments()[0];
                    var value = x.GetValue(this) as Mock;

                    return (underlyingType, value.Object);
                })
                .ToArray();
        }
    }
}

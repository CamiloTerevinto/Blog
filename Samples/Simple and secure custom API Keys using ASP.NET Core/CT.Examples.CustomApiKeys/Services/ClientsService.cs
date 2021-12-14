namespace CT.Examples.CustomApiKeys.Services
{
    public interface IClientsService
    {
        Task<Dictionary<string, Guid>> GetActiveClients();
        Task InvalidateApiKey(string apiKey);
    }

    internal class InMemoryClientsService : IClientsService
    {
        private static readonly Dictionary<string, Guid> _clients = new()
        {
            { "CT-mTbC4r1Eh7wvXrXE1UDl18NGH1fRzcrRz", Guid.NewGuid() }
        };

        public Task<Dictionary<string, Guid>> GetActiveClients()
        {
            return Task.FromResult(_clients);
        }

        public Task InvalidateApiKey(string apiKey)
        {
            _clients.Remove(apiKey);

            return Task.CompletedTask;
        }
    }
}
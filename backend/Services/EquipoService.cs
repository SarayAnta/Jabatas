using MongoDB.Driver;
using backend.Models;

namespace backend.Services
{
    public class EquipoService
    {
        private readonly IMongoCollection<SeniorFem> _seniorFemCollection;

        public EquipoService(IMongoDBSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _seniorFemCollection = database.GetCollection<SeniorFem>("SeniorFem");
        }

        // Métodos para CRUD aquí, por ejemplo:
        public async Task<List<SeniorFem>> GetSeniorFemAsync() =>
            await _seniorFemCollection.Find(seniorFem => true).ToListAsync();

        // Agrega métodos para SeniorMasc y Admin aquí
    }
}

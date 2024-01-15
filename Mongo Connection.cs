// oto fragment kodu, który służy do połączenia z wybraną bazą MongoDB o nazwie "CookingDB" w klasie MongoDBContext

using MongoDB.Driver;

public class MongoDBContext
{
    private IMongoDatabase _database;

    public MongoDBContext(string databaseName)
    {
        // Adres hosta to localhost, a nazwa bazy danych to "CookingDB"
        var client = new MongoClient("mongodb://localhost:27017");
        _database = client.GetDatabase(databaseName);
    }

    public IMongoCollection<Przepis> Przepisy
    {
        get { return _database.GetCollection<Przepis>("Przepisy"); }
    }
}
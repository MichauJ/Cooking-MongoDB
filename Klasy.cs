using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

public class Przepis
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string Nazwa { get; set; }
    public List<string> Skladniki { get; set; }
    public  List<string> Tagi { get; set; }
    public List<DateTime> DatyPrzygotowania { get; set; }
    public string Zrodlo { get; set; }
    public string ZdjeciePath { get; set; }
    public List<string> Uwagi { get; set; }
    public int Ocena { get; set; }
    public string Instrukcja { get; set; }
    public KosztEnum Koszt { get; set; }
    public int CzasPrzygotowania { get; set; }

    public Przepis()
    {
        Skladniki = new List<string>();
        Tagi = new List<string>();
        DatyPrzygotowania = new List<DateTime>();
        Uwagi = new List<string>();
    }
    public enum KosztEnum
    {
    drogi,
    umiarkowany,
    tani,
    NA
    }
}

public class PrzepisRepository
{
    private IMongoCollection<Przepis> _przepisyCollection;

    public PrzepisRepository(MongoDBContext context)
    {
        _przepisyCollection = context.Przepisy;
    }

    public void DodajPrzepis(Przepis przepis)
    {
        _przepisyCollection.InsertOne(przepis);
    }

    public List<Przepis> PobierzWszystkiePrzepisy()
    {
        return _przepisyCollection.Find(_ => true).ToList();
    }
    public bool CzyNazwaPrzepisuIstnieje(string nazwa)
    {
        return _przepisyCollection.Find(p => p.Nazwa.Equals(nazwa, StringComparison.OrdinalIgnoreCase)).Any();
    }
    public void UsunPrzepis(Przepis przepis)
    {
        var filter = Builders<Przepis>.Filter.Eq(p => p.Id, przepis.Id);
        _przepisyCollection.DeleteOne(filter);
    }

    public void ZaktualizujPrzepis(Przepis przepis)
    {
        var filter = Builders<Przepis>.Filter.Eq(p => p.Id, przepis.Id);
        var update = Builders<Przepis>.Update
            .Set(p => p.Tagi, przepis.Tagi)
            .Set(p => p.Ocena, przepis.Ocena);

        _przepisyCollection.UpdateOne(filter, update);
    }
}
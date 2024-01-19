using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using MongoDB.Bson.IO;
using Newtonsoft.Json;
using NewtonsoftJsonConvert = Newtonsoft.Json.JsonConvert;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

public class Przepis
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string Nazwa { get; set; }
    public List<string> Skladniki { get; set; }
    public List<string> Tagi { get; set; }
    public List<DateTime> DatyPrzygotowania { get; set; }
    public string Zrodlo { get; set; }
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
    public void DodajPrzepisyDoBazy(List<Przepis> przepisy)
    {
        _przepisyCollection.InsertMany(przepisy);
    }
    public List<Przepis> ImportujJson(string json)
    {
        //Parsuje JSON do listy obiektów Przepis
        List<Przepis> przepisy = NewtonsoftJsonConvert.DeserializeObject<List<Przepis>>(json);
        
        return przepisy;
    }
    public List<Przepis> PobierzPrzepisyWedługFiltru(string filtrTag)
    {
        // Utwórz filtr na podstawie przekazanej nazwy przepisu
        var filtr = Builders<Przepis>.Filter.Where(p => p.Tagi.Any(tag => tag.ToLower() == filtrTag.ToLower()));

        // Wywołanie zapytania z użyciem filtru
        var wyniki = _przepisyCollection.Find(filtr).ToList();

        return wyniki;
    }
    public void ExportToJson<T>(List<T> data, string filePath)
    {
        var json = NewtonsoftJsonConvert.SerializeObject(data, Formatting.Indented);
        File.WriteAllText(filePath, json);
    }
    public void ExportToCsv(List<Przepis> data, string filePath)
    {
        using (var writer = new StreamWriter(filePath))
        using (var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture)))
        {
            csv.WriteRecords(data);
        }
    }
}

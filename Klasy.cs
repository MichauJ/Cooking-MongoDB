using System;
using System.Collections.Generic;

public class Przepis
{
    public string Nazwa { get; set; }
    public List<Skladnik> Skladniki { get; set; }
    public  List<string> Tagi { get; set; }
    public List<DateTime> DatyPrzygotowania { get; set; }
    public string Zrodlo { get; set; }
    public string ZdjeciePath { get; set; }
    public List<string> Uwagi { get; set; }
    public int Ocena { get; set; }
    public string Instrukcja { get; set; }
    public string Koszt { get; set; }
    public int CzasPrzygotowania { get; set; }

    public Przepis()
    {
        Skladniki = new List<Skladnik>();
        Tagi = new List<string>();
        DatyPrzygotowania = new List<DateTime>();
        Uwagi = new List<string>();
    }
}

public class Skladnik
{
    public string Nazwa { get; set; }
    public double Ilosc { get; set; }
    public  string Jednostka { get; set; }
}

using System;
using System.Collections.Generic;

public class Przepis
{
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
    tani
    }
}

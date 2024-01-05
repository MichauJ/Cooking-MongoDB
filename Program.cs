using System;
using System.Collections.Generic;

#nullable enable

class Program
{
    static List<Przepis> listaPrzepisow = new List<Przepis>();

    static void Main()
    {
        bool exit = false;

        while (!exit)
        {
            Console.WriteLine("1. Wyświetl dane");
            Console.WriteLine("2. Wprowadź dane");
            Console.WriteLine("3. Modyfikuj dane");
            Console.WriteLine("4. Wyjście");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    WyswietlDane();
                    break;
                case "2":
                    WprowadzDane();
                    break;
                case "3":
                    ModyfikujDane();
                    break;
                case "4":
                    exit = true;
                    break;
                default:
                    Console.WriteLine("Nieprawidłowy wybór. Spróbuj ponownie.");
                    break;
            }
        }
    }

    static void WyswietlDane()
    {
        Console.WriteLine("Wybierz przepis do wyświetlenia:");

        for (int i = 0; i < listaPrzepisow.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {listaPrzepisow[i].Nazwa}");
        }

        Console.Write("Podaj numer przepisu do wyświetlenia: ");
        if (int.TryParse(Console.ReadLine(), out int numerPrzepisu) && numerPrzepisu > 0 && numerPrzepisu <= listaPrzepisow.Count)
        {
            Przepis wybranyPrzepis = listaPrzepisow[numerPrzepisu - 1];

            Console.WriteLine($"Dane przepisu: {wybranyPrzepis.Nazwa}");
            Console.WriteLine($"Skladniki: {string.Join(", ", wybranyPrzepis.Skladniki)}");
            Console.WriteLine($"Tagi: {string.Join(", ", wybranyPrzepis.Tagi)}");
            Console.WriteLine($"Daty przygotowania: {string.Join(", ", wybranyPrzepis.DatyPrzygotowania)}");
            Console.WriteLine($"Źródło: {wybranyPrzepis.Zrodlo}");
            Console.WriteLine($"Ścieżka do zdjęcia: {wybranyPrzepis.ZdjeciePath}");
            Console.WriteLine($"Uwagi: {string.Join(", ", wybranyPrzepis.Uwagi)}");
            Console.WriteLine($"Ocena: {wybranyPrzepis.Ocena}");
            Console.WriteLine($"Instrukcja: {wybranyPrzepis.Instrukcja}");
            Console.WriteLine($"Koszt: {wybranyPrzepis.Koszt}");
            Console.WriteLine($"Czas przygotowania: {wybranyPrzepis.CzasPrzygotowania} minut");
        }
        else
        {
            Console.WriteLine("Nieprawidłowy numer przepisu.");
        }
    }


    static void WprowadzDane()
    {
        Przepis nowyPrzepis = new Przepis();

        Console.WriteLine("Wprowadź dane nowego przepisu:");

        Console.Write("Nazwa: ");
        nowyPrzepis.Nazwa = Console.ReadLine();

        Console.Write("Skladniki (oddzielone przecinkami): ");
        string skladnikiInput = Console.ReadLine();
        nowyPrzepis.Skladniki = skladnikiInput.Split(',').ToList();

        Console.Write("Tagi (oddzielone przecinkami): ");
        string tagiInput = Console.ReadLine();
        nowyPrzepis.Tagi = tagiInput.Split(',').ToList();

        Console.Write("Daty przygotowania (oddzielone przecinkami): ");
        string datyInput = Console.ReadLine();
        nowyPrzepis.DatyPrzygotowania = datyInput.Split(',').Select(DateTime.Parse).ToList();

        Console.Write("Źródło: ");
        nowyPrzepis.Zrodlo = Console.ReadLine();

        Console.Write("Ścieżka do zdjęcia: ");
        nowyPrzepis.ZdjeciePath = Console.ReadLine();

        Console.Write("Uwagi (oddzielone średnikami): ");
        string uwagiInput = Console.ReadLine();
        nowyPrzepis.Uwagi = uwagiInput.Split(';').ToList();

        Console.Write("Ocena: ");
        int ocena;
        if (int.TryParse(Console.ReadLine(), out ocena))
        {
            nowyPrzepis.Ocena = ocena;
        }
        else
        {
            Console.WriteLine("Nieprawidłowa ocena. Ustawiono domyślną wartość 0.");
            nowyPrzepis.Ocena = 0;
        }

        Console.Write("Instrukcja: ");
        nowyPrzepis.Instrukcja = Console.ReadLine();

        Console.Write("Koszt (Drogi/Umiarkowany/tani): ");
        string kosztInput = Console.ReadLine();
        if (Enum.TryParse(typeof(Przepis.KosztEnum), kosztInput, out object koszt))
        {
            nowyPrzepis.Koszt = (Przepis.KosztEnum)koszt;
        }
        else
        {
            Console.WriteLine("Nieprawidłowy koszt. Ustawiono domyślną wartość tani.");
            nowyPrzepis.Koszt = Przepis.KosztEnum.tani;
        }

        Console.Write("Czas przygotowania (w minutach): ");
        int czasPrzygotowania;
        if (int.TryParse(Console.ReadLine(), out czasPrzygotowania))
        {
            nowyPrzepis.CzasPrzygotowania = czasPrzygotowania;
        }
        else
        {
            Console.WriteLine("Nieprawidłowy czas przygotowania. Ustawiono domyślną wartość 0.");
            nowyPrzepis.CzasPrzygotowania = 0;
        }

        listaPrzepisow.Add(nowyPrzepis);
        Console.WriteLine("Przepis dodany do listy.");
    }


    static void ModyfikujDane()
    {
        Console.WriteLine("Wybierz przepis do modyfikacji:");

        for (int i = 0; i < listaPrzepisow.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {listaPrzepisow[i].Nazwa}");
        }

        Console.Write("Podaj nazwę przepisu do modyfikacji: ");
        string wybranaNazwa = Console.ReadLine();

        Przepis wybranyPrzepis = listaPrzepisow.Find(p => p.Nazwa.Equals(wybranaNazwa, StringComparison.OrdinalIgnoreCase));

        if (wybranyPrzepis != null)
        {
            Console.WriteLine($"Edytujesz przepis: {wybranyPrzepis.Nazwa}");
            Console.WriteLine("1. Dodaj datę przygotowania do listy");
            Console.WriteLine("2. Usuń datę przygotowania z listy");
            Console.WriteLine("3. Dodaj uwagi");
            Console.WriteLine("4. Usuń uwagi");
            Console.WriteLine("5. Zmień ocenę");
            Console.WriteLine("6. Usuń wartość z listy dat przygotowania");
            Console.WriteLine("7. Usuń tagi");
            Console.WriteLine("8. Powrót do menu głównego");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.Write("Dodaj datę przygotowania: ");
                    if (DateTime.TryParse(Console.ReadLine(), out DateTime nowaData))
                    {
                        wybranyPrzepis.DatyPrzygotowania.Add(nowaData);
                        Console.WriteLine("Data dodana do listy.");
                    }
                    else
                    {
                        Console.WriteLine("Nieprawidłowy format daty.");
                    }
                    break;
                case "2":
                    Console.WriteLine("Usuwam datę przygotowania z listy.");
                    if (wybranyPrzepis.DatyPrzygotowania.Count > 0)
                    {
                        Console.WriteLine("Obecne daty przygotowania:");
                        for (int i = 0; i < wybranyPrzepis.DatyPrzygotowania.Count; i++)
                        {
                            Console.WriteLine($"{i + 1}. {wybranyPrzepis.DatyPrzygotowania[i]}");
                        }

                        Console.Write("Podaj numer daty do usunięcia: ");
                        if (int.TryParse(Console.ReadLine(), out int numerDaty) && numerDaty > 0 && numerDaty <= wybranyPrzepis.DatyPrzygotowania.Count)
                        {
                            wybranyPrzepis.DatyPrzygotowania.RemoveAt(numerDaty - 1);
                            Console.WriteLine("Data usunięta z listy.");
                        }
                        else
                        {
                            Console.WriteLine("Nieprawidłowy numer daty.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Brak dat przygotowania do usunięcia.");
                    }
                    break;
                case "3":
                    Console.Write("Dodaj uwagi: ");
                    wybranyPrzepis.Uwagi.Add(Console.ReadLine());
                    Console.WriteLine("Uwagi dodane do listy.");
                    break;
                case "4":
                    Console.WriteLine("Usuwam uwagi.");
                    if (wybranyPrzepis.Uwagi.Count > 0)
                    {
                        Console.WriteLine("Obecne uwagi:");
                        for (int i = 0; i < wybranyPrzepis.Uwagi.Count; i++)
                        {
                            Console.WriteLine($"{i + 1}. {wybranyPrzepis.Uwagi[i]}");
                        }

                        Console.Write("Podaj numer uwagi do usunięcia: ");
                        if (int.TryParse(Console.ReadLine(), out int numerUwagi) && numerUwagi > 0 && numerUwagi <= wybranyPrzepis.Uwagi.Count)
                        {
                            wybranyPrzepis.Uwagi.RemoveAt(numerUwagi - 1);
                            Console.WriteLine("Uwaga usunięta z listy.");
                        }
                        else
                        {
                            Console.WriteLine("Nieprawidłowy numer uwagi.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Brak uwag do usunięcia.");
                    }
                    break;
                case "5":
                    Console.Write("Zmień ocenę: ");
                    if (int.TryParse(Console.ReadLine(), out int nowaOcena))
                    {
                        wybranyPrzepis.Ocena = nowaOcena;
                        Console.WriteLine("Ocena została zmieniona.");
                    }
                    else
                    {
                        Console.WriteLine("Nieprawidłowa ocena.");
                    }
                    break;
                case "6":
                    Console.WriteLine("Usuwam wartość z listy dat przygotowania.");
                    if (wybranyPrzepis.DatyPrzygotowania.Count > 0)
                    {
                        Console.WriteLine("Obecne daty przygotowania:");
                        for (int i = 0; i < wybranyPrzepis.DatyPrzygotowania.Count; i++)
                        {
                            Console.WriteLine($"{i + 1}. {wybranyPrzepis.DatyPrzygotowania[i]}");
                        }

                        Console.Write("Podaj numer daty do usunięcia: ");
                        if (int.TryParse(Console.ReadLine(), out int numerDaty) && numerDaty > 0 && numerDaty <= wybranyPrzepis.DatyPrzygotowania.Count)
                        {
                            wybranyPrzepis.DatyPrzygotowania.RemoveAt(numerDaty - 1);
                            Console.WriteLine("Data usunięta z listy.");
                        }
                        else
                        {
                            Console.WriteLine("Nieprawidłowy numer daty.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Brak dat przygotowania do usunięcia.");
                    }
                    break;
                case "7":
                    Console.WriteLine("Usuwam tagi.");
                    if (wybranyPrzepis.Tagi.Count > 0)
                    {
                        Console.WriteLine("Obecne tagi:");
                        for (int i = 0; i < wybranyPrzepis.Tagi.Count; i++)
                        {
                            Console.WriteLine($"{i + 1}. {wybranyPrzepis.Tagi[i]}");
                        }

                        Console.Write("Podaj numer tagu do usunięcia: ");
                        if (int.TryParse(Console.ReadLine(), out int numerTagu) && numerTagu > 0 && numerTagu <= wybranyPrzepis.Tagi.Count)
                        {
                            wybranyPrzepis.Tagi.RemoveAt(numerTagu - 1);
                            Console.WriteLine("Tag usunięty z listy.");
                        }
                        else
                        {
                            Console.WriteLine("Nieprawidłowy numer tagu.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Brak tagów do usunięcia.");
                    }
                    break;
                case "8":
                    Console.WriteLine("Powrót do menu głównego.");
                    break;
                default:
                    Console.WriteLine("Nieprawidłowy wybór.");
                    break;
            }
        }
        else
        {
            Console.WriteLine("Przepis o podanej nazwie nie został znaleziony.");
        }
    }


}

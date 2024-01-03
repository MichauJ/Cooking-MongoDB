using System;
using System.Collections.Generic;

Main();

    static void Main()
    {
        List<Przepis> listaPrzepisow = new List<Przepis>();

        // Dodawanie nowego przepisu przez użytkownika
        DodajNowyPrzepis(listaPrzepisow);

        // Wyświetlanie dodanych przepisów
        foreach (var przepis in listaPrzepisow)
        {
            WyswietlInformacje(przepis);
        }
    }

    static void DodajNowyPrzepis(List<Przepis> listaPrzepisow)
    {
        Przepis nowyPrzepis = new Przepis();

        Console.Write("Nazwa przepisu: ");
        nowyPrzepis.Nazwa = Console.ReadLine();

        // Dodawanie składników
        Console.WriteLine("Dodaj składniki (wpisz 'koniec' aby zakończyć):");
        while (true)
        {
            Skladnik skladnik = new Skladnik();
            Console.Write("Nazwa składnika: ");
            skladnik.Nazwa = Console.ReadLine();

            if (skladnik.Nazwa.ToLower() == "koniec")
                break;

            Console.Write("Ilość: ");
            if (double.TryParse(Console.ReadLine(), out double ilosc))
            {
                skladnik.Ilosc = ilosc;
            }

            Console.Write("Jednostka: ");
            skladnik.Jednostka = Console.ReadLine();

            nowyPrzepis.Skladniki.Add(skladnik);
        }

        // Dodawanie tagów
        Console.WriteLine("Dodaj tagi (wpisz 'koniec' aby zakończyć):");
        while (true)
        {
            Console.Write("Tag: ");
            string tag = Console.ReadLine();

            if (tag.ToLower() == "koniec")
                break;

            nowyPrzepis.Tagi.Add(tag);
        }

        // Dodawanie dat przygotowania
        Console.WriteLine("Dodaj daty przygotowania (wpisz 'koniec' aby zakończyć):");
        while (true)
        {
            Console.Write("Data (dd.mm.rrrr): ");
            string dataString = Console.ReadLine();

            if (dataString.ToLower() == "koniec")
                break;

            if (DateTime.TryParseExact(dataString, "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out DateTime data))
            {
                nowyPrzepis.DatyPrzygotowania.Add(data);
            }
            else
            {
                Console.WriteLine("Nieprawidłowy format daty. Spróbuj ponownie.");
            }
        }

        // Pozostałe dane
        Console.Write("Źródło: ");
        nowyPrzepis.Zrodlo = Console.ReadLine();

        Console.Write("Zdjęcie (ścieżka do pliku): ");
        nowyPrzepis.ZdjeciePath = Console.ReadLine();

        Console.Write("Uwagi: ");
        nowyPrzepis.Uwagi.Add(Console.ReadLine());

        Console.Write("Ocena (1-10): ");
        if (int.TryParse(Console.ReadLine(), out int ocena) && ocena >= 1 && ocena <= 10)
        {
            nowyPrzepis.Ocena = ocena;
        }
        else
        {
            Console.WriteLine("Nieprawidłowa ocena. Ustawiono domyślnie na 5.");
            nowyPrzepis.Ocena = 5;
        }

        Console.Write("Instrukcja: ");
        nowyPrzepis.Instrukcja = Console.ReadLine();

        Console.Write("Koszt: ");
        nowyPrzepis.Koszt = Console.ReadLine();

        Console.Write("Czas przygotowania (w minutach): ");
        if (int.TryParse(Console.ReadLine(), out int czasPrzygotowania) && czasPrzygotowania >= 0)
        {
            nowyPrzepis.CzasPrzygotowania = czasPrzygotowania;
        }
        else
        {
            Console.WriteLine("Nieprawidłowy czas przygotowania. Ustawiono domyślnie na 0.");
            nowyPrzepis.CzasPrzygotowania = 0;
        }


        // Dodanie nowego przepisu do listy
        listaPrzepisow.Add(nowyPrzepis);

        Console.WriteLine("Przepis został dodany!");
    }

    static void WyswietlInformacje(Przepis przepis)
    {
        Console.WriteLine($"Nazwa: {przepis.Nazwa}");
        Console.WriteLine("Skladniki:");
        foreach (var skladnik in przepis.Skladniki)
        {
            Console.WriteLine($"- {skladnik.Nazwa}: {skladnik.Ilosc} {skladnik.Jednostka}");
        }
        Console.WriteLine("Tagi: " + string.Join(", ", przepis.Tagi));
        Console.WriteLine("Daty przygotowania: " + string.Join(", ", przepis.DatyPrzygotowania));
        Console.WriteLine($"Źródło: {przepis.Zrodlo}");
        Console.WriteLine($"Zdjęcie: {przepis.ZdjeciePath}");
        Console.WriteLine("Uwagi: " + string.Join(", ", przepis.Uwagi));
        Console.WriteLine($"Ocena: {przepis.Ocena}");
        Console.WriteLine($"Instrukcja: {przepis.Instrukcja}");
        Console.WriteLine($"Koszt: {przepis.Koszt}");
        Console.WriteLine($"Czas przygotowania: {przepis.CzasPrzygotowania} minut");
        Console.WriteLine();
    }

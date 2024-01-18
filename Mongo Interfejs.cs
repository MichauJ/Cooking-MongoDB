using MongoDB.Bson.Serialization.Serializers;
using System.Globalization;
using System.Text;
//Dodaj poniższą referencję do biblioteki CsvHelper w terminalu
//dotnet add package CsvHelper;

Main();
static void Main()
{
    var context = new MongoDBContext("CookingDB");
    var przepisRepository = new PrzepisRepository(context);

    bool exit = false;

    while (!exit)
    {
        Console.WriteLine("\nWybierz opcję: ");
        Console.WriteLine("1. Wyświetl przepis");
        Console.WriteLine("2. Dodaj przepis");
        Console.WriteLine("3. Modyfikuj przepis");
        Console.WriteLine("4. Importuj przepisy z JSON");
        Console.WriteLine("5. Eksportuj przepisy");
        Console.WriteLine("6. Usuń wszystkie przepisy");
        Console.WriteLine("7. Wyjście\n");

        string choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                WyświetlDane(przepisRepository);
                break;
            case "2":
                DodajPrzepis(przepisRepository); // Zmiana nazwy funkcji
                break;
            case "3":
                ModyfikujDane(przepisRepository);
                break;
            case "4":
                ImportujPrzepisy(przepisRepository);
                break;
            case "5":
                EksportujPrzepisy(przepisRepository);
                break;
            case "6":
                UsunPrzepisy(przepisRepository);
                break;
            case "7":
                exit = true;
                break;

            default:
                Console.WriteLine("Nieprawidłowy wybór. Spróbuj ponownie.");
                break;
        }
    }

}

static void DodajPrzepis(PrzepisRepository przepisRepository)
{
    Przepis nowyPrzepis = new Przepis();

    Console.Write("Nazwa: ");
    nowyPrzepis.Nazwa = Console.ReadLine();

    // Sprawdź, czy przepis o tej samej nazwie już istnieje
    if (przepisRepository.CzyNazwaPrzepisuIstnieje(nowyPrzepis.Nazwa))
    {
        Console.WriteLine($"Przepis o nazwie '{nowyPrzepis.Nazwa}' już istnieje w bazie danych. Nie można dodać zduplikowanego przepisu.");
        return;
    }


    Console.Write("Skladniki (wprowadzaj tekst, używaj znaków końca linii, zakończ wprowadzanie trzema pustymi liniami): ");
    StringBuilder skladnikiBuilder = new StringBuilder();
    int pusteLinieCounter = 0;

    while (true)
    {
        string linia = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(linia))
        {
            pusteLinieCounter++;

            if (pusteLinieCounter >= 3)
            {
                break;
            }
        }
        else
        {
            skladnikiBuilder.AppendLine(linia);
            pusteLinieCounter = 0;
        }
    }

    nowyPrzepis.Skladniki = skladnikiBuilder.ToString().Trim().Split(',').ToList();

    Console.Write("Tagi (oddzielone przecinkami): ");
    string tagiInput = Console.ReadLine();
    nowyPrzepis.Tagi = tagiInput.Split(',').ToList();

    Console.Write("Źródło: ");
    nowyPrzepis.Zrodlo = Console.ReadLine();

    Console.Write("Ścieżka do zdjęcia: ");
    nowyPrzepis.ZdjeciePath = Console.ReadLine();

    Console.Write("Uwagi (oddzielone średnikami): ");
    string uwagiInput = Console.ReadLine();
    nowyPrzepis.Uwagi = uwagiInput.Split(';').ToList();

    Console.Write("Ocena 1-10: ");
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

    Console.Write("Instrukcja (wprowadzaj tekst, używaj znaków końca linii, zakończ wprowadzanie trzema pustymi liniami): ");
    StringBuilder instrukcjaBuilder = new StringBuilder();
    int pusteLinieCounter1 = 0;

    while (true)
    {
        string linia = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(linia))
        {
            pusteLinieCounter1++;

            if (pusteLinieCounter1 >= 3)
            {
                break;
            }
        }
        else
        {
            instrukcjaBuilder.AppendLine(linia);
            pusteLinieCounter1 = 0;
        }
    }

    nowyPrzepis.Instrukcja = instrukcjaBuilder.ToString().Trim();

    Console.Write("Koszt (drogi/umiarkowany/tani): ");
    string kosztInput = Console.ReadLine();
    if (Enum.TryParse(typeof(Przepis.KosztEnum), kosztInput, out object koszt))
    {
        nowyPrzepis.Koszt = (Przepis.KosztEnum)koszt;
    }
    else
    {
        Console.WriteLine("Nieprawidłowy koszt. Ustawiono domyślną wartość NA.");
        nowyPrzepis.Koszt = Przepis.KosztEnum.NA;
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

    // Dodaj przepis do bazy danych
    przepisRepository.DodajPrzepis(nowyPrzepis);

    Console.WriteLine("Przepis dodany do bazy danych.\n");
}
static void WyświetlDane(PrzepisRepository przepisRepository)
{
    
    Console.WriteLine("Wprowadź odpowiednią cechę (tag) aby wyświetlić tylko pasujące przepisy lub pozostaw puste aby wyświetlić wszystkie.");
    string filtrTag = Console.ReadLine();

    List<Przepis> listaPrzepisow;

    if (string.IsNullOrWhiteSpace(filtrTag))
    {
        listaPrzepisow = przepisRepository.PobierzWszystkiePrzepisy();
    }
    else
    {
        listaPrzepisow = przepisRepository.PobierzPrzepisyWedługFiltru(filtrTag);
    }
    if (listaPrzepisow.Count!=0)
    {
        Console.WriteLine("Lista przepisów:");

        for (int i = 0; i < listaPrzepisow.Count; i++)
        {
            Console.WriteLine($"{i + 1}. {listaPrzepisow[i].Nazwa}");
        }

        Console.Write("Podaj numer przepisu do wyświetlenia: ");
        if (int.TryParse(Console.ReadLine(), out int numerPrzepisu) && numerPrzepisu > 0 && numerPrzepisu <= listaPrzepisow.Count)
        {
            Przepis wybranyPrzepis = listaPrzepisow[numerPrzepisu - 1];

            Console.WriteLine($"Dane przepisu: {wybranyPrzepis.Nazwa} \n");
            Console.WriteLine($"Skladniki: {string.Join(", ", wybranyPrzepis.Skladniki)} \n");
            Console.WriteLine($"Tagi: {string.Join(", ", wybranyPrzepis.Tagi)} \n");
            Console.WriteLine($"Daty przygotowania: {string.Join(", ", wybranyPrzepis.DatyPrzygotowania)} \n");
            Console.WriteLine($"Źródło: {wybranyPrzepis.Zrodlo} \n");
            Console.WriteLine($"Ścieżka do zdjęcia: {wybranyPrzepis.ZdjeciePath} \n");
            Console.WriteLine($"Uwagi: {string.Join(", ", wybranyPrzepis.Uwagi)}\n");
            Console.WriteLine($"Ocena: {wybranyPrzepis.Ocena} \n");
            Console.WriteLine($"Instrukcja: {wybranyPrzepis.Instrukcja}\n");
            Console.WriteLine($"Koszt: {wybranyPrzepis.Koszt}\n");
            Console.WriteLine($"Czas przygotowania: {wybranyPrzepis.CzasPrzygotowania} minut\n");
        }
        else
        {
            Console.WriteLine("Nieprawidłowy numer przepisu.");
        }
    }
    else 
    {
        Console.WriteLine("Brak przepisów spełniających określone kryteria.");
    }
    
}
static void ModyfikujDane(PrzepisRepository przepisRepository)
{
    Console.WriteLine("Wybierz przepis do modyfikacji:");

    var listaPrzepisow = przepisRepository.PobierzWszystkiePrzepisy();

    for (int i = 0; i < listaPrzepisow.Count; i++)
    {
        Console.WriteLine($"{i + 1}. {listaPrzepisow[i].Nazwa}");
    }

    Console.Write("Podaj numer przepisu do modyfikacji: ");
    if (int.TryParse(Console.ReadLine(), out int numerPrzepisu) && numerPrzepisu > 0 && numerPrzepisu <= listaPrzepisow.Count)
    {
        Przepis wybranyPrzepis = listaPrzepisow[numerPrzepisu - 1];

        Console.WriteLine($"Edytujesz przepis: {wybranyPrzepis.Nazwa}");
        Console.WriteLine("1. Usuń przepis");
        Console.WriteLine("2. Zmodyfikuj składniki");
        Console.WriteLine("3. Dodaj datę przygotowania");
        Console.WriteLine("4. Modyfikuj tagi");
        Console.WriteLine("5. Zmień ocenę");
        Console.WriteLine("6. Zmień instrukcję");
        Console.WriteLine("7. Usuń datę przygotowania");
        Console.WriteLine("8. Powrót do menu głównego\n");

        string choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                przepisRepository.UsunPrzepis(wybranyPrzepis);
                Console.WriteLine("Przepis usunięty z bazy danych.");
                break;
            case "2":
                Console.WriteLine("Modyfikacja składników:");
                Console.Write("Obecne składniki: ");
                Console.WriteLine(string.Join(", ", wybranyPrzepis.Skladniki));

                Console.WriteLine("Obecne składniki zostaną usunięte. Podaj nowe wartości i zakończ trzema enterami: ");
                StringBuilder noweSkladnikiBuilder = new StringBuilder();
                int pusteLinieCounterSkladniki = 0;

                while (true)
                {
                    string liniaSkladniki = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(liniaSkladniki))
                    {
                        pusteLinieCounterSkladniki++;

                        if (pusteLinieCounterSkladniki >= 3)
                        {
                            break;
                        }
                    }
                    else
                    {
                        noweSkladnikiBuilder.AppendLine(liniaSkladniki);
                        pusteLinieCounterSkladniki = 0;
                    }
                }

                wybranyPrzepis.Skladniki = noweSkladnikiBuilder.ToString().Trim().Split(',').ToList();
                Console.WriteLine("Składniki zostały zmienione.");
                break;
            case "3":
                Console.Write("Dodaj datę przygotowania (format dd/MM/yyyy): ");
                if (DateTime.TryParseExact(Console.ReadLine(), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime nowaData))
                {
                    wybranyPrzepis.DatyPrzygotowania.Add(nowaData);
                    przepisRepository.ZaktualizujPrzepis(wybranyPrzepis);
                    Console.WriteLine("Data dodana do listy.");
                }
                else
                {
                    Console.WriteLine("Nieprawidłowy format daty.");
                }
                break;
            case "4":
                Console.WriteLine("Modyfikuj tagi:");

                Console.WriteLine("Obecne tagi:");
                for (int i = 0; i < wybranyPrzepis.Tagi.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {wybranyPrzepis.Tagi[i]}");
                }

                Console.Write("Nowe tagi (oddzielone przecinkami): ");
                string noweTagiInput = Console.ReadLine();
                wybranyPrzepis.Tagi = noweTagiInput.Split(',').ToList();
                przepisRepository.ZaktualizujPrzepis(wybranyPrzepis);
                Console.WriteLine("Tagi zaktualizowane.\n");
                break;
            case "5":
                Console.Write("Nowa ocena: ");
                if (int.TryParse(Console.ReadLine(), out int nowaOcena))
                {
                    wybranyPrzepis.Ocena = nowaOcena;
                    przepisRepository.ZaktualizujPrzepis(wybranyPrzepis);
                    Console.WriteLine("Ocena została zmieniona.");
                }
                else
                {
                    Console.WriteLine("Nieprawidłowa ocena.");
                }
                break;
            case "6":
                Console.WriteLine("Zmiana instrukcji:");
                Console.WriteLine(wybranyPrzepis.Instrukcja);

                Console.WriteLine("Wprowadź nową instrukcję. Zakończ trzema enterami: ");
                StringBuilder nowaInstrukcjaBuilder = new StringBuilder();
                int pusteLinieCounterInstrukcja = 0;

                while (true)
                {
                    string liniaInstrukcja = Console.ReadLine();

                    if (string.IsNullOrWhiteSpace(liniaInstrukcja))
                    {
                        pusteLinieCounterInstrukcja++;

                        if (pusteLinieCounterInstrukcja >= 3)
                        {
                            break;
                        }
                    }
                    else
                    {
                        nowaInstrukcjaBuilder.AppendLine(liniaInstrukcja);
                        pusteLinieCounterInstrukcja = 0;
                    }
                }

                wybranyPrzepis.Instrukcja = nowaInstrukcjaBuilder.ToString().Trim();
                Console.WriteLine("Instrukcja została zmieniona.");
                break;
            case "7":
                Console.WriteLine("Usuń datę przygotowania:");

                if (wybranyPrzepis.DatyPrzygotowania.Count > 0)
                {
                    Console.WriteLine("Obecne daty przygotowania:");
                    for (int i = 0; i < wybranyPrzepis.DatyPrzygotowania.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. {wybranyPrzepis.DatyPrzygotowania[i]:dd/MM/yyyy}");
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
        Console.WriteLine("Nieprawidłowy numer przepisu.");
    }
}
static void ImportujPrzepisy(PrzepisRepository przepisRepository)
{
    // Wczytanie zawartości pliku JSON

    Console.Write("Ścieżka do pliku JSON: ");
    string JsonPath = Console.ReadLine();
    string json = File.ReadAllText(JsonPath);

    //PrzepisRepository importer = new ImportJsonToMongoDB();
    List<Przepis> przepisy = przepisRepository.ImportujJson(json);
    przepisRepository.DodajPrzepisyDoBazy(przepisy);

    Console.WriteLine("Import zakończony pomyślnie.");
}
static void EksportujPrzepisy(PrzepisRepository przepisRepository)
{
    Console.WriteLine("Wprowadź odpowiednią cechę aby wyeksportować przepisy o określonym tagu lub pozostaw puste aby wyeksportować całość");
    string filtrTag = Console.ReadLine();

    List<Przepis> listaPrzepisow;

    if (string.IsNullOrWhiteSpace(filtrTag))
    {
        listaPrzepisow = przepisRepository.PobierzWszystkiePrzepisy();
    }
    else
    {
        listaPrzepisow = przepisRepository.PobierzPrzepisyWedługFiltru(filtrTag);
    }

    if (listaPrzepisow.Count != 0)
    {
        Console.WriteLine("W jakim formacie danych chcesz zapisać plik?");
        Console.WriteLine("1. CSV");
        Console.WriteLine("2. JSON");
        string typExport = Console.ReadLine();

        switch (typExport)
        {
            case "1":
                Console.WriteLine("Wprowadź ścieżkę do pliku");
                string exportCsvPath = Console.ReadLine();

                przepisRepository.ExportToCsv(listaPrzepisow, exportCsvPath);

                Console.WriteLine("Eksport zakończony.");
                break;
            case "2":
                Console.WriteLine("Wprowadź ścieżkę do pliku");
                string exportJsonPath = Console.ReadLine();

                przepisRepository.ExportToJson(listaPrzepisow, exportJsonPath);

                Console.WriteLine("Eksport zakończony.");
                break;
            default:
                Console.WriteLine("Nieprawidłowy wybór. ");
                break;
        }
    }
    else
    {
        Console.WriteLine("Brak przepisów do eksportu");
    }

}
static void UsunPrzepisy(PrzepisRepository przepisRepository)
{
    Console.WriteLine("UWAGA: Ta opcja spowoduje usunięcie wszystkich przepisów z bazy danych!");
    Console.Write("Aby potwierdzić, dwukrotnie wpisz i zatwierdź swoje imię: ");

    string imie1 = Console.ReadLine();
    string imie2 = Console.ReadLine();

    if (imie1.Equals(imie2, StringComparison.OrdinalIgnoreCase) && !string.IsNullOrEmpty(imie1))
    {
        Console.WriteLine("Potwierdzenie przyjęte. Usuwanie wszystkich przepisów...");

        // Pobierz wszystkie przepisy z bazy danych
        var listaPrzepisow = przepisRepository.PobierzWszystkiePrzepisy();

        // Usuń każdy przepis z bazy danych
        foreach (var przepis in listaPrzepisow)
        {
            przepisRepository.UsunPrzepis(przepis);
        }

        Console.WriteLine("Wszystkie przepisy zostały usunięte.");
    }
    else
    {
        Console.WriteLine("Potwierdzenie nieudane. Brak zmian w bazie danych.");
    }
}


using NLog;
string path = Directory.GetCurrentDirectory() + "//nlog.config";

// create instance of Logger
var logger = LogManager.Setup().LoadConfigurationFromFile(path).GetCurrentClassLogger();

logger.Info("Program started");

string file = "mario.csv";

if (!File.Exists(file))
{
    logger.Error("File does not exist: {File}", file);
}
else
{
    // create parallel lists of character details
    // lists are used since we do not know number of lines of data
    List<UInt64> Ids = [];
    List<string> Names = [];
    List<string> Descriptions = [];
    List<string> Species = [];
    List<string> FirstAppearance = [];
    List<UInt64> YearCreated = [];

    // to populate the lists with data, read from the data file
    try
    {
        StreamReader sr = new(file);
        // first line contains column headers
        sr.ReadLine();
        while (!sr.EndOfStream)
        {
            string? line = sr.ReadLine();
            if (line is not null)
            {
                string[] characterDetails = line.Split(',');
                Ids.Add(UInt64.Parse(characterDetails[0]));
                Names.Add(characterDetails[1]);
                Descriptions.Add(characterDetails[2]);
                Species.Add(characterDetails[3]);
                FirstAppearance.Add(characterDetails[4]);
                YearCreated.Add(UInt64.Parse(characterDetails[5]));
            }        
        }
        sr.Close();
    }
    catch (Exception ex)
    {
        logger.Error(ex.Message);
    }
    string? choice;
    do
    {
        // display choices to user
        Console.WriteLine("1) Add Character");
        Console.WriteLine("2) Display All Characters");
        Console.WriteLine("Enter to quit");

        // input selection
        choice = Console.ReadLine();
        logger.Info("User choice: {Choice}", choice);

        if (choice == "1")
        {
            Console.WriteLine("Enter new character name: ");
            string? Name = Console.ReadLine();
            if (!string.IsNullOrEmpty(Name)){
                List<string> LowerCaseNames = Names.ConvertAll(n => n.ToLower());
                if (LowerCaseNames.Contains(Name.ToLower()))
                {
                    logger.Info($"Duplicate name {Name}");
                }
                else
                {
                    UInt64 Id = Ids.Max() + 1;
                    // input character description
                    Console.WriteLine("Enter description:");
                    string? Description = Console.ReadLine();
                    Console.WriteLine("Enter species: ");
                    string? characterSpecies = Console.ReadLine();
                    Console.WriteLine("Enter character's first appearance: ");
                    string? characterFirstAppearance = Console.ReadLine();
                    Console.WriteLine("Enter year created: ");
                    string? characterYearCreated = Console.ReadLine(); 

                    Console.WriteLine($"{Id}, {Name}, {Description}, {characterSpecies}, {characterFirstAppearance}, {characterYearCreated}");                }
            } else {
                logger.Error("You must enter a name");
            }
        }
        else if (choice == "2")
        {
            for (int i = 0; i < Ids.Count; i++)
            {
                // display character details
                Console.WriteLine($"Id: {Ids[i]}");
                Console.WriteLine($"Name: {Names[i]}");
                Console.WriteLine($"Description: {Descriptions[i]}");
                Console.WriteLine($"Species: {Species[i]}");
                Console.WriteLine($"First Appearance: {FirstAppearance[i]}");
                Console.WriteLine($"Year Created: {YearCreated[i]}");
                Console.WriteLine();
            }
        }
    } while (choice == "1" || choice == "2");
}

logger.Info("Program ended");
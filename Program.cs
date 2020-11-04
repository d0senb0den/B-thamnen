using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Båthamnen
{
    class Program
    {
        static Boat[,] harbor = new Boat[64, 2];
        static readonly Random random = new Random();
        static int rejectedBoats = 0;
        static int dayCount = 0;
        private static bool hasReseted;
        static void Main(string[] args)
        {
            ReadFromFile();
            var hasLoaded = ReadFromFile();
            do
            {
                if (!hasLoaded && !hasReseted)
                {
                    NextDay();
                }
                int weightInHarbor = 0, openSpot = 0, averageSpeed = 0, totalMaxSpeed = 0, totalBoats = 0;
                Console.WriteLine("Plats\t\tBåttyp\t\t\tID\t\tVikt(kg)\tMaxhast(km/h)\tÖvrigt\n");
                for (int i = 0; i < harbor.GetLength(0); i++) // Kollar igenom alla platser i array.
                {
                    if (harbor[i, 0] != null) // Om platsen är upptagen.
                    {
                        weightInHarbor += harbor[i, 0].weight;
                        totalBoats++;
                        totalMaxSpeed += (int)(harbor[i, 0].maxSpeed * 1.852);
                        averageSpeed = totalMaxSpeed / totalBoats;
                        if (harbor[i, 0].size > 1) // allt som är större än motorbåtar och roddbåtar.
                        {
                            Console.WriteLine(i + 1 + "-" + (i + harbor[i, 0].size) + "\t\t" + harbor[i, 0].GetType().Name.PadRight(10) + "\t\t" + harbor[i, 0]);
                            i += harbor[i, 0].size - 1; // Skippar index med innehåll av samma båt.
                        }
                        else // motorbåtar och roddbåtar.
                        {
                            Console.WriteLine(i + 1 + "\t\t" + harbor[i, 0].GetType().Name.PadRight(10) + "\t\t" + harbor[i, 0]); // Sista harbor[i, 0] tar från ToString() i basclass Boat.
                            if (harbor[i, 1] != null)
                            {
                                Console.WriteLine(i + 1 + "\t\t" + harbor[i, 1].GetType().Name.PadRight(10) + "\t\t" + harbor[i, 1]);
                                weightInHarbor += harbor[i, 1].weight;
                                totalBoats++;
                                totalMaxSpeed += (int)(harbor[i, 1].maxSpeed * 1.852);
                                averageSpeed = totalMaxSpeed / totalBoats;
                            }
                            i += harbor[i, 0].size - 1;
                        }
                    }
                    else
                    {
                        Console.WriteLine(i + 1 + "\t\t" + "Tom");
                        openSpot++;
                    }
                }
                Console.WriteLine($"\nAvvisade båtar: {rejectedBoats}\t\tDag: {dayCount}\t\t\tLediga platser: {openSpot}");
                Console.WriteLine($"Total vikt(kg): {weightInHarbor}\t\tMedelhastighet: {averageSpeed}");
                Console.WriteLine($"\nRoddbåtar: {CountBoats<Rowboat>()}\t\tMotorbåtar: {CountBoats<Motorboat>()}\t\tSegelbåtar: {CountBoats<Sailboat>()}\t\tLastfartyg: {CountBoats<Cargoship>()}");
                WriteToFile();
                if(hasReseted)
                {
                    hasReseted = false;
                }
                var input = Console.ReadKey(true);
                if (input.KeyChar == 'r')  // Tryck "r" för att återställa programmet.
                {
                    harbor = new Boat[64, 2];
                    rejectedBoats = 0;
                    dayCount = 0;
                    hasReseted = true;
                }
                if(hasLoaded)
                {
                    hasLoaded = false;
                }
                Console.Clear();
            } while (true);


        }
        static void NextDay()
        {
            ReduceDaysInPort();
            RemoveBoat();
            CreateAndPlaceBoats();
            dayCount++;
        }
        static void CreateAndPlaceBoats()
        {

            for (int i = 0; i < 5; i++)
            {
                int boatType = random.Next(1, 5);

                if (boatType == 1)
                {
                    PlaceBoat(CreateRowboat());
                }
                else if (boatType == 2)
                {
                    PlaceBoat(CreateMotorboat());
                }
                else if (boatType == 3)
                {
                    PlaceBoat(CreateSailboat());
                }
                else if (boatType == 4)
                {
                    PlaceBoat(CreateCargoship());
                }
            }
        }

        private static Rowboat CreateRowboat()
        {
            string identification = RandomID("R-");
            int weight = random.Next(100, 300);
            int maxSpeed = random.Next(0, 3);
            int maxPassengers = random.Next(1, 6);
            Rowboat boat = new Rowboat(identification, weight, maxSpeed, maxPassengers);
            return boat;
        }
        private static Motorboat CreateMotorboat()
        {
            string identification = RandomID("M-");
            int weight = random.Next(200, 3000);
            int maxSpeed = random.Next(0, 60);
            int horsePower = random.Next(10, 1000);
            Motorboat boat = new Motorboat(identification, weight, maxSpeed, horsePower);
            return boat;
        }
        private static Sailboat CreateSailboat()
        {
            string identification = RandomID("S-");
            int weight = random.Next(800, 6000);
            int maxSpeed = random.Next(0, 12);
            int length = random.Next(10, 60);
            Sailboat boat = new Sailboat(identification, weight, maxSpeed, length);
            return boat;
        }
        private static Cargoship CreateCargoship()
        {
            string identification = RandomID("C-");
            int weight = random.Next(3000, 20000);
            int maxSpeed = random.Next(0, 20);
            int containers = random.Next(0, 500);
            Cargoship boat = new Cargoship(identification, weight, maxSpeed, containers);
            return boat;
        }
        private static void PlaceBoat(Boat boat)
        {
            bool placedBoat = false;
            if (boat.size == 4)
            {
                for (int i = harbor.GetLength(0) - 1; i > 0 + boat.size - 1; i--)
                {
                    bool hasFreeSpotForCargoship = harbor[i, 0] == null && harbor[i - 1, 0] == null && harbor[i - 2, 0] == null && harbor[i - 3, 0] == null;
                    if (hasFreeSpotForCargoship)
                    {

                        harbor[i, 0] = boat;
                        harbor[i - 1, 0] = boat;
                        harbor[i - 2, 0] = boat;
                        harbor[i - 3, 0] = boat;
                        placedBoat = true;
                        break;
                    }
                }
            }
            if (boat.size < 4)
            {
                for (int i = 0; i < harbor.GetLength(0) - boat.size + 1; i++)
                {
                    if (boat.size == 2)
                    {
                        bool hasFreeSpotForSailboat = harbor[i, 0] == null && harbor[i + 1, 0] == null;
                        if (hasFreeSpotForSailboat)
                        {
                            harbor[i, 0] = boat;
                            harbor[i + 1, 0] = boat;
                            placedBoat = true;
                            break;
                        }
                    }
                    else if (boat.size == 1)
                    {
                        bool hasFreeSpotForMotorboatOrRowboat = harbor[i, 0] == null;
                        if (harbor[i, 0] != null && harbor[i, 1] == null)
                        {
                            if (harbor[i, 0] is Rowboat && boat is Rowboat)
                            {
                                harbor[i, 1] = boat;
                                placedBoat = true;
                                break;
                            }
                        }
                        if (hasFreeSpotForMotorboatOrRowboat)
                        {
                            harbor[i, 0] = boat;
                            placedBoat = true;
                            break;
                        }
                    }
                }
            }
            if (placedBoat == false)
            {
                rejectedBoats++;
            }
        }

        private static void RemoveBoat()
        {
            for (int i = 0; i < harbor.GetLength(0); i++)
            {
                for (int j = 0; j < harbor.GetLength(1); j++)
                {
                    if (harbor[i, j] != null && harbor[i, j].daysInPort == 0)
                    {
                        harbor[i, j] = null;
                    }
                }
            }
        }

        private static void ReduceDaysInPort()
        {
            for (int i = 0; i < harbor.GetLength(0); i++)
            {
                if (harbor[i, 0] != null)
                {
                    harbor[i, 0].daysInPort--;
                    if (harbor[i, 1] != null)
                    {
                        harbor[i, 1].daysInPort--;
                    }
                    i += harbor[i, 0].size - 1;
                }

            }
        }
        private static int CountBoats<T>()  // Generisk metod.
        {
            int Count = 0;
            for (int i = 0; i < harbor.GetLength(0); i++)
            {
                if (harbor[i, 0] != null && harbor[i, 0] is T)
                {
                    Count++;
                    if (harbor[i, 1] != null && harbor[i, 1] is T)
                    {
                        Count++;
                    }
                    i += harbor[i, 0].size - 1;
                }
            }

            return Count;
        }
        private static string RandomID(string prefix)
        {
            string alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZÅÄÖ";
            bool IDexists;
            string randomID;
            do
            {
                var randomLetter1 = alphabet[random.Next(0, alphabet.Length)];
                var randomLetter2 = alphabet[random.Next(0, alphabet.Length)];
                var randomLetter3 = alphabet[random.Next(0, alphabet.Length)];
                randomID = prefix + randomLetter1 + randomLetter2 + randomLetter3;

                IDexists = CheckIfIdExists(randomID);

            } while (IDexists); //Bool för att se till att båtarna inte får samma ID.
            return randomID;
        }

        private static bool CheckIfIdExists(string randomID)
        {
            for (int i = 0; i < harbor.GetLength(0); i++)
            {
                for (int j = 0; j < harbor.GetLength(1); j++)
                {
                    if (harbor[i, j] != null)
                    {
                        if (randomID == harbor[i, j].identification)
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        private static void WriteToFile()
        {
            string filePath = "Hamnen.txt";
            // platser, båttyp(GetType), props, unika props(virtual metod + ovveride).
            List<string> output = new List<string>();
            for (int i = 0; i < harbor.GetLength(0); i++)
            {
                for (int j = 0; j < harbor.GetLength(1); j++)
                {
                    if (harbor[i, j] != null)
                    {
                        var b = harbor[i, j];
                        output.Add($"{i},{j},{b.GetType().Name},{b.identification},{b.weight},{b.maxSpeed},{b.UniqueProp()},{b.daysInPort}");
                    }
                }
            }
            File.WriteAllLines(filePath, output);
        }
        private static bool ReadFromFile()
        {
            string filePath = "Hamnen.txt";

            if (File.Exists(filePath))
            {
                List<Boat> boat = new List<Boat>();

                List<string> lines = File.ReadAllLines(filePath).ToList();
                foreach (string line in lines)
                {
                    string[] entries = line.Split(',');

                    if (entries[2] == "Rowboat")
                    {
                        Boat b = new Rowboat(entries[3], int.Parse(entries[4]), int.Parse(entries[5]), int.Parse(entries[6]), int.Parse(entries[7]));
                        harbor[int.Parse(entries[0]), int.Parse(entries[1])] = b;
                    }
                    else if (entries[2] == "Motorboat")
                    {
                        Boat b = new Motorboat(entries[3], int.Parse(entries[4]), int.Parse(entries[5]), int.Parse(entries[6]), int.Parse(entries[7]));
                        harbor[int.Parse(entries[0]), int.Parse(entries[1])] = b;
                    }
                    else if (entries[2] == "Sailboat")
                    {
                        Boat b = new Sailboat(entries[3], int.Parse(entries[4]), int.Parse(entries[5]), int.Parse(entries[6]), int.Parse(entries[7]));
                        harbor[int.Parse(entries[0]), int.Parse(entries[1])] = b;
                    }
                    else if (entries[2] == "Cargoship")
                    {
                        Boat b = new Cargoship(entries[3], int.Parse(entries[4]), int.Parse(entries[5]), int.Parse(entries[6]), int.Parse(entries[7]));
                        harbor[int.Parse(entries[0]), int.Parse(entries[1])] = b;
                    }
                }
                return true;
            }
            return false;
        }
    }
}

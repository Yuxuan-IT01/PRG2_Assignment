using S10270275_PRG2Assignment;
using System.Collections.Generic;


Console.WriteLine("testing");
// yuxuan qn1,4,7,8

// qn1
public void LoadAirlines(string filePath)
{
    foreach (var line in File.ReadAllLines(filePath))
    {
        var parts = line.Split(',');
        airlines[parts[0]] = new Airline(parts[0], parts[1]);
        terminal.AddAirline(airlines[parts[0]]);
    }
    Console.WriteLine("Airlines loaded successfully.");
}

public void LoadBoardingGates(string filePath)
{
    foreach (var line in File.ReadAllLines(filePath))
    {
        var parts = line.Split(',');

        string gateName = parts[0];
        bool supportsCFFT = parts[1] == "true";
        bool supportsDDJB = parts[2] == "true";
        bool supportsLWTT = parts[3] == "true";

        boardingGates[gateName] = new BoardingGate(gateName, supportsCFFT, supportsDDJB, supportsLWTT);
        terminal.AddBoardingGate(boardingGates[gateName]);
    }
    Console.WriteLine("Boarding gates loaded successfully.");
}

public void LoadFlights(string filePath)
{
    foreach (var line in File.ReadAllLines(filePath))
    {
        var parts = line.Split(',');

        string flightNumber = parts[0];
        string airlineCode = parts[1];
        string origin = parts[2];
        string destination = parts[3];
        string timeString = parts[4];
        string status = parts[5];

        DateTime expectedTime;
        try
        {
            expectedTime = DateTime.Parse(timeString);
        }
        catch (FormatException)
        {
            Console.WriteLine($"Invalid time format for flight {flightNumber}. Skipping this entry.");
            continue;
        }

        flights[flightNumber] = new Flight(flightNumber, origin, destination, expectedTime, status);
        if (airlines.ContainsKey(airlineCode))
        {
            airlines[airlineCode].AddFlight(flights[flightNumber]);
        }
        else
        {
            Console.WriteLine($"Airline code {airlineCode} not found for flight {flightNumber}. Skipping this entry.");
            continue;
        }

        terminal.AddFlight(flights[flightNumber]);
    }
    Console.WriteLine("Flights loaded successfully.");
}
// qn4
public void ListBoardingGates()
{
    foreach (var gate in boardingGates.Values)
    {
        Console.WriteLine(gate);
    }
}
// qn7
public void DisplayFlightsByAirline()
{
    Console.Write("Enter Airline Code: ");
    string airlineCode = Console.ReadLine();

    if (!airlines.ContainsKey(airlineCode))
    {
        Console.WriteLine("Airline not found.");
        return;
    }

    var airline = airlines[airlineCode];
    foreach (var flight in airline.Flights)
    {
        Console.WriteLine(flight);
    }
}
// qn8
public void ModifyFlightDetails()
{
    Console.Write("Enter Flight Number to modify: ");
    string flightNumber = Console.ReadLine();
    if (!flights.ContainsKey(flightNumber))
    {
        Console.WriteLine("Flight not found.");
        return;
    }
    var flight = flights[flightNumber];
    Console.Write("Enter new Origin (leave blank to keep current): ");
    string origin = Console.ReadLine();
    if (origin.Length > 0)
    {
        flight.Origin = origin;
    }
    Console.Write("Enter new Destination (leave blank to keep current): ");
    string destination = Console.ReadLine();
    if (destination.Length > 0)
    {
        flight.Destination = destination;
    }
    Console.Write("Enter new Expected Departure/Arrival Time (yyyy-MM-dd HH:mm) (leave blank to keep current): ");
    string timeInput = Console.ReadLine();
    if (timeInput.Length > 0)
    {
        DateTime newTime;
        Console.Write("Confirm new time (yes/no): ");
        string confirm = Console.ReadLine();
        if (confirm == "yes")
        {
            newTime = DateTime.Parse(timeInput);
            flight.ExpectedTime = newTime;
        }
    }
    Console.Write("Enter new Status (leave blank to keep current): ");
    string status = Console.ReadLine();
    if (status.Length > 0)
    {
        flight.Status = status;
    }
    Console.WriteLine("Flight details updated successfully.");
}


// Kaiwen qn2,3,5,6,9

// template
//foreach (string Line in File.ReadLines("flights.csv").Skip(1))
//{
//    String[] split = Line.Split(",");
//}
// qn1 temp

Terminal terminal = new Terminal("Changi Terminal 5");
void initairline(Terminal terminal)
{
    foreach (string Line in File.ReadLines("airlines.csv").Skip(1))
    {
        String[] split = Line.Split(",");
        Airline airlinetemp = new Airline(split[0], split[1]);
        terminal.AddAirline(airlinetemp);
        Console.WriteLine("great success");
    }
}

void initboarding(Terminal terminal)
{
    foreach (string Line in File.ReadLines("boardinggates.csv").Skip(1))
    {
        String[] split = Line.Split(",");
        BoardingGate boarding = new BoardingGate(null, split[0], Convert.ToBoolean(split[1]), Convert.ToBoolean(split[2]), Convert.ToBoolean(split[3]));
        terminal.AddBoardingGate(boarding);
    }
}

// qn2 
void initflight(Terminal terminal)
{
    foreach (string Line in File.ReadLines("flights.csv").Skip(1))
    {
        String[] split = Line.Split(",");
        String[] numsplit = split[0].Split(" ");
        Airline airline = terminal.Airlines[numsplit[0]];
        if (split[4] == "DDJB")
        {
            Flight ftemp = new DDJBFlight(split[0], split[1], split[2], Convert.ToDateTime(split[3]), "Scheduled", 300);
            terminal.Flights[split[0]] = ftemp;
        }
        else if (split[4] == "CFFT")
        {
            Flight ftemp = new CFFTFlight(split[0], split[1], split[2], Convert.ToDateTime(split[3]), "Scheduled", 150);
            terminal.Flights[split[0]] = ftemp;
        }
        else if (split[4] == "LWTT")
        {
            Flight ftemp = new LWTTFlight(split[0], split[1], split[2], Convert.ToDateTime(split[3]), "Scheduled", 500);
            terminal.Flights[split[0]] = ftemp;
        }
        else
        {
            Flight ftemp = new NORMFlight(split[0], split[1], split[2], "Scheduled", Convert.ToDateTime(split[3]));
            terminal.Flights[split[0]] = ftemp;
        }
    }
}



//foreach (KeyValuePair<string, Flight> kvp in FlightDict)
//{
//    Console.WriteLine(kvp.Value.Status);
//}

// qn3 
void displayflight(Terminal terminal)
{
    Console.WriteLine("=============================================" + '\n' +"List of Flights for Changi Airport Terminal 5" + '\n' +"=============================================");
    Console.WriteLine("{0,-18}{1,-22}{2,-22}{3,-23}{4,-15}","Flight number","Airline Name","Origin","Destination","Expected Departure/Arrival Time");
    foreach (KeyValuePair<string, Flight> flightkvp in terminal.Flights)
    {
        String[] splitkey = flightkvp.Key.Split(" ");
        Console.WriteLine("{0,-18}{1,-22}{2,-22}{3,-23}{4,-15}", flightkvp.Value.FlightNumber, terminal.Airlines[splitkey[0]].Name,flightkvp.Value.Origin, flightkvp.Value.Destination, flightkvp.Value.expectedTime);
    }
}
// qn5
void assigngate(Terminal terminal)
{
    Console.Write("Enter Flight Number: ");
    string Flightnum = Console.ReadLine();
    Console.Write("Enter Boarding Gate Name: ");
    string Boardingnum = Console.ReadLine();

    if (terminal.Flights.ContainsKey(Flightnum) & (terminal.boardingGates.ContainsKey(Boardingnum.ToUpper())))
    {
        Console.WriteLine("Flight Number: {0}", terminal.Flights[Flightnum].FlightNumber);
        Console.WriteLine("Origin: {0}", terminal.Flights[Flightnum].Origin);
        Console.WriteLine("Expected Time: {0}", terminal.Flights[Flightnum].expectedTime);
        Console.WriteLine(Convert.ToString(terminal.Flights[Flightnum].GetType()));
        string type = Convert.ToString(terminal.Flights[Flightnum].GetType());
        string[] typesplit = type.Split(".");
        if (typesplit[1] == "CFFTFlight")
        {
            Console.WriteLine("Special Request Code: {0}", "CFFT");
        }
        else if (typesplit[1] == "DDJBFlight")
        {
            Console.WriteLine("Special Request Code: {0}", "DDJB");
        }
        else if (typesplit[1] == "LWTTFlight")
        {
            Console.WriteLine("Special Request Code: {0}", "LWTT");
        }
        else
        {
            Console.WriteLine("Special Request Code: {0}", "None");
        }
        Console.WriteLine("Boarding Gate Name: {0}", terminal.boardingGates[Boardingnum].gateName);
        Console.WriteLine("Supports DDJB: {0}", terminal.boardingGates[Boardingnum].supportsDDJB);
        Console.WriteLine("Supports CFFT: {0}", terminal.boardingGates[Boardingnum].supportsCFFT);
        Console.WriteLine("Supports LWTT: {0}", terminal.boardingGates[Boardingnum].supportsLWTT);
        Console.WriteLine("Would you like to update the status of the flight? (Y/N)");
        string option = Console.ReadLine();
        if (option == "Y")
        {
            List<string> statuslist = new List<string>() {"Delayed","Boarding","On Time"};
            Console.WriteLine("1.Delayed" + '\n' + "2.Boarding" + '\n' + "3.On Time");
            Console.Write("Please select the new status of the flight: ");
            int statusoption = Convert.ToInt32(Console.ReadLine());
            terminal.Flights[Flightnum].Status = statuslist[statusoption - 1];
            if (typesplit[1] == "CFFTFlight")
            {
                terminal.boardingGates[Boardingnum].Flight = new CFFTFlight(Flightnum, terminal.Flights[Flightnum].Origin, terminal.Flights[Flightnum].Destination, terminal.Flights[Flightnum].expectedTime, statuslist[statusoption -1], 150);
            }
            else if (typesplit[1] == "DDJBFlight")
            {
                terminal.boardingGates[Boardingnum].Flight = new DDJBFlight(Flightnum, terminal.Flights[Flightnum].Origin, terminal.Flights[Flightnum].Destination, terminal.Flights[Flightnum].expectedTime, statuslist[statusoption - 1], 300);
            }
            else if (typesplit[1] == "LWTTFlight")
            {
                terminal.boardingGates[Boardingnum].Flight = new LWTTFlight(Flightnum, terminal.Flights[Flightnum].Origin, terminal.Flights[Flightnum].Destination, terminal.Flights[Flightnum].expectedTime, statuslist[statusoption - 1], 500);
            }
            else
            {
                terminal.boardingGates[Boardingnum].Flight = new NORMFlight(Flightnum, terminal.Flights[Flightnum].Origin, terminal.Flights[Flightnum].Destination, statuslist[statusoption - 1], terminal.Flights[Flightnum].expectedTime);
            }            
        }
        else if (option == "N")
        {
            if (typesplit[1] == "CFFTFlight")
            {
                terminal.boardingGates[Boardingnum].Flight = new CFFTFlight(Flightnum, terminal.Flights[Flightnum].Origin, terminal.Flights[Flightnum].Destination, terminal.Flights[Flightnum].expectedTime, terminal.Flights[Flightnum].Status, 150);
            }
            else if (typesplit[1] == "DDJBFlight")
            {
                terminal.boardingGates[Boardingnum].Flight = new DDJBFlight(Flightnum, terminal.Flights[Flightnum].Origin, terminal.Flights[Flightnum].Destination, terminal.Flights[Flightnum].expectedTime, terminal.Flights[Flightnum].Status, 300);
            }
            else if (typesplit[1] == "LWTTFlight")
            {
                terminal.boardingGates[Boardingnum].Flight = new LWTTFlight(Flightnum, terminal.Flights[Flightnum].Origin, terminal.Flights[Flightnum].Destination, terminal.Flights[Flightnum].expectedTime, terminal.Flights[Flightnum].Status, 500);
            }
            else
            {
                terminal.boardingGates[Boardingnum].Flight = new NORMFlight(Flightnum, terminal.Flights[Flightnum].Origin, terminal.Flights[Flightnum].Destination, terminal.Flights[Flightnum].Status, terminal.Flights[Flightnum].expectedTime);
            }
        }
        Console.WriteLine("Flight {0} has been assigned to Boarding Gate {1}!", Flightnum, Boardingnum);
    }

}
// qn6
void createflight(Terminal terminal)
{
    List<string> flighttype = new List<string>() {"DDJB","CFFT","LWTT","None"};
    try
    {
        Console.Write("Enter Flight Number: ");
        string flightnum = Console.ReadLine();
        Console.Write("Enter Origin: ");
        string origin = Console.ReadLine();
        Console.Write("Enter Destination: ");
        string destination = Console.ReadLine();
        Console.Write("Enter Expected Departure/Arrival Time (dd/mm/yyyy hh:mm): ");
        DateTime datetime = DateTime.Parse(Console.ReadLine());
        Console.Write("Enter Special Request Code (CFFT/DDJB/LWTT/None): ");
        string requestcode = Console.ReadLine();
        String[] split = flightnum.Split(" ");
        Airline airline = terminal.Airlines[split[0]];
        if (new[] { "DDJB", "CFFT", "LWTT", "NONE" }.Contains(requestcode.ToUpper()))
        {
            if (requestcode == "CFFT")
            {
                Flight CFFT = new CFFTFlight(flightnum, origin, destination, datetime, "Scheduled", 150);
                terminal.Flights[flightnum] = CFFT;
            }
            else if (requestcode == "DDJBFlight")
            {
                Flight DDJB = new DDJBFlight(flightnum, origin, destination, datetime, "Scheduled", 300);
                terminal.Flights[flightnum] = DDJB;
            }
            else if (requestcode == "LWTTFlight")
            {
                Flight LWTT = new LWTTFlight(flightnum, origin, destination, datetime, "Scheduled", 500);
                terminal.Flights[flightnum] = LWTT;
            }
            else
            {
                Flight NORM = new NORMFlight(flightnum, origin, destination, "Scheduled", datetime);
                terminal.Flights[flightnum] = NORM;
            }
            Console.WriteLine("Flight {0} has been added!", flightnum);
        }
        else
        {
            Console.WriteLine("Invalid option, please choose one of these 4 CFFT/DDJB/LWTT/None");
        }
    }
    catch (Exception e)
    {
        Console.WriteLine("Error: "+ e.Message);
    }
}
// qn9
// find if flight number is in boarding gate, then it would put the boarding gate name instead otherwise it would put unassigned there
void displayschedule(Terminal terminal)
{
    List<Flight> flightslist = terminal.Flights.Values.ToList();
    flightslist.Sort();
    Console.WriteLine("=============================================" + '\n' + "List of Flights for Changi Airport Terminal 5" + '\n' + "=============================================");
    Console.WriteLine("{0,-18}{1,-22}{2,-22}{3,-23}{4,-25}{5,-20}{6,-20}", "Flight number", "Airline Name", "Origin", "Destination", "Expected Departure/Arrival Time","Status","Boarding Gate");
    foreach (Flight flight in flightslist)
    {
        String[] split = flight.FlightNumber.Split(" ");
        string gatenum = "unassigned";
        foreach (var gatedata in terminal.boardingGates)
        {
            if (gatedata.Value.Flight != null && gatedata.Value.Flight.FlightNumber == flight.FlightNumber)
            {
                gatenum = gatedata.Value.gateName;
            }
        }
        Console.WriteLine("{0,-18}{1,-22}{2,-22}{3,-23}{4,-25}{5,-26}{6,-20}", flight.FlightNumber, terminal.Airlines[split[0]].Name, flight.Origin, flight.Destination, flight.expectedTime, flight.Status, gatenum);
    }
}

//advanced questions 
// QN1 

// QN2

// running code for menu
initairline(terminal);
initboarding(terminal);
initflight(terminal);
void displaymenu()
{
    Console.WriteLine("=============================================" + '\n' + "Welcome to Changi Airport Terminal 5" + '\n' + "=============================================" + '\n' + "1. List All Flights" + '\n' + "2. List Boarding Gates" + '\n' + "3. Assign a Boarding Gate to a Flight" + '\n' +"4. Create Flight" + '\n' +"5. Display Airline Flights" + '\n' +"6. Modify Flight Details"+'\n'+"7. Display Flight Schedule"+'\n'+"0. Exit" + '\n');
}



while (true)
{
    try
    {
        displaymenu();
        Console.Write("Please select your option:");
        int option = Convert.ToInt32(Console.ReadLine());
        if (option == 1)
        {
            displayflight(terminal);
        }
        else if (option == 2)
        {

        }
        else if (option == 3)
        {
            assigngate(terminal);
        }
        else if (option == 4)
        {
            createflight(terminal);
        }
        else if (option == 5)
        {

        }
        else if (option == 6)
        {

        }
        else if (option == 7)
        {
            displayschedule(terminal);
        }
        else if (option == 0)
        {
            return;
        }
        else
        {
            Console.WriteLine("Invalid option, please write the number of the above options");
        }
    }
    catch (Exception e)
    {
        Console.WriteLine("Error: " +e.Message);
    }
}


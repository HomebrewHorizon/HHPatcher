using System;
using System.Threading;

class HHPatcher
{
    static string version = "HHPatcher v1.0.0"; // Version Indicator
    static string[] consoles = { "Wii", "Wii U", "DSi", "3DS" };
    static string selectedConsole = "";

    static string[][] apps = {
        new string[] { "ProxWii", "LinkMii", "CitraWii", "XError", "EPU-Wii", "WiiBoot", "ARemi Lite", "WiiLib", "TTRC", "BSNV", "AeroRadio" },
        new string[] { "ARemi Lite", "ARemi Pro", "ARemi Pro-X", "LK App", "LinkU", "EPU-WiiU", "BootU" },
        new string[] { "ActionStrap" },
        new string[] { "Nothing yet!" } // 3DS placeholder (unselectable)
    };

    static bool[] selectedApps;

    static void Main()
    {
        DrawFrame(version); // Display Version on Startup
        SelectConsole();
        ShowAppSelection();
    }

    static void ClearScreen()
    {
        Console.Clear();
    }

    static void DrawFrame(string title)
    {
        Console.Clear();
        Console.WriteLine("+==================================+");
        Console.WriteLine($"| {title,-30} |");
        Console.WriteLine("+==================================+");
    }

    static void SelectConsole()
    {
        ClearScreen();
        Console.WriteLine("+========================+");
        Console.WriteLine("| Select your Console:   |");
        Console.WriteLine("+------------------------+");

        for (int i = 0; i < consoles.Length; i++)
        {
            Console.WriteLine($"| {i + 1} - {consoles[i]}");
        }

        Console.WriteLine("+========================+");
        Console.Write("\nEnter number: ");

        if (int.TryParse(Console.ReadLine(), out int choice) && choice > 0 && choice <= consoles.Length)
        {
            selectedConsole = consoles[choice - 1];
            selectedApps = new bool[apps[choice - 1].Length]; // Initialize selection array based on console
        }
        else
        {
            Console.WriteLine("Invalid choice, defaulting to Wii U.");
            selectedConsole = "Wii U";
            selectedApps = new bool[apps[1].Length]; // Wii U default
        }

        Console.WriteLine($"\nConsole set to: {selectedConsole}");
        Thread.Sleep(1000);
    }

    static void ShowAppSelection()
    {
        while (true)
        {
            ClearScreen();
            Console.WriteLine($"+===============================+");
            Console.WriteLine($"| HH Patcher - {selectedConsole} |");
            Console.WriteLine("+-------------------------------+");
            Console.WriteLine("| Select Apps to Patch (Enter #)|");
            Console.WriteLine("| Press Enter to toggle 'X'      |");
            Console.WriteLine("| Type '0' to apply patches      |");
            Console.WriteLine("+===============================+");

            int consoleIndex = Array.IndexOf(consoles, selectedConsole);

            for (int i = 0; i < apps[consoleIndex].Length; i++)
            {
                string status = selectedApps[i] ? "[X]" : "[ ]";
                
                // Disable selection for 3DS
                if (selectedConsole == "3DS")
                {
                    Console.WriteLine($"| {i + 1} - {apps[consoleIndex][i]} (Unselectable)");
                }
                else
                {
                    Console.WriteLine($"| {i + 1} - {apps[consoleIndex][i]} {status}");
                }
            }

            Console.WriteLine("+===============================+");
            Console.Write("\nEnter selection: ");

            if (int.TryParse(Console.ReadLine(), out int choice))
            {
                if (choice == 0)
                {
                    ApplyPatches();
                    return;
                }
                else if (choice > 0 && choice <= selectedApps.Length)
                {
                    // Prevent selection for 3DS
                    if (selectedConsole == "3DS")
                    {
                        Console.WriteLine("\nNo apps available for 3DS yet.");
                        Thread.Sleep(1000);
                    }
                    else
                    {
                        selectedApps[choice - 1] = !selectedApps[choice - 1]; // Toggle selection
                    }
                }
                else
                {
                    Console.WriteLine("Invalid selection, try again.");
                    Thread.Sleep(500);
                }
            }
        }
    }

    static void ApplyPatches()
    {
        ClearScreen();
        Console.WriteLine("+===============================+");
        Console.WriteLine("| Applying Patches...           |");
        Console.WriteLine("+===============================+");

        bool anySelected = false;
        string[] appList = apps[Array.IndexOf(consoles, selectedConsole)];

        for (int i = 0; i < appList.Length; i++)
        {
            if (selectedApps[i])
            {
                Console.WriteLine($"- Patching: {appList[i]}...");
                Thread.Sleep(1000);
                anySelected = true;
            }
        }

        if (!anySelected)
        {
            Console.WriteLine("\nNo apps selected. Returning to menu...");
            Thread.Sleep(1000);
            ShowAppSelection();
        }
        else
        {
            Console.WriteLine("\nPatch process complete!");
            Thread.Sleep(1500);
        }
    }
}

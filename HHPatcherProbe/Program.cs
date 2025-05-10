using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

class Program
{
    static string version = "HHPatcher Probe v1.0.0"; // Version Indicator
    static Dictionary<string, string> users = new Dictionary<string, string>
    {
        { "petrofizkulture", "hiho" },
        { "koptreq404", "wadinst" },
        { "t33hislost", "blaa" },
        { "LamineYamal123", "olps" }
    };

    static string logFilePath = "HHPatcher_Probe_Log.txt";
    static string[] consoles = { "Wii", "Wii U", "DSi", "3DS" };
    static string selectedConsole = "";

    static string[][] apps = {
        new string[] { "ProxWii Lite", "WiiBoot Manager", "CitraWii LiteX" },
        new string[] { "LK Management App", "LinkU Config Creator" },
        new string[] { "MPP Parser", "DSiLoad" },
        new string[] { "StrapNX FW", "NXPatch3D" }
    };

    static bool[] selectedApps;

    static void Main()
    {
        DrawFrame(version);
        if (!AuthenticateUser()) return;
        InitializeLog();
        SelectConsole();
        ShowAppSelection();
        PerformDeepScan();
        ApplyPatches();
    }

    static void DrawFrame(string title)
    {
        Console.Clear();
        Console.WriteLine("+==================================+");
        Console.WriteLine($"| {title,-30} |");
        Console.WriteLine("+==================================+");
    }

    static bool AuthenticateUser()
    {
        DrawFrame("Login Required");
        Console.Write("Username: ");
        string username = Console.ReadLine();
        Console.Write("Password: ");
        string password = Console.ReadLine();

        if (users.TryGetValue(username, out string correctPassword) && correctPassword == password)
        {
            Console.WriteLine($"✅ Login Successful! Welcome, {username}!");
            return true;
        }
        else
        {
            Console.WriteLine("❌ Invalid credentials. Exiting...");
            return false;
        }
    }

    static void InitializeLog()
    {
        File.WriteAllText(logFilePath, "=== HHPatcher Probe Log ===\n");
        WriteLog("Log initialized.");
    }

    static void WriteLog(string message)
    {
        File.AppendAllText(logFilePath, $"{DateTime.Now}: {message}\n");
    }

    static void SelectConsole()
    {
        DrawFrame("Select Console");
        for (int i = 0; i < consoles.Length; i++)
        {
            Console.WriteLine($"| {i + 1} - {consoles[i],-26} |");
        }
        Console.WriteLine("+==================================+");
        Console.Write("\nEnter number: ");

        if (int.TryParse(Console.ReadLine(), out int choice) && choice > 0 && choice <= consoles.Length)
        {
            selectedConsole = consoles[choice - 1];
            selectedApps = new bool[apps[choice - 1].Length];
            WriteLog($"Console selected: {selectedConsole}");
        }
        else
        {
            Console.WriteLine("Invalid choice, defaulting to Wii U.");
            selectedConsole = "Wii U";
            selectedApps = new bool[apps[1].Length];
            WriteLog("Invalid console selection. Defaulting to Wii U.");
        }
    }

    static void ShowAppSelection()
    {
        while (true)
        {
            DrawFrame($"HHPatcher Probe - {selectedConsole}");
            Console.WriteLine("| Select Apps to Patch (Enter #)  |");
            Console.WriteLine("| Press Enter to toggle 'X'       |");
            Console.WriteLine("| Type '0' to proceed to deep scan |");
            Console.WriteLine("+==================================+");

            int consoleIndex = Array.IndexOf(consoles, selectedConsole);

            for (int i = 0; i < apps[consoleIndex].Length; i++)
            {
                string status = selectedApps[i] ? "[X]" : "[ ]";
                Console.WriteLine($"| {i + 1} - {apps[consoleIndex][i],-24} {status} |");
            }

            Console.WriteLine("+==================================+");
            Console.Write("\nEnter selection: ");

            if (int.TryParse(Console.ReadLine(), out int choice))
            {
                if (choice == 0)
                {
                    PerformDeepScan();
                    return;
                }
                else if (choice > 0 && choice <= selectedApps.Length)
                {
                    selectedApps[choice - 1] = !selectedApps[choice - 1];
                    WriteLog($"App selection changed: {apps[consoleIndex][choice - 1]} [{(selectedApps[choice - 1] ? "Selected" : "Deselected")}]");
                }
                else
                {
                    Console.WriteLine("Invalid selection, try again.");
                }
            }
        }
    }

    static void PerformDeepScan()
    {
        DrawFrame("Running Deep Scan...");
        WriteLog("Running deep scan...");

        bool anySelected = false;
        string[] appList = apps[Array.IndexOf(consoles, selectedConsole)];

        for (int i = 0; i < appList.Length; i++)
        {
            if (selectedApps[i])
            {
                Console.WriteLine($"| Scanning: {appList[i],-26} |");
                Thread.Sleep(1000);

                Random rand = new Random();
                bool hasIssues = rand.Next(0, 3) == 0; // Simulated issue detection

                if (hasIssues)
                {
                    Console.WriteLine($"| ⚠️ WARNING: Missing files!       |");
                    WriteLog($"Issue detected: {appList[i]} - Missing dependencies.");
                }
                else
                {
                    Console.WriteLine($"| ✅ Ready for patching.           |");
                    WriteLog($"Scan passed: {appList[i]} - Ready for patching.");
                }

                anySelected = true;
            }
        }

        Console.WriteLine("+==================================+");
        if (!anySelected)
        {
            Console.WriteLine("\nNo apps selected. Returning to menu...");
            WriteLog("Deep scan aborted. No apps selected.");
            ShowAppSelection();
        }
        else
        {
            Console.WriteLine("\nDeep scan complete! Proceeding with patches...");
            WriteLog("Deep scan complete. Proceeding with patching.");
            Thread.Sleep(1500);
            ApplyPatches();
        }
    }

    static void ApplyPatches()
    {
        DrawFrame("Applying Patches...");
        WriteLog("Starting patch process...");

        bool anySelected = false;
        string[] appList = apps[Array.IndexOf(consoles, selectedConsole)];

        for (int i = 0; i < appList.Length; i++)
        {
            if (selectedApps[i])
            {
                Console.WriteLine($"| Patching: {appList[i],-26} |");
                WriteLog($"Patching started: {appList[i]}");
                Thread.Sleep(1000);
                anySelected = true;
            }
        }

        Console.WriteLine("+==================================+");
        Console.WriteLine("\nPatch process complete!");
        WriteLog("Patch process complete.");
    }
}

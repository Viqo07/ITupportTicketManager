using System;
using System.IO;

namespace TicketManagerIO
{
	internal static class Program
	{
		private static void Main()
		{
			var manager = new TicketManager();

			Console.WriteLine("=== IT Support Ticket Manager ===");

			bool running = true;

			while (running)
			{
				Console.WriteLine("\nMenu:");
				Console.WriteLine("1. Add Ticket");
				Console.WriteLine("2. Remove Ticket");
				Console.WriteLine("3. Display All Tickets");
				Console.WriteLine("4. Close Ticket");
				Console.WriteLine("5. Reopen Ticket");
				Console.WriteLine("6. Save Tickets to File");
				Console.WriteLine("7. Load Tickets from File");
				Console.WriteLine("8. Show Open Ticket Count");
				Console.WriteLine("9. Exit");
				Console.Write("Choose: ");

				string? choice = Console.ReadLine()?.Trim();

				try
				{
					switch (choice)
					{
						case "1":
							AddTicketMenu(manager);
							break;

						case "2":
							RemoveTicketMenu(manager);
							break;

						case "3":
							manager.DisplayAllTickets();
							break;

						case "4":
							ChangeTicketStatus(manager, true);
							break;

						case "5":
							ChangeTicketStatus(manager, false);
							break;

						case "6":
							SaveMenu(manager);
							break;

						case "7":
							LoadMenu(manager);
							break;

						case "8":
							Console.WriteLine($"Open tickets: {manager.GetOpenCount()}");
							break;

						case "9":
							running = false;
							break;

						default:
							Console.WriteLine("Invalid option.");
							break;
					}
				}
				catch (Exception ex)
				{
					Console.WriteLine($"Error: {ex.Message}");
				}
			}

			Console.WriteLine("Goodbye!");
		}

		private static void AddTicketMenu(TicketManager manager)
		{
			Console.Write("Enter Ticket ID (e.g., T1001): ");
			string id = Console.ReadLine() ?? "";

			Console.Write("Enter Description: ");
			string desc = Console.ReadLine() ?? "";

			Console.Write("Enter Priority (Low/Medium/High): ");
			string priority = NormalizeCase(Console.ReadLine());

			Console.Write("Enter Status (Open/In Progress/Closed): ");
			string status = NormalizeCase(Console.ReadLine());

			var ticket = new Ticket(id, desc, priority, status);

			manager.AddTicket(ticket);

			Console.WriteLine("Ticket added.");
		}

		private static void RemoveTicketMenu(TicketManager manager)
		{
			Console.Write("Enter Ticket ID to remove: ");
			string id = Console.ReadLine() ?? "";

			bool removed = manager.RemoveTicket(id);

			Console.WriteLine(removed ? "Ticket removed." : "Ticket not found.");
		}

		private static void ChangeTicketStatus(TicketManager manager, bool close)
		{
			Console.Write("Enter Ticket ID: ");
			string id = Console.ReadLine() ?? "";

			var ticket = manager.FindTicket(id);

			if (ticket == null)
			{
				Console.WriteLine("Ticket not found.");
				return;
			}

			if (close)
			{
				ticket.CloseTicket();
				Console.WriteLine("Ticket closed.");
			}
			else
			{
				ticket.ReopenTicket();
				Console.WriteLine("Ticket reopened.");
			}
		}

		private static void SaveMenu(TicketManager manager)
		{
			Console.Write("Enter path to save CSV (e.g., tickets.csv): ");
			string path = Console.ReadLine() ?? "";

			try
			{
				manager.SaveTickets(path);
				Console.WriteLine($"Saved to {Path.GetFullPath(path)}");
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Save failed: {ex.Message}");
			}
		}

		private static void LoadMenu(TicketManager manager)
		{
			Console.Write("Enter path to load CSV (e.g., tickets.csv): ");
			string path = Console.ReadLine() ?? "";

			try
			{
				manager.LoadTickets(path);
			}
			catch (FileNotFoundException)
			{
				Console.WriteLine("File not found.");
			}
			catch (InvalidDataException ex)
			{
				Console.WriteLine($"Invalid file format: {ex.Message}");
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Load failed: {ex.Message}");
			}
		}

		private static string NormalizeCase(string? input)
		{
			if (string.IsNullOrWhiteSpace(input)) return "";

			var s = input.Trim().ToLowerInvariant();

			if (s == "low") return "Low";
			if (s == "medium" || s == "med") return "Medium";
			if (s == "high") return "High";
			if (s == "open") return "Open";
			if (s == "in progress" || s == "in-progress" || s == "progress") return "In Progress";
			if (s == "closed" || s == "close") return "Closed";

			return input.Trim();
		}
	}
}
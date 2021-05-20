﻿using System;
using System.Collections.Generic;

namespace cab301_assignment {
	static class Program {
		public static ToolLibrarySystem system;

		public static string selectedCategory = "";
		public static string selectedType = "";

		private static List<string> getToolCategories() {
			List<string> categories = new List<string>();
			foreach (var entry in Database.toolCollections) {
				categories.Add(entry.Key);
			}

			return categories;
		}

		private static bool getToolTypes(string category, out List<string> types) {
			types = new List<string>();
			foreach (var entry in Database.toolCollections) {
				if (entry.Key == category) {
					foreach (ToolCollection collection in entry.Value) {
						types.Add(collection.Name);
					}

					return true;
				}
			}

			return false;
		}

		private static bool getTools(string category, string type, out ToolCollection tools) {
			foreach (var entry in Database.toolCollections) {
				if (entry.Key == category) {
					foreach (ToolCollection collection in entry.Value) {
						if (collection.Name == type) {
							tools = collection;
							return true;
						}
					}
				}
			}

			tools = default(ToolCollection);
			return false;
		}

		static void staffAddToolMenu() {
			Console.Clear();
			Console.WriteLine("Adding tool menu");
			Console.WriteLine();

			string toolName = UI.getTextInput("Enter the name of the new tool (0 to exit): ");
			if (toolName == "0")
				return;

			Console.WriteLine();

			int toolQuantity = UI.getIntInputStrict("Enter the tool quantity: ", true);

			Console.WriteLine();

			// get categories
			getToolCategories();

			bool exiting = false;
			while (!UI.listSelector("Select tool category: ", "Tool category (0 to exit): ", getToolCategories(), out selectedCategory, out exiting)) {
				if (exiting)
					return;

				Console.WriteLine("Please select a valid option");
			}

			Console.WriteLine();

			List<string> types;
			if (!getToolTypes(selectedCategory, out types)) {
				Console.WriteLine("No tool types were found for the given category");
			}

			while (!UI.listSelector("Select tool type: ", "Tool type (0 to exit): ", types, out selectedType, out exiting)) {
				if (exiting)
					return;

				Console.WriteLine("Please select a valid option");
			}

			Console.WriteLine();

			system.add(new Tool(toolName, toolQuantity));

			Console.WriteLine("Tool successfully added");

			Console.WriteLine();

			UI.waitForInput();

			staffMenu();
		}

		static void adjustToolStockMenu(bool add) {
			Console.Clear();
			Console.WriteLine($"{(add ? "Adding" : "Removing")} tool stock menu");
			Console.WriteLine();

			// get categories
			getToolCategories();

			bool exiting = false;
			while (!UI.listSelector("Select tool category: ", "Tool category (0 to exit): ", getToolCategories(), out selectedCategory, out exiting)) {
				if (exiting)
					return;

				Console.WriteLine("Please select a valid option");
			}

			Console.WriteLine();

			List<string> types;
			if (!getToolTypes(selectedCategory, out types)) {
				Console.WriteLine("No tool types were found for the given category");
			}

			while (!UI.listSelector("Select tool type: ", "Tool type (0 to exit): ", types, out selectedType, out exiting)) {
				if (exiting)
					return;

				if (selectedType == "0")
					return;

				Console.WriteLine("Please select a valid option");
			}

			Console.WriteLine();

			ToolCollection collection;
			if (!getTools(selectedCategory, selectedType, out collection))
				return;

			Tool selectedTool;
			while (!UI.toolSelector($"Select tool to {(add ? "add" : "remove")} stock for: ", "Tool (0 to exit): ", collection, out selectedTool, out exiting)) {
				if (exiting)
					return;

				Console.WriteLine("Please select a valid option");
			}

			Console.WriteLine();

			int stockChange = UI.getIntInputStrict($"Enter the stock to {(add ? "add" : "remove")}: ", true);

			if (add)
				system.add(selectedTool, stockChange);
			else
				system.delete(selectedTool, stockChange);

			Console.WriteLine();

			Console.WriteLine($"{stockChange} stock {(add ? "added" : "removed")}");

			Console.WriteLine();

			UI.waitForInput();

			staffMenu();
		}

		static void staffAddToolStockMenu() {
			adjustToolStockMenu(true);
		}

		static void staffRemoveToolStockMenu() {
			adjustToolStockMenu(false);
		}

		static void staffRegisterMenu() {
			Console.Clear();
			Console.WriteLine("Member registration menu");
			Console.WriteLine();

			string firstName = UI.getTextInput("Enter the first name of the new member: ");
			string lastName = UI.getTextInput("Enter the last name of the new member: ");
			string contactNumber = UI.getTextInput("Enter the mobile number of the new member: ");
			string pin = UI.getTextInput("Enter PIN: ");

			Console.WriteLine();

			system.add(new Member(firstName, lastName, contactNumber, pin));

			Console.WriteLine();

			UI.waitForInput();

			staffMenu();
		}

		static void staffRemoveMemberMenu() {

		}

		static void staffMemberBorrowingToolsMenu() {

		}

		static void staffMenu() {
			UI.drawMenu("Staff Menu", new List<UI.IMenuItem> {
				new UI.MenuOption("Add a new tool", staffAddToolMenu),
				new UI.MenuOption("Add new stock of an existing tool", staffAddToolStockMenu),
				new UI.MenuOption("Remove stock of an existing tool", staffRemoveToolStockMenu),
				new UI.MenuOption("Register a new member", staffRegisterMenu),
				new UI.MenuOption("Remove a member", staffRemoveMemberMenu),
				new UI.MenuOption("Show tools a member is borrowing", staffMemberBorrowingToolsMenu),
				new UI.MenuOption("Return to main menu", mainMenu, true)
			});
		}

		static void memberDisplayToolsMenu() {
			Console.Clear();
			Console.WriteLine("Member registration menu");
			Console.WriteLine();


			// get categories
			getToolCategories();

			while (!UI.listSelector("Select tool category: ", "Tool category: ", getToolCategories(), out selectedCategory, out exiting)) {
				Console.WriteLine("Please select a valid option");
			}

			Console.WriteLine();

			List<string> types;
			if (!getToolTypes(selectedCategory, out types)) {
				Console.WriteLine("No tool types were found for the given category");
			}

			while (!UI.listSelector("Select tool type: ", "Tool type (0 to exit): ", types, out selectedType, out exiting)) {
				if (exiting)
					return;

				if (selectedType == "0")
					return;

				Console.WriteLine("Please select a valid option");
			}

			Console.WriteLine();

			ToolCollection collection;
			if (!getTools(selectedCategory, selectedType, out collection))
				return;

			Tool selectedTool;
			while (!UI.toolSelector($"Select tool to {(add ? "add" : "remove")} stock for: ", "Tool (0 to exit): ", collection, out selectedTool, out exiting)) {
				if (exiting)
					return;

				Console.WriteLine("Please select a valid option");
			}
		}

		static void memberBorrowToolMenu() {

		}

		static void memberReturnToolMenu() {

		}

		static void memberListBorrowedTools() {

		}

		static void memberMostFrequentToolsMenu() {

		}

		static void memberMenu() {
			Console.Clear();
			Console.WriteLine("Member login");
			Console.WriteLine();

			string login = UI.getTextInput("Enter your member login ID: ");

			// check if user exists
			Member loggedInUser = null;
			foreach (Member member in Database.memberCollection.toArray()) {
				if ($"{member.LastName}{member.FirstName}" == login) {
					loggedInUser = member;
					break;
				}
			}

			if (loggedInUser == null) {
				Console.WriteLine();
				Console.WriteLine("Member not found");
				Console.WriteLine();

				UI.waitForInput();

				mainMenu();
				return;
			}

			// check pin
			string pin = UI.getTextInput("Enter PIN: ");

			while (pin != loggedInUser.PIN) {
				Console.WriteLine();
				Console.WriteLine("PIN incorrect");
				Console.WriteLine();

				UI.waitForInput();
				mainMenu();

				return;
			}

			// draw member menu
			UI.drawMenu("Member Menu", new List<UI.IMenuItem> {
				new UI.MenuOption("Display tools by category", memberDisplayToolsMenu),
				new UI.MenuOption("Borrow tool from library", memberBorrowToolMenu),
				new UI.MenuOption("Return tool to library", memberReturnToolMenu),
				new UI.MenuOption("List borrowed tools", memberListBorrowedTools),
				new UI.MenuOption("Display most frequently borrowed tools", memberMostFrequentToolsMenu),
				new UI.MenuOption("Return to main menu", mainMenu, true)
			});
		}

		static void exit() {
			System.Environment.Exit(1);
		}

		static void mainMenu() {
			UI.drawMenu("Main Menu", new List<UI.IMenuItem>{
				new UI.MenuTitle("Welcome to the Tool Library System"),
				new UI.MenuSpacer(),
				new UI.MenuOption("Staff Operations", staffMenu),
				new UI.MenuOption("Member Operations", memberMenu),
				new UI.MenuOption("Exit Application", exit, true)
			});
		}

		static void Main(string[] args) {
			// initialise tools and system
			var toolCategoriesAndTypes = new Dictionary<string, List<string>>();
			toolCategoriesAndTypes.Add("Gardening Tools", new List<string> { "Line Trimmers", "Lawn Mowers", "Hand Tools", "Wheelbarrows", "Garden Power Tools" });
			toolCategoriesAndTypes.Add("Flooring Tools", new List<string> { "Scrapers", "Floor Lasers", "Floor Levelling Tools", "Floor Levelling Materials", "Floor Hand Tools", "Tiling Tools" });
			toolCategoriesAndTypes.Add("Fencing Tools", new List<string> { "Hand Tools", "Electric Fencing", "Steel Fencing Tools", "Power Tools", "Fencing Accessories" });
			toolCategoriesAndTypes.Add("Measuring Tools", new List<string> { "Distance Tools", "Laser Measurer", "Measuring Jugs", "Temperature & Humidity Tools", "Levelling Tools", "Markers" });
			toolCategoriesAndTypes.Add("Cleaning Tools", new List<string> { "Draining", "Car Cleaning", "Vacuum", "Pressure Cleaners", "Pool Cleaning", "Floor Cleaning" });
			toolCategoriesAndTypes.Add("Painting Tools", new List<string> { "Sanding Tools", "Brushes", "Rollers", "Paint Removal Tools", "Paint Scrapers", "Sprayers" });
			toolCategoriesAndTypes.Add("Electronic Tools", new List<string> { "Voltage Tester", "Oscilloscopes", "Thermal Imaging", "Data Test Tool", "Insulation Testers" });
			toolCategoriesAndTypes.Add("Electricity Tools", new List<string> { "Test Equipment", "Safety Equipment", "Basic Hand tools", "Circuit Protection", "Cable Tools" });
			toolCategoriesAndTypes.Add("Automotive Tools", new List<string> { "Jacks", "Air Compressors", "Battery Chargers", "Socket Tools", "Braking", "Drivetrain" });

			system = new ToolLibrarySystem(toolCategoriesAndTypes);

			// add some default tools
			selectedCategory = "Gardening Tools";
			selectedType = "Line Trimmers";
			system.add(new Tool("Line trimmer #1", 100));
			system.add(new Tool("Line trimmer #2", 21));

			// add a default user
			system.add(new Member("Bob", "Jeff", "12345678", "1234"));

			mainMenu();
		}
	}
}
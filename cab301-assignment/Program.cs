using System;
using System.Collections.Generic;

namespace cab301_assignment {
	static class Program {
		public static Dictionary<string, List<string>> toolCategoriesAndTypes;
		public static ToolLibrarySystem system;

		public static string selectedCategory = "";
		public static string selectedType = "";

		static void addToolMenu() {
			string toolName = UI.getTextInput("Enter the name of the new tool (0 to exit): ");
			if (toolName == "0")
				return;

			string toolCategory = UI.getTextInput("Enter the name of the new tool (0 to exit): ");

		}

		static void addToolStockMenu() {

		}

		static void removeToolStockMenu() {

		}

		static void registerMenu() {

		}

		static void removeMemberMenu() {

		}

		static void memberBorrowingToolsMenu() {

		}

		static void staffMenu() {
			UI.drawMenu("Staff Menu", new List<UI.IMenuItem> {
				new UI.MenuOption("Return to main menu", mainMenu),
				new UI.MenuOption("Add a new tool", addToolMenu),
				new UI.MenuOption("Add new stock of an existing tool", addToolStockMenu),
				new UI.MenuOption("Remove stock of an existing tool", removeToolStockMenu),
				new UI.MenuOption("Register a new member", registerMenu),
				new UI.MenuOption("Remove a member", removeMemberMenu),
				new UI.MenuOption("Show tools a member is borrowing", memberBorrowingToolsMenu)
			});
		}

		static void memberMenu() {
			Console.WriteLine("This is the Member Menu");
			Console.ReadKey();
		}

		static void exit() {
			System.Environment.Exit(1);
		}

		static void mainMenu() {
			UI.drawMenu("Main Menu", new List<UI.IMenuItem>{
				new UI.MenuTitle("Welcome to the Tool Library System"),
				new UI.MenuSpacer(),
				new UI.MenuOption("Exit Application", exit),
				new UI.MenuOption("Staff Operations", staffMenu),
				new UI.MenuOption("Member Operations", staffMenu),
			});
		}

		static void Main(string[] args) {
			// initialise tools and system
			toolCategoriesAndTypes = new Dictionary<string, List<string>>();
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

			mainMenu();
		}
	}
}
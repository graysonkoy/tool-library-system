using System;
using System.Collections.Generic;
using System.IO;

namespace cab301_assignment {
	static class Program {
		private static ToolLibrarySystem system;

		private static Member loggedInUser = null;
		private static bool staffLoggedIn = false;

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
			Database.getToolCategories();

			if (!UI.listSelector("Select tool category: ", "Tool category (0 to exit): ", Database.getToolCategories(), out Database.selectedCategory))
				return;

			Console.WriteLine();

			List<string> types;
			if (!Database.getToolTypes(Database.selectedCategory, out types)) {
				Console.WriteLine("No tool types were found for the given category");
				return;
			}

			if (!UI.listSelector("Select tool type: ", "Tool type (0 to exit): ", types, out Database.selectedType))
				return;

			Console.WriteLine();

			try {
				Tool existingTool;
				if (Database.getToolByName(toolName, out existingTool)) { // TODO: check if case sensitive
					int oldStock = existingTool.Quantity;

					system.add(existingTool, toolQuantity);

					Console.WriteLine($"Tool already exists, additional stock added (stock {oldStock} -> {existingTool.Quantity})");
				} else {
					system.add(new Tool(toolName, toolQuantity));

					Console.WriteLine("Tool successfully added");
				}
			} catch(ToolException e) {
				Console.WriteLine(e.Message);
			}
		}

		static void adjustToolStockMenu(bool add) {
			Console.Clear();
			Console.WriteLine($"{(add ? "Adding" : "Removing")} tool stock menu");
			Console.WriteLine();

			// get categories
			Database.getToolCategories();
			
			if (!UI.listSelector("Select tool category: ", "Tool category (0 to exit): ", Database.getToolCategories(), out Database.selectedCategory))
				return;

			Console.WriteLine();

			List<string> types;
			if (!Database.getToolTypes(Database.selectedCategory, out types)) {
				Console.WriteLine("No tool types were found for the given category");
				return;
			}

			if (!UI.listSelector("Select tool type: ", "Tool type (0 to exit): ", types, out Database.selectedType))
				return;

			Console.WriteLine();

			ToolCollection collection;
			if (!Database.getTools(Database.selectedCategory, Database.selectedType, out collection))
				return;

			Tool selectedTool;
			if (!UI.listSelector($"Select tool to {(add ? "add" : "remove")} stock for: ", "Tool (0 to exit): ", new List<Tool>(collection.toArray()), out selectedTool))
				return;

			Console.WriteLine();

			int stockChange = UI.getIntInputStrict($"Enter the stock to {(add ? "add" : "remove")}: ", true);
			
			try {
				if (add)
					system.add(selectedTool, stockChange);
				else
					system.delete(selectedTool, stockChange);

				Console.WriteLine();

				Console.WriteLine($"{stockChange} stock {(add ? "added" : "removed")}");
			} catch(ToolException e) {
				Console.WriteLine(e.Message);
			}
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

			string contactNumber;
			while (true) {
				contactNumber = UI.getTextInput("Enter the mobile number of the new member: ");

				// validate phone number
				int validatedNumber;
				if (!int.TryParse(contactNumber, out validatedNumber)) {
					Console.WriteLine("Please enter only numbers");
				} else {
					break;
				}
			}

			string pin;
			while (true) {
				pin = UI.getTextInput("Enter PIN: ");

				// validate pin
				int validatedNumber;
				if (!int.TryParse(pin, out validatedNumber)) {
					Console.WriteLine("Please enter only numbers");
				}
				else if (pin.Length != 4) {
					Console.WriteLine("Only 4-digit pins are allowed");
				} else {
					break;
				}
			}

			Console.WriteLine();

			try {
				Member newMember = new Member(firstName, lastName, contactNumber, pin);
				system.add(newMember);

				Console.WriteLine($"Added member {newMember.ToString()} successfully");
			} catch(ToolException e) {
				Console.WriteLine(e.Message);
			}
		}

		static void staffRemoveMemberMenu() {
			Console.Clear();
			Console.WriteLine("Member deletion menu");
			Console.WriteLine();

			List<Member> members = new List<Member>(Database.memberCollection.toArray());

			Member selectedMember;
			if (!UI.listSelector("Select member to delete: ", "Member (0 to exit): ", members, out selectedMember))
				return;

			Console.WriteLine();

			try {
				system.delete(selectedMember);

				Console.WriteLine($"Deleted member {selectedMember.ToString()} successfully");
			}
			catch (ToolException e) {
				Console.WriteLine(e.Message);
			}
		}

		/*
		static void staffMemberBorrowingToolsMenu() {
			Console.Clear();
			Console.WriteLine("Member borrowing tools menu");
			Console.WriteLine();

			List<Member> members = new List<Member>(Database.memberCollection.toArray());

			Member selectedMember;
			if (!UI.listSelector("Select member to view borrowed tools for: ", "Member (0 to exit): ", members, out selectedMember))
				return;

			Console.WriteLine();

			system.displayBorrowingTools(selectedMember);
		}
		*/

		static void staffMemberContactNumber() {
			Console.Clear();
			Console.WriteLine("Member contact number finder");
			Console.WriteLine();

			// TODO: check if they want list selector or just type in name and find number
			string firstName = UI.getTextInput("Enter the first name of the member: ");
			string lastName = UI.getTextInput("Enter the last name of the member: ");

			Console.WriteLine();

			// find member
			Member selectedMember = null;
			foreach (Member member in Database.memberCollection.toArray()) {
				if (member.FirstName == firstName && member.LastName == lastName) {
					selectedMember = member;
					break;
				}
			}

			if (selectedMember == null) {
				Console.WriteLine("Member not found");
				return;
			}

			Console.WriteLine($"{firstName} {lastName} has the contact number {selectedMember.ContactNumber}");
		}

		static void staffMenuLogin() {
			Console.Clear();
			Console.WriteLine("Staff login");
			Console.WriteLine();

			string login = UI.getTextInput("Enter username: ");
			string password = UI.getTextInput("Enter password: ");

			if (login == "staff" && password == "today123") {
				staffLoggedIn = true;
			} else {
				Console.WriteLine();
				Console.WriteLine("Login incorrect");
				return;
			}

			staffMenu();
		}

		static void staffMenu() {
			// check login
			if (!staffLoggedIn) {
				staffMenuLogin();

				Console.WriteLine();
				UI.waitToContinue();

				mainMenu();
				return;
			}

			// draw staff menu
			UI.drawMenu("Staff Menu", new List<UI.IMenuItem> {
				new UI.MenuOption("Add a new tool", staffAddToolMenu),
				new UI.MenuOption("Add new stock of an existing tool", staffAddToolStockMenu),
				new UI.MenuOption("Remove stock of an existing tool", staffRemoveToolStockMenu),
				new UI.MenuOption("Register a new member", staffRegisterMenu),
				new UI.MenuOption("Remove a member", staffRemoveMemberMenu),
				// new UI.MenuOption("Show tools a member is borrowing", staffMemberBorrowingToolsMenu), // TODO: this isn't even needed? check task sheet
				new UI.MenuOption("Find contact number for member", staffMemberContactNumber),
				new UI.MenuOption("Return to main menu", mainMenu, true)
			});

			Console.WriteLine();
			UI.waitToContinue();

			staffMenu();
		}

		static void memberDisplayToolsMenu() {
			Console.Clear();
			Console.WriteLine("Tool display menu");
			Console.WriteLine();

			// get categories
			Database.getToolCategories();

			if (!UI.listSelector("Select tool category: ", "Tool category (0 to exit): ", Database.getToolCategories(), out Database.selectedCategory))
				return;

			Console.WriteLine();

			List<string> types;
			if (!Database.getToolTypes(Database.selectedCategory, out types)) {
				Console.WriteLine("No tool types were found for the given category");
				return;
			}

			if (!UI.listSelector("Select tool type: ", "Tool type (0 to exit): ", types, out Database.selectedType))
				return;

			Console.WriteLine();

			system.displayTools(Database.selectedType);
		}

		static void memberBorrowToolMenu() {
			Console.Clear();
			Console.WriteLine("Tool borrowing menu");
			Console.WriteLine();

			if (loggedInUser.Tools.Length >= 3) {
				Console.WriteLine("Cannot borrow any more tools");
				return;
			}

			// get categories
			Database.getToolCategories();

			if (!UI.listSelector("Select tool category: ", "Tool category (0 to exit): ", Database.getToolCategories(), out Database.selectedCategory))
				return;

			Console.WriteLine();

			List<string> types;
			if (!Database.getToolTypes(Database.selectedCategory, out types)) {
				Console.WriteLine("No tool types were found for the given category");
				return;
			}

			if (!UI.listSelector("Select tool type: ", "Tool type (0 to exit): ", types, out Database.selectedType))
				return;

			Console.WriteLine();

			ToolCollection collection;
			if (!Database.getTools(Database.selectedCategory, Database.selectedType, out collection))
				return;

			List<Tool> borrowableTools = Database.getBorrowableTools(collection);

			Tool selectedTool;
			if (!UI.listSelector($"Select tool to borrow: ", "Tool (0 to exit): ", borrowableTools, out selectedTool))
				return;

			Console.WriteLine();

			try {
				system.borrowTool(loggedInUser, selectedTool);

				Console.WriteLine($"{loggedInUser.ToString()} borrowed tool {selectedTool.Name} successfully");
			}
			catch (ToolException e) {
				Console.WriteLine(e.Message);
			}
		}

		static void memberReturnToolMenu() {
			Console.Clear();
			Console.WriteLine("Tool return menu");
			Console.WriteLine();

			string returningToolName;
			if (!UI.listSelector("Select tool: ", "Tool (0 to exit): ", new List<string>(loggedInUser.Tools), out returningToolName))
				return;

			Console.WriteLine();

			// get tool
			Tool returningTool;
			if (!Database.getToolByName(returningToolName, out returningTool)) { // TODO: check what other people do for this since you cant loop through tools because its private thanks maolin
				Console.WriteLine("Error returning tool (member not borrowing tool)");
				return;
			}

			try {
				system.returnTool(loggedInUser, returningTool);

				Console.WriteLine($"Tool '{returningToolName}' returned successfully");
			}
			catch (ToolException e) {
				Console.WriteLine(e.Message);
			}
		}

		static void memberListBorrowedTools() {
			Console.Clear();
			Console.WriteLine($"Borrowed tools for {loggedInUser.ToString()}");
			Console.WriteLine();

			system.displayBorrowingTools(loggedInUser);
		}

		static void memberMostFrequentToolsMenu() {
			Console.Clear();
			Console.WriteLine($"Most frequently borrowed tools");
			Console.WriteLine();

			system.displayTopTHree();
		}

		static void memberMenuLogin() {
			Console.Clear();
			Console.WriteLine("Member login");
			Console.WriteLine();

			string login = UI.getTextInput("Enter your member login ID: ");

			// check if user exists
			foreach (Member member in Database.memberCollection.toArray()) {
				if ($"{member.LastName}{member.FirstName}" == login) {
					loggedInUser = member;
					break;
				}
			}

			if (loggedInUser == null) {
				Console.WriteLine();
				Console.WriteLine("Member not found");
				return;
			}

			// check pin
			string pin = UI.getTextInput("Enter PIN: ");

			while (pin != loggedInUser.PIN) {
				Console.WriteLine();
				Console.WriteLine("PIN incorrect");
				return;
			}

			memberMenu();
		}

		static void memberMenu() {
			// check login
			if (loggedInUser == null) {
				memberMenuLogin();

				Console.WriteLine();
				UI.waitToContinue();

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

			Console.WriteLine();
			UI.waitToContinue();

			memberMenu();
		}

		static void exit() {
			System.Environment.Exit(1);
		}

		static void mainMenu() {
			// log out user
			staffLoggedIn = false;
			loggedInUser = null;

			// draw main menu
			UI.drawMenu("Main Menu", new List<UI.IMenuItem>{
				new UI.MenuTitle("Welcome to the Tool Library System"),
				new UI.MenuSpacer(),
				new UI.MenuOption("Staff Operations", staffMenu),
				new UI.MenuOption("Member Operations", memberMenu),
				new UI.MenuOption("Exit Application", exit, true)
			});
		}

		static void Main(string[] args) {
			try {
				var saved = Console.Out;
				Console.SetOut(TextWriter.Null);
				{
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
					Database.selectedCategory = "Gardening Tools";
					Database.selectedType = "Line Trimmers";

					var trimmer1 = new Tool("Bad Line Trimmer", 100);
					trimmer1.NoBorrowings = 4;
					system.add(trimmer1);

					var trimmer2 = new Tool("Ultra Line Trimmer", 21);
					trimmer2.NoBorrowings = 3;
					system.add(trimmer2);

					var trimmer3 = new Tool("Luxury Line Trimmer", 47);
					trimmer3.NoBorrowings = 7;
					system.add(trimmer3);

					var trimmer4 = new Tool("Another Line Trimmer", 55);
					trimmer4.NoBorrowings = 10000;
					system.add(trimmer4);

					var trimmer5 = new Tool("Unavailable Line Trimmer", 0);
					trimmer5.NoBorrowings = 10000;
					system.add(trimmer5);

					// add a default user
					system.add(new Member("Bob", "Jeff", "12345678", "1234"));
				}
				Console.SetOut(saved);

				mainMenu();
			} catch(Exception e) {
				Console.WriteLine(e);
			}

			Console.ReadKey();
		}
	}
}
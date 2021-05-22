using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

using cab301_assignment;

namespace testing {
	[TestClass]
	public class ToolTests {
		ToolLibrarySystem system;

		[TestInitialize]
		public void InitialiseTests() {
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

			// reset selected category and type
			Database.selectedCategory = Database.selectedType = "";
		}

		private void SelectCategoryAndType(string category, string type) {
			Database.selectedCategory = category;
			Database.selectedType = type;
		}

		private Tool AddTool(string name, int quantity) {
			// add tool
			Tool newTool = new Tool(name, quantity);
			system.add(newTool);

			return newTool;
		}

		[TestMethod]
		public void TestAddingTool() {
			SelectCategoryAndType("Gardening Tools", "Line Trimmers");

			// add tool
			Tool newTool = AddTool("Line Trimmer #1", 100);

			// check that the tool has been added
			Tool foundTool = null;
			Assert.IsTrue(Database.getToolByName(newTool.Name, out foundTool));
			Assert.AreEqual(newTool, foundTool);
		}

		[TestMethod]
		public void TestAddingToolStock() {
			SelectCategoryAndType("Gardening Tools", "Line Trimmers");

			// add tool
			Tool newTool = AddTool("Line Trimmer #1", 100);

			// add stock to tool
			system.add(newTool, 100);

			// check the correct amount was added
			Assert.IsTrue(newTool.Quantity == 200);
		}

		[TestMethod]
		[ExpectedException(typeof(ToolException))]
		public void TestAddingToolStockNegative() {
			SelectCategoryAndType("Gardening Tools", "Line Trimmers");

			// add tool
			Tool newTool = AddTool("Line Trimmer #1", 100);

			// add stock to tool (should fail)
			system.add(newTool, -1);
		}

		[TestMethod]
		public void TestDeletingToolStock() {
			SelectCategoryAndType("Gardening Tools", "Line Trimmers");

			// add tool
			Tool newTool = AddTool("Line Trimmer #1", 100);

			// remove tool stock
			system.delete(newTool, 50);

			// check the correct amount was added
			Assert.IsTrue(newTool.Quantity == 50);
		}

		[TestMethod]
		[ExpectedException(typeof(ToolException))]
		public void TestDeletingToolStockTooMuch() {
			SelectCategoryAndType("Gardening Tools", "Line Trimmers");

			// add tool
			Tool newTool = AddTool("Line Trimmer #1", 100);

			// remove tool stock (should fail)
			system.delete(newTool, 200);
		}

		[TestMethod]
		[ExpectedException(typeof(ToolException))]
		public void TestDeletingToolStockNegative() {
			SelectCategoryAndType("Gardening Tools", "Line Trimmers");

			// add tool
			Tool newTool = AddTool("Line Trimmer #1", 100);

			// remove tool stock (should fail)
			system.delete(newTool, -1);
		}

		[TestMethod]
		public void TestDeletingTool() {
			SelectCategoryAndType("Gardening Tools", "Line Trimmers");

			// add tool
			Tool newTool = AddTool("Line Trimmer #1", 100);

			// delete the tool
			system.delete(newTool);

			// make sure it was deleted
			Tool foundTool = null;
			Assert.IsFalse(Database.getToolByName(newTool.Name, out foundTool));
			Assert.IsNull(foundTool);
		}

		[TestMethod]
		public void TestAddMember() {
			// add a new member
			Member newMember = new Member("Bob", "Jeff", "12345678", "1234");
			system.add(newMember);

			// check they were added
			Assert.IsTrue(Database.memberCollection.search(newMember));

			// additional check
			Member foundMember = null;
			Assert.IsTrue(Database.getMemberByName(newMember.FirstName, newMember.LastName, out foundMember));
			Assert.AreEqual(foundMember, newMember);
		}

		[TestMethod]
		public void TestDeleteMember() {
			// add a new member
			Member newMember = new Member("Bob", "Jeff", "12345678", "1234");
			system.add(newMember);

			// delete the member
			system.delete(newMember);

			// check they were delete
			Assert.IsFalse(Database.memberCollection.search(newMember));
		}

		[TestMethod]
		[ExpectedException(typeof(ToolException))]
		public void TestDeleteMemberBorrowing() {
			SelectCategoryAndType("Gardening Tools", "Line Trimmers");

			// add tool
			Tool newTool = AddTool("Line Trimmer #1", 100);

			// add a new member
			Member newMember = new Member("Bob", "Jeff", "12345678", "1234");
			system.add(newMember);

			// borrow a tool
			system.borrowTool(newMember, newTool);

			// delete the member (should fail)
			system.delete(newMember);
		}

		[TestMethod]
		public void TestBorrowTool() {
			SelectCategoryAndType("Gardening Tools", "Line Trimmers");

			// add tool
			Tool newTool = AddTool("Line Trimmer #1", 100);

			// add a new member
			Member newMember = new Member("Bob", "Jeff", "12345678", "1234");
			system.add(newMember);

			// borrow a tool
			system.borrowTool(newMember, newTool);

			// check the member is borrowing the tool
			Assert.IsTrue(newMember.Tools.Length == 1);

			// check the member is in the tool's borrowing list
			MemberCollection toolBorrowers = newTool.GetBorrowers;
			Assert.IsTrue(toolBorrowers.search(newMember));

			// check the tool's borrowed count went up
			Assert.IsTrue(newTool.NoBorrowings == 1);
		}

		[TestMethod]
		public void TestReturnTool() {
			SelectCategoryAndType("Gardening Tools", "Line Trimmers");

			// add tool
			Tool newTool = AddTool("Line Trimmer #1", 100);

			// add a new member
			Member newMember = new Member("Bob", "Jeff", "12345678", "1234");
			system.add(newMember);

			// borrow a tool
			system.borrowTool(newMember, newTool);

			// return the tool
			system.returnTool(newMember, newTool);

			// check the member isn't borrowing the tool anymore
			Assert.IsTrue(newMember.Tools.Length == 0);

			// check the member isn't in the tool's borrowing list anymore
			MemberCollection toolBorrowers = newTool.GetBorrowers;
			Assert.IsFalse(toolBorrowers.search(newMember));
		}
	}
}
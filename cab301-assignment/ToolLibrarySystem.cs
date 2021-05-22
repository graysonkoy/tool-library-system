using System;
using System.Collections.Generic;

namespace cab301_assignment {
	public class ToolLibrarySystem {
		// constructors
		public ToolLibrarySystem(Dictionary<string, List<string>> toolCategoriesAndTypes) {
			// create tool collections
			Database.toolCollections = new Dictionary<string, List<ToolCollection>>();

			// loop through categories and create dictionary entries
			foreach (var entry in toolCategoriesAndTypes) {
				Database.toolCollections.Add(entry.Key, new List<ToolCollection>());

				// loop through types in category and add to list
				foreach (var type in entry.Value) {
					ToolCollection newToolCollection = new ToolCollection(type);
					Database.toolCollections[entry.Key].Add(newToolCollection);
				}
			}

			/*
			int toolCategories = tools.Length;
			this.toolCollections = new ToolCollection[toolCategories][];
			for (int i = 0; i < toolCategories; i++) {
				int toolTypesForThisCategory = tools[i].Length;
				this.toolCollections[i] = new ToolCollection[toolTypesForThisCategory];
			}
			*/

			// create member collection
			Database.memberCollection = new MemberCollection();
		}

		// private functions
		private void changeQuantityOfTool(ToolCollection collection, Tool aTool, int quantity) {
			Tool[] tools = collection.toArray();
			for (int i = 0; i < collection.Number; i++) {
				// check if this is the tool
				if (tools[i].CompareTo(aTool) == 0) {
					// check if we're removing too many TODO: CHECK THIS
					int newQuantity = tools[i].Quantity + quantity;
					int newAvailableQuantity = tools[i].AvailableQuantity + quantity;
					if (newQuantity < 0 || newAvailableQuantity < 0)
						throw new ToolException($"Quantity change for {tools[i].Name} failed (removing too many)");

					// change the quantity
					tools[i].Quantity += quantity;
					tools[i].AvailableQuantity += quantity;

					return;
				}
			}

			// tool wasn't found
			throw new ToolException($"Failed to add {quantity} extra {aTool.ToString()} to the system (tool not found)");
		}

		// public functions
		public void add(Tool aTool) { // add a new tool to the system
			ToolCollection currentCollection = Database.getCurrentCollection();
			currentCollection.add(aTool);
		}

		public void add(Tool aTool, int quantity) { // add new pieces of an existing tool to the system
			if (quantity < 0)
				throw new ToolException("Failed to add stock, additional quantity is negative");

			ToolCollection currentCollection = Database.getCurrentCollection();
			changeQuantityOfTool(currentCollection, aTool, quantity);
		}

		public void delete(Tool aTool) { // delete a given tool from the system
			ToolCollection currentCollection = Database.getCurrentCollection();
			currentCollection.delete(aTool);
		}

		public void delete(Tool aTool, int quantity) { // remove some pieces of a tool from the system
			if (quantity < 0)
				throw new ToolException("Failed to remove stock, removed quantity is negative");

			ToolCollection currentCollection = Database.getCurrentCollection();
			changeQuantityOfTool(currentCollection, aTool, -quantity);
		}

		public void add(Member aMember) { // add a new member to the system
			if (Database.memberCollection.search(aMember))
				throw new ToolException("A member with the same name already exists");

			Database.memberCollection.add(aMember);
		}

		public void delete(Member aMember) { // delete a member from the system
			// check if the member is holding any tools
			if (aMember.Tools.Length != 0)
				throw new ToolException($"Can't delete member {aMember.ToString()} (member is holding tools)");

			Database.memberCollection.delete(aMember);
		}

		public void displayBorrowingTools(Member aMember) { // given a member, display all the tools that the member are currently renting
			string[] borrowedToolNames = listTools(aMember);
			if (borrowedToolNames.Length == 0) {
				Console.WriteLine($"{aMember.ToString()} is not borrowing any tools");
			} else {
				Console.WriteLine($"{aMember.ToString()}'s borrowed tools");

				for (int i = 0; i < borrowedToolNames.Length; i++) {
					Console.WriteLine($"{i + 1}. {borrowedToolNames[i]}");
				}
			}
		}

		public void displayTools(string aToolType) { // display all the tools of a tool type selected by a member
			ToolCollection selectedCollection = null;
			foreach (var entry in Database.toolCollections) {
				var collections = entry.Value;

				foreach (var collection in collections) {
					if (collection.Name == aToolType) {
						selectedCollection = collection;
						goto breakLoop;
					}
				}
			}

			breakLoop:
			if (selectedCollection == null) {
				Console.WriteLine($"There are no tools in tool type '{aToolType}'");
				return;
			}

			if (selectedCollection.Number == 0) {
				Console.WriteLine($"No tools found in tool type '{aToolType}'");
			} else {
				Console.WriteLine($"Tools in tool type '{aToolType}'");
				Tool[] tools = selectedCollection.toArray();
				for (int i = 0; i < selectedCollection.Number; i++) {
					Console.WriteLine($"{i + 1}. {tools[i].ToString()}");
				}
			}
		}

		public void borrowTool(Member aMember, Tool aTool) { // a member borrows a tool from the tool library
			aMember.addTool(aTool);
		}

		public void returnTool(Member aMember, Tool aTool) { // a member return a tool to the tool library
			aMember.deleteTool(aTool);
		}

		public string[] listTools(Member aMember) { // get a list of tools that are currently held by a given member
			return aMember.Tools;
		}

		public void displayTopTHree() { // display top three most frequently borrowed tools by the members in the descending order by the number of times each tool has been borrowed.
			const int topAmount = 3;
			Tool[] topBorrowed = new Tool[topAmount];

			foreach (var entry in Database.toolCollections) {
				var collections = entry.Value;

				foreach (var collection in collections) {
					Tool[] tools = collection.toArray();

					foreach (Tool tool in tools) {
						for (int i = 0; i < topBorrowed.Length; i++) {
							if (topBorrowed[i] != tool &&
								(topBorrowed[i] == null || tool.NoBorrowings > topBorrowed[i].NoBorrowings))
							{
								// move all the numbers down one place
								for (int j = (topBorrowed.Length - 1) - 1; j >= i; j--) {
									topBorrowed[j + 1] = topBorrowed[j];
								}

								// assign the new place value
								topBorrowed[i] = tool;

								break;
							}
						}
					}
				}
			}

			Console.WriteLine($"Top {topBorrowed.Length} most frequently borrowed tools:");
			for (int i = 0; i < topBorrowed.Length; i++) {
				Console.WriteLine($"#{i + 1}: {topBorrowed[i].Name} - {topBorrowed[i].NoBorrowings} borrows");
			}
		}
	}
}
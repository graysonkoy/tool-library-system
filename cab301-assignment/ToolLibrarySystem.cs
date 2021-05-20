using System;
using System.Collections.Generic;

namespace cab301_assignment {
	class ToolLibrarySystem {
		// constructors
		public ToolLibrarySystem(Dictionary<string, List<string>> toolCategoriesAndTypes) {
			// create tool collections
			Database.toolCollections = new Dictionary<string, List<ToolCollection>>(); // TODO: check if this is allowed

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
		private bool getCurrentToolCollection(out ToolCollection outCollection) {
			foreach (var entry in Database.toolCollections) {
				var collections = entry.Value;

				if (entry.Key == Program.selectedCategory) {
					foreach (var collection in collections) {
						if (collection.Name == Program.selectedType) {
							outCollection = collection;
							return true;
						}
					}
				}
			}

			// didn't find collection
			Console.WriteLine($"No current tool collection found");
			outCollection = default(ToolCollection);
			return false;
		}

		private void changeQuantityOfTool(ToolCollection collection, Tool aTool, int quantity) {
			Tool[] tools = collection.toArray();
			for (int i = 0; i < collection.Number; i++) {
				// check if this is the tool
				if (tools[i].CompareTo(aTool) == 0) {
					// add the extra quantity
					tools[i].Quantity += quantity;
					tools[i].AvailableQuantity += quantity;

					// check if there's under 0 tools and clamp
					if (tools[i].Quantity < 0) {
						tools[i].Quantity = 0;
						Console.WriteLine($"{tools[i].ToString()} quantity clamped to 0 (removed too many");
					}

					// TODO: check this
					if (tools[i].AvailableQuantity < 0) {
						tools[i].AvailableQuantity = 0;
						Console.WriteLine($"{tools[i].ToString()} available quantity clamped to 0 (removed too many");
					}

					return;
				}
			}

			// tool wasn't found
			Console.WriteLine($"Failed to add {quantity} extra {aTool.ToString()} to the system (tool not found)");
		}

		// public functions
		public void add(Tool aTool) { // add a new tool to the system
			ToolCollection currentCollection;
			if (!getCurrentToolCollection(out currentCollection))
				return;

			currentCollection.add(aTool);
		}

		public void add(Tool aTool, int quantity) { // add new pieces of an existing tool to the system
			ToolCollection currentCollection;
			if (!getCurrentToolCollection(out currentCollection))
				return;

			changeQuantityOfTool(currentCollection, aTool, quantity);
		}

		public void delete(Tool aTool) { // delete a given tool from the system
			ToolCollection currentCollection;
			if (!getCurrentToolCollection(out currentCollection))
				return;

			currentCollection.delete(aTool);
		}

		public void delete(Tool aTool, int quantity) { // remove some pieces of a tool from the system
			ToolCollection currentCollection;
			if (!getCurrentToolCollection(out currentCollection))
				return;

			changeQuantityOfTool(currentCollection, aTool, -quantity);
		}

		public void add(Member aMember) { // add a new memeber to the system
			Database.memberCollection.add(aMember);
		}

		public void delete(Member aMember) { // delete a member from the system
			Database.memberCollection.delete(aMember);
		}

		public void displayBorrowingTools(Member aMember) { // given a member, display all the tools that the member are currently renting
			Console.Write($"{aMember.ToString()}'s borrowed tools");
			foreach (string tool in listTools(aMember)) {
				Console.Write(tool);
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

			Console.WriteLine($"Tools in tool type '{aToolType}'");
			Tool[] tools = selectedCollection.toArray();
			for (int i = 0; i < selectedCollection.Number; i++) {
				Console.WriteLine($"{i + 1}. ${tools[i].ToString()}");
			}

			Console.WriteLine();
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

		public void displayTopThree() { // display top three most frequently borrowed tools by the members in the descending order by the number of times each tool has been borrowed.
			const int topAmount = 3;
			Tool[] topBorrowed = new Tool[topAmount];

			foreach (var entry in Database.toolCollections) {
				var collections = entry.Value;

				foreach (var collection in collections) {
					Tool[] tools = collection.toArray();

					foreach (Tool tool in tools) {
						for (int i = 0; i < topBorrowed.Length; i++) {
							if (tool.NoBorrowings > topBorrowed[i].NoBorrowings) {
								// move all the numbers down one place
								for (int j = i; j < topBorrowed.Length - 1; j++)
									topBorrowed[j + 1] = topBorrowed[j];

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
				Console.WriteLine($"#{i}: {topBorrowed[i].Name} - {topBorrowed[i].NoBorrowings} borrows");
			}
		}
	}
}

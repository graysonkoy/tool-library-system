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

			// create member collection
			Database.memberCollection = new MemberCollection();
		}

		// private functions
		/// <summary>
		/// Changes the quantity of a tool
		/// </summary>
		/// <param name="aTool">Tool to adjust quantity for</param>
		/// <param name="quantity">Quantity change</param>
		private void changeQuantityOfTool(Tool aTool, int quantity) {
			// check if we're removing too many
			int newQuantity = aTool.Quantity + quantity;
			int newAvailableQuantity = aTool.AvailableQuantity + quantity;
			if (newQuantity < 0 || newAvailableQuantity < 0)
				throw new ToolException($"Quantity change for {aTool.Name} failed (removing too many)");

			// change the quantity
			aTool.Quantity += quantity;
			aTool.AvailableQuantity += quantity;
		}

		// public functions
		/// <summary>
		/// Adds a new tool to the system
		/// </summary>
		/// <param name="aTool">Tool to add</param>
		public void add(Tool aTool) {
			ToolCollection currentCollection = Database.getCurrentCollection();
			currentCollection.add(aTool);
		}

		/// <summary>
		/// Adds additional stock of a tool
		/// </summary>
		/// <param name="aTool">Tool to add stock to</param>
		/// <param name="quantity">Quantity change</param>
		public void add(Tool aTool, int quantity) {
			if (quantity < 0)
				throw new ToolException("Failed to add stock, additional quantity is negative");

			changeQuantityOfTool(aTool, quantity);
		}

		/// <summary>
		/// Deletes a tool from the system
		/// </summary>
		/// <param name="aTool">Tool to delete</param>
		public void delete(Tool aTool) {
			ToolCollection currentCollection = Database.getCurrentCollection();
			currentCollection.delete(aTool);
		}

		/// <summary>
		/// Removes stock of a tool
		/// </summary>
		/// <param name="aTool">Tool to remove stock from</param>
		/// <param name="quantity">Quantity change</param>
		public void delete(Tool aTool, int quantity) {
			if (quantity < 0)
				throw new ToolException("Failed to remove stock, removed quantity is negative");

			changeQuantityOfTool(aTool, -quantity);
		}

		/// <summary>
		/// Adds a new member to the system
		/// </summary>
		/// <param name="aMember">Member to add</param>
		public void add(Member aMember) {
			// check if the member already exists
			if (Database.memberCollection.search(aMember))
				throw new ToolException("A member with the same name already exists");

			Database.memberCollection.add(aMember);
		}

		/// <summary>
		/// Deletes a member from the system
		/// </summary>
		/// <param name="aMember">Member to delete</param>
		public void delete(Member aMember) {
			// check if the member is holding any tools
			if (aMember.Tools.Length != 0)
				throw new ToolException($"Can't delete member {aMember.ToString()} (member is holding tools)");

			Database.memberCollection.delete(aMember);
		}

		/// <summary>
		/// Displays a member's borrowed tools
		/// </summary>
		/// <param name="aMember">Member to display borrowed tools for</param>
		public void displayBorrowingTools(Member aMember) {
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

		/// <summary>
		/// Displays all of the tools of a certain type
		/// </summary>
		/// <param name="aToolType">Tool type</param>
		public void displayTools(string aToolType) {
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
				Console.WriteLine($"Tool type '{aToolType}' not found");
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

		/// <summary>
		/// Borrows a tool for a member
		/// </summary>
		/// <param name="aMember">Borrowing member</param>
		/// <param name="aTool">Tool to borrow</param>
		public void borrowTool(Member aMember, Tool aTool) {
			aMember.addTool(aTool);
		}

		/// <summary>
		/// Returns a tool for a member
		/// </summary>
		/// <param name="aMember">Borrowing member</param>
		/// <param name="aTool">Tool to return</param>
		public void returnTool(Member aMember, Tool aTool) {
			aMember.deleteTool(aTool);
		}

		/// <summary>
		/// Returns a member's borrowed tools
		/// </summary>
		/// <param name="aMember">Member to get borrowed tools for</param>
		/// <returns>An array containing the member's borrowed tools</returns>
		public string[] listTools(Member aMember) { // get a list of tools that are currently held by a given member
			return aMember.Tools;
		}

		/// <summary>
		/// Displays the top three most-borrowed tools
		/// </summary>
		public void displayTopTHree() {
			const int topAmount = 3;
			Tool[] topBorrowed = new Tool[topAmount];

			foreach (var entry in Database.toolCollections) {
				var collections = entry.Value;

				foreach (var collection in collections) {
					Tool[] tools = collection.toArray();

					foreach (Tool tool in tools) {
						for (int i = 0; i < topBorrowed.Length; i++) {
							// check if this tool is already at this place
							if (topBorrowed[i] == tool)
								continue;

							if (topBorrowed[i] == null || tool.NoBorrowings > topBorrowed[i].NoBorrowings) {
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

			bool noTopTools = true;
			for (int i = 0; i < topBorrowed.Length; i++) {
				if (topBorrowed[i].NoBorrowings != 0) {
					noTopTools = false;
					break;
				}
			}

			if (noTopTools) {
				Console.WriteLine("No tools have been borrowed");
				return;
			}

			Console.WriteLine($"Top {topBorrowed.Length} most frequently borrowed tools:");

			for (int i = 0; i < topBorrowed.Length; i++) {
				Console.WriteLine($"#{i + 1}: {topBorrowed[i].Name} - {topBorrowed[i].NoBorrowings} borrows");
			}
		}
	}
}
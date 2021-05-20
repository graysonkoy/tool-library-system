using System;
using System.Collections.Generic;
using System.Text;

namespace cab301_assignment {
	class ToolLibrarySystem {
		private Dictionary<string, List<ToolCollection>> toolCollections;
		private MemberCollection memberCollection;

		// constructors
		public ToolLibrarySystem(Dictionary<string, List<string>> toolCategoriesAndTypes) {
			// create tool collections
			this.toolCollections = new Dictionary<string, List<ToolCollection>>(); // TODO: check if this is allowed

			// loop through categories and create dictionary entries
			foreach (var entry in toolCategoriesAndTypes) {
				this.toolCollections.Add(entry.Key, new List<ToolCollection>());

				// loop through types in category and add to list
				foreach (var type in entry.Value) {
					ToolCollection newToolCollection = new ToolCollection(type);
					this.toolCollections[entry.Key].Add(newToolCollection);
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
			this.memberCollection = new MemberCollection();
		}

		// private functions
		private bool getToolCollectionForTool(Tool aTool, out ToolCollection collection) {
			foreach (var entry in toolCollections) {
				var collections = entry.Value;

				for (int i = 0; i < collections.Count; i++) {
					Tool[] tools = collections[i].toArray();

					foreach (Tool tool in tools) {
						// check if this is the tool
						if (tool.CompareTo(aTool) == 0) {
							// found the collection
							collection = collections[i];
							return true;
						}
					}
				}
			}

			// didn't find collection (tool doesn't exist in the system)
			Console.WriteLine($"tool collection not found for tool {aTool.ToString()}");
			return false;
		}

		private void changeQuantityOfTool(Tool aTool, int quantity) {
			ToolCollection collection;
			if (!getToolCollectionForTool(aTool, out collection))
				return;

			Tool[] tools = collection.toArray();
			for (int i = 0; i < collection.Number; i++) {
				// check if this is the tool
				if (tools[i].CompareTo(aTool) == 0) {
					// add the extra quantity
					tools[i].Quantity += quantity;

					// check if there's under 0 tools and clamp
					if (tools[i].Quantity < 0) {
						tools[i].Quantity = 0;
						Console.WriteLine($"{tools[i].ToString()} quantity clamped to 0 (removed too many");
					}

					return;
				}
			}

			// tool wasn't found
			Console.WriteLine($"Failed to add {quantity} extra {aTool.ToString()} to the system (tool not found)");
		}

		// public functions
		public void add(Tool aTool) { // add a new tool to the system
			toolCollection.add(aTool);
		}

		public void add(Tool aTool, int quantity) { // add new pieces of an existing tool to the system
			changeQuantityOfTool(aTool, quantity);
		}

		public void delete(Tool aTool) { // delete a given tool from the system
			toolCollection.delete(aTool);
		}

		public void delete(Tool aTool, int quantity) { // remove some pieces of a tool from the system
			changeQuantityOfTool(aTool, -quantity);
		}

		public void add(Member aMember) { // add a new memeber to the system
			memberCollection.add(aMember);
		}

		public void delete(Member aMember) { // delete a member from the system
			memberCollection.delete(aMember);
		}

		public void displayBorrowingTools(Member aMember) { // given a member, display all the tools that the member are currently renting
			Member[] members = memberCollection.toArray();
			
		}

		public void displayTools(string aToolType) { // display all the tools of a tool type selected by a member

		}

		public void borrowTool(Member aMember, Tool aTool) { // a member borrows a tool from the tool library

		}

		public void returnTool(Member aMember, Tool aTool) { // a member return a tool to the tool library

		}

		public string[] listTools(Member aMember) { // get a list of tools that are currently held by a given member

		}

		public void displayTopTHree() { // display top three most frequently borrowed tools by the members in the descending order by the number of times each tool has been borrowed.

		}
	}
}

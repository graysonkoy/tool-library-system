using System;
using System.Collections.Generic;
using System.Text;

namespace cab301_assignment {
	class ToolCollection : iToolCollection {
		private string name;
		private int number;
		private Tool[] tools;

		// constructors
		public ToolCollection(string name) {
			this.name = name;
			this.number = 0;

			// initialise tool array
			tools = new Tool[number];
		}

		public ToolCollection(Member member) { // TODO: is this okay?
			this.name = $"{member.ToString()}'s Tools";
			this.number = 0;

			// initialise tool array
			tools = new Tool[number];
		}

		// public variable accessors
		public int Number { // get the number of the types of tools in the community library
			get => number;
		}

		// private functions
		private void resize(int newSize) {
			Array.Resize(ref tools, newSize);
		}

		// public functions
		public void add(Tool aTool) { // add a given tool to this tool collection
			number++;

			// resize the tool array to fit the new tool
			resize(number);

			// insert the new tool at the end
			tools[number - 1] = aTool;
		}

		public void delete(Tool aTool) { // delete a given tool from this tool collection
			bool deleted = false;
			for (int i = 0; i < number; i++) {
				if (!deleted) {
					// haven't deleted yet, check if this is the tool
					if (tools[i].CompareTo(aTool) == 0) {
						tools[i] = null;
						deleted = true;
					}
				}
				else {
					// deleted a tool, move the rest back
					tools[i - 1] = tools[i];
				}
			}

			if (deleted) {
				// resize the tool array to account for the deleted tool
				number--;
				resize(number);
			}
			else {
				Console.WriteLine($"Failed to delete tool {aTool.ToString()} from collection {name} (tool was not found)");
			}
		}

		public Boolean search(Tool aTool) { // search a given tool in this tool collection. Return true if this tool is in the tool collection; return false otherwise
			for (int i = 0; i < number; i++) {
				// check if this is the tool
				if (tools[i].CompareTo(aTool) == 0)
					return true;
			}

			// tool wasn't found
			return false;
		}

		public Tool[] toArray() { // output the tools in this tool collection to an array of Tool
			return tools;
		}
	}
}

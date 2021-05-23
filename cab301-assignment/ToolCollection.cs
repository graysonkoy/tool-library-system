using System;
using System.Collections.Generic;
using System.Text;

namespace cab301_assignment {
	public class ToolCollection : iToolCollection {
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

		public string Name { // TODO: check if we're allowed to make this, it's not in the interface
			get => name;
		}

		// private functions
		/// <summary>
		/// Resizes the tool collection array
		/// </summary>
		/// <param name="newSize">New array size</param>
		private void resize(int newSize) {
			Array.Resize(ref tools, newSize);
		}

		// public functions
		/// <summary>
		/// Adds a new tool to the collection
		/// </summary>
		/// <param name="aTool">Tool to add</param>
		public void add(Tool aTool) {
			number++;

			// resize the tool array to fit the new tool
			resize(number);

			// insert the new tool at the end
			tools[number - 1] = aTool;
		}

		/// <summary>
		/// Deletes a tool from the collection
		/// </summary>
		/// <param name="aTool">Tool to delete</param>
		public void delete(Tool aTool) {
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

			if (!deleted)
				throw new ToolException($"Failed to delete tool {aTool.ToString()} from collection {name} (tool was not found)");

			// resize the tool array to account for the deleted tool
			number--;
			resize(number);
		}

		/// <summary>
		/// Searches for a tool in the collection
		/// </summary>
		/// <param name="aTool">Tool to search for</param>
		/// <returns>Whether the tool was found</returns>
		public Boolean search(Tool aTool) {
			for (int i = 0; i < number; i++) {
				// check if this is the tool
				if (tools[i].CompareTo(aTool) == 0)
					return true;
			}

			// tool wasn't found
			return false;
		}

		/// <summary>
		/// Returns an array of tools in the collection
		/// </summary>
		/// <returns>Tool array</returns>
		public Tool[] toArray() {
			return tools;
		}
	}
}

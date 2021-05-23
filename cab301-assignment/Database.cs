using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace cab301_assignment {
	/// <summary>
	/// Database class containing the data for the application as well as helper functions
	/// for accessing data
	/// </summary>
	public static class Database {
		public static Dictionary<string, List<ToolCollection>> toolCollections;
		public static MemberCollection memberCollection;

		public static string selectedCategory = "";
		public static string selectedType = "";

		// public functions
		/// <summary>
		/// Gets all of the tool categories
		/// </summary>
		/// <returns>List of categories</returns>
		public static List<string> getToolCategories() {
			List<string> categories = new List<string>();
			foreach (var entry in toolCollections) {
				categories.Add(entry.Key);
			}

			return categories;
		}

		/// <summary>
		/// Gets the currently selected tool collection
		/// </summary>
		/// <returns>Currently selected tool collection</returns>
		public static ToolCollection getCurrentCollection() {
			ToolCollection currentCollection;
			if (!getTools(selectedCategory, selectedType, out currentCollection))
				throw new ToolException($"Failed to get tool collection for category '{selectedCategory}' and type '{selectedType}'");

			return currentCollection;
		}

		/// <summary>
		/// Gets all of the tool types in a category
		/// </summary>
		/// <param name="category">Tool category</param>
		/// <param name="types">Output of tool types</param>
		/// <returns>Success</returns>
		public static bool getToolTypes(string category, out List<string> types) {
			types = new List<string>();
			foreach (var entry in toolCollections) {
				if (entry.Key == category) {
					foreach (ToolCollection collection in entry.Value) {
						types.Add(collection.Name);
					}

					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Gets the tool collection for a specific category and type
		/// </summary>
		/// <param name="category">Tool category</param>
		/// <param name="type">Tool type</param>
		/// <param name="tools">Output of tool collection</param>
		/// <returns>Success</returns>
		public static bool getTools(string category, string type, out ToolCollection tools) {
			foreach (var entry in toolCollections) {
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

		/// <summary>
		/// Gets a list of borrowable tools from a tool collection
		/// </summary>
		/// <param name="collection">Tool collection</param>
		/// <returns>List of borrowable tools</returns>
		public static List<Tool> getBorrowableTools(ToolCollection collection) {
			List<Tool> tools = new List<Tool>();

			foreach (Tool tool in collection.toArray()) {
				if (tool.AvailableQuantity > 0) {
					tools.Add(tool);
				}
			}

			return tools;
		}

		/// <summary>
		/// Gets a tool by name
		/// </summary>
		/// <param name="toolName">Tool name to search for</param>
		/// <param name="output">Output of tool</param>
		/// <param name="caseInsensitive">Whether the search is case insensitive</param>
		/// <returns>Whether the tool was found or not</returns>
		public static bool getToolByName(string toolName, out Tool output, bool caseInsensitive = false) {
			foreach (var entry in toolCollections) {
				foreach (ToolCollection collection in entry.Value) {
					foreach (Tool tool in collection.toArray()) {
						if (caseInsensitive) {
							if (tool.Name.ToLower() == toolName.ToLower()) {
								output = tool;
								return true;
							}
						} else {
							if (tool.Name == toolName) {
								output = tool;
								return true;
							}
						}
					}
				}
			}

			output = default(Tool);
			return false;
		}

		/// <summary>
		/// Gets a member by name
		/// </summary>
		/// <param name="firstName">First name to search for</param>
		/// <param name="lastName">Last name to search for</param>
		/// <param name="output">Output of member</param>
		/// <returns>Whether the member was found</returns>
		public static bool getMemberByName(string firstName, string lastName, out Member output) {
			Member dummy = new Member(firstName, lastName, "0", "0");

			foreach (Member member in memberCollection.toArray()) {
				if (member.CompareTo(dummy) == 0) {
					output = member;
					return true;
				}
			}

			output = default(Member);
			return false;
		}
	}

	/// <summary>
	/// Custom exception class
	/// </summary>
	public class ToolException : Exception {
		public ToolException() {}
		public ToolException(string message) : base(message) {}
	}
}
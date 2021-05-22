using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace cab301_assignment {
	public static class Database {
		public static Dictionary<string, List<ToolCollection>> toolCollections;
		public static MemberCollection memberCollection;

		// TODO: this is gross
		public static string selectedCategory = "";
		public static string selectedType = "";

		// public functions
		public static List<string> getToolCategories() {
			List<string> categories = new List<string>();
			foreach (var entry in toolCollections) {
				categories.Add(entry.Key);
			}

			return categories;
		}

		public static ToolCollection getCurrentCollection() {
			ToolCollection currentCollection;
			if (!getTools(selectedCategory, selectedType, out currentCollection))
				throw new ToolException($"Failed to get tool collection for category '{selectedCategory}' and type '{selectedType}'");

			return currentCollection;
		}

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

		public static List<Tool> getBorrowableTools(ToolCollection collection) {
			List<Tool> tools = new List<Tool>();

			foreach (Tool tool in collection.toArray()) {
				if (tool.AvailableQuantity > 0) {
					tools.Add(tool);
				}
			}

			return tools;
		}

		public static bool getToolByName(string toolName, out Tool output) {
			foreach (var entry in toolCollections) {
				foreach (ToolCollection collection in entry.Value) {
					foreach (Tool tool in collection.toArray()) {
						if (tool.Name == toolName) {
							output = tool;
							return true;
						}
					}
				}
			}

			output = default(Tool);
			return false;
		}

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

	public class ToolException : Exception {
		public ToolException() {}
		public ToolException(string message) : base(message) {}
	}
}
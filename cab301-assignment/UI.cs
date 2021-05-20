using System;
using System.Collections.Generic;
using System.Text;

namespace cab301_assignment {
	public class UI {
		public enum MenuItemType_t {
			MENU_TITLE,
			MENU_SPACER,
			MENU_OPTION
		}

		public interface IMenuItem {
			public MenuItemType_t Type {
				get;
			}

			public string Text {
				get;
			}

			public void call() { }
		}

		public class MenuTitle : IMenuItem {
			private MenuItemType_t type = MenuItemType_t.MENU_TITLE;

			private string text;

			public MenuTitle(string text) {
				this.text = text;
			}

			public MenuItemType_t Type {
				get => type;
			}

			public string Text {
				get => text;
			}
		}

		public class MenuSpacer : IMenuItem {
			private MenuItemType_t type = MenuItemType_t.MENU_SPACER;

			private string text = "";

			public MenuItemType_t Type {
				get => type;
			}

			public string Text {
				get => text;
			}
		}

		public class MenuOption : IMenuItem {
			private MenuItemType_t type = MenuItemType_t.MENU_OPTION;

			private string text;

			public delegate void Callback();
			private Callback callbackFunc;

			public MenuOption(string text, Callback callback) {
				this.text = text;
				this.callbackFunc = callback;
			}

			public MenuItemType_t Type {
				get => type;
			}

			public string Text {
				get => text;
			}

			public void call() {
				callbackFunc();
			}
		}

		public static string getTextInput(string text) {
			Console.Write(text);
			string resp = Console.ReadLine();
			Console.WriteLine();

			return resp;
		}

		public static bool getIntInput(string text, out int index) {
			Console.Write(text);
			char resp = Console.ReadKey().KeyChar;
			Console.WriteLine();

			return int.TryParse(resp.ToString(), out index);
		}

		public static int listSelector(string selectText, string[] inputs) {
			// print list

			// get input
			int index;
			getIntInput(selectText, out index);

			// validate index
			if (index < 0 || index >= inputs.Length)
				return -1;

			return index;
		}

		public static void drawMenu(string title, List<IMenuItem> items, bool waitAtEnd = false) {
			Console.Clear();

			Console.WriteLine($"[{title}]");

			List<IMenuItem> options = new List<IMenuItem>();
			bool has_options = false;
			foreach (IMenuItem item in items) {
				switch (item.Type) {
					case MenuItemType_t.MENU_TITLE: {
						Console.WriteLine($"{item.Text}");
						break;
					}
					case MenuItemType_t.MENU_SPACER: {
						Console.WriteLine();
						break;
					}
					case MenuItemType_t.MENU_OPTION: {
						Console.WriteLine($"{options.Count}: {item.Text}");
						options.Add(item);

						if (!has_options)
							has_options = true;

						break;
					}
				}
			}

			if (has_options) {
				while (true) {
					int index;
					if (getIntInput("Enter option: ", out index)) {
						options[index].call();
						break;
					} else {
						Console.WriteLine("Please select a valid option");
					}
				}
			}
			else {
				if (waitAtEnd) {
					Console.WriteLine("Press any key to continue");
					Console.ReadKey();
				}
			}
		}
	}
}

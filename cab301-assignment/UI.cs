using System;
using System.Collections.Generic;
using System.Text;

namespace cab301_assignment {
	class UI {
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
			private bool zeroth = false;

			public delegate void Callback();
			private Callback callbackFunc;

			public MenuOption(string text, Callback callback) {
				this.text = text;
				this.callbackFunc = callback;
			}

			public MenuOption(string text, Callback callback, bool zeroth) : this(text, callback) {
				this.zeroth = zeroth;
			}

			public MenuItemType_t Type {
				get => type;
			}

			public string Text {
				get => text;
			}

			public bool Zeroth {
				get => zeroth;
			}

			public void call() {
				callbackFunc();
			}
		}

		public static void waitToContinue() {
			Console.Write("Press any key to continue");
			Console.ReadKey();
		}

		public static string getTextInput(string text) {
			Console.Write(text);
			string resp = Console.ReadLine();

			return resp;
		}

		public static bool getIntInput(string text, out int output) {
			Console.Write(text);
			string resp = Console.ReadLine();

			return int.TryParse(resp.ToString(), out output);
		}

		public static int getIntInputStrict(string text, bool positiveOnly) {
			int output;
			while (true) {
				if (!UI.getIntInput(text, out output)) {
					Console.WriteLine("Please enter a valid number");
				}
				else if (positiveOnly && output < 0) {
					Console.WriteLine("Please enter a valid amount");
				}
				else {
					break;
				}
			}

			return output;
		}

		public static bool listSelector<T>(string text, string selectText, List<T> inputs, out T selected) {
			Console.WriteLine(text);

			// print list
			for (int i = 0; i < inputs.Count; i++) {
				Console.WriteLine($" {i + 1}. {inputs[i]}");
			}

			// get input
			while (true) {
				int index;
				getIntInput(selectText, out index);

				if (index == 0) {
					selected = default(T);
					return false;
				}

				int actualIndex = index - 1;
				if (actualIndex < 0 || actualIndex >= inputs.Count) {
					Console.WriteLine("Please select a valid option");
					continue;
				}

				selected = inputs[actualIndex];

				return true;
			}
		}

		public static void drawMenu(string titleStr, List<IMenuItem> items, bool waitAtEnd = false) {
			Console.Clear();

			Console.WriteLine($"[{titleStr}]");

			List<MenuOption> options = new List<MenuOption>();
			MenuOption lastOption = null;

			bool has_options = false;
			foreach (IMenuItem item in items) {
				switch (item.Type) {
					case MenuItemType_t.MENU_TITLE: {
						var title = (MenuTitle)item;

						Console.WriteLine($"{title.Text}");

						break;
					}
					case MenuItemType_t.MENU_SPACER: {
						var spacer = (MenuSpacer)item;

						Console.WriteLine();

						break;
					}
					case MenuItemType_t.MENU_OPTION: {
						var option = (MenuOption)item;

						if (lastOption == null && option.Zeroth) {
							lastOption = option;
							break;
						}

						Console.WriteLine($"{options.Count + 1}: {option.Text}");
						options.Add(option);

						if (!has_options)
							has_options = true;

						break;
					}
				}
			}

			if (lastOption != null) {
				Console.WriteLine($"0: {lastOption.Text}");
			}

			if (has_options) {
				while (true) {
					int index;
					if (!getIntInput("Enter option: ", out index)) {
						Console.WriteLine("Please enter a valid number");
						continue;
					}

					if (lastOption != null && index == 0) {
						lastOption.call();
					}
					else {
						int actualIndex = index - 1;
						if (actualIndex < 0 || actualIndex >= options.Count) {
							Console.WriteLine("Please select a valid option");
							continue;
						}

						options[actualIndex].call();
					}

					break;
				}
			}
			else {
				if (waitAtEnd) {
					waitToContinue();
				}
			}
		}
	}
}
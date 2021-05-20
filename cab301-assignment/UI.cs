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

		public static void waitForInput() {
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

		public static bool listSelector(string text, string selectText, List<string> inputs, out string selected) {
			Console.WriteLine(text);

			// print list
			for (int i = 0; i < inputs.Count; i++) {
				Console.WriteLine($" {i + 1}. {inputs[i]}");
			}

			// get input
			int index;
			getIntInput(selectText, out index);

			try {
				selected = inputs[index - 1];
				return true;
			}
			catch (Exception) {
				selected = "";
				return false;
			}
		}

		public static bool toolSelector(string text, string selectText, ToolCollection inputs, out Tool selected) {
			Console.WriteLine(text);

			// print list
			Tool[] tools = inputs.toArray();
			for (int i = 0; i < inputs.Number; i++) {
				Console.WriteLine($" {i + 1}. {tools[i].ToString()}");
			}

			// get input
			int index;
			getIntInput(selectText, out index);

			try {
				selected = tools[index - 1];
				return true;
			}
			catch (Exception) {
				selected = default(Tool);
				return false;
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
					try {
						if (!getIntInput("Enter option: ", out index))
							throw new Exception();

						if (lastOption != null && index == 0) {
							lastOption.call();
						}
						else {
							options[index - 1].call();
						}

						break;
					} catch(Exception) {
						Console.WriteLine("Please select a valid option");
					}
				}
			}
			else {
				if (waitAtEnd) {
					waitForInput();
				}
			}
		}
	}
}

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
			MenuItemType_t Type {
				get;
			}

			string Text {
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

		/// <summary>
		/// Displays a 'press any key to continue' message and waits for input
		/// </summary>
		public static void waitToContinue() {
			Console.Write("Press any key to continue");
			Console.ReadKey();
		}

		/// <summary>
		/// Gets text input
		/// </summary>
		/// <param name="text">Input message</param>
		/// <returns>Text input</returns>
		public static string getTextInput(string text) {
			Console.Write(text);
			string resp = Console.ReadLine();

			return resp;
		}

		/// <summary>
		/// Gets integer input
		/// </summary>
		/// <param name="text">Input message</param>
		/// <param name="output">Output of entered integer</param>
		/// <returns>If an integer was entered</returns>
		public static bool getIntInput(string text, out int output) {
			Console.Write(text);
			string resp = Console.ReadLine();

			return int.TryParse(resp.ToString(), out output);
		}

		/// <summary>
		/// Gets integer input with extra checks, and waits for a valid entry 
		/// </summary>
		/// <param name="text">Input message</param>
		/// <param name="positiveOnly">Whether the number can only be positive</param>
		/// <returns>The entered integer</returns>
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

		/// <summary>
		/// Displays a list and lets the user select an element from it. Pressing 0 will return.
		/// </summary>
		/// <typeparam name="T">Type of list element</typeparam>
		/// <param name="text">List title</param>
		/// <param name="selectText">Input message</param>
		/// <param name="inputs">List of list elements</param>
		/// <param name="selected">Output selected list element</param>
		/// <returns>Whether a list entry was selected, or if the user wants to return</returns>
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

		/// <summary>
		/// Draws a menu
		/// </summary>
		/// <param name="titleStr">Menu title</param>
		/// <param name="items">List of menu items</param>
		public static void drawMenu(string titleStr, List<IMenuItem> items) {
			Console.Clear();

			// draw title
			Console.WriteLine($"[{titleStr}]");

			List<MenuOption> options = new List<MenuOption>();
			MenuOption lastOption = null;

			// go through the menu items, drawing each
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

						// store the option if it's the selected 0 item so it can be drawn at the end
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

			// draw the 0 item
			if (lastOption != null) {
				Console.WriteLine($"0: {lastOption.Text}");
			}

			// handle option selection
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
		}
	}
}
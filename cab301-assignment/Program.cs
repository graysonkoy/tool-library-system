using System;
using System.Collections.Generic;

namespace cab301_assignment {
	class Program {
		public static Dictionary<string, List<string>> toolCategoriesAndTypes;

		public static string selectedCategory = "";
		public static string selectedType = "";

		static void Main(string[] args) {
			toolCategoriesAndTypes = new Dictionary<string, List<string>>();
			toolCategoriesAndTypes.Add("Gardening tools", new List<string> { "Line Trimmers", "Lawn Mowers", "Hand Tools", "Wheelbarrows", "Garden Power Tools" });
			toolCategoriesAndTypes.Add("Flooring Tools", new List<string> { "Scrapers", "Floor Lasers", "Floor Levelling Tools", "Floor Levelling Materials", "Floor Hand Tools", "Tiling Tools" });
			toolCategoriesAndTypes.Add("Fencing Tools", new List<string> { "Hand Tools", "Electric Fencing", "Steel Fencing Tools", "Power Tools", "Fencing Accessories" });
			toolCategoriesAndTypes.Add("Measuring Tools", new List<string> { "Distance Tools", "Laser Measurer", "Measuring Jugs", "Temperature & Humidity Tools", "Levelling Tools", "Markers" });
			toolCategoriesAndTypes.Add("Cleaning Tools", new List<string> { "Draining", "Car Cleaning", "Vacuum", "Pressure Cleaners", "Pool Cleaning", "Floor Cleaning" });
			toolCategoriesAndTypes.Add("Painting Tools", new List<string> { "Sanding Tools", "Brushes", "Rollers", "Paint Removal Tools", "Paint Scrapers", "Sprayers" });
			toolCategoriesAndTypes.Add("Electronic Tools", new List<string> { "Voltage Tester", "Oscilloscopes", "Thermal Imaging", "Data Test Tool", "Insulation Testers" });
			toolCategoriesAndTypes.Add("Electricity Tools", new List<string> { "Test Equipment", "Safety Equipment", "Basic Hand tools", "Circuit Protection", "Cable Tools" });
			toolCategoriesAndTypes.Add("Automotive Tools", new List<string> { "Jacks", "Air Compressors", "Battery Chargers", "Socket Tools", "Braking", "Drivetrain" });
		}
	}
}
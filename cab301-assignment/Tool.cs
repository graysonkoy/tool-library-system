using System;
using System.Collections.Generic;
using System.Text;

namespace cab301_assignment {
	public class Tool : iTool, IComparable<Tool> {
		private string name; // name of this tool
		private int quantity; // quantity of this tool
		private int availableQuantity; // quantity of this tool currently available to lend
		private int noBorrowings; // number of times that this tool has been borrowed
		private MemberCollection borrowers; // members that are currently borrowing the tool

		// constructors
		public Tool(string name, int quantity) {
			this.name = name;
			this.quantity = quantity;

			this.availableQuantity = this.quantity;
			this.noBorrowings = 0;
			this.borrowers = new MemberCollection();
		}

		// public variable accessors
		public string Name {
			get => name;
			set => name = value;
		}

		public int Quantity {
			get => quantity;
			set => quantity = value;
		}

		public int AvailableQuantity {
			get => availableQuantity;
			set => availableQuantity = value;
		}

		public int NoBorrowings {
			get => noBorrowings;
			set => noBorrowings = value;
		}

		public MemberCollection GetBorrowers {
			get => borrowers;
		}

		// public functions
		/// <summary>
		/// Adds a borrower of the tool
		/// </summary>
		/// <param name="aMember">Borrowing member</param>
		public void addBorrower(Member aMember) {
			if (availableQuantity <= 0)
				throw new ToolException($"Can't add borrower for tool {name} (none available)");

			// add borrower and reduce available quantity
			borrowers.add(aMember);
			availableQuantity--;

			// add to total borrowings
			noBorrowings++;
		}

		/// <summary>
		/// Deletes a borrower of the tool
		/// </summary>
		/// <param name="aMember">Borrowing member</param>
		public void deleteBorrower(Member aMember) {
			// remove borrower and increase available quantity
			borrowers.delete(aMember);
			availableQuantity++;
		}

		// overrides
		public override string ToString() { // return a string containing the name and the available quantity this tool 
			return $"{name} - {availableQuantity}/{quantity} available";
		}

		public int CompareTo(Tool other) {
			return other.Name.CompareTo(this.name);
		}
	}
}

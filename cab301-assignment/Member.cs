using System;
using System.Collections.Generic;
using System.Text;

namespace cab301_assignment {
	class Member : iMember, IComparable<Member> {
		private string firstName, lastName;
		private string contactNumber;
		private string pin;

		private ToolCollection tools; // TODO: does this have to be a string array?

		// constructors
		public Member(string firstName, string lastName, string contactNumber, string pin) {
			this.firstName = firstName;
			this.lastName = lastName;
			this.contactNumber = contactNumber;
			this.pin = pin;

			tools = new ToolCollection(this);
		}

		// public variable accessors
		public string FirstName { // get and set the first name of this member
			get => firstName;
			set => firstName = value;
		}

		public string LastName { // get and set the last name of this member
			get => lastName;
			set => lastName = value;
		}

		public string ContactNumber { // get and set the contact number of this member
			get => contactNumber;
			set => contactNumber = value;
		}

		public string PIN { // get and set the password of this member
			get => pin;
			set => pin = value;
		}

		public string[] Tools { // get a list of tools that this member is currently holding
			get {
				// TODO: check if this is okay
				string[] toolString = new string[tools.Number];

				Tool[] toolArray = tools.toArray();
				for (int i = 0; i < tools.Number; i++) {
					toolString[i] = toolArray[i].Name;
				}

				return toolString;
			}
		}

		// public functions
		public void addTool(Tool aTool) {
			const int maxTools = 3;
			if (tools.Number >= maxTools)
				throw new ToolException($"Cannot borrow any more tools (limit of {maxTools} reached)");

			aTool.addBorrower(this);
			tools.add(aTool);
		}

		public void deleteTool(Tool aTool) {
			aTool.deleteBorrower(this);
			tools.delete(aTool);
		}

		// overrides
		public override string ToString() { // return a string containing the first name, lastname, and contact phone number of this member
			return $"{firstName} {lastName} ({contactNumber})";
		}

		public int CompareTo(Member aMember) {
			int lastNameComparison = lastName.CompareTo(aMember.LastName);
			if (lastNameComparison != 0) {
				// last name differs, return that
				return lastNameComparison;
			}
			else {
				// last name is the same, return first name difference
				return firstName.CompareTo(aMember.FirstName);
			}
		}
	}
}
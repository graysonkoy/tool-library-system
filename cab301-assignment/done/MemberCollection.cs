using System;
using System.Collections.Generic;
using System.Text;

namespace cab301_assignment {
	class MemberCollection : iMemberCollection {
		private int number;
		private Member[] members;

		// constructors
		public MemberCollection() {
			this.number = 0;
			this.members = new Member[number];
		}

		// public variable accessors
		public int Number { // get the number of members in the community library
			get => number;
		}

		// private functions
		private void resize(int newSize) {
			Array.Resize(ref members, newSize);
		}

		// public functions
		public void add(Member aMember) { // add a new member to this member collection, make sure there are no duplicates in the member collection
			// check if the member already exists in the collection
			if (search(aMember)) {
				Console.WriteLine($"Failed to add member {aMember.ToString()} to the collection (member already found)");
				return;
			}

			number++;

			// resize the member array to fit the new member
			resize(number);

			// insert the new member at the end
			members[number - 1] = aMember;
		}

		public void delete(Member aMember) { // delete a given member from this member collection, a member can be deleted only when the member currently is not holding any tool
			// check if the member is holding any tools
			if (aMember.Tools.Length != 0) {
				Console.WriteLine($"Can't delete member {aMember.ToString()} (member is holding tools)");
				return;
			}

			bool deleted = false;
			for (int i = 0; i < number; i++) {
				if (!deleted) {
					// haven't deleted yet, check if this is the member
					if (members[i].CompareTo(aMember) == 0) {
						members[i] = null;
						deleted = true;
					}
				}
				else {
					// deleted a member, move the rest back
					members[i - 1] = members[i];
				}
			}

			if (deleted) {
				// resize the member array to account for the deleted member
				number--;
				resize(number);
			}
			else {
				Console.WriteLine($"Failed to delete member {aMember.ToString()} from collection (member was not found)");
			}
		}

		public Boolean search(Member aMember) { // search a given member in this member collection. Return true if this memeber is in the member collection; return false otherwise.
			for (int i = 0; i < number; i++) {
				// check if this is the member
				if (members[i].CompareTo(aMember) == 0)
					return true;
			}

			// member wasn't found
			return false;
		}

		public Member[] toArray() { // output the members in this collection to an array of Member
			return members;
		}
	}
}

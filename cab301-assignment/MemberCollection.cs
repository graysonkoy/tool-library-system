﻿using System;
using System.Collections.Generic;
using System.Text;

namespace cab301_assignment {
	class MemberCollection : iMemberCollection {
		private int number;
		private BSTree members;

		// constructors
		public MemberCollection() {
			this.number = 0;
			this.members = new BSTree();
		}

		// public variable accessors
		public int Number { // get the number of members in the community library
			get => number;
		}

		// public functions
		public void add(Member aMember) { // add a new member to this member collection, make sure there are no duplicates in the member collection
			// check if the member already exists in the collection
			if (search(aMember)) {
				Console.WriteLine($"Failed to add member {aMember.ToString()} to the collection (member already found)");
				return;
			}

			// insert the new member
			number++;
			members.Insert(aMember);
		}

		public void delete(Member aMember) { // delete a given member from this member collection, a member can be deleted only when the member currently is not holding any tool
			if (!members.Delete(aMember)) {
				Console.WriteLine($"Failed to delete member {aMember.ToString()} from collection (member was not found)");
				return;
			}

			number--;
		}

		public Boolean search(Member aMember) { // search a given member in this member collection. Return true if this memeber is in the member collection; return false otherwise.
			return members.Search(aMember);
		}

		public Member[] toArray() { // output the members in this collection to an array of Member
			List<Member> membersList = members.InOrderTraverse();
			return membersList.ToArray();
		}
	}
}

using System;
using System.Collections.Generic;
using System.Text;

namespace cab301_assignment {
	public class MemberCollection : iMemberCollection {
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
		/// <summary>
		/// Adds a new member to this member collection
		/// </summary>
		/// <param name="aMember">Member to add</param>
		public void add(Member aMember) {
			number++;
			members.Insert(aMember);
		}

		/// <summary>
		/// Deletes a member from the member collection
		/// </summary>
		/// <param name="aMember">Member to delete</param>
		public void delete(Member aMember) {
			if (!members.Delete(aMember))
				throw new ToolException($"Failed to delete member {aMember.ToString()} from collection (member was not found)");

			number--;
		}

		/// <summary>
		/// Searches for a member in the collection
		/// </summary>
		/// <param name="aMember">Member to search for</param>
		/// <returns>Whether the member was found</returns>
		public Boolean search(Member aMember) {
			return members.Search(aMember);
		}

		/// <summary>
		/// Returns an array of members in the collection
		/// </summary>
		/// <returns>Member array</returns>
		public Member[] toArray() {
			List<Member> membersList = members.InOrderTraverse();
			return membersList.ToArray();
		}
	}
}

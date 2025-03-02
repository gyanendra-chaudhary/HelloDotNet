
using DSA.Recursion;

// ListNode list1 = new ListNode()
// {
//     val = 10,
//     next = new ListNode()
//     {
//         val = 15,
//         next = new ListNode()
//         {
//             val = 20,
//             next = null
//         }
//     }
// };
// ListNode list2 = new ListNode()
// {
//     val = 15,
//     next = new ListNode()
//     {
//         val = 10,
//         next = new ListNode()
//         {
//             val = 25,
//             next = null
//         }
//     }
// };

Beginner beginnerSolutions = new Beginner();

// // var result = beginnerSolutions.MergeTwoLists(list1, list2);
// Console.WriteLine(beginnerSolutions.RemoveDuplicates(new int[]{1,1,2}));



// Console.WriteLine("Result => {0}", beginnerSolutions.SearchInsert(new int[] {1,3,5,6 }, 2));
// Console.WriteLine("Result => {0}", beginnerSolutions.LengthOfLastWord("   fly me   to   the moon  "));
// var result = beginnerSolutions.PlusOne(new int[] { 9, 9 });
// Console.WriteLine("Result => {0}", result);

var result = beginnerSolutions.AddBinary("11", "1");

Console.WriteLine(result);

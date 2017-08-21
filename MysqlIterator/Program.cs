using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MysqlIterator
{
	class Program
	{
		static void Main(string[] args) {
			printl("target db: ");
			string db = Console.ReadLine();
			printl("Search Target: ");
			string target = Console.ReadLine();

			Searcher searcher = new Searcher(db);
			try {
				searcher.search(target);
			}
			catch (Exception e1) {
				print(e1.ToString());
			}
			finally {
				searcher = null;
			}
			print("DONE");
			Console.ReadKey();
		}

		private static void print(string s) {
			Console.WriteLine("MAIN > " + s);
		}
		private static void printl(string s) {
			Console.Write("MAIN > " + s);
		}
	}
}

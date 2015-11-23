using System;
using System.IO;

namespace Scheduler
{
	class MainClass
	{
		public static void Main (string[] args)
		{
            String input_file;
            //String input_file = args[0];
            //if ( == null) {
                input_file = "../../input_file.txt";
            //}
            new Simulator(input_file);

		}
	}
}

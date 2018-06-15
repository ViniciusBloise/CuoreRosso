using System;
using CuoreRosso.Dal;

namespace CuoreRosso_CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            var p = new PersonRepository();
            p.DeleteFor<EducationDto>(0);

            Console.WriteLine("Hello World!");
        }
    }
}

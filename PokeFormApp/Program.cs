using Autofac;
using PokeFormApp.Owner;
using PokeFormApp.Review;
using PokeFormApp.Reviewer;
using PokeFormApp.Services;
using System;
using System.Windows.Forms;

namespace PokeFormApp
{
    internal static class Program
    {
        public static IContainer Container { get; private set; }

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
             Application.Run(new Login());
            

            
        }
    }
}

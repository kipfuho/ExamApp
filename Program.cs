using System;
using System.Windows.Forms;

namespace ExamApp
{
    internal static class Program
    {
        public static ExamApp Instance { get; private set; }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Instance = new ExamApp();
            Application.Run(Instance);
        }
    }
}

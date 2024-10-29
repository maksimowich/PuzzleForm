using System;
using System.Windows.Forms;

namespace PuzzleApp
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new PuzzleForm()); // Запускаем форму PuzzleForm как главное окно приложения
        }
    }
}

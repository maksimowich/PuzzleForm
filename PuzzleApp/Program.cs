using System;
using System.Windows.Forms;

namespace PuzzleApp
{
    static class Program
    {
        /// <summary>
        /// ������� ����� ����� ��� ����������.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new PuzzleForm()); // ��������� ����� PuzzleForm ��� ������� ���� ����������
        }
    }
}

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace PuzzleApp
{
    public partial class PuzzleForm : Form
    {
        private Bitmap originalImage;             // Оригинальное изображение
        private List<PuzzlePiece> puzzlePieces;   // Список кусочков пазла
        private PuzzlePiece selectedPiece;        // Выбранный кусочек для перемещения
        private Point mouseDownLocation;          // Начальная позиция мыши

        public PuzzleForm()
        {
            InitializeComponent();
            this.Text = "Пазл - Drag & Drop";
            this.ClientSize = new Size(800, 600);

            // Создание кнопок
            Button loadImageButton = new Button() { Text = "Загрузить изображение", Location = new Point(10, 10) };
            loadImageButton.Click += LoadImageButton_Click;
            this.Controls.Add(loadImageButton);

            Button helpButton = new Button() { Text = "Справка", Location = new Point(150, 10) };
            helpButton.Click += HelpButton_Click;
            this.Controls.Add(helpButton);

            puzzlePieces = new List<PuzzlePiece>();
        }

        // Метод для загрузки изображения
        private void LoadImageButton_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    originalImage = new Bitmap(openFileDialog.FileName);
                    CreatePuzzlePieces();
                }
            }
        }

        // Метод для создания и перемешивания кусочков пазла
        private void CreatePuzzlePieces()
        {
            ClearPuzzlePieces();

            int rows = 3; // Количество строк
            int cols = 3; // Количество столбцов
            int pieceWidth = originalImage.Width / cols;
            int pieceHeight = originalImage.Height / rows;

            Random random = new Random();

            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < cols; x++)
                {
                    Rectangle srcRect = new Rectangle(x * pieceWidth, y * pieceHeight, pieceWidth, pieceHeight);
                    Bitmap pieceImage = originalImage.Clone(srcRect, originalImage.PixelFormat);
                    Point correctPosition = new Point(x * pieceWidth, y * pieceHeight);

                    // Создаем кусочек пазла
                    PuzzlePiece piece = new PuzzlePiece(pieceImage, correctPosition);
                    piece.Location = new Point(random.Next(10, this.ClientSize.Width - pieceWidth - 10),
                                               random.Next(50, this.ClientSize.Height - pieceHeight - 10));

                    // Добавляем обработчики событий для перетаскивания
                    piece.MouseDown += Piece_MouseDown;
                    piece.MouseMove += Piece_MouseMove;
                    piece.MouseUp += Piece_MouseUp;

                    puzzlePieces.Add(piece);
                    this.Controls.Add(piece);
                }
            }
        }

        // Очистка старых кусочков пазла с формы
        private void ClearPuzzlePieces()
        {
            foreach (var piece in puzzlePieces)
            {
                this.Controls.Remove(piece);
                piece.Dispose();
            }
            puzzlePieces.Clear();
        }

        // Метод для отображения справки
        private void HelpButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Добро пожаловать в игру пазл! Загрузите изображение, чтобы начать. Перетаскивайте кусочки, чтобы собрать исходное изображение.", "Справка");
        }

        // Начало перетаскивания кусочка
        private void Piece_MouseDown(object sender, MouseEventArgs e)
        {
            selectedPiece = sender as PuzzlePiece;
            mouseDownLocation = e.Location;
        }

        // Перетаскивание кусочка
        private void Piece_MouseMove(object sender, MouseEventArgs e)
        {
            if (selectedPiece != null && e.Button == MouseButtons.Left)
            {
                int x = e.X + selectedPiece.Left - mouseDownLocation.X;
                int y = e.Y + selectedPiece.Top - mouseDownLocation.Y;
                selectedPiece.Location = new Point(x, y);
            }
        }

        // Окончание перетаскивания кусочка
        private void Piece_MouseUp(object sender, MouseEventArgs e)
        {
            selectedPiece = null;
        }
    }

    // Класс для кусочков пазла
    public class PuzzlePiece : PictureBox
    {
        public Point CorrectPosition { get; private set; }

        public PuzzlePiece(Image image, Point correctPosition)
        {
            this.Image = image;
            this.CorrectPosition = correctPosition;
            this.SizeMode = PictureBoxSizeMode.StretchImage;
            this.Size = image.Size;
        }

        public bool IsCorrectPosition()
        {
            // Проверяет, находится ли кусочек на своем месте
            return this.Location == CorrectPosition;
        }
    }
}

using System;
using System.Windows.Forms;

namespace WindowsFormsAppTriangle
{
    public partial class Form1 : Form
    {
        private TextBox textBox1;
        private TextBox textBox2;
        private TextBox textBox3;
        private Label labelResult;
        private Button button;
        private const double Epsilon = 0.0000001; // Для сравнения double

        public Form1()
        {
            InitializeComponent();
            SetupForm();
        }

        private void SetupForm()
        {
            // Настройка формы
            this.Text = "Тип треугольника";
            this.Size = new System.Drawing.Size(350, 300);
            this.FormBorderStyle = FormBorderStyle.FixedSingle; // Фиксированный размер
            this.StartPosition = FormStartPosition.CenterScreen;

            // Создание меток
            Label labelA = new Label() { Text = "Длина первой стороны:", Location = new System.Drawing.Point(20, 20), Size = new System.Drawing.Size(150, 25) };
            Label labelB = new Label() { Text = "Длина второй стороны:", Location = new System.Drawing.Point(20, 60), Size = new System.Drawing.Size(150, 25) };
            Label labelC = new Label() { Text = "Длина третьей стороны:", Location = new System.Drawing.Point(20, 100), Size = new System.Drawing.Size(150, 25) };

            // Создание текстовых полей
            textBox1 = new TextBox() { Location = new System.Drawing.Point(180, 20), Size = new System.Drawing.Size(120, 23) };
            textBox2 = new TextBox() { Location = new System.Drawing.Point(180, 60), Size = new System.Drawing.Size(120, 23) };
            textBox3 = new TextBox() { Location = new System.Drawing.Point(180, 100), Size = new System.Drawing.Size(120, 23) };

            // Кнопка
            button = new Button() { Text = "Определить тип треугольника", Location = new System.Drawing.Point(20, 140), Size = new System.Drawing.Size(280, 35) };
            button.Click += Button_Click;

            // Метка для результата
            labelResult = new Label()
            {
                Location = new System.Drawing.Point(20, 190),
                Size = new System.Drawing.Size(290, 50),
                TextAlign = System.Drawing.ContentAlignment.MiddleCenter,
                Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold),
                BackColor = System.Drawing.Color.LightGray
            };

            // Добавление элементов на форму
            this.Controls.AddRange(new Control[] { labelA, labelB, labelC, textBox1, textBox2, textBox3, button, labelResult });
        }

        private void Button_Click(object sender, EventArgs e)
        {
            // Сброс цвета
            labelResult.BackColor = System.Drawing.Color.LightGray;
            labelResult.Text = "";

            // Валидация и парсинг сторон
            if (!TryParseSide(textBox1.Text, "первой стороны", out double sideA)) return;
            if (!TryParseSide(textBox2.Text, "второй стороны", out double sideB)) return;
            if (!TryParseSide(textBox3.Text, "третьей стороны", out double sideC)) return;

            // Проверка существования треугольника
            if (!IsTriangleValid(sideA, sideB, sideC))
            {
                labelResult.Text = "Треугольник с такими сторонами не существует!";
                labelResult.BackColor = System.Drawing.Color.LightCoral;
                return;
            }

            // Определение типа треугольника
            string triangleType = DetermineTriangleType(sideA, sideB, sideC);
            labelResult.Text = $"Тип треугольника: {triangleType}";
            labelResult.BackColor = System.Drawing.Color.LightGreen;
        }

        private bool TryParseSide(string input, string sideName, out double value)
        {
            value = 0;

            // Проверка на пустой ввод или пробелы
            if (string.IsNullOrWhiteSpace(input))
            {
                MessageBox.Show($"Введите длину {sideName}.", "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Проверка на число (поддерживает как точку, так и запятую)
            string normalizedInput = input.Replace(',', '.');
            if (!double.TryParse(normalizedInput, System.Globalization.NumberStyles.Any,
                System.Globalization.CultureInfo.InvariantCulture, out value))
            {
                MessageBox.Show($"Значение '{input}' не является числом.", "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // Проверка на положительное число (не ноль и не отрицательное)
            if (value <= 0)
            {
                MessageBox.Show($"Длина {sideName} должна быть положительным числом (больше 0).", "Ошибка ввода", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private bool IsTriangleValid(double a, double b, double c)
        {
            // Теорема о неравенстве треугольника: сумма любых двух сторон больше третьей
            // Используем epsilon для учета погрешности double
            return (a + b > c + Epsilon) && (a + c > b + Epsilon) && (b + c > a + Epsilon);
        }

        private string DetermineTriangleType(double a, double b, double c)
        {
            // Проверка на равносторонний (с учетом погрешности)
            if (Math.Abs(a - b) < Epsilon && Math.Abs(b - c) < Epsilon)
            {
                return "равносторонний";
            }

            // Проверка на равнобедренный (хотя бы две стороны равны с учетом погрешности)
            if (Math.Abs(a - b) < Epsilon || Math.Abs(a - c) < Epsilon || Math.Abs(b - c) < Epsilon)
            {
                return "равнобедренный";
            }

            // Иначе - разносторонний
            return "разносторонний";
        }
    }
}
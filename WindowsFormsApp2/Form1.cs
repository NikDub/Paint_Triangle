using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form
    {
        readonly string path = @"..\..\coordinates"; //путь к файлу
        readonly Pen pen = new Pen(Color.Black, 1); //обводка
        int colorNum = 0; //счетчик палитры
        readonly List<Triangel> pointsList = new List<Triangel>(); //списко обьектов фигуры треугольник
        public Form1()
        {
            InitializeComponent();
            this.Size = new Size(1000, 1000);
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            ReadFromFile();
            CheackEntry();
            SetColor();
            if (pointsList.Any())
                this.label1.Text = "Для закрашивания требуеться " + (colorNum + 1) + " оттенков цвета";
        }
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            CheackIntersection();
            foreach (var item in pointsList)
                Draw(e.Graphics, item);
        }
        private void Draw(Graphics g, Triangel triangel) //рисование фигур
        {
            g.FillPolygon(triangel.Brush, triangel.PointCoordinates);
            g.DrawPolygon(pen, triangel.PointCoordinates);
        }
        private void ReadFromFile() //чтение координат из файла и занесение в список обектов фигуры
        {
            try
            {
                using (StreamReader sr = new StreamReader(Path.GetFullPath(path)))
                {

                    int size = Convert.ToInt32(sr.ReadLine());
                    for (int i = 0; i < size; i++)
                    {
                        pointsList.Add(new Triangel(sr.ReadLine().Split(new char[] { ' ' })));
                    }
                }
            }
            catch (FileNotFoundException)
            {
                pointsList.Clear();
                this.label1.Text = "Не найден файл";
            }
            catch (Exception)
            {
                pointsList.Clear();
                this.label1.Text = "Ошибка в оформлении входных данных";
            }
        }
        private void SetColor() //установка кисти для каждой фигуры в соответствии виличене оттенка фигуры
        {
            int greenColorCount = 1;
            foreach (var item in pointsList)
            {
                int blueColor, redColor = blueColor = 190 - 50 * (item.ColorNumber + 1),
                    greenColor = 255;
                if (redColor <= 0 && blueColor <= 0)
                {
                    greenColorCount += 2;
                    redColor = 0;
                    greenColor = 255 - 20 * greenColorCount;
                    blueColor = 0;
                    if (greenColor < 0)
                        greenColor = 0;
                }
                item.Brush = new SolidBrush(Color.FromArgb(255, redColor, greenColor, blueColor));
            }

        }
        private void CheackEntry() //проверка на вхождение фигуры в фигуру
        {
            for (int i = 0; i < pointsList.Count; i++)
                for (int j = 0; j < pointsList.Count; j++)
                    if (i < j)
                        if (!pointsList[i].Check_point(pointsList[j].PointCoordinates[0]))
                            if (colorNum < ++pointsList[j].ColorNumber)
                                colorNum = pointsList[j].ColorNumber;
        }
        private async void CheackIntersection() //проверка на пересечение
        {
            await Task.Run(() =>
            {
                for (int i = 0; i < pointsList.Count; i++)
                    for (int j = 0; j < pointsList.Count; j++)
                    {
                        if (i == j)
                            continue;
                        if (pointsList[i].AreCrossingTriangle(pointsList[j].PointCoordinates))
                            label1.Invoke(new Action(() => label1.Text = "ERROR"));
                    }
            });
        }
        private void button1_MouseDown(object sender, MouseEventArgs e) //кнопка обновления данных в программе
        {
            colorNum = 0;
            this.label1.Text = "";
            pointsList.Clear();
            this.OnLoad(e);
            this.Invalidate();
            this.Update();
            this.Refresh();
        }
    }
}

using Figuren;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace FigurWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region fields

        private List<Figur> allFiguren = new List<Figur>();
        private double scaleRatio = 1D;

        #endregion

        #region constants

        static double FigurLineThikness = 3D;

        #endregion

        public MainWindow()
        {
            InitializeComponent();
        }

        #region Drawing Methods

        private void DrawLine(double x1, double y1, double x2, double y2, double strokeThikness, Brush brush)
        {
            Line line = new Line();

            line.X1 = x1;
            line.Y1 = y1;

            line.X2 = x2;
            line.Y2 = y2;

            line.StrokeThickness = strokeThikness;
            line.Stroke = brush;

            CoordinateSystem.Children.Add(line);
        }

        private void Generate2dimCoordinateSys()
        {
            CoordinateSystem.Children.Clear();

            if (CoordinateSystemBorder.ActualWidth < CoordinateSystemBorder.ActualHeight)
            {
                CoordinateSystem.Width = CoordinateSystemBorder.ActualWidth;
                CoordinateSystem.Height = CoordinateSystemBorder.ActualWidth;
            }
            else
            {
                CoordinateSystem.Height = CoordinateSystemBorder.ActualHeight;
                CoordinateSystem.Width = CoordinateSystemBorder.ActualHeight;
            }

            DrawLine(CoordinateSystem.Width / 2D, 0, CoordinateSystem.Width / 2D, CoordinateSystem.Height, 1.5, Brushes.Black);
            DrawLine(0, CoordinateSystem.Height / 2D, CoordinateSystem.Width, CoordinateSystem.Height / 2, 1.5, Brushes.Black);

            DrawSecondaryXLines();
            DrawSecondarYLines();

            DrawAllFiguren();
        }

        private void DrawSecondaryXLines()
        {
            int numOfLines = (int)sliderXLines.Value * 2;
            double spaceBetweenLines = CoordinateSystem.Width / (double)numOfLines;

            Line line = null;

            for (int count = 0; count < numOfLines; count++)
            {
                line = new Line();

                line.X1 = (count + 1) * spaceBetweenLines;
                line.Y1 = CoordinateSystem.Height;

                line.X2 = line.X1;
                line.Y2 = 0;

                line.Stroke = Brushes.Black;
                line.StrokeThickness = 0.75;

                CoordinateSystem.Children.Add(line);
            }
        }

        private void DrawSecondarYLines()
        {
            int numOfLines = (int)sliderYLines.Value * 2;
            double spaceBetweenLines = CoordinateSystem.Height / (double)numOfLines;

            Line line = null;

            for (int count = 0; count < numOfLines; count++)
            {
                line = new Line();

                line.X1 = CoordinateSystem.Width;
                line.Y1 = (count + 1) * spaceBetweenLines;

                line.X2 = 0;
                line.Y2 = line.Y1; ;

                line.Stroke = Brushes.Black;
                line.StrokeThickness = 0.75;

                CoordinateSystem.Children.Add(line);
            }
        }

        private void DrawAllFiguren()
        {
            foreach (Figur Figur in allFiguren)
            {
                DrawFigur(Figur);
            }
        }

        private void DrawFigur(Figur figurToDraw)
        {
            Punkt[] punkte = figurToDraw.GetEckpunkte();
            Line l = null;

            for (int idx = 1; idx < punkte.Length; idx++)
            {
                l = new Line();

                l.X1 = GetRealX(punkte[idx - 1].x);
                l.Y1 = GetRealY(punkte[idx - 1].y);

                l.X2 = GetRealX(punkte[idx].x);
                l.Y2 = GetRealY(punkte[idx].y);

                l.Stroke = Brushes.Red;
                l.StrokeThickness = FigurLineThikness;

                CoordinateSystem.Children.Add(l);
            }

            DrawLastLineOfFigur(figurToDraw);
        }

        private void DrawLastLineOfFigur(Figur figurToDraw)
        {
            Punkt[] punkte = figurToDraw.GetEckpunkte();
            int lastElement = punkte.Length - 1;

            Line l = new Line();

            l.X1 = GetRealX(punkte[lastElement].x);
            l.Y1 = GetRealY(punkte[lastElement].y);

            l.X2 = GetRealX(punkte[0].x);
            l.Y2 = GetRealY(punkte[0].y);

            l.Stroke = Brushes.Red;
            l.StrokeThickness = FigurLineThikness;

            CoordinateSystem.Children.Add(l);
        }

        #endregion

        #region UI Elements Events

        private void GenerateCoordinateSystem_Click(object sender, RoutedEventArgs e)
        {
            Generate2dimCoordinateSys();

            resetCoordinateSystem.IsEnabled = true;
            generateCoordinateSystem.IsEnabled = false;
        }

        private void ResetCoordinateSystem_Click(object sender, RoutedEventArgs e)
        {
            CoordinateSystem.Children.Clear();
            ResetScale();

            allFiguren.Clear();

            resetCoordinateSystem.IsEnabled = false;
            generateCoordinateSystem.IsEnabled = true;
        }

        private void CoordinateSysGrid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (CoordinateSystem.Children.Count != 0)
            {
                Generate2dimCoordinateSys();
            }
        }

        private void SliderXLines_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (sliderYLines.IsEnabled == false)
            {
                sliderYLines.Value = sliderXLines.Value;
            }

            if (CoordinateSystem.Children.Count != 0)
            {
                Generate2dimCoordinateSys();
            }
        }

        private void SliderYLines_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (CoordinateSystem.Children.Count != 0)
            {
                Generate2dimCoordinateSys();
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            sliderYLines.IsEnabled = false;
            sliderYLines.Value = sliderXLines.Value;
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            sliderYLines.IsEnabled = true;
        }

        private void SaveFigur_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CheckTextBoxes();
                allFiguren.Add(CreateFigur());
                MessageBox.Show("Figur saved.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private Figur CreateFigur()
        {
            Figur figurToCreate = null;

            if (SelectFigurComboBox.SelectedIndex == 0)
            {
                figurToCreate = new Quadrat(double.Parse(xCord.Text), double.Parse(yCord.Text), (int)sliderSeiteA.Value);
            }

            else if (SelectFigurComboBox.SelectedIndex == 1)
            {
                figurToCreate = new Rechteck(double.Parse(xCord.Text), double.Parse(yCord.Text), (int)sliderSeiteA.Value, (int)sliderSeiteB.Value);
            }

            else if (SelectFigurComboBox.SelectedIndex == 2)
            {
                figurToCreate = new RechtwinkligesDreieck(double.Parse(xCord.Text), double.Parse(yCord.Text), (int)sliderSeiteA.Value, (int)sliderSeiteB.Value);
            }

            else if (SelectFigurComboBox.SelectedIndex == 3)
            {
                figurToCreate = new Kreis(double.Parse(xCord.Text), double.Parse(yCord.Text), (int)sliderSeiteA.Value);
            }

            else
            {
                throw new Exception("No Figur selected!");
            }

            return figurToCreate;
        }

        private void DrawFiguren_Click(object sender, RoutedEventArgs e)
        {
            DrawAllFiguren();
        }

        private void SelectFigurComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SelectFigurComboBox.SelectedIndex == 0)
            {
                sliderSeiteA.IsEnabled = true;
                sliderSeiteB.IsEnabled = false;
            }

            else if (SelectFigurComboBox.SelectedIndex == 1)
            {
                sliderSeiteA.IsEnabled = true;
                sliderSeiteB.IsEnabled = true;
            }

            else if (SelectFigurComboBox.SelectedIndex == 2)
            {
                sliderSeiteA.IsEnabled = true;
                sliderSeiteB.IsEnabled = true;
            }

            else if (SelectFigurComboBox.SelectedIndex == 3)
            {
                sliderSeiteA.IsEnabled = true;
                sliderSeiteB.IsEnabled = false;
            }
        }

        private void CoordinateSystem_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Point mousePos = Mouse.GetPosition(CoordinateSystem);

            xCord.Text = string.Format("{0}", (int)mousePos.X - CoordinateSystem.Width / 2D);
            yCord.Text = string.Format("{0}", (int)-mousePos.Y + CoordinateSystem.Height / 2D);
        }

        private void SaveFigurenToFileBttn_Click(object sender, RoutedEventArgs e)
        {
            SaveToFile();
        }

        private void InitSaveFileDiag(SaveFileDialog saveFileDiag)
        {
            saveFileDiag.Filter = "Figuren CSV File|*.csv|Figuren Binary File|*.bin";
            saveFileDiag.OverwritePrompt = true;
        }

        private void SaveToFile()
        {
            string path;
            string[] filenameAndExtension;
            SaveFileDialog saveFileDiag = new SaveFileDialog();

            InitSaveFileDiag(saveFileDiag);

            saveFileDiag.ShowDialog();
            path = saveFileDiag.FileName;

            filenameAndExtension = path.Split('.');

            try
            {
                int count = 0;
                string[] contentToWriteToFile = new string[allFiguren.Count];

                if (filenameAndExtension[1] == "csv")
                {
                    foreach (Figur f in allFiguren)
                    {
                        contentToWriteToFile[count] = f.GetCSV();

                        count++;
                    }

                    File.WriteAllLines(path, contentToWriteToFile);
                }

                else
                {
                    double[] Seitenlaengen = null;
                    BinaryWriter writer = null;

                    writer = new BinaryWriter(File.Open(path, FileMode.Create));
                    using (writer)
                    {
                        foreach (Figur f in allFiguren)
                        {
                            byte r = 0, g = 0, b = 0;

                            writer.Write((byte)f.GetFigurenCode());

                            writer.Write((short)f.Lagepunkt.x);
                            writer.Write((short)f.Lagepunkt.y);

                            switch (f.Farbe)
                            {
                                case Figurenfarbe.Blau:
                                    b = 255;

                                    break;

                                case Figurenfarbe.Gruen:
                                    g = 255;

                                    break;

                                case Figurenfarbe.Rot:
                                    r = 255;

                                    break;
                            }

                            writer.Write(r);
                            writer.Write(g);
                            writer.Write(b);

                            Seitenlaengen = f.GetSeitenlaengen();

                            for(int counter = 0; counter <  Seitenlaengen.Length; counter++)
                            {
                                writer.Write((byte)Seitenlaengen[count]);
                            }
                        }
                    }

                }

                MessageBox.Show("File saved.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch(Exception ex)
            {
                MessageBox.Show($"File could not be saved. {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        public void InitOpenFileDiag(OpenFileDialog openFileDiag)
        {
            openFileDiag.Filter = "Figuren CSV File|*.csv";
            openFileDiag.Multiselect = false;
        }

        public void loadFigurenFromFileBttn_Click(object sender, RoutedEventArgs e)
        {
            LoadFromFile();
        }

        public void LoadFromFile()
        {
            int idx = 0;
            string path;
            string[] allLines = new string[allFiguren.Count];
            List<Figur> allFigurenNew = new List<Figur>();
            OpenFileDialog openFileDiag = new OpenFileDialog();
            
            InitOpenFileDiag(openFileDiag);

            openFileDiag.ShowDialog();
            path = openFileDiag.FileName;

            try
            {
                allLines = File.ReadAllLines(path);

                foreach (string line in allLines)
                {
                    string[] valuesOfFigur = line.Split(';');

                    switch (valuesOfFigur[0])
                    {
                        case "Quadrat":
                            {
                                allFigurenNew.Add(Quadrat.CheckAndCreateFigur(valuesOfFigur));

                                break;
                            }

                        case "Rechteck":
                            {
                                allFigurenNew.Add(Rechteck.CheckAndCreateFigur(valuesOfFigur));

                                break;
                            }

                        case "RechtwinkligesDreieck":
                            {
                                allFigurenNew.Add(RechtwinkligesDreieck.CheckAndCreateFigur(valuesOfFigur));

                                break;
                            }

                        case "Kreis":
                            {
                                allFigurenNew.Add(Kreis.CheckAndCreateFigur(valuesOfFigur));

                                break;
                            }

                        default:
                            {
                                throw new FileFormatException("Figur type is not valid!");
                            }
                    }

                    idx++;
                }

                allFiguren = allFigurenNew;

                CoordinateSystem.Children.Clear();
                Generate2dimCoordinateSys();
            }

            catch(Exception ex)
            {
                MessageBox.Show($"Error in line {idx + 1}. " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        public void ContextMenuZoomReset_Click(object sender, RoutedEventArgs e)
        {
            ResetScale();
        }

        public void ResetScale()
        {
            scaleRatio = 1D;

            CoordinateSystem.RenderTransform = new ScaleTransform(scaleRatio, scaleRatio, CoordinateSystem.Width / 2D, CoordinateSystem.Height / 2D);
        }

        public void ContextMenuZoomPlus_Click(object sender, RoutedEventArgs e)
        {
            Point mousepos = Mouse.GetPosition(CoordinateSystem);
            scaleRatio += 0.25D;

            CoordinateSystem.RenderTransform = new ScaleTransform(scaleRatio, scaleRatio, mousepos.X, mousepos.Y);
        }

        public void ContextMenuZoomMinus_Click(object sender, RoutedEventArgs e)
        {
            Point mousepos = Mouse.GetPosition(CoordinateSystem);
            scaleRatio -= 0.25D;

            CoordinateSystem.RenderTransform = new ScaleTransform(scaleRatio, scaleRatio, mousepos.X, mousepos.Y);
        }

        public void ContextMenuSaveAs_Click(object sender, RoutedEventArgs e)
        {
            SaveToFile();
        }

        #endregion

        #region Calc and Check Methods

        private double GetRealX(double x)
        {
            return x + CoordinateSystem.Width / 2D;
        }

        private double GetRealY(double y)
        {
            return -y + CoordinateSystem.Height / 2D;
        }

        private void CheckTextBoxes()
        {
            bool result = true;
            List<TextBox> textBoxesToCheck = new List<TextBox>();

            textBoxesToCheck.Add(xCord);
            textBoxesToCheck.Add(yCord);

            foreach(TextBox textBoxToCheck in textBoxesToCheck)
            {
                if (System.Text.RegularExpressions.Regex.IsMatch(textBoxToCheck.Text, "[^(-9)-9]"))
                {
                    textBoxToCheck.BorderBrush = System.Windows.Media.Brushes.Red;
                    textBoxToCheck.Text = textBoxToCheck.Text.Remove(textBoxToCheck.Text.Length - 1);
                    textBoxToCheck.Text = "0";
                    result = false;
                }

                else
                {
                    textBoxToCheck.BorderBrush = System.Windows.Media.Brushes.Black;
                }
            }

            if(!result)
            {
                throw new Exception("Only numbers are allowed!");
            }
        }

        #endregion

        #region file example

        //private void BttnDemoFile01_Click(object sender, RoutedEventArgs e)
        // {
        //     //todo: Absolute Pfadangaben sind wenn möglich zu vermeiden!
        //     //string path = @"c:\temp\MyTest.txt";
        //     string path = @"MyFirstFile.txt";

        //     // This text is added only once to the file.
        //     if (!File.Exists(path))
        //     {
        //         // Create a file to write to.
        //         string[] createText = { "Hello", "And", "Welcome" };
        //         File.WriteAllLines(path, createText);   //object initialiser
        //         MessageBox.Show("File created!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        //     }

        //     else
        //     {
        //         MessageBox.Show("File already exists", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
        //     }

        //     // This text is always added, making the file longer over time
        //     // if it is not deleted.
        //     string appendText = "This is extra text" + Environment.NewLine;
        //     File.AppendAllText(path, appendText);
        //     //MessageBox.Show("text added");

        //     try
        //     {
        //         Kreis k1 = null;

        //         k1.GetEckpunkte();

        //         path = "now.txt";

        //         // Open the file to read from.
        //         string[] allLines = File.ReadAllLines(path);

        //         foreach (string line in allLines)
        //         {
        //             Console.WriteLine(line);
        //         }

        //         FileContent.Content = "";

        //         foreach (string line in allLines)
        //         {
        //             FileContent.Content += line + Environment.NewLine;
        //         }

        //         MessageBox.Show("Text added and complete file content written to console and label", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        //     }
        //     catch(System.IO.FileNotFoundException ex)
        //     {
        //         MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        //     }
        //     catch(Exception ex)
        //     {
        //         MessageBox.Show("Something unexpected has occured: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        //     }

        //     //File Save Dialog
        //     //.csv file format
        // }

        #endregion

    }
}
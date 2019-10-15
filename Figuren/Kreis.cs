using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Figuren
{
    public class Kreis : Figur
    {
        #region constants

        private static double PI = 3.1415926535897932384626433832795;
        static int FigurenCode = 3;

        #endregion

        #region internal fields and properties

        public double Radius { get; set; }

        #endregion

        #region constructros

        public Kreis(double xCord, double yCord, Figurenfarbe color, double radius) :base(xCord, yCord, color)
        {
            this.Radius = radius;
        }

        public Kreis(double xCord, double yCord, double radius) : base(xCord, yCord)
        {
            this.Radius = radius;
        }

        #endregion

        #region public methods

        public override string ToString()
        {
            return String.Format("{0}, Radius: {1}, Flaeche: {2}", base.ToString(), this.Radius, this.GetFlaeche());
        }

        public override double GetFlaeche()
        {
            return Radius * Radius * PI;
        }

        public override Punkt[] GetEckpunkte()
        {
            Punkt[] punkte = new Punkt[360];
            double winkel = 0;

            for(int count = 0; count < punkte.Length; count++)
            {
                winkel = count * PI / 180;

                punkte[count].x = Lagepunkt.x + Radius * Math.Cos(winkel);
                punkte[count].y = Lagepunkt.y + Radius * Math.Sin(winkel);
            }

            return punkte;
        }

        public override string GetCSV()
        {
            return $"Kreis;{this.Lagepunkt.x};{this.Lagepunkt.y};{this.Farbe};{this.Radius}";
        }

        public static Kreis CheckAndCreateFigur(string[] valuesOfFigur)
        {
            double lageX = 0D, lageY = 0D, radius = 0D;
            Figurenfarbe color;

            if (valuesOfFigur.Length != 5)
            {
                throw new Exception("Number of arguments for the Kreis is not valid!");
            }

            if (!double.TryParse(valuesOfFigur[1], out lageX))
            {
                throw new Exception("Lagepunkt X is not a number!");
            }

            if (!double.TryParse(valuesOfFigur[2], out lageY))
            {
                throw new Exception("Lagepunkt Y is not a number!");
            }

            if (!Enum.TryParse<Figurenfarbe>(valuesOfFigur[3], out color))
            {
                throw new Exception("Figurenfarbe is not a valid color!");
            }

            if (!double.TryParse(valuesOfFigur[4], out radius))
            {
                throw new Exception("Radius is not a number!");
            }

            return new Kreis(lageX, lageY, color, radius);
        }

        public override int GetFigurenCode()
        {
            return FigurenCode;
        }

        public override double[] GetSeitenlaengen()
        {
            return new double[] { Radius };
        }

        #endregion
    }
}

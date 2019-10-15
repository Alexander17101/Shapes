using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Figuren
{
    public class Rechteck : Figur
    {
        #region Properties
        
        public double SeitenlaengeA { get; set; }
        public double SeitenlaengeB { get; set; }

        #endregion

        #region constants

        static int FigurenCode = 1;

        #endregion

        #region constructors

        public Rechteck(double xCord, double yCord, Figurenfarbe color, double SeitenlaengeA, double SeitenlaengeB) : base(xCord, yCord, color)
        {
            this.SeitenlaengeA = SeitenlaengeA;
            this.SeitenlaengeB = SeitenlaengeB;
        }

        public Rechteck(double xCord, double yCord, double SeitenlaengeA, double SeitenlaengeB) : base(xCord, yCord)
        {
            this.SeitenlaengeA = SeitenlaengeA;
            this.SeitenlaengeB = SeitenlaengeB;
        }

        #endregion

        #region public methods

        public override string ToString()
        {
            return String.Format("{0}, SeitenlaengeA: {1}, SeitenlaengeB: {2}, Flaeche: {3}", base.ToString(), this.SeitenlaengeA, this.SeitenlaengeB, this.GetFlaeche());
        }

        public override double GetFlaeche()
        {
            return SeitenlaengeA * SeitenlaengeB;
        }

        public override Punkt[] GetEckpunkte()
        {
            Punkt[] punkte = new Punkt[4];

            punkte[0].x = Lagepunkt.x + SeitenlaengeA / 2D;
            punkte[0].y = Lagepunkt.y + SeitenlaengeB / 2D;

            punkte[1].x = Lagepunkt.x + SeitenlaengeA / 2D;
            punkte[1].y = Lagepunkt.y - SeitenlaengeB / 2D;

            punkte[2].x = Lagepunkt.x - SeitenlaengeA / 2D;
            punkte[2].y = Lagepunkt.y - SeitenlaengeB / 2D;

            punkte[3].x = Lagepunkt.x - SeitenlaengeA  / 2D;
            punkte[3].y = Lagepunkt.y + SeitenlaengeB / 2D;

            return punkte;
        }

        public override string GetCSV()
        {
            return $"Rechteck;{this.Lagepunkt.x};{this.Lagepunkt.y};{this.Farbe};{this.SeitenlaengeA};{this.SeitenlaengeB}";
        }

        public static Rechteck CheckAndCreateFigur(string[] valuesOfFigur)
        {
            double lageX = 0D, lageY = 0D, seitenlaengeA = 0D, seitenlaengeB = 0D;
            Figurenfarbe color;

            if (valuesOfFigur.Length != 6)
            {
                throw new Exception("Number of arguments for the Rechteck is not valid!");
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

            if (!double.TryParse(valuesOfFigur[4], out seitenlaengeA))
            {
                throw new Exception("SeitenlaengeA is not a number!");
            }
            if (!double.TryParse(valuesOfFigur[5], out seitenlaengeB))
            {
                throw new Exception("SeitenlaengeB is not a number!");
            }

            return new Rechteck(lageX, lageY, color, seitenlaengeA, seitenlaengeB);
        }

        public override int GetFigurenCode()
        {
            return FigurenCode;
        }

        public override double[] GetSeitenlaengen()
        {
            return new double[] { SeitenlaengeA, SeitenlaengeB };
        }

        #endregion
    }
}

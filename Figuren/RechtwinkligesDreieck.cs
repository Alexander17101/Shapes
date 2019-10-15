using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Figuren
{
    public class RechtwinkligesDreieck : Figur
    {
        #region properties

        public double SeitenlaengeA { get; set; }
        public double SeitenlaengeB { get; set; }
        public double Hypotenuse
        {
            get
            {
                return Math.Sqrt(Math.Pow(SeitenlaengeA, 2) + Math.Pow(SeitenlaengeB, 2));
            }
        }

        #endregion

        #region constants

        static int FigurenCode = 2;

        #endregion

        #region constructors

        public RechtwinkligesDreieck(double xCord, double yCord, Figurenfarbe color, double SeitenlaengeA, double SeitenlaengeB)
            :base(xCord, yCord, color)
        {
            this.SeitenlaengeA = SeitenlaengeA;
            this.SeitenlaengeB = SeitenlaengeB;
        }

        public RechtwinkligesDreieck(double xCord, double yCord, double SeitenlaengeA, double SeitenlaengeB)
            : base(xCord, yCord)
        {
            this.SeitenlaengeA = SeitenlaengeA;
            this.SeitenlaengeB = SeitenlaengeB;
        }

        #endregion

        #region public methods

        public override string ToString()
        {
            //return String.Format("X: {0}, Y: {1}, Colour: {2}, Seitenlaenge: {3}", this.Lagepunkt.x, this.Lagepunkt.y, this.Farbe, this.seitenlaenge);
            return String.Format("{0}, SeitenlaengeA: {1}, SeitenlaengeB: {2}, SeitenlaengeC: {3}, Flaeche: {4}", base.ToString(), SeitenlaengeA, SeitenlaengeB, Hypotenuse, GetFlaeche()); 
        }

        public override double GetFlaeche()
        {
            return SeitenlaengeA * SeitenlaengeB / 2D;
        }

        public override Punkt[] GetEckpunkte()
        {
            Punkt[] punkte = new Punkt[3];

            punkte[0].x = Lagepunkt.x;
            punkte[0].y = Lagepunkt.y + SeitenlaengeA;

            punkte[1].x = Lagepunkt.x;
            punkte[1].y = Lagepunkt.y;

            punkte[2].x = Lagepunkt.x + SeitenlaengeB;
            punkte[2].y = Lagepunkt.y;

            return punkte;
        }

        public override string GetCSV()
        {
            return $"RechtwinkligesDreieck;{this.Lagepunkt.x};{this.Lagepunkt.y};{this.Farbe};{this.SeitenlaengeA};{this.SeitenlaengeB}";
        }

        public static RechtwinkligesDreieck CheckAndCreateFigur(string[] valuesOfFigur)
        {

            double lageX = 0D, lageY = 0D, seitenlaengeA = 0D, seitenlaengeB = 0D;
            Figurenfarbe color;

            if (valuesOfFigur.Length != 6)
            {
                throw new Exception("Number of arguments for the Rechtwinkliges Dreieck is not valid!");
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

            return new RechtwinkligesDreieck(lageX, lageY, color, seitenlaengeA, seitenlaengeB);
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

        #region private methods

        #endregion
    }
}

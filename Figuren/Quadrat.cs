using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Figuren
{
    public class Quadrat : Figur
    {
        #region properties

        public double Seitenlaenge { get; set; }

        #endregion

        #region static constants

        static int FigurenCode = 0;

        #endregion

        #region constructors

        public Quadrat(double xCord, double yCord, Figurenfarbe color, double seitenlaenge) : base(xCord, yCord, color)
        {
            this.Seitenlaenge = seitenlaenge;
        }

        public Quadrat(double xCord, double yCord, double seitenlaenge) : base(xCord, yCord)
        {
            this.Seitenlaenge = seitenlaenge;
        }

        #endregion

        #region public methods

        public override string ToString()
        {
            //return String.Format("X: {0}, Y: {1}, Colour: {2}, Seitenlaenge: {3}", this.Lagepunkt.x, this.Lagepunkt.y, this.Farbe, this.seitenlaenge);
            return String.Format("{0}, Seitenlaenge: {1}, Flaeche: {2}", base.ToString(), this.Seitenlaenge, this.GetFlaeche());
        }

        public override double GetFlaeche()
        {
            return Seitenlaenge * Seitenlaenge;
        }

        public override Punkt[] GetEckpunkte()
        {
            Punkt[] punkte = new Punkt[4];

            punkte[0].x = Lagepunkt.x + Seitenlaenge / 2D;
            punkte[0].y = Lagepunkt.y + Seitenlaenge / 2D;

            punkte[1].x = Lagepunkt.x + Seitenlaenge / 2D;
            punkte[1].y = Lagepunkt.y - Seitenlaenge / 2D;

            punkte[2].x = Lagepunkt.x - Seitenlaenge / 2D;
            punkte[2].y = Lagepunkt.y - Seitenlaenge / 2D;

            punkte[3].x = Lagepunkt.x - Seitenlaenge / 2D;
            punkte[3].y = Lagepunkt.y + Seitenlaenge / 2D;

            return punkte;
        }

        public override string GetCSV()
        {
            return $"Quadrat;{this.Lagepunkt.x};{Lagepunkt.y};{Farbe};{Seitenlaenge}";
        }

        public static Quadrat CheckAndCreateFigur(string[] valuesOfFigur)
        {
            double lageX = 0D, lageY = 0D, seitenlaenge = 0D;
            Figurenfarbe color;

            if(valuesOfFigur.Length != 5)
            {
                throw new Exception("Number of arguments for the Quadrat is not valid!");
            }

            if(!double.TryParse(valuesOfFigur[1], out lageX))
            {
                throw new Exception("Lagepunkt X is not a number!");
            }

            if(!double.TryParse(valuesOfFigur[2], out lageY))
            {
                throw new Exception("Lagepunkt Y is not a number!");
            }

            if (!Enum.TryParse<Figurenfarbe>(valuesOfFigur[3], out color))
            {
                throw new Exception("Figurenfarbe is not a valid color!");
            }

            if(!double.TryParse(valuesOfFigur[4], out seitenlaenge))
            {
                throw new Exception("Seitenlaenge is not a number!");
            }

            return new Quadrat(lageX, lageY, color, seitenlaenge);
        }

        public override int GetFigurenCode()
        {
            return FigurenCode;
        }

        public override double[] GetSeitenlaengen()
        {
            return new double[] { Seitenlaenge };
        }

        #endregion
    }
}

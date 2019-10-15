using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Figuren
{
    public enum Figurenfarbe { Gruen, Blau, Rot, Schwarz }

    //struct wie klasse ohne methods, also nur mit eigenschaften
    public struct Punkt
    {
        public double x;
        public double y;
    }

    public abstract class Figur
    {
        #region fields and properties

        private static int NextID = 1000;
        public string ID { get; private set; }

        public Punkt Lagepunkt;
        public Figurenfarbe Farbe { get; set; }

        #endregion

        #region constructors

        public Figur()
        {
            this.Init(0, 0, Figurenfarbe.Rot);

            //Figur(xCord, yCord, colour)
            //funktioniert nicht, kann nicht dezidiert aufgerufen werden
        }

        public Figur(double xCord, double yCord)
        {
            this.Init(xCord, yCord, Figurenfarbe.Rot);
        }

        public Figur(double xCord, double yCord, Figurenfarbe colour)
        {
            this.Init(xCord, yCord, colour);
        }

        #endregion

        #region public methods

        public override string ToString()
        {
            return String.Format("X: {0}, Y: {1}, Colour: {2}", this.Lagepunkt.x, this.Lagepunkt.y, this.Farbe);
        }

        public abstract double GetFlaeche();

        public abstract Punkt[] GetEckpunkte();

        public abstract string GetCSV();

        public abstract int GetFigurenCode();

        public abstract double[] GetSeitenlaengen();

        #endregion

        #region private methods

        private void Init(double xCord, double yCord, Figurenfarbe farbe)
        {
            this.ID = NextID.ToString();
            NextID++;

            this.Lagepunkt.x = xCord;
            this.Lagepunkt.y = yCord;

            this.Farbe = farbe;
        }

        #endregion
    }
}

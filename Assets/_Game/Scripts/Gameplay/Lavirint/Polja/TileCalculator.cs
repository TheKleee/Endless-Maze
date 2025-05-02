using System.Collections.Generic;
using UnityEngine;

namespace Lavirint
{
    //Koristicemo neke stvari odavde ako zatreba, ali ostavicu svakako kao primer potencijalnih metod exstenzija.
    public static class TileCalculator
    {
        static int tileSize = 5;
        public static int SetTileSize(this int size) => tileSize = size > 0 ? size : tileSize;
        public static float TileAnchor(this float t) => tileSize / 2;

       
    }
}

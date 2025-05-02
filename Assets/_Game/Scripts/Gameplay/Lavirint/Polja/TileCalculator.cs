using UnityEngine;

namespace Lavirint
{
    //Koristicemo ako zatreba, ali za sada mi se cini da nam ipak nece trebati posto je projekat krajnje jednostavan.
    //Ostavicu ovde kao primer potencijalne metod exstenzije.
    public static class TileCalculator
    {
        static int tileSize = 5;
        public static int SetTileSize(this int size) => tileSize = size > 0 ? size : tileSize;
        public static float TileAnchor(this float t) => tileSize / 2;
    }
}

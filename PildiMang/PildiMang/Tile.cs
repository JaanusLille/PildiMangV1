using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace PildiMang
{
    public class Tile
    {
        const string UrlPrefix = "https://www.tlu.ee/~tluur/img/";
        public Tile(int row, int col)
        {
            Row = row;
            Col = col;

            TileView = new ContentView

            {
                Padding = new Thickness(1),

                Content = new Image
                {
                    Source = ImageSource.FromUri(new Uri(UrlPrefix + "10/" + "Bitmap" + row + col + ".jpg"))
                }
            };

            Dictionary.Add(TileView, this);
        }

        public static Dictionary<View, Tile> Dictionary { get; } = new Dictionary<View, Tile>();

        public int Row { set; get; }

        public int Col { set; get; }

        public View TileView { private set; get; }
    }
}
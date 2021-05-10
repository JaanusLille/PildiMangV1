using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace PildiMang
{
    public partial class MainPage : ContentPage
    {


        Tile[,] tiles;

        int emptyRow = 3;
        int emptyCol = 3;

        double tileSize;


        public MainPage()
        {
            InitializeComponent();

            tiles = new Tile[4, 4];

            for (int row = 0; row < 4; row++)
            {
                for (int col = 0; col < 4; col++)
                {
                    if (row == 4 - 1 && col == 4 - 1)
                        break;

                    Tile tile = new Tile(row, col);

                    TapGestureRecognizer tapGestureRecognizer = new TapGestureRecognizer();
                    tapGestureRecognizer.Tapped += OnTileTapped;
                    tile.TileView.GestureRecognizers.Add(tapGestureRecognizer);

                    tiles[row, col] = tile;
                    absoluteLayout.Children.Add(tile.TileView);
                }
            }

            // TODO: Randomize

        }


        async void OnTileTapped(object sender, EventArgs args)
        {


            View tileView = (View)sender;
            Tile tappedTile = Tile.Dictionary[tileView];

            await ShiftIntoEmpty(tappedTile.Row, tappedTile.Col);

        }

        void OnContainerSizeChanged(object sender, EventArgs args)
        {
            View container = (View)sender;
            double width = container.Width;
            double height = container.Height;

            if (width <= 0 || height <= 0)
                return;

            if (width < height)
            {
                stackLayout.Orientation = StackOrientation.Vertical;
            }
            else
            {
                stackLayout.Orientation = StackOrientation.Horizontal;
            }

            tileSize = Math.Min(width, height) / 4;
            absoluteLayout.WidthRequest = 4 * tileSize;
            absoluteLayout.HeightRequest = 4 * tileSize;

            foreach (View fileView in absoluteLayout.Children)
            {
                Tile tile = Tile.Dictionary[fileView];

                AbsoluteLayout.SetLayoutBounds(fileView, new Rectangle(tile.Col * tileSize,
                                                                       tile.Row * tileSize,
                                                                       tileSize,
                                                                       tileSize));
            }
        }

        async Task ShiftIntoEmpty(int tappedRow, int tappedCol, uint length = 100)
        {
            if (tappedRow == emptyRow && tappedCol != emptyCol)
            {
                int inc = Math.Sign(tappedCol - emptyCol);
                int begCol = emptyCol + inc;
                int endCol = tappedCol + inc;

                for (int col = begCol; col != endCol; col += inc)
                {
                    await AnimateTile(emptyRow, col, emptyRow, emptyCol, length);
                }
            }
            else if (tappedCol == emptyCol && tappedRow != emptyRow)
            {
                int inc = Math.Sign(tappedRow - emptyRow);
                int begRow = emptyRow + inc;
                int endRow = tappedRow + inc;

                for (int row = begRow; row != endRow; row += inc)
                {
                    await AnimateTile(row, emptyCol, emptyRow, emptyCol, length);
                }
            }
        }

        async Task AnimateTile(int row, int col, int newRow, int newCol, uint length)
        {

            Tile tile = tiles[row, col];
            View tileView = tile.TileView;


            Rectangle rect = new Rectangle(emptyCol * tileSize,
                                           emptyRow * tileSize,
                                           tileSize,
                                           tileSize);

            tileView.Layout(rect);

            AbsoluteLayout.SetLayoutBounds(tileView, rect);

            tiles[newRow, newCol] = tile;
            tile.Row = newRow;
            tile.Col = newCol;
            tiles[row, col] = null;
            emptyRow = row;
            emptyCol = col;
        }

        public void Randomize()
        {
            // shuffle tiles
        }

        async void OnRandomizeButtonClicked(object sender, EventArgs args)
        {
            Randomize();
        }
    }
}
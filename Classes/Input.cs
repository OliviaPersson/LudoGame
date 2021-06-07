using System.Numerics;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Microsoft.Graphics.Canvas;


namespace LudoGame.Classes
{
    internal class Input
    {
        private static object _lastHovered = null;

        public static string MouseMoved(PointerRoutedEventArgs e, UIElement uIElement)
        {
            string objectDescription = "null";
            if (GameEngine.currentGameState == GameState.PlayerPlaying)
            {
                object returned = GameEngine.ClickHitDetection(new Vector2((float)e.GetCurrentPoint(uIElement).Position.X, (float)e.GetCurrentPoint(uIElement).Position.Y));

                if (returned != _lastHovered && _lastHovered is GamePiece) //Check if hoverd object is not the last hoverd object
                {
                    ((GamePiece)_lastHovered).Hover(false, Dice.DiceSave);
                    _lastHovered = null; // Reset last hoverd
                }

                if (returned is GamePiece) //Check if hoverd object is gamepiece
                {
                    ((GamePiece)returned).Hover(true, Dice.DiceSave); // Calls hover method in GamePiece
                    _lastHovered = returned; //Store last hoverd gamepiece
                }

                if (returned is GamePiece) objectDescription = "GamePiece";
                else if (returned is GameTile) objectDescription = "GameTile";
            }
            return objectDescription;
        }


        public static string MouseClicked(PointerRoutedEventArgs e, UIElement uIElement)
        {
            string objectDescription = "null";
            if (GameEngine.currentGameState == GameState.PlayerPlaying)
            {
                object returned = GameEngine.ClickHitDetection(new Vector2((float)e.GetCurrentPoint(uIElement).Position.X, (float)e.GetCurrentPoint(uIElement).Position.Y));

                if (returned is GamePiece)
                {
                    GameEngine.player.MovePiece((GamePiece)returned);
                    Turn.EndTurn();
                }

                if (returned is GamePiece) objectDescription = "GamePiece";
                else if (returned is GameTile) objectDescription = "GameTile";
            }
            return objectDescription;
        }
    }
}
using System;
using UnityEngine;

namespace MagmaHeart.Core.Input
{
    public class OnMouseChangedTileEventArgs : EventArgs
    {
        public Vector3Int TilePosition { get; private set; }
        public OnMouseChangedTileEventArgs(Vector3Int tilePosition) => TilePosition = tilePosition;
    }
}
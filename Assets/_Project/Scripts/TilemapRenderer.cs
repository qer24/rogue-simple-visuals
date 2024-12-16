using RogueProject;
using RogueProject.Models;
using UnityEngine;
using UnityEngine.Tilemaps;
using Vector2Int = RogueProject.Utils.Vector2Int;

public class TilemapRenderer : GameBehaviour
{
    [SerializeField] private Tilemap Tilemap;

    [Space]
    [SerializeField] private TileBase FloorVisibleTile;
    [SerializeField] private TileBase FloorRevealedTile;
    [SerializeField] private TileBase WallTile;
    [SerializeField] private TileBase DoorTile, DoorOpenTile;
    [SerializeField] private TileBase CorridorTile;
    [SerializeField] private TileBase StairsTile;

    protected override void GameUpdate()
    {
        var sizeX = Constants.WORLD_SIZE.x;
        var sizeY = Constants.WORLD_SIZE.y;

        for (int y = 0; y < sizeY; y++)
        {
            for (int x = 0; x < sizeX; x++)
            {
                var position = new Vector3Int(x, y, 0);
                var cell = World.GetCell(new Vector2Int(x, y));

                var currentTile = Tilemap.GetTile(position);

                if (!cell.DoRender())
                {
                    Tilemap.SetTile(position, null);
                    continue;
                }

                var tileType = cell.TileType;
                var tile = GetTile(tileType, new Vector2Int(x, y), tileType != TileType.Floor || cell.Visible);

                // Only update the tile if it has changed
                if (tile != currentTile)
                {
                    Tilemap.SetTile(position, tile);
                }
            }
        }
    }

    private TileBase GetTile(TileType tileType, Vector2Int pos, bool visible = true)
    {
        // return tileType switch
        // {
        //     TileType.Floor        => '.',
        //     TileType.WallTop      => '-',
        //     TileType.WallBottom   => '-',
        //     TileType.WallVertical => '|',
        //     TileType.Door         => '+',
        //     TileType.Corridor     => 'o',
        //     TileType.Empty        => ' ',
        //     TileType.Stairs       => '%',
        //     _                     => '?'
        // };

        return tileType switch
        {
            TileType.Floor                                                   => visible ? FloorVisibleTile : FloorRevealedTile,
            TileType.WallTop or TileType.WallBottom or TileType.WallVertical => WallTile,
            TileType.Door                                                    => World.Player.Position == pos ? DoorOpenTile : DoorTile,
            TileType.Corridor                                                => CorridorTile,
            TileType.Stairs                                                  => StairsTile,
            _                                                                => null
        };
    }

    public Vector3 GetPosition(Vector2Int position)
    {
        var position3D = new Vector3Int(position.x, position.y, 0);
        var worldPosition = Tilemap.GetCellCenterWorld(position3D);
        worldPosition.z = 0;

        return worldPosition;
    }
}

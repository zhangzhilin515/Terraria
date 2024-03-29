using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Tilemaps;
namespace UnityEditor
{
    [CustomGridBrush(true, false, false, "CustomBrush")]
    [CreateAssetMenu(fileName = "CustomBrush", menuName = "Terraria/CustomBrush")]
    public class CustomBrush : GridBrush
    {
        public List<TileClass> tiles;
        public bool reFresh;
        private void OnValidate()
        {
            if(reFresh)
            {
                for (int i = 0; i < tiles.Count; i++)
                {
                    ((CustomTile)tiles[i].tile).blockId = tiles[i].tileId;
                }
            }
            reFresh = false;
        }
        public override void Paint(GridLayout gridLayout, GameObject brushTarget, Vector3Int position)
        {
            base.Paint(gridLayout, brushTarget, position);
        }
        public override void BoxFill(GridLayout gridLayout, GameObject brushTarget, BoundsInt position)
        {
            if (Application.isPlaying)
            {
                foreach (var pos in position.allPositionsWithin)
                {
                    BrushCell cell = cells[GetCellIndexWrapAround(pos.x, pos.y, pos.z)];
                    if (cell.tile is CustomTile)
                    {
                        int id = ((CustomTile)cell.tile).blockId;
                        TerrainManager.Instance.PlaceTile(pos.x, pos.y, tiles[id]);//方块数量需要和最大id相同
                    }
                }
            }
            else
            {
                base.BoxFill(gridLayout, brushTarget, position);
            }
        }
        public override void Erase(GridLayout gridLayout, GameObject brushTarget, Vector3Int position)
        {
            if(Application.isPlaying)
            {
                BrushCell cell = cells[GetCellIndexWrapAround(position.x, position.y, position.z)];
                if (cell.tile is CustomTile)
                {
                    int id = ((CustomTile)cell.tile).blockId;
                    TerrainManager.Instance.Erase(position.x, position.y, (int)tiles[id].layer);
                }
            }
            else
            {
                base.Erase(gridLayout, brushTarget, position);
            }
        }
    }
    [CustomEditor(typeof(CustomBrush))]
    public class CustomBrushEditor : GridBrushEditor
    {
        public override void OnPaintSceneGUI(GridLayout gridLayout, GameObject brushTarget, BoundsInt position, GridBrushBase.Tool tool, bool executing)
        {
            base.OnPaintSceneGUI(gridLayout, brushTarget, position, tool, executing);
            string labelText = "Pos:" + new Vector2Int(position.x, position.y);
            if(position.size.x>1||position.size.y>1)
            {
                labelText += ",Size:" + new Vector2Int(position.size.x, position.size.y);
            }
            Handles.Label(new Vector3(position.x, position.y), labelText);
        }
    }
}

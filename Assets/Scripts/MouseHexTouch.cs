using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace HexMap
{
    public class MouseHexTouch : MonoBehaviour
    {
        void Awake()
        {
            _grid = FindObjectOfType<HexGrid>();
        }
        void Update()
        {
            if (Input.GetMouseButton(0))
            {
                HandleInput();
            }
        }

        void HandleInput()
        {
            Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(inputRay, out hit))
            {
                TouchCell(hit.point);
            }
        }

        private HexGrid _grid;

        void TouchCell(Vector3 position)
        {
            position = _grid.transform.InverseTransformPoint(position);
            HexCoordinates coordinates = HexCoordinates.FromPosition(position);
            int index = coordinates.X + coordinates.Z * _grid.Width + coordinates.Z / 2;
            HexCell cell = _grid.Cells[index];
            cell.Color = _grid.TouchedColor;
            _grid.Mesh.Triangulate(_grid.Cells);
        }
    }
}
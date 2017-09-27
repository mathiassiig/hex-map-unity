using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace HexMap
{
    public class HexMapEditor : MonoBehaviour
    {
        [SerializeField] private ToggleGroup _colorToggleGroup;
        [SerializeField] private Toggle _togglePrefab;
        [SerializeField] private Slider _elevationSlider;
        public Color[] Colors;
        public HexGrid HexGrid;
        private Color _activeColor;

        private int _activeElevation;

        void Awake()
        {
            foreach(var c in Colors)
            {
                var toggle = Instantiate(_togglePrefab) as Toggle;
                var image = toggle.GetComponentInChildren<RawImage>();
                image.color = c;
                toggle.group = _colorToggleGroup;
                toggle.transform.SetParent(_colorToggleGroup.transform);
                toggle.onValueChanged.AddListener((on) =>
                {
                    if (on)
                    {
                        SelectColor(c);
                    }
                });
            }
            _colorToggleGroup.transform.GetChild(0).GetComponent<Toggle>().isOn = true;


            _elevationSlider.onValueChanged.AddListener((elevation) =>
            {
                SetElevation(elevation);
            });
        }

        void Update()
        {
            if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject())
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
                EditCell(HexGrid.GetCell(hit.point));
            }
        }

        public void SetElevation(float elevation)
        {
            _activeElevation = (int)elevation;
        }

        void EditCell(HexCell cell)
        {
            cell.Color = _activeColor;
            cell.Elevation = _activeElevation;
            HexGrid.Refresh();
        }

        public void SelectColor(Color c)
        {
            _activeColor = c;
        }
    }
}
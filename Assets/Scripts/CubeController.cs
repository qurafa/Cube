using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeController : MonoBehaviour
{
    [SerializeField] private GameObject[,,] _cubeMatrix = new GameObject[3, 3, 3];
    [Range(0,5)][SerializeField] private float _speed;

    private bool _lockH = false;
    private bool _lockV = false;
    private float _yawMouse;
    private float _pitchMouse;
    private Rigidbody _rb;
    private Rigidbody _pRb;
    private GameObject _partSelect;
    private int _cubeLayerMask;
    private int _cubePartLayerMask;

    // Start is called before the first frame update
    void Start()
    {
        _cubeLayerMask = 1 << 6;
        _cubePartLayerMask = 1 << 7;
        _rb = GetComponent<Rigidbody>();
        _partSelect = new GameObject("PartSelect");
        _partSelect.transform.parent = transform.parent;
        _pRb = _partSelect.AddComponent<Rigidbody>();
        _pRb.useGravity = false;
        for (int i = 0; i < transform.childCount; i++)
        {
            _cubeMatrix[GetX(transform.GetChild(i).name), GetY(transform.GetChild(i).name), GetZ(transform.GetChild(i).name)] = transform.GetChild(i).gameObject;
            //Debug.Log("Index :" + GetXIndex(transform.GetChild(i).name) + GetYIndex(transform.GetChild(i).name) + GetZIndex(transform.GetChild(i).name) + " Object: " + transform.GetChild(i).gameObject.name);
        }
    }

    void Update()
    {
        RotateCube();
        RotatePart();
        PartSelect();

        if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
        {
            _yawMouse = Input.GetAxis("Mouse X");
            _pitchMouse = Input.GetAxis("Mouse Y");
        }
        else
        {
            _yawMouse = 0;
            _pitchMouse = 0;
        }
    }

    private void RotateCube()
    {
        if (Input.GetMouseButton(0))
        {
            _rb.isKinematic = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            
            if (_yawMouse != 0) _rb.AddTorque(-transform.parent.transform.up * _speed * _yawMouse);

            if (_pitchMouse != 0) _rb.AddTorque(-transform.parent.transform.right * _speed * _pitchMouse);
        }
        else
        {
            _rb.isKinematic = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    private void RotatePart()
    {
        if (Input.GetMouseButton(1))
        {
            _pRb.isKinematic = false;

            if (Mathf.Abs(_yawMouse) > Mathf.Abs(_pitchMouse) && !_lockH && !_lockV) _lockV = true;
            if (Mathf.Abs(_pitchMouse) > Mathf.Abs(_yawMouse) && !_lockH && !_lockV) _lockH = true;

            if (!_lockH && _lockV) _pRb.AddTorque(-transform.up * _speed * _yawMouse);

            if (!_lockV && _lockH) _pRb.AddTorque(-transform.right * _speed * _pitchMouse);
        }
        else
        {
            _pRb.isKinematic = true;
            _lockH = false;
            _lockV = false;
        }
    }

    private void PartSelect()
    {
        if (Input.GetMouseButton(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, 100f, _cubePartLayerMask))
            {
                Debug.Log("Ray");
                if (raycastHit.transform != null)
                {
                    GameObject g = raycastHit.transform.gameObject;

                    if (!_lockH && _lockV) AddPartsH(g);

                    if (!_lockV && _lockH) AddPartsV(g);
                }
            }
        }
        else
        {
            if (_partSelect.transform.childCount != 0)
                for(int i = 0; i < _partSelect.transform.childCount; i++)
                    _partSelect.transform.GetChild(i).parent = transform;
        }
    }

    private void AddPartsH(GameObject o)
    {
        int y = GetY(o.name);
        GameObject[] output = new GameObject[9];

        for (int x = 0; x < 3; x++)
            for (int z = 0; z < 3; z++)
            {
                _cubeMatrix[x, y, z].transform.parent = _partSelect.transform;
                Debug.Log("Part: " + _cubeMatrix[x, y, z].name);
            }
    }

    private void AddPartsV(GameObject o)
    {
        int x = GetX(o.name);
        GameObject[] output = new GameObject[9];
        
        for (int y = 0; y < 3; y++)
            for (int z = 0; z < 3; z++)
            {
                _cubeMatrix[x, y, z].transform.parent = _partSelect.transform;
                Debug.Log("Part: " + _cubeMatrix[x, y, z].name);
            }
    }

    private void UpdateOrientation()
    {

    }

    private GameObject[] AddPartsV2(GameObject o)
    {
        return null;
    }

    private int GetX(string name)
    {
        return System.Convert.ToInt32(name.Substring(0, 1));
    }

    private int GetY(string name)
    {
        return System.Convert.ToInt32(name.Substring(1, 1));
    }

    private int GetZ(string name)
    {
        return System.Convert.ToInt32(name.Substring(2, 1));
    }

}

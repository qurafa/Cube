using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeController : MonoBehaviour
{

    [SerializeField] private GameObject[,,] _cubeMatrix = new GameObject[3, 3, 3];
    [Range(0,1)][SerializeField] private float _speed;

    private bool _lockH = false;
    private bool _lockV = false;
    private bool _initSet = false;
    private float _initialX;
    private float _initialY;

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            _cubeMatrix[GetX(transform.GetChild(i).name), GetY(transform.GetChild(i).name), GetZ(transform.GetChild(i).name)] = transform.GetChild(i).gameObject;
            //Debug.Log("Index :" + GetXIndex(transform.GetChild(i).name) + GetYIndex(transform.GetChild(i).name) + GetZIndex(transform.GetChild(i).name) + " Object: " + transform.GetChild(i).gameObject.name);
        }
    }

    void Update()
    {
        RotateCube();
        //PartSelect();
    }

    private void RotateCube()
    {
        if (Input.GetMouseButton(0) && !Input.GetMouseButton(1))
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            float yawMouse = Input.GetAxis("Mouse X");
            float pitchMouse = Input.GetAxis("Mouse Y");

            Vector3 rotation = Vector3.zero;

            if (yawMouse > 0) rotation += -transform.parent.transform.up;
            else if (yawMouse < 0) rotation += transform.parent.transform.up;

            if (pitchMouse > 0) rotation += -transform.parent.transform.right;
            else if (pitchMouse < 0) rotation += transform.parent.transform.right;

            //Debug.Log("yawMouse: " + yawMouse + ", pitchMouse: " + pitchMouse);

            transform.Rotate(rotation, _speed);
            
            /*
            float newX = pitchMouse * Mathf.Cos(Mathf.Deg2Rad * transform.eulerAngles.z) + yawMouse * Mathf.Sin(Mathf.Deg2Rad * transform.eulerAngles.y);
            float newY = yawMouse * Mathf.Cos(Mathf.Deg2Rad * transform.eulerAngles.z) + pitchMouse * Mathf.Sin(Mathf.Deg2Rad * transform.eulerAngles.x);
            float newZ = pitchMouse * Mathf.Cos(Mathf.Deg2Rad * transform.eulerAngles.x) + yawMouse * Mathf.Sin(Mathf.Deg2Rad * transform.eulerAngles.y);

            transform.Rotate(new Vector3(-newX * 2, -newY * 2, 0));
            */
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    private void PartSelect()
    {
        if (Input.GetMouseButton(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, 100f))
            {
                if (raycastHit.transform != null)
                {
                    GameObject g = raycastHit.transform.gameObject;
                    RotateParts(g);
                }
            }
        }
        else
        {
            _lockH = false;
            _lockV = false;
        }
    }

    private void RotateParts(GameObject g)
    {
        float yawMouse = Input.GetAxis("Mouse X");
        float pitchMouse = Input.GetAxis("Mouse Y");

        if (yawMouse < pitchMouse && !_lockH && !_lockV) _lockH = true;
        if (yawMouse > pitchMouse && !_lockH && !_lockV) _lockV = true;

        if (_lockV) RotatePartsH(g);
        //if (_lockH) RotatePartsV(g);
    }

    private void RotatePartsH(GameObject o)
    {
        GameObject[] parts = GetPartsH(o);

        foreach(GameObject g in parts) g.transform.Rotate(transform.up, _speed);
    }

    private void RotatePartsV(GameObject o)
    {}

    private void RotatePartsV1(GameObject o)
    { }

    private void RotatePartsV2(GameObject o)
    { }

    private GameObject[] GetPartsH(GameObject o)
    {
        int y = GetY(o.name);
        GameObject[] output = new GameObject[9];

        int i = 0;
        for (int x = 0; x < 3; x++)
            for (int z = 0; z < 3; z++)
            {
                output[i] = _cubeMatrix[x, y, z];
                Debug.Log(_cubeMatrix[x, y, z]);
            }

        return output;
    }

    private GameObject[] GetPartsV1(GameObject o)
    {
        return null;
    }

    private GameObject[] GetPartsV2(GameObject o)
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

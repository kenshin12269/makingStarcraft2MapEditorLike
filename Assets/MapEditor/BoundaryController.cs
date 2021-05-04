using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundaryController : MonoBehaviour
{
    [SerializeField] BoxCollider left;
    [SerializeField] BoxCollider right;
    [SerializeField] BoxCollider up;
    [SerializeField] BoxCollider down;
    public Vector2Int Size = Vector2Int.one;
    private Vector2Int beforeSize = Vector2Int.one;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // if (Size < 1)
        //     Size = 1;
        if (beforeSize == Size)
            return;
        beforeSize = Size;
        UpdateBoundary();
    }
    void UpdateBoundary()
    {
        left.transform.localPosition = Vector3.left * (float)Size.x / 2.0f + (Vector3.left / 2.0f);
        right.transform.localPosition = Vector3.right * (float)Size.x / 2.0f + (Vector3.right / 2.0f);
        up.transform.localPosition = Vector3.forward * (float)Size.y / 2.0f + (Vector3.forward / 2.0f);
        down.transform.localPosition = Vector3.back * (float)Size.y / 2.0f + (Vector3.back / 2.0f);
        if (Size.x == 1)
        {
            left.transform.localPosition = Vector3.left;
            right.transform.localPosition = Vector3.right;
        }
        if (Size.y == 1)
        {
            up.transform.localPosition = Vector3.forward;
            down.transform.localPosition = Vector3.back;
        }

        Vector3 beforeScale = Vector3.zero;
        beforeScale = left.transform.localScale;
        beforeScale.z = Size.y;
        left.transform.localScale = beforeScale;
        right.transform.localScale = beforeScale;

        beforeScale = up.transform.localScale;
        beforeScale.x = Size.x;
        up.transform.localScale = beforeScale;
        down.transform.localScale = beforeScale;
    }
}

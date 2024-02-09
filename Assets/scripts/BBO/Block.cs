using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block2 : MonoBehaviour
{
    int height = 30, playerZone= 5, width = 10;


    [SerializeField]
    Transform emptysprite;
    // Start is called before the first frame update
    void Start()
    {
        CreateBoard();
    }

    private void CreateBoard()
    {
        if (emptysprite != null)
        {
            for (int y = 0 ; y< height ; y++)
            {
                for (int x = 0 ; x< width; x++)
                {
                    Transform clone = Instantiate(emptysprite,
                        new Vector3(x*40, (y+playerZone)*7, 0),Quaternion.identity);
                    clone.transform.parent = transform;
                }
            }
        }

    }
}

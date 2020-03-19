using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mergemeshes : MonoBehaviour
{
    
    public GameObject juan;
    public GameObject Tu;
    // Start is called before the first frame update
    void Start()
    {
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        List<MeshFilter> meshlist = new List<MeshFilter>(meshFilters);
        meshlist.RemoveAt(0);
        meshFilters = meshlist.ToArray();
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        int i = 0;
        while (i < meshFilters.Length)
        {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
            meshFilters[i].gameObject.SetActive(false);

            i++;
        }
        //gameObject.AddComponent<MeshFilter>();
        transform.GetComponent<MeshFilter>().mesh = new Mesh();
        transform.GetComponent<MeshFilter>().mesh.CombineMeshes(combine);
        transform.gameObject.SetActive(true);
    }

}

using UnityEngine;

public class RandomiseScientist : MonoBehaviour
{
    [SerializeField] Mesh m_ScientistVar1;
    [SerializeField] Mesh m_ScientistVar2;
    [SerializeField] Mesh m_ScientistVar3;
    [SerializeField] Mesh m_ScientistVar4;
    MeshFilter myMesh;
    private void Start()
    {
        myMesh = GetComponent<MeshFilter>();
        myMesh.mesh = Random.Range(0, 4) switch
        {
            0 => m_ScientistVar1,
            1 => m_ScientistVar2,
            2 => m_ScientistVar3,
            _ => m_ScientistVar4,
        };
    }
}

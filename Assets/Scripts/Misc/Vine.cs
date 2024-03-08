using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vine : MonoBehaviour
{
    public List<MeshRenderer> vineMeshes;
    public float timeToGrow = 5;
    public float refreshRate = 0.05f;

    [Range(0,1)]
    public float minGrow = .2f;
    [Range(0,1)]
    public float maxGrow = 1f;

    private List<Material> vinesMaterials = new List<Material>();
    private bool fullyGrown=false;
    [SerializeField] GameObject levelUp;

    private void OnEnable() {
        
        for (int i = 0; i <vineMeshes.Count; i++)
        {
            for (int k = 0; k < vineMeshes[i].materials.Length; k++)
            {
                if (vineMeshes[i].materials[k].HasProperty("Grow_"))
                {
                    vineMeshes[i].materials[k].SetFloat("Grow_",minGrow);
                    vinesMaterials.Add(vineMeshes[i].materials[k]);
                }
            }
        }    
    
    }
    

    public void GrowVines()
    {       

        for (int i = 0; i < vinesMaterials.Count; i++)
        {
            StartCoroutine(GrowVinesRoutine(vinesMaterials[i]));
        }
    }

    IEnumerator GrowVinesRoutine(Material mat)
    {
        float growValue = mat.GetFloat("Grow_");
        if (!fullyGrown)
        {
            while (growValue<maxGrow)
            {
                growValue += 1/(timeToGrow/refreshRate);
                mat.SetFloat("Grow_",growValue);

                yield return new WaitForSeconds(refreshRate);
            }
        }

        if (growValue >= maxGrow)
        {
            fullyGrown=true;
            // levelUp.SetActive(true);
        }
        else
        {
            fullyGrown=false;
        }
        
    }

}

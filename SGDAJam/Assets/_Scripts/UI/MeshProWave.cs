using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Simple per-character Vertical Sin Wave animation for TextMeshPro objects
/// </summary>
/// 
[RequireComponent(typeof(TMP_Text))]
public class MeshProWave : MonoBehaviour
{
    public float speed = 1f;
    public float ampitude = 2f;
    private TMP_Text mText;
    private TMP_MeshInfo[] mCachedMesh;
    private void Awake()
    {
        mText = GetComponent<TMP_Text>();
    }

    private void Update()
    {
        mText.ForceMeshUpdate();
        var textInfo = mText.textInfo;

        if (mCachedMesh == null)
        {
            mCachedMesh = mText.textInfo.CopyMeshInfoVertexData();
        }

        //On each frame, set the vertex for each character
        for(int i = 0; i < textInfo.characterInfo.Length; i++)
        {
            TMP_CharacterInfo character = textInfo.characterInfo[i];
            int materialIndex = character.materialReferenceIndex;
            int vertexIndex = character.vertexIndex;
            Vector3[] cachedVertices = mCachedMesh[materialIndex].vertices;

            // Determine the center point of each character at the baseline.
            //Vector2 charMidBasline = new Vector2((sourceVertices[vertexIndex + 0].x + sourceVertices[vertexIndex + 2].x) / 2, charInfo.baseLine);
            // Determine the center point of each character.
            Vector2 charMidBasline = (cachedVertices[vertexIndex + 0] + cachedVertices[vertexIndex + 2]) / 2;

            float sinT = Mathf.Sin((Time.timeSinceLevelLoad + charMidBasline.x) * speed ) * ampitude;
            Vector3 sinVector = new Vector3(0, sinT, 0);

            Vector3[] destinationVertices = textInfo.meshInfo[materialIndex].vertices;
            
            for(int j = 0; j < 4; j++)
            {
                destinationVertices[vertexIndex + j] = cachedVertices[vertexIndex + j] + sinVector;
            }

        }

        //Force geometry update
        for (int i = 0; i < textInfo.meshInfo.Length; i++)
        {
            textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;
            mText.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
        }
    }
}

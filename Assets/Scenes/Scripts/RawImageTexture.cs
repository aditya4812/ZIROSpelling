using UnityEngine;
using UnityEngine.UI;

public class RawImageTexture : MonoBehaviour {
    RawImage m_RawImage;
    public Texture[] m_Texture;
    public string picName;
    public int libSize;

    void Start () {
        libSize = m_Texture.Length;
        changeImage ();
        
    }

    public void changeImage () {
        int rand = Random.Range (0, libSize); //0 to (image library size -1)
        //Fetch the RawImage component from the GameObject
        m_RawImage = GetComponent<RawImage> ();
        //Change the Texture to be the one you define in the Inspector
        m_RawImage.texture = m_Texture[rand];
        picName = (m_RawImage.texture).ToString ().Replace(" (UnityEngine.Texture2D)","");
        //Debug.Log(picName);

    }
}
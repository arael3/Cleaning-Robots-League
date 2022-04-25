using UnityEngine;

public class LinkedInUrl : MonoBehaviour
{
    public string url;

    public void Open()
    {
        Application.OpenURL(url);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenRes : MonoBehaviour
{
    public int width;
    public int height;

    public void setfullscreen()
    {
        Screen.SetResolution(width, height, true);
    }

    public void setwindowed()
    {
        Screen.SetResolution(width, height, false);
    }
  

}

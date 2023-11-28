using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonReset : MonoBehaviour
{
    public void Reset()
    {
        //sahne 0 ı tekrar yüklesin
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}

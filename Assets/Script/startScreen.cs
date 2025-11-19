using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreen : MonoBehaviour
{
    void Update()
    {

        if (Input.GetMouseButtonDown(0) || Input.touchCount > 0)
        {

            SceneManager.LoadScene("Main");
        }
    }


}

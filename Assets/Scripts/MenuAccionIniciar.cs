// using System.Collections;
// using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class NewBehaviourScript : MonoBehaviour{
    public void jugar(){
        SceneManager.LoadScene("TriviaSegmentos");
    }

    public void salir(){
        Debug.Log("Saliendo...");
        Application.Quit();
    }
}

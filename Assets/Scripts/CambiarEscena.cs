using UnityEngine;
using UnityEngine.SceneManagement;

public class CambiarEscena : MonoBehaviour
{
    public string nombreDeLaEscena = "TriviaScene"; // Nombre de la escena de trivia

    public void CambiarAEscenaTrivia()
    {
        SceneManager.LoadScene(nombreDeLaEscena); // Cargar la escena de trivia
    }
}

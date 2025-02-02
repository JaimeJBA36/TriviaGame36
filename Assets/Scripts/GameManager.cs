using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Asegura que este script solo se ejecute en el GameObject con un AudioSource
[RequireComponent(typeof(AudioSource))]
public class GameManager : MonoBehaviour
{
    // Sonidos para respuestas correctas e incorrectas
    [SerializeField] private AudioClip m_correctSound = null;
    [SerializeField] private AudioClip m_incorrectSound = null;
    
    // Tiempo de espera entre respuestas y configuraciones del juego
    [SerializeField] private float m_waitTime = 1.0f; 
    [SerializeField] private int pointsPerQuestion = 100;
    [SerializeField] private int maxQuestions = 10;
    [SerializeField] private int targetScore = 1000;
    [SerializeField] private int passingScore = 700;

    // Referencias a los elementos UI
    [SerializeField] private GameObject endGamePanel = null;
    [SerializeField] private Text scoreText = null;
    [SerializeField] private Text resultText = null;
    [SerializeField] private Button playAgainButton = null;
    [SerializeField] private Button mainMenuButton = null;
    [SerializeField] private Text timerText = null;
    [SerializeField] private Text finalScoreText = null;

    private QuizDB m_quizDB = null;
    private QuizzUI m_quizUI = null;
    private AudioSource m_audioSource = null;
    private int score = 0; // Puntaje actual del jugador
    private int questionCount = 0; // Contador de preguntas respondidas
    [SerializeField] private float questionTimer = 15.0f; // Temporizador de preguntas
    private Coroutine timerCoroutine;
    private bool isAnswering = false; // Indica si el jugador está respondiendo

    private void Start()
    {
        // Inicialización de referencias
        m_quizDB = GameObject.FindObjectOfType<QuizDB>();
        m_quizUI = GameObject.FindObjectOfType<QuizzUI>();
        m_audioSource = GetComponent<AudioSource>();

        // Configuración de botones
        playAgainButton.onClick.AddListener(PlayAgain);
        mainMenuButton.onClick.AddListener(GoToMainMenu);

        // Mostrar la primera pregunta
        NextQuestion();
    }

    private void Update()
    {
        // Actualiza el temporizador de la pregunta
        if (!isAnswering && timerCoroutine != null && questionTimer > 0)
        {
            questionTimer -= Time.deltaTime;
            timerText.text = "" + Mathf.Ceil(questionTimer).ToString();

            // Si se acaba el tiempo, pasa a la siguiente pregunta
            if (questionTimer <= 0)
            {
                questionTimer = 0;
                NextQuestion();
            }
        }
    }

    // Obtiene la siguiente pregunta del banco de preguntas
    private void NextQuestion()
    {
        if (questionCount < maxQuestions)
        {
            questionCount++;
            var question = m_quizDB.GetRandom();
            m_quizUI.Constructor(question, GiveAnswer);
            StartQuestionTimer();
        }
        else
        {
            EndGame();
        }
    }

    // Inicia el temporizador de la pregunta
    private void StartQuestionTimer()
    {
        questionTimer = 15.0f;
        timerText.text = "" + Mathf.Ceil(questionTimer).ToString();
        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
        }
        timerCoroutine = StartCoroutine(QuestionTimer());
    }

    // Coroutine para el temporizador de la pregunta
    private IEnumerator QuestionTimer()
    {
        while (questionTimer > 0 && !isAnswering)
        {
            yield return null;
        }
        if (!isAnswering) 
        {
            NextQuestion();
        }
    }

    // Maneja la respuesta del jugador
    private void GiveAnswer(OptionsButton optionsButton)
    {
        if (!isAnswering) 
        {
            isAnswering = true; 
            StartCoroutine(GiveAnswerRoutine(optionsButton));
        }
    }

    // Coroutine para procesar la respuesta del jugador
    private IEnumerator GiveAnswerRoutine(OptionsButton optionsButton)
    {
        // Detiene cualquier sonido que se esté reproduciendo
        if (m_audioSource.isPlaying)
            m_audioSource.Stop();

        // Reproduce el sonido correcto o incorrecto basado en la respuesta
        m_audioSource.clip = optionsButton.opciones.correct ? m_correctSound : m_incorrectSound;
        optionsButton.SetSprite(optionsButton.opciones.correct);

        // Actualiza el puntaje si la respuesta es correcta
        if (optionsButton.opciones.correct)
        {
            score += pointsPerQuestion;
            scoreText.text = "" + score;
        }

        m_audioSource.Play();

        // Espera el tiempo especificado antes de proceder
        yield return new WaitForSeconds(m_waitTime);

        // Si se han respondido todas las preguntas, termina el juego
        if (questionCount >= maxQuestions)
        {
            EndGame();
        }
        else
        {
            isAnswering = false; 
            NextQuestion();
        }
    }

    // Muestra el resultado del juego
    private void ShowResult(bool passed)
    {
        endGamePanel.SetActive(true);
        resultText.text = passed ? "¡Aprobaste!" : "No aprobaste.";
        finalScoreText.text = "Puntaje final: " + score;
    }

    // Botón para reiniciar el juego
    public void PlayAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Botón para regresar al menú principal
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("TriviaSegmentos");
    }

    // Finaliza el juego y muestra los resultados
    private void EndGame()
    {
        if (score >= passingScore)
        {
            ShowResult(true);
        }
        else
        {
            ShowResult(false);
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class QuizzUI : MonoBehaviour
{
    // Referencia al componente de texto para mostrar la pregunta
    [SerializeField] private Text m_question = null;

    // Lista de botones de opciones
    [SerializeField] private List<OptionsButton> m_buttonList = null;

    // Referencia al componente de texto para mostrar el temporizador (si es necesario)
    [SerializeField] private Text timerText = null;

    // Método para configurar la interfaz del cuestionario
    public void Constructor(Preguntas q, Action<OptionsButton> callback)
    {
        // Establece el texto de la pregunta
        m_question.text = q.text;

        // Configura cada botón de opción con sus correspondientes datos y callback
        for (int n = 0; n < m_buttonList.Count; n++)
        {
            m_buttonList[n].Constructor(q.options[n], callback);
        }
    }
}

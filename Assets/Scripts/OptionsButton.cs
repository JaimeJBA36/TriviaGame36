using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

// Asegura que este script solo se ejecute en un GameObject con un Button y un Image
[RequireComponent(typeof(Button))]
[RequireComponent(typeof(Image))]
public class OptionsButton : MonoBehaviour
{
    // Referencias a los componentes UI
    private Text m_text = null;
    private Button m_button = null;
    private Image m_image = null;
    private Sprite m_originalSprite = null; // Sprite original del botón

    public Opciones opciones { get; set; } // Propiedad para almacenar las opciones del botón

    // Sprites para respuestas correctas e incorrectas
    [SerializeField] private Sprite correctSprite = null;
    [SerializeField] private Sprite incorrectSprite = null;

    // Inicialización de componentes
    private void Awake()
    {
        m_button = GetComponent<Button>();
        m_image = GetComponent<Image>();
        m_text = transform.GetChild(0).GetComponent<Text>();

        m_originalSprite = m_image.sprite; // Guarda el sprite original del botón
    }

    // Método para configurar el botón con las opciones y el callback
    public void Constructor(Opciones opciones, Action<OptionsButton> callback)
    {
        m_text.text = opciones.text; // Establece el texto del botón

        m_button.onClick.RemoveAllListeners(); // Elimina cualquier listener previo
        m_button.enabled = true; // Habilita el botón
        m_image.sprite = m_originalSprite; // Restablece el sprite original del botón
        this.opciones = opciones; // Asigna las opciones al botón

        // Agrega el callback al evento onClick del botón
        m_button.onClick.AddListener(delegate {
            callback(this);
        });
    }

    // Método para establecer el sprite basado en si la respuesta es correcta o incorrecta
    public void SetSprite(bool isCorrect)
    {
        m_button.enabled = false; // Deshabilita el botón para evitar múltiples clics
        m_image.sprite = isCorrect ? correctSprite : incorrectSprite; // Cambia el sprite basado en la corrección de la respuesta
    }
}

using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class QuizDB : MonoBehaviour
{
    // Lista de preguntas inicializada desde el editor
    [SerializeField] private List<Preguntas> m_questionList = null;

    // Copia de respaldo de la lista de preguntas
    private List<Preguntas> m_backup = null;

    // Método llamado al inicializar el script
    private void Awake()
    {
        // Crear una copia de respaldo de la lista de preguntas original
        m_backup = m_questionList.ToList();
    }

    // Método para obtener una pregunta aleatoria de la base de datos
    public Preguntas GetRandom(bool remove = true)
    {
        // Si no hay preguntas disponibles, restaurar la copia de respaldo
        if (m_questionList.Count == 0)
            RestoreBackup();

        // Obtener un índice aleatorio dentro del rango de la lista de preguntas
        int index = Random.Range(0, m_questionList.Count);

        // Si no se debe remover la pregunta de la lista, devolverla directamente
        if (!remove)
            return m_questionList[index];

        // Obtener la pregunta en el índice aleatorio
        Preguntas q = m_questionList[index];

        // Remover la pregunta de la lista para que no se repita
        m_questionList.RemoveAt(index);

        // Devolver la pregunta seleccionada
        return q;
    }

    // Método para restaurar la lista de preguntas desde la copia de respaldo
    private void RestoreBackup()
    {
        m_questionList = m_backup.ToList();
    }
}

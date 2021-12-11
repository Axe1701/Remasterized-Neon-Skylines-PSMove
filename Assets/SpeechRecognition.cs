using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;
using System.Linq;
using TMPro;

public class SpeechRecognition : MonoBehaviour
{

    public TextMeshProUGUI myTMP;

    private KeywordRecognizer reconocePalabras;
    private ConfidenceLevel confidencialidad = ConfidenceLevel.Low;
    private Dictionary<string, Accion> palabrasAccion = new Dictionary<string, Accion>();
    private TurretMovement myturretcontroller;

    //crear Delegado para la acción a ejecutar
    private delegate void Accion();

    // Start is called before the first frame update
    void Start()
    {
        myTMP = GameObject.FindGameObjectWithTag("SpeechText").GetComponent<TextMeshProUGUI>();
        myturretcontroller = GetComponent<TurretMovement>();

        palabrasAccion.Add("arriba", myturretcontroller.DestroyTurret);
        palabrasAccion.Add("abajo", myturretcontroller.DestroyTurret);
        palabrasAccion.Add("izquierda", myturretcontroller.DestroyTurret);
        palabrasAccion.Add("derecha", myturretcontroller.DestroyTurret);
        reconocePalabras = new KeywordRecognizer(palabrasAccion.Keys.ToArray(), confidencialidad);
        reconocePalabras.OnPhraseRecognized += OnKeywordsRecognized;
        reconocePalabras.Start();
    }

    void OnDestroy()
    {
        if (reconocePalabras != null && reconocePalabras.IsRunning)
        {
            reconocePalabras.Stop();
            reconocePalabras.Dispose();
        }
    }

    private void OnKeywordsRecognized(PhraseRecognizedEventArgs args)
    {
        myTMP.text = args.text;
        palabrasAccion[args.text].Invoke();
    }
}
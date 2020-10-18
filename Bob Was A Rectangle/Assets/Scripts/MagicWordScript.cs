using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MagicWordScript : MonoBehaviour
{
    [SerializeField] Text textBox;
    [SerializeField] string textUsed;
    private void Awake()
    {
        
    }
    // Start is called before the first frame update
    void Start()
    {
        textBox.text = " ";
        StartCoroutine(setText(textUsed));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator setText(string textUsed)
    {
        for(int i = 0; i < textUsed.Length; i++)
        {
            textBox.text = textUsed.Substring(0, i) + "|";
            yield return new WaitForSeconds(0.1f);
        }
        textBox.text = textUsed;
        yield return new WaitForSeconds(2.0f);
        textBox.text = " ";
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class dialogue : MonoBehaviour
{
    public TextAsset dialogDataFile;
    public TMP_Text nameText;
    public TMP_Text dialogText;
    public int DialogIndex;
    public string[] DialogRows;
    public GameObject optionButton;
    public Transform buttonGroup;
    private bool btnfull = true;
    private bool optionsGenerated = false;
    public AudioSource audioSource; // ��Inspector��ָ��
    public AudioClip[] dialogAudioClips; // ��Inspector��ָ����ȷ����Ի���������Ӧ
    public Animator FaceAnimation;



    void Start()
    {
   
            ReadText(dialogDataFile);
            ShowDiaLogRow();

    }

    

    public void UpdateText(string _name, string _text, int audioClipIndex = -1, int animatorIntValue = -1)
    {
        nameText.text = _name;
        dialogText.text = _text;
        FaceAnimation.SetInteger("DialogueNumber", animatorIntValue);
        if (audioClipIndex >= 0 && audioClipIndex < dialogAudioClips.Length)
        {
            AudioClip clip = dialogAudioClips[audioClipIndex];
            audioSource.clip = clip;
            audioSource.Play();
            StartCoroutine(WaitForAudioClip(audioSource));
        }
    }

    public void ReadText(TextAsset _textAsset)
    {
        DialogRows = _textAsset.text.Split('\n');

    }
    public void ShowDiaLogRow()
    {
        for (int i = 0; i < DialogRows.Length; i++)
        {
            string[] cell = DialogRows[i].Split(',');
            if (cell[0] == "#" && int.Parse(cell[1]) == DialogIndex)
            {
                int audioIndex = int.TryParse(cell[5], out var index) ? index : -1;
                int animatorIntValue = int.TryParse(cell[5], out var animValue) ? animValue : -1;
                UpdateText(cell[2], cell[3], audioIndex, animatorIntValue);
                DialogIndex = int.Parse(cell[4]);
                optionsGenerated = false; // ����ѡ�����ɱ�־
                break;
            }
            else if (cell[0] == "!" && int.Parse(cell[1]) == DialogIndex && !optionsGenerated)
            {

                GenerateOption(i);
                optionsGenerated = true; // ���ñ�־�Է�ֹ�ٴ�����
            }
            else if (cell[0] == "NEXT" && int.Parse(cell[1]) == DialogIndex)
            {

                SceneManager.LoadScene(1);

            }
            else if (cell[0] == "END" && int.Parse(cell[1]) == DialogIndex)
            {
                  Application.Quit();
            }
        }
    }


    public void GenerateOption(int _index)
    {
        // ʹ����ȷ�ı���i�������Ի���
        for (int i = _index; i < DialogRows.Length; i++)
        {
            string[] cells = DialogRows[i].Split(','); // ע������ʹ��i������_index
            if (cells[0] == "!")
            {

                // ʵ����ѡ�ť
                GameObject button = Instantiate(optionButton, buttonGroup);

                // ���谴ť����һ��TextMeshProUGUI�������ʾ�ı�
                TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>();
                buttonText.text = cells[3];
                button.GetComponent<Button>().onClick.AddListener
                    (
                    delegate
                    {
                        OnOptionClick(int.Parse(cells[4]));

                    }
                    );
            }
            else if (cells[0] != "!" && i > _index)
            {
                // ����������в�����"!"��ͷ�������Ѿ����������һ��ѡ��ж�ѭ��
                break;
            }
        }
    }
    public void OnOptionClick(int _id)
    {
        DialogIndex = _id;
        ShowDiaLogRow();
        // �����־��ȷ�ϴ˷���������
        Debug.Log("OnOptionClick called with _id: " + _id);
        foreach (Transform child in buttonGroup)
        {
            Destroy(child.gameObject);
        }
    }
    IEnumerator WaitForAudioClip(AudioSource source)
    {
        yield return new WaitWhile(() => source.isPlaying);
        ShowDiaLogRow();
    }
}
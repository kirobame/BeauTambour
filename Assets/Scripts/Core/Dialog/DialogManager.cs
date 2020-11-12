using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handle the progress of a dialog between characters and display each reply
/// </summary>
public class DialogManager : MonoBehaviour
{
    private const int NB_MAX_LINE_TO_DISPLAY = 2;
    private const int NB_MAX_CHAR_PER_LINE = 20;
    private const float MARGIN = 1;

    private Cue[] dialog; //TODO : remplace by the dialog type
    [SerializeField]private Transform cueBubble;
    [SerializeField]private Transform bubbleSprite;
    [SerializeField]private TextMesh bubbleText;

    private int currentIndexInDialog = -1;

    private string[] currentCueAsLines;    
    private int cueTotalLinesDisplayed = 0;
    private int nbLines = 0;
    private bool isOverflowing = false;

    
    /// <summary>
    /// Start playing the dialog.
    /// </summary>
    /// <param name="p_dialog">Dialog to play.</param>
    public void InitializeDialog(Cue[] p_dialog)
    {
        InitTester.Instance.OnPassCue += OnPassCue;
        dialog = p_dialog;
        NextCue();
    }

    ////////////////////////////////////////////////////////////////////////
    //                         CALLBACKS                                  //
    ////////////////////////////////////////////////////////////////////////
    public void OnPassCue()
    {
        if ( !isOverflowing &&(currentIndexInDialog == dialog.Length - 1 ||(currentIndexInDialog != dialog.Length-1 && !dialog[currentIndexInDialog].HasSameActorAs(dialog[currentIndexInDialog + 1]))) )
        {
            DepopBubble();
        }        
        NextCue();
    }

    ////////////////////////////////////////////////////////////////////////
    //                         PRIVATE METHODS                            //
    ////////////////////////////////////////////////////////////////////////

    /// <summary>
    /// Pass to the next cue of the dialog.
    /// </summary>
    private void NextCue()
    {
        if (!isOverflowing)
        {
            currentIndexInDialog++;
            cueTotalLinesDisplayed = 0;
        }
        if (currentIndexInDialog < dialog.Length)
        {
            nbLines = 0;
            PopBubble();
        }
        else
        {
            EndDialog();
        }
    }

    /// <summary>
    /// Fill, Resize, Replace and show the bubble containing the cue.
    /// </summary>
    private void PopBubble()
    {
        Cue currentCue = dialog[currentIndexInDialog];
        float side = (currentCue.Actor.side == ActorPositionTest.Player) ? 1 : -1;
        HandleTextFill(currentCue);
        ResizeBubble();
        ReplaceBubble(currentCue, side);
        cueBubble.gameObject.SetActive(true);

    }

    /// <summary>
    /// Handle the filling of the text mesh cutting the text in lines and in different bubbles if
    /// the text is too big for a single bubble.
    /// </summary>
    /// <param name="currentCue">tThe current cue.</param>
    private void HandleTextFill(Cue currentCue)
    {
        if (!isOverflowing) { 
            if(LengthWithoutTag(currentCue.Text) >= NB_MAX_CHAR_PER_LINE)
            {
                currentCueAsLines = DivideTextInLines(currentCue.Text);
            }
            else
            {
                currentCueAsLines = new string[] { currentCue.Text };
            }
        }
        if(currentCueAsLines.Length > NB_MAX_LINE_TO_DISPLAY)
        {
            if (!isOverflowing)
            {
                isOverflowing = true;
            }
            
            if(cueTotalLinesDisplayed + NB_MAX_LINE_TO_DISPLAY > currentCueAsLines.Length)
            {
                int diff = currentCueAsLines.Length - cueTotalLinesDisplayed;
                bubbleText.text = LinesToString(cueTotalLinesDisplayed, diff, currentCueAsLines);
                cueTotalLinesDisplayed += diff;
                nbLines = diff;
            }
            else
            {
                bubbleText.text = LinesToString(cueTotalLinesDisplayed, NB_MAX_LINE_TO_DISPLAY, currentCueAsLines)+"...";
                cueTotalLinesDisplayed += NB_MAX_LINE_TO_DISPLAY;
                nbLines = NB_MAX_LINE_TO_DISPLAY;
            }
        }
        else
        {
            bubbleText.text = LinesToString(currentCueAsLines);
        }
        bubbleText.color = currentCue.Actor.fontColor;
        if(isOverflowing && cueTotalLinesDisplayed >= currentCueAsLines.Length)
        {
            isOverflowing = false;
        }
    }

    /// <summary>
    /// Resize the bubble depending on the text settins and content.
    /// </summary>
    private void ResizeBubble()
    {
        bubbleSprite.localScale = new Vector3(GetTextMaxWidth() + MARGIN, GetTextHeight() + MARGIN);
    }

    /// <summary>
    /// Place the bubble to the right position.
    /// </summary>
    /// <param name="currentCue">The current cue.</param>
    /// <param name="side">On which side of the screen is the actor.</param>
    private void ReplaceBubble(Cue currentCue, float side)
    {
        Vector3 offSetBubble = new Vector2(bubbleSprite.localScale.x / 2f, bubbleSprite.localScale.y / 2f);
        Vector3 offSetCharacter = new Vector2(side * (0.5f + offSetBubble.x), 0.5f + offSetBubble.y);
        Vector2 newPosition = currentCue.Actor.selfTransform.position + offSetCharacter;
        cueBubble.position = newPosition;
    }

    /// <summary>
    /// Hide the Bubble.
    /// </summary>
    private void DepopBubble()
    {
        //cacher
        cueBubble.gameObject.SetActive(false);
    }

    /// <summary>
    /// Called at the end of the dialog.
    /// </summary>
    private void EndDialog()
    {
        InitTester.Instance.OnPassCue -= OnPassCue;
    }

    /// <summary>
    /// Get the max width in text.( Calcul the width of the biggest line )
    /// </summary>
    /// <returns>The max width of the cue.</returns>
    private float GetTextMaxWidth()
    {
        int totalLength = 0;

        Font myFont = bubbleText.font;
        CharacterInfo characterInfo = new CharacterInfo();
        char[] arr = GetBiggestLine().ToCharArray();
        bool isTag = false;

        foreach (char c in arr)
        {
            if (!isTag && IsATagCharacter(c))
            {
                isTag = true;
            }
            else if (isTag && IsATagCharacter(c))
            {
                isTag = false;
                continue;
            }
            if (isTag) {continue; }
            if(c == '\n') { continue; }
            myFont.GetCharacterInfo(c, out characterInfo, bubbleText.fontSize, bubbleText.fontStyle);

            totalLength += characterInfo.advance;
        }

        return totalLength * bubbleText.characterSize * 0.1f;
    }

    /// <summary>
    /// Get the biggest line of the text.
    /// </summary>
    /// <returns>The biggest line of the cue.</returns>
    private string GetBiggestLine()
    {
        string[] lines = currentCueAsLines;
        int start = cueTotalLinesDisplayed - nbLines;
        string maxLengthLine = lines[start];

        for (int index = start+1; index <cueTotalLinesDisplayed; index++)
        {
            if (LengthWithoutTag(lines[index]) > LengthWithoutTag(maxLengthLine))
            {
                maxLengthLine = lines[index];
            }
        }

        return maxLengthLine;
    }

    /// <summary>
    /// Get the text height depending on the number of lines displayed.
    /// </summary>
    /// <returns>The height of the cue.</returns>
    private float GetTextHeight()
    {
        return bubbleText.characterSize * bubbleText.fontSize * 0.1f * ( currentCueAsLines.Length >= NB_MAX_LINE_TO_DISPLAY ? (cueTotalLinesDisplayed >= currentCueAsLines.Length)? currentCueAsLines.Length % NB_MAX_LINE_TO_DISPLAY:NB_MAX_LINE_TO_DISPLAY:currentCueAsLines.Length) ;
    }

    /// <summary>
    /// Divide a text in Lines depending on the max number of char per line.
    /// </summary>
    /// <param name="text"></param>
    /// <returns>The lines of the text.</returns>
    private string[] DivideTextInLines(string text)
    {
        string result = new string(text.ToCharArray());
        int count = 0;
        int index = 0;
        bool isWord = false;
        bool isTag = false;
        int lastEndWordIndex = 0;
        while(index < result.Length)
        {
            //Doesnt count the characters if we are in a tag
            if(!isTag && IsATagCharacter(result[index]))
            {
                isTag = true;
            }else if (isTag && IsATagCharacter(result[index]))
            {
                isTag = false;
                index++;
                continue;
            }
            if (isTag) { index++; continue; }

            //Detect the words 
            if (!isWord && IsAWordCharacter(result[index]))
            {
                isWord = true;
            }
            else if (isWord && !IsAWordCharacter(result[index]))
            {
                lastEndWordIndex = index-1;
                isWord = false;
            }
            count++;
            //If there is there is a new Line wrote in the text, reset count
            if (result[index] == '\n') count = 0;
            if(count > NB_MAX_CHAR_PER_LINE)
            {
                if(result[lastEndWordIndex+1] == ' ')
                {
                    result = result.Remove(lastEndWordIndex + 1,1);
                }
                result = result.Insert(lastEndWordIndex+1, "\n");
                count = 0;
            }
            index++;
        }
        return result.Split('\n');
    }

    /// <summary>
    /// Check if a char is a word component.
    /// </summary>
    /// <param name="c">The char to check.</param>
    /// <returns>TRUE : the char is a word component
    /// else FALSE
    /// </returns>
    private bool IsAWordCharacter(char c)
    {
        return c != '\n' && c != '\'' && c != '.' && c != ',' && c != ' ';
    }
    
    /// <summary>
    /// Check if a char is a tag component.
    /// </summary>
    /// <param name="c">The char to check.</param>
    /// <returns>TRUE : the char is a tag component
    /// else FALSE
    /// </returns>
    private bool IsATagCharacter(char c)
    {
        return c == '<' || c == '>';
    }

    /// <summary>
    /// Get the length of a string without counting the tags.
    /// </summary>
    /// <param name="text">The string to count.</param>
    /// <returns>The length without tags.</returns>
    private int LengthWithoutTag(string text)
    {
        int count = 0;
        
        bool isTag = false;
        for(int index = 0;index < text.Length;index++)
        {
            if (!isTag && IsATagCharacter(text[index]))
            {
                isTag = true;
            }
            else if(isTag && IsATagCharacter(text[index]))
            {
                isTag = false;
                continue;
            }
            if (isTag) { continue; }
            if (text[index] == '\n') continue;
            count++;
        }
        return count;
    }

    /// <summary>
    /// Transform lines into string from a start index and for a specific
    /// number of lines
    /// </summary>
    /// <param name="startIndex">The first line to get.</param>
    /// <param name="count">The count of line to transform.</param>
    /// <param name="lines">The tab from where get the lines.</param>
    /// <returns>The string result</returns>
    private string LinesToString(int startIndex, int count, string[] lines)
    {
        string result = "";
        if (startIndex >= lines.Length) return null;
        for (int index = startIndex; index < startIndex + count; index++)
        {
            if(index == startIndex + count - 1)
            {
                result = string.Format("{0}{1}", result, lines[index]);
            }
            else
            {
                result = string.Format("{0}{1}{2}", result, lines[index], '\n');
            }
        }
        return result;
    }

    /// <summary>
    /// Transform all the lines in the tab in a string
    /// </summary>
    /// <param name="lines">The lines.</param>
    /// <returns>The string result.</returns>
    private string LinesToString(string[] lines)
    {
        return LinesToString(0,lines.Length,lines);
    }
}

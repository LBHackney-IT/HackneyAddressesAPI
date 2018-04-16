using System;
using System.Drawing;
using System.Windows.Forms;

namespace Hackney.ConfigEncryptor
{
    public partial class FormCompare : Form
    {
        private UseWhichFileEnum _useWhichFile; 

        public FormCompare()
        {
            InitializeComponent();
        }

        public string LocalFileContent {
            get { return this.richTextBoxLocal.Text; }
            set { this.richTextBoxLocal.Text = value; }
        }   
        public string ServerFileContent
        {
            get { return this.richTextBoxServer.Text;}
            set { this.richTextBoxServer.Text = value; }
        }

        public RichTextBox RichTectboBoxLocal
        {
            get { return this.richTextBoxLocal; }
        }

        public RichTextBox RichTextBoxServer
        {
            get { return this.richTextBoxServer; }
        }

        public string LocalFileName
        {
            set { labelLocalConfigFilename.Text = $"Local Filename: {value}";}
        }

        public string ServerFileName
        {
            set { labelServerConfigFilename.Text = $"Server Filename: {value}"; }
        }

        public UseWhichFileEnum UseWhichFile
        {
            get { return _useWhichFile; }
        }

        private void labelUseLocalConfig_Click(object sender, EventArgs e)
        {
            _useWhichFile = UseWhichFileEnum.Local;
            this.Hide();
        }

        private void labelUseServerConfig_Click(object sender, EventArgs e)
        {
            _useWhichFile = UseWhichFileEnum.Server;
            this.Hide();
        }

        public void HighlightCompare()
        {
            //for ease of reading conceptualise left and right.
            RichTextBox rtbLeft = richTextBoxLocal;
            RichTextBox rtbRight = richTextBoxServer;

            RichTextBox leftSide = (rtbLeft.Lines.Length > rtbRight.Lines.Length) ? rtbLeft : rtbRight;
            RichTextBox rightSide = (rtbLeft.Lines.Length > rtbRight.Lines.Length) ? rtbRight : rtbLeft;

            int rightLineIndex = 0;

            int extraLines = leftSide.Lines.Length - rightSide.Lines.Length;

            for (int leftLineIndex = 0; leftLineIndex < leftSide.Lines.Length; leftLineIndex++)
            {
                var markAsNew = false;

                if (rightLineIndex >= rightSide.Lines.Length)
                    break;
                
                if (leftSide.Lines[leftLineIndex] == rightSide.Lines[rightLineIndex])
                {
                    rightSide.Select(rightSide.GetFirstCharIndexFromLine(rightLineIndex),
                        rightSide.Lines[rightLineIndex].Length);
                    rightSide.SelectionBackColor = Color.PaleGreen;
                    leftSide.Select(leftSide.GetFirstCharIndexFromLine(leftLineIndex),
                        leftSide.Lines[leftLineIndex].Length + 1);


                    leftSide.SelectionBackColor = Color.White;

                }
                else
                {
                    //if there's more than x percentage difference then see if its likely to be a new line
                    if (HighlightCompareCharacters(leftLineIndex, leftSide.Lines[leftLineIndex], rightSide.Lines[rightLineIndex], rightSide, rightLineIndex) > 0.6m)
                    {
                        //if there is a line that is brand new then highlight it blue.
                        //a new line will be if the following line doesnt match the right hand sides next line
                        if (extraLines >= 1)
                        {
                            for (int i = 1; i < extraLines+1; i++)
                            {
                                if (leftSide.Lines[leftLineIndex + i] != rightSide.Lines[rightLineIndex])
                                {
                                    
                                    if (rightSide.Lines.Length == rightLineIndex + 1)
                                        markAsNew = true;

                                    if (!markAsNew)
                                    {
                                        if (IsLikelyNewLine(leftSide.Lines[leftLineIndex + i],
                                            rightSide.Lines[rightLineIndex + 1]))
                                            markAsNew = true;
                                    }

                                    if (markAsNew)
                                    {
                                        leftSide.Select(leftSide.GetFirstCharIndexFromLine(leftLineIndex),
                                            leftSide.GetFirstCharIndexFromLine(leftLineIndex + 1) - 1);
                                        System.Diagnostics.Debug.WriteLine($"Line '{leftSide.Lines[leftLineIndex]}' looks new, marking new.");
                                        leftSide.SelectionBackColor = Color.Aqua;

                                        //once background changed need to set it back to white.
                                        leftSide.Select(leftSide.GetFirstCharIndexFromLine(leftLineIndex+1),
                                            leftSide.GetFirstCharIndexFromLine(leftLineIndex + 2) - 1);
                                        leftSide.SelectionBackColor = Color.White;
                                        extraLines--;
                                        break;
                                    }
                                }
                                else
                                {
                                    if (rightLineIndex + 1 == rightSide.Lines.Length)
                                    {
                                        leftSide.Select(leftSide.GetFirstCharIndexFromLine(leftLineIndex),
                                            leftSide.GetFirstCharIndexFromLine(leftLineIndex + 1) - 1);
                                        System.Diagnostics.Debug.WriteLine($"Line '{leftSide.Lines[leftLineIndex]}' looks new, marking new.");
                                        leftSide.SelectionBackColor = Color.Aqua;

                                        leftSide.Select(
                                            leftSide.GetFirstCharIndexFromLine(leftLineIndex + 1) +
                                            leftSide.Lines[leftLineIndex + 1].Length + 1, 1);
                                            
                                        leftSide.SelectionBackColor = Color.White;

                                        markAsNew = true;
                                        extraLines--;
                                        break;
                                    }

                                    if (leftSide.GetFirstCharIndexFromLine(leftLineIndex +1 +i) < 0)
                                        break;

                                    leftSide.Select(leftSide.GetFirstCharIndexFromLine(leftLineIndex + i), leftSide.Lines[leftLineIndex + i].Length);
                                    System.Diagnostics.Debug.WriteLine($"Line '{leftSide.Lines[leftLineIndex]}' looks comparable, marking plain. " +
                                                                       $"{leftSide.Text.Substring(leftSide.GetFirstCharIndexFromLine(leftLineIndex + i), (leftSide.GetFirstCharIndexFromLine(leftLineIndex + 1 + i) - 1) - leftSide.GetFirstCharIndexFromLine(leftLineIndex + i))}");
                                    leftSide.SelectionBackColor = Color.White;
                                    break;
                                }
                            }
                        }
                    }
                }

                if (!markAsNew) rightLineIndex++;
            }
        }

        private bool IsLikelyNewLine(string leftString, string rightString)
        {

            int i = 0;
            decimal trueness = 1;

            foreach (var leftChar in leftString)
            {
                if (leftChar != rightString[i])
                {
                    trueness -= ((decimal)i / leftString.Length);
                    i++;
                }
            }

            return Convert.ToBoolean(trueness);

        }

        private decimal HighlightCompareCharacters(int lineIndex, string line, string rightSideLine, RichTextBox rightSide, int rightLineIndex)
        {
            int selectStart = rightSide.GetFirstCharIndexFromLine(rightLineIndex);
            int idxCurrentPos = 0;
            int pinkChars = 0;

            foreach (char c in line)
            {
                rightSide.Select(selectStart+idxCurrentPos, 1);
                if (idxCurrentPos >= rightSideLine.Length)
                    continue;
                if (c == rightSideLine[idxCurrentPos])
                {
                    rightSide.SelectionBackColor = Color.PaleGreen;
                }
                else
                {
                    rightSide.SelectionBackColor = Color.Pink;
                    pinkChars++;
                }
                idxCurrentPos++;
            }

            return pinkChars == 0 ? 0 : (decimal)pinkChars / (decimal)line.Length;

        }
    }
}

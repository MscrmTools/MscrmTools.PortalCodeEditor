using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Folding;
using ICSharpCode.AvalonEdit.Highlighting;
using Jsbeautifier;
using MscrmTools.PortalCodeEditor.AppCode;
using MscrmTools.PortalCodeEditor.Forms;
using Yahoo.Yui.Compressor;

namespace MscrmTools.PortalCodeEditor.Controls
{
    public partial class CodeEditor : UserControl
    {
        #region Variables

        private readonly TextEditor textEditor;
        private readonly CodeItem item;
        private readonly Settings mySettings;
        private FoldingManager foldingManager;

        private bool foldingManagerInstalled;
        private HtmlFoldingStrategy htmlFoldingStrategy;
        private string content;

        #endregion

        #region Constructor
        
        public CodeEditor(CodeItem item, Settings mySettings)
        {
            InitializeComponent();

            content = item.Content;
            this.item = item;
            this.mySettings = mySettings;

            textEditor = new TextEditor
            {
                ShowLineNumbers = true,
                FontSize = 12,
                FontFamily = new System.Windows.Media.FontFamily("Consolas"),
            };

            var wpfHost = new ElementHost
            {
                Child = textEditor,
                Dock = DockStyle.Fill,
                BackColorTransparent = true,
            };

            Controls.Add(wpfHost);
        }

        #endregion

        #region Properties

        public bool FoldingEnabled { get; private set; }

        #endregion

        #region Events

        private void CodeEditor_Load(object sender, EventArgs e)
        {
            switch (item.Type)
            {
                case CodeItemType.JavaScript:
                    textEditor.SyntaxHighlighting = HighlightingManager.Instance.GetDefinition("JavaScript");
                    break;
                case CodeItemType.Style:
                    textEditor.SyntaxHighlighting = HighlightingManager.Instance.GetDefinition("CSS");
                    break;
                case CodeItemType.LiquidTemplate:
                    textEditor.SyntaxHighlighting = HighlightingManager.Instance.GetDefinition("HTML");
                    break;
            }

            textEditor.Text = content;
            textEditor.TextChanged += TextEditor_TextChanged;
        }

        private void TextEditor_TextChanged(object sender, EventArgs e)
        {
            content = textEditor.Text;
            item.State = item.Content != content ? CodeItemState.Draft : CodeItemState.None;
        }

        #endregion

        #region Methods

        public void Save()
        {
            item.Content = textEditor.Text;
        }

        public void RefreshContent(string newContent)
        {
            textEditor.Text = newContent;
        }

      
        public void Find(bool replace, MyPluginControl myPluginControl)
        {
            FindAndReplaceForm.ShowForReplace(textEditor, replace);
        }

        public void GoToLine()
        {
            GoToLineDialog gotoLineForm = new GoToLineDialog(textEditor);
            gotoLineForm.ShowDialog();
            gotoLineForm.Activate();
        }

        public void MinifyJs()
        {
            try
            {
                textEditor.Text = DoMinifyJs(textEditor.Text);
            }
            catch (Exception error)
            {
                MessageBox.Show(ParentForm, "Error while minifying code: " + error.Message, "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void Beautify()
        {
            Beautifier b = new Beautifier(new BeautifierOptions
            {
                BraceStyle = BraceStyle.Expand,
                BreakChainedMethods = false,
                EvalCode = true,
                IndentChar = '\t',
                IndentSize = 1,
                IndentWithTabs = true,
                JslintHappy = true,
                KeepArrayIndentation = true,
                KeepFunctionIndentation = true,
                MaxPreserveNewlines = 1,
                PreserveNewlines = true
            });

            textEditor.Text = b.Beautify(textEditor.Text);
        }

        public void EnableFolding(bool enableFolding)
        {
            if (enableFolding)
            {
                switch (item.Type)
                {
                    case CodeItemType.JavaScript:
                    case CodeItemType.Style:
                        {
                            if (!foldingManagerInstalled)
                            {
                                foldingManager = FoldingManager.Install(textEditor.TextArea);
                                foldingManagerInstalled = true;
                            }

                            foldingManager.UpdateFoldings(CreateBraceFoldings(textEditor.Document), -1);
                        }
                        break;

                    case CodeItemType.LiquidTemplate:
                        {
                            if (!foldingManagerInstalled)
                            {
                                foldingManager = FoldingManager.Install(textEditor.TextArea);
                                foldingManagerInstalled = true;
                            }

                            htmlFoldingStrategy = new HtmlFoldingStrategy();
                            htmlFoldingStrategy.UpdateFoldings(foldingManager, textEditor.Document);
                        }
                        break;
                }

                FoldingEnabled = true;
            }
            else
            {
                if (foldingManager != null)
                {
                    foldingManager.Clear();
                }

                FoldingEnabled = false;
            }
        }

        public void CommentSelectedLines()
        {
            Comment(true);
        }

        public void UncommentSelectedLines()
        {
            Comment(false);
        }

        private void Comment(bool comment)
        {
            TextDocument document = textEditor.Document;
            DocumentLine start = document.GetLineByOffset(textEditor.SelectionStart);
            DocumentLine end = document.GetLineByOffset(textEditor.SelectionStart + textEditor.SelectionLength);

            // Specific comment behavior for JavaScript (//)
            if (item.Type == CodeItemType.JavaScript)
            {
                for (DocumentLine line = start; line?.LineNumber < end.LineNumber + 1; line = line.NextLine)
                {
                    if (comment)
                    {
                        if (document.GetText(line).Trim().StartsWith("//")) continue;

                        document.Insert(line.Offset, "//");
                    }
                    else
                    {
                        if (!document.GetText(line).Trim().StartsWith("//")) continue;

                        document.Remove(line.Offset, 2);
                    }
                }
            }

            // Specific comment for HTML, XML and XSLT (<!-- -->)
            if (item.Type == CodeItemType.LiquidTemplate)
            {
                var selectedText = textEditor.SelectedText.Trim();
                string line = document.GetText(document.GetLineByOffset(textEditor.SelectionStart)).Trim();

                if (comment && (selectedText.StartsWith("<!--") && selectedText.EndsWith("-->") || line.StartsWith("<!--") && line.EndsWith("-->")))
                {
                    return;
                }

                if (!comment && !selectedText.StartsWith("<!--") && !selectedText.EndsWith("-->") && !line.StartsWith("<!--") && !line.EndsWith("-->"))
                {
                    return;
                }

                if (comment)
                {
                    var numberOfCommentStarts = Regex.Matches(selectedText, "<!--").Count;
                    var numberOfCommentEnds = Regex.Matches(selectedText, "-->").Count;

                    if (numberOfCommentEnds != numberOfCommentStarts)
                    {
                        MessageBox.Show(ParentForm,
                            "You cannot comment this selection because the result will contain an orphan comment start or end tag",
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        return;
                    }

                    document.Insert(start.Offset, "<!--");
                    document.Insert(end.Offset + end.Length, "-->");
                }
                else
                {
                    document.Remove(start.Offset, 4);
                    document.Remove(end.Offset + end.Length - 3, 3);
                }
            }

            // Specific comment for Css (/* */)
            if (item.Type == CodeItemType.Style)
            {
                var selectedText = textEditor.SelectedText.Trim();
                string line = document.GetText(document.GetLineByOffset(textEditor.SelectionStart)).Trim();

                if (comment && selectedText.StartsWith("/*") && selectedText.EndsWith("*/") || line.StartsWith("/*") && line.EndsWith("*/"))
                {
                    return;
                }

                if (!comment && !selectedText.StartsWith("/*") && !selectedText.EndsWith("*/") && !line.StartsWith("/*") && !line.EndsWith("*/"))
                {
                    return;
                }

                if (comment)
                {
                    var numberOfCommentStarts = Regex.Matches(selectedText, "/\\*").Count;
                    var numberOfCommentEnds = Regex.Matches(selectedText, "\\*/").Count;

                    if (numberOfCommentEnds != numberOfCommentStarts)
                    {
                        MessageBox.Show(ParentForm,
                            "You cannot comment this selection because the result will contain an orphan comment start or end tag",
                            "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        return;
                    }

                    document.Insert(start.Offset, "/*");
                    document.Insert(end.Offset + end.Length, "*/");
                }
                else
                {
                    document.Remove(start.Offset, 2);
                    document.Remove(end.Offset + end.Length - 2, 2);
                }
            }
        }

        public IEnumerable<NewFolding> CreateBraceFoldings(ITextSource document)
        {
            List<NewFolding> newFoldings = new List<NewFolding>();

            Stack<int> startOffsets = new Stack<int>();
            int lastNewLineOffset = 0;
            char openingBrace = '{';
            char closingBrace = '}';
            for (int i = 0; i < document.TextLength; i++)
            {
                char c = document.GetCharAt(i);
                if (c == openingBrace)
                {
                    startOffsets.Push(i);
                }
                else if (c == closingBrace && startOffsets.Count > 0)
                {
                    int startOffset = startOffsets.Pop();
                    // don't fold if opening and closing brace are on the same line
                    if (startOffset < lastNewLineOffset)
                    {
                        newFoldings.Add(new NewFolding(startOffset, i + 1));
                    }
                }
                else if (c == '\n' || c == '\r')
                {
                    lastNewLineOffset = i + 1;
                }
            }
            newFoldings.Sort((a, b) => a.StartOffset.CompareTo(b.StartOffset));
            return newFoldings;
        }

        private string DoMinifyJs(string originalContent)
        {
            try
            {
                if (item.Type == CodeItemType.JavaScript)
                {
                    var compressor = new JavaScriptCompressor {ObfuscateJavascript = mySettings.ObfuscateJavascript};
                    return compressor.Compress(textEditor.Text);
                }

                if (item.Type == CodeItemType.Style)
                {
                    var compressor = new CssCompressor {RemoveComments = mySettings.RemoveCssComments};
                    return compressor.Compress(textEditor.Text);
                }

                return textEditor.Text;
            }
            catch(Exception error)
            {
                MessageBox.Show(error.ToString());
                return originalContent;
            }
        }

        #endregion
    }
}


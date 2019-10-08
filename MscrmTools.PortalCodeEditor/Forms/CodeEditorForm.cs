using Jsbeautifier;
using Microsoft.Xrm.Sdk;
using MscrmTools.PortalCodeEditor.AppCode;
using MscrmTools.PortalCodeEditor.AppCode.EventArgs;
using ScintillaNET;
using ScintillaNET_FindReplaceDialog;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
using Yahoo.Yui.Compressor;

namespace MscrmTools.PortalCodeEditor.Forms
{
    public partial class CodeEditorForm : DockContent
    {
        public CodeEditorForm()
        {
            InitializeComponent();
        }

        public event EventHandler<UpdateRequestedEventArgs> UpdateRequested;

        #region Variables

        private readonly FindReplace findReplace;
        private readonly CodeItem item;
        private readonly Settings mySettings;
        private string content;

        private IOrganizationService service;

        #endregion Variables

        #region Constructor

        public CodeEditorForm(CodeItem item, Settings mySettings, IOrganizationService service)
        {
            InitializeComponent();

            content = item.Content;
            this.item = item;
            item.StateChanged += ItemStateChanged;
            this.service = service;
            this.mySettings = mySettings;
            this.mySettings.OnColorChanged += (sender, e) =>
            {
                HighlightLiquidObjects();
                HighlightLiquidTags();
            };

            tsbBeautify.Visible = item.Type != CodeItemType.LiquidTemplate;
            tsbMinifyJS.Visible = item.Type != CodeItemType.LiquidTemplate;

            tslName.Text = $"{item.Parent.Name} {(item.Parent is WebPage ? (item.Type == CodeItemType.JavaScript ? "(JavaScript)" : item.Type == CodeItemType.Style ? "(Style)" : "(Content)") : "")}";
            TabText = tslName.Text;

            scintilla.Margins[0].Width = 50;

            scintilla.StyleResetDefault();
            scintilla.Styles[Style.Default].Font = "Consolas";
            scintilla.Styles[Style.Default].Size = 10;
            scintilla.StyleClearAll();

            switch (item.Type)
            {
                case CodeItemType.JavaScript:

                    // Configure the CPP (C#) lexer styles
                    scintilla.Styles[Style.Cpp.Default].ForeColor = Color.Silver;
                    scintilla.Styles[Style.Cpp.Comment].ForeColor = Color.FromArgb(0, 128, 0); // Green
                    scintilla.Styles[Style.Cpp.CommentLine].ForeColor = Color.FromArgb(0, 128, 0); // Green
                    scintilla.Styles[Style.Cpp.CommentLineDoc].ForeColor = Color.FromArgb(128, 128, 128); // Gray
                    scintilla.Styles[Style.Cpp.Number].ForeColor = Color.Black;
                    scintilla.Styles[Style.Cpp.Word].ForeColor = Color.Blue;
                    scintilla.Styles[Style.Cpp.Word2].ForeColor = Color.Blue;
                    scintilla.Styles[Style.Cpp.String].ForeColor = Color.FromArgb(163, 21, 21); // Red
                    scintilla.Styles[Style.Cpp.Character].ForeColor = Color.FromArgb(163, 21, 21); // Red
                    scintilla.Styles[Style.Cpp.Verbatim].ForeColor = Color.FromArgb(163, 21, 21); // Red
                    scintilla.Styles[Style.Cpp.StringEol].BackColor = Color.Pink;
                    scintilla.Styles[Style.Cpp.Operator].ForeColor = Color.Black;
                    scintilla.Styles[Style.Cpp.Preprocessor].ForeColor = Color.Black;
                    scintilla.Lexer = Lexer.Cpp;

                    // Set the keywords
                    scintilla.SetKeywords(0, "abstract as base break case catch checked continue default delegate do else event explicit extern false finally fixed for foreach function goto if implicit in interface internal is lock namespace new null object operator out override params private protected public readonly ref return sealed sizeof stackalloc switch this throw true try typeof unchecked unsafe using var virtual while");
                    scintilla.SetKeywords(1, "bool byte char class const decimal double enum float int long sbyte short static string struct uint ulong ushort void");
                    break;

                case CodeItemType.Style:

                    scintilla.Styles[Style.Css.Directive].ForeColor = Color.Red;
                    scintilla.Styles[Style.Css.Variable].ForeColor = Color.Red;
                    scintilla.Styles[Style.Css.Comment].ForeColor = Color.FromArgb(0, 128, 0); // Green
                    scintilla.Styles[Style.Css.Attribute].ForeColor = Color.Red;
                    scintilla.Styles[Style.Css.Class].ForeColor = Color.FromArgb(128, 0, 0);
                    scintilla.Styles[Style.Css.Id].ForeColor = Color.FromArgb(128, 0, 0);
                    scintilla.Styles[Style.Css.DoubleString].ForeColor = Color.Blue;
                    scintilla.Styles[Style.Css.Important].ForeColor = Color.Blue;
                    scintilla.Styles[Style.Css.SingleString].ForeColor = Color.Blue;
                    scintilla.Styles[Style.Css.Value].ForeColor = Color.Blue;
                    scintilla.Styles[Style.Css.Media].ForeColor = Color.Blue;
                    scintilla.Styles[Style.Css.Tag].ForeColor = Color.FromArgb(128, 0, 0);
                    scintilla.Styles[Style.Css.Operator].ForeColor = Color.FromArgb(128, 0, 0);
                    scintilla.Styles[Style.Css.PseudoClass].ForeColor = Color.FromArgb(128, 0, 0);
                    scintilla.Styles[Style.Css.PseudoElement].ForeColor = Color.FromArgb(128, 0, 0);
                    scintilla.Styles[Style.Css.UnknownPseudoClass].ForeColor = Color.FromArgb(128, 0, 0);
                    scintilla.Styles[Style.Css.ExtendedIdentifier].ForeColor = Color.FromArgb(128, 0, 0);
                    scintilla.Styles[Style.Css.ExtendedPseudoClass].ForeColor = Color.FromArgb(128, 0, 0);
                    scintilla.Styles[Style.Css.ExtendedPseudoElement].ForeColor = Color.FromArgb(128, 0, 0);
                    scintilla.Styles[Style.Css.UnknownIdentifier].ForeColor = Color.Red;
                    scintilla.Styles[Style.Css.Identifier].ForeColor = Color.Red;
                    scintilla.Styles[Style.Css.Identifier2].ForeColor = Color.Red;
                    scintilla.Styles[Style.Css.Identifier3].ForeColor = Color.Red;

                    scintilla.Lexer = Lexer.Css;
                    break;

                case CodeItemType.LiquidTemplate:

                    scintilla.Styles[Style.Html.Asp].ForeColor = Color.Black;
                    scintilla.Styles[Style.Html.Asp].BackColor = Color.Yellow;
                    scintilla.Styles[Style.Html.AspAt].ForeColor = Color.Black;
                    scintilla.Styles[Style.Html.AspAt].BackColor = Color.Yellow;
                    scintilla.Styles[Style.Html.AttributeUnknown].ForeColor = Color.Red;
                    scintilla.Styles[Style.Html.Attribute].ForeColor = Color.Red;
                    scintilla.Styles[Style.Html.CData].ForeColor = Color.Blue;
                    scintilla.Styles[Style.Html.Comment].ForeColor = Color.Green;
                    scintilla.Styles[Style.Html.Default].ForeColor = Color.Black;
                    scintilla.Styles[Style.Html.DoubleString].ForeColor = Color.Blue;
                    scintilla.Styles[Style.Html.Other].ForeColor = Color.FromArgb(128, 0, 0);
                    scintilla.Styles[Style.Html.Script].ForeColor = Color.FromArgb(128, 0, 0);
                    scintilla.Styles[Style.Html.SingleString].ForeColor = Color.Blue;
                    scintilla.Styles[Style.Html.Tag].ForeColor = Color.FromArgb(128, 0, 0);
                    scintilla.Styles[Style.Html.TagEnd].ForeColor = Color.FromArgb(128, 0, 0);
                    scintilla.Styles[Style.Html.XcComment].ForeColor = Color.Green;
                    scintilla.Styles[Style.Html.XmlStart].ForeColor = Color.Blue;
                    scintilla.Styles[Style.Html.XmlEnd].ForeColor = Color.Blue;

                    scintilla.Lexer = Lexer.Html;
                    break;
            }

            // Instruct the lexer to calculate folding
            scintilla.SetProperty("fold", "1");
            scintilla.SetProperty("fold.compact", "1");
            scintilla.SetProperty("fold.html", "1");

            // Configure a margin to display folding symbols
            scintilla.Margins[2].Type = MarginType.Symbol;
            scintilla.Margins[2].Mask = Marker.MaskFolders;
            scintilla.Margins[2].Sensitive = true;
            scintilla.Margins[2].Width = 20;

            // Set colors for all folding markers
            for (int i = 25; i <= 31; i++)
            {
                scintilla.Markers[i].SetForeColor(SystemColors.ControlLightLight);
                scintilla.Markers[i].SetBackColor(SystemColors.ControlDark);
            }

            // Configure folding markers with respective symbols
            scintilla.Markers[Marker.Folder].Symbol = MarkerSymbol.BoxPlus;
            scintilla.Markers[Marker.FolderOpen].Symbol = MarkerSymbol.BoxMinus;
            scintilla.Markers[Marker.FolderEnd].Symbol = MarkerSymbol.BoxPlusConnected;
            scintilla.Markers[Marker.FolderMidTail].Symbol = MarkerSymbol.TCorner;
            scintilla.Markers[Marker.FolderOpenMid].Symbol = MarkerSymbol.BoxMinusConnected;
            scintilla.Markers[Marker.FolderSub].Symbol = MarkerSymbol.VLine;
            scintilla.Markers[Marker.FolderTail].Symbol = MarkerSymbol.LCorner;

            findReplace = new FindReplace();
            findReplace.Scintilla = scintilla;
            findReplace.KeyPressed += MyFindReplace_KeyPressed;

            ManageCmdKeys();
        }

        private void ItemStateChanged(object sender, EventArgs e)
        {
            Invoke(new Action(() =>
            {
                var ci = (CodeItem)sender;
                tslName.Text = $"{ci.Parent.Name} {(ci.Parent is WebPage ? (ci.Type == CodeItemType.JavaScript ? "(JavaScript)" : ci.Type == CodeItemType.Style ? "(Style)" : "(Content)") : "")}";
                switch (ci.State)
                {
                    case CodeItemState.None:
                        TabText = ci.Parent.Name;
                        tslName.ForeColor = Color.Black;
                        tsmiSave.Enabled = false;
                        tsmiUpdate.Enabled = false;
                        break;

                    case CodeItemState.Draft:
                        TabText = $"{ci.Parent.Name}*";
                        tslName.ForeColor = Color.Red;
                        tslName.Text += " - not saved";

                        tsmiSave.Enabled = true;
                        tsmiUpdate.Enabled = false;
                        break;

                    case CodeItemState.Saved:
                        TabText = $"{ci.Parent.Name}!";
                        tslName.ForeColor = Color.Blue;
                        tslName.Text += " - not updated";
                        tsmiSave.Enabled = false;
                        tsmiUpdate.Enabled = true;
                        break;
                }
            }));
        }

        #endregion Constructor

        #region Properties

        public CodeItem Item => item;

        #endregion Properties

        #region Events

        private void CodeEditor_Load(object sender, EventArgs e)
        {
            scintilla.Text = content;

            scintilla.Margins[0].Width = scintilla.Lines.Count.ToString().Length * 12;

            HighlightLiquidTags();
            HighlightLiquidObjects();
        }

        /// <summary>
        /// Key down event for each Scintilla. Tie each Scintilla to this event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void genericScintilla_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.F)
            {
                findReplace.ShowFind();
                e.SuppressKeyPress = true;
            }
            else if (e.Shift && e.KeyCode == Keys.F3)
            {
                findReplace.Window.FindPrevious();
                e.SuppressKeyPress = true;
            }
            else if (e.KeyCode == Keys.F3)
            {
                findReplace.Window.FindNext();
                e.SuppressKeyPress = true;
            }
            else if (e.Control && e.KeyCode == Keys.H)
            {
                findReplace.ShowReplace();
                e.SuppressKeyPress = true;
            }
            else if (e.Control && e.KeyCode == Keys.I)
            {
                findReplace.ShowIncrementalSearch();
                e.SuppressKeyPress = true;
            }
            else if (e.Control && e.KeyCode == Keys.G)
            {
                GoTo MyGoTo = new GoTo((Scintilla)sender);
                MyGoTo.ShowGoToDialog();
                e.SuppressKeyPress = true;
            }
        }

        private void MyFindReplace_KeyPressed(object sender, KeyEventArgs e)
        {
            genericScintilla_KeyDown(sender, e);
        }

        private void scintilla_TextChanged(object sender, EventArgs e)
        {
            content = scintilla.Text;
            item.State = item.Content != content ? CodeItemState.Draft : CodeItemState.None;

            scintilla.Margins[0].Width = scintilla.Lines.Count.ToString().Length * 12;

            HighlightLiquidTags();
            HighlightLiquidObjects();
        }

        #endregion Events

        #region Methods

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

            scintilla.Text = b.Beautify(scintilla.Text);
        }

        public void CommentSelectedLines()
        {
            Comment(true);
        }

        public void Find(bool replace, MyPluginControl myPluginControl)
        {
            if (replace)
                findReplace.ShowReplace();
            else
                findReplace.ShowFind();
        }

        public void GoToLine()
        {
            GoTo goTo = new GoTo(findReplace.Scintilla);
            goTo.ShowGoToDialog();
        }

        public void MinifyJs()
        {
            try
            {
                scintilla.Text = DoMinifyJs(scintilla.Text);
            }
            catch (Exception error)
            {
                MessageBox.Show(ParentForm, "Error while minifying code: " + error.Message, "Error",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void RefreshContent(string newContent)
        {
            scintilla.Text = newContent;
        }

        public void Save()
        {
            item.Content = scintilla.Text;
            item.State = CodeItemState.Saved;
        }

        public void UncommentSelectedLines()
        {
            Comment(false);
        }

        public void UpdateItem()
        {
            UpdateRequested?.Invoke(this, new UpdateRequestedEventArgs(item));
        }

        private void Comment(bool comment)
        {
            int start = scintilla.SelectionStart;
            int end = scintilla.SelectionEnd;

            if (item.Type == CodeItemType.JavaScript)
            {
                foreach (var line in scintilla.Lines.Where(l => l.Position <= start && l.EndPosition > start
                || l.Position >= start && l.EndPosition < end
                || l.Position <= end && l.EndPosition > end))
                {
                    if (comment)
                    {
                        scintilla.InsertText(line.Position, "//");
                    }
                    else
                    {
                        var i = line.Text.IndexOf("//", StringComparison.Ordinal);
                        if (i >= 0)
                        {
                            scintilla.DeleteRange(line.Position + i, 2);
                        }
                    }
                }
            }
            else if (item.Type == CodeItemType.Style)
            {
                var startLine = scintilla.Lines.First(l => l.Position <= start && l.EndPosition > start);
                var endLine = start == end ? startLine : scintilla.Lines.First(l => l.Position < end && l.EndPosition >= end);
                DoCommentWithStartAndEndTags(comment, startLine, endLine, "/*", "*/");
            }
            else if (item.Type == CodeItemType.LiquidTemplate)
            {
                if (comment)
                {
                    var indexOfComment = scintilla.Text.IndexOf("<!--", start, StringComparison.Ordinal);
                    if (indexOfComment >= 0 && indexOfComment < end)
                    {
                        MessageBox.Show(this, "Cannot comment a block that already contains a comment", "Warning",
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                var startLine = scintilla.Lines.First(l => l.Position <= start && l.EndPosition > start);
                var endLine = start == end ? startLine : scintilla.Lines.First(l => l.Position < end && l.EndPosition >= end);
                DoCommentWithStartAndEndTags(comment, startLine, endLine, "<!--", "-->");
            }
        }

        private void DoCommentWithStartAndEndTags(bool comment, Line startLine, Line endLine, string startString, string endString)
        {
            if (comment)
            {
                scintilla.InsertText(startLine.Position, startString);
                scintilla.InsertText(endLine.EndPosition - 1, endString);
            }
            else
            {
                Line tempLine = startLine;
                int i = tempLine.Text.IndexOf(startString, StringComparison.Ordinal);
                while (i < 0)
                {
                    tempLine = scintilla.Lines[tempLine.Index - 1];
                    if (tempLine.Index == 0)
                    {
                        break;
                    }

                    i = tempLine.Text.IndexOf(startString, StringComparison.Ordinal);
                }

                if (i < 0)
                {
                    tempLine = startLine;
                    while (i < 0)
                    {
                        tempLine = scintilla.Lines[tempLine.Index + 1];
                        if (tempLine.Index > endLine.Index)
                        {
                            break;
                        }

                        i = tempLine.Text.IndexOf(startString, StringComparison.Ordinal);
                    }
                }

                if (i < 0)
                {
                    return;
                }

                scintilla.DeleteRange(tempLine.Position + i, startString.Length);

                tempLine = endLine;
                i = tempLine.Text.IndexOf(endString, StringComparison.Ordinal);
                while (i < 0)
                {
                    tempLine = scintilla.Lines[tempLine.Index + 1];
                    if (tempLine.Index == scintilla.Lines.Count - 1)
                    {
                        break;
                    }

                    i = tempLine.Text.IndexOf(endString, StringComparison.Ordinal);
                }

                if (i < 0)
                {
                    tempLine = endLine;
                    i = tempLine.Text.IndexOf(endString, StringComparison.Ordinal);

                    while (i < 0)
                    {
                        tempLine = scintilla.Lines[tempLine.Index - 1];

                        if (tempLine.Index < startLine.Index)
                        {
                            break;
                        }

                        i = tempLine.Text.IndexOf(endString, StringComparison.Ordinal);
                    }
                }

                if (i < 0)
                {
                    MessageBox.Show(this, "Unable to find Comment end tag", "Error", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                scintilla.DeleteRange(tempLine.Position + i, endString.Length);
            }
        }

        private string DoMinifyJs(string originalContent)
        {
            try
            {
                if (item.Type == CodeItemType.JavaScript)
                {
                    var compressor = new JavaScriptCompressor { ObfuscateJavascript = mySettings.ObfuscateJavascript };
                    return compressor.Compress(scintilla.Text);
                }

                if (item.Type == CodeItemType.Style)
                {
                    var compressor = new CssCompressor { RemoveComments = mySettings.RemoveCssComments };
                    return compressor.Compress(scintilla.Text);
                }

                return scintilla.Text;
            }
            catch (Exception error)
            {
                MessageBox.Show(error.ToString());
                return originalContent;
            }
        }

        private void HighlightLiquidObjects()
        {
            string text = "{{";
            // Indicators 0-7 could be in use by a lexer
            // so we'll use indicator 8 to highlight words.
            const int NUM = 22;

            // Remove all uses of our indicator
            scintilla.IndicatorCurrent = NUM;
            scintilla.IndicatorClearRange(0, scintilla.TextLength);

            // Update indicator appearance
            scintilla.Indicators[NUM].Style = IndicatorStyle.StraightBox;
            scintilla.Indicators[NUM].Under = true;
            scintilla.Indicators[NUM].ForeColor = ColorTranslator.FromHtml(mySettings.LiquidObjectColor);
            scintilla.Indicators[NUM].OutlineAlpha = 50;
            scintilla.Indicators[NUM].Alpha = 30;

            // Search the document
            scintilla.TargetStart = 0;
            scintilla.TargetEnd = scintilla.TextLength;
            scintilla.SearchFlags = SearchFlags.None;
            while (scintilla.SearchInTarget(text) != -1)
            {
                var start = scintilla.TargetStart;
                var end = scintilla.TargetEnd;

                scintilla.TargetStart = end;
                scintilla.TargetEnd = scintilla.TextLength;

                if (scintilla.SearchInTarget("}}") == -1)
                {
                    // Search the remainder of the document
                    scintilla.TargetStart = end;
                    scintilla.TargetEnd = scintilla.TextLength;
                    continue;
                }
                var endEnd = scintilla.TargetEnd;

                // Mark the search results with the current indicator
                scintilla.IndicatorFillRange(start, endEnd - start);

                scintilla.TargetStart = endEnd;
                scintilla.TargetEnd = scintilla.TextLength;
            }
        }

        private void HighlightLiquidTags()
        {
            var text = "{%";

            // Indicators 0-7 could be in use by a lexer
            // so we'll use indicator 8 to highlight words.
            const int NUM = 21;

            // Remove all uses of our indicator
            scintilla.IndicatorCurrent = NUM;
            scintilla.IndicatorClearRange(0, scintilla.TextLength);

            // Update indicator appearance
            scintilla.Indicators[NUM].Style = IndicatorStyle.StraightBox;
            scintilla.Indicators[NUM].Under = true;
            scintilla.Indicators[NUM].ForeColor = ColorTranslator.FromHtml(mySettings.LiquidTagColor);
            scintilla.Indicators[NUM].OutlineAlpha = 50;
            scintilla.Indicators[NUM].Alpha = 30;

            // Search the document
            scintilla.TargetStart = 0;
            scintilla.TargetEnd = scintilla.TextLength;
            scintilla.SearchFlags = SearchFlags.None;
            while (scintilla.SearchInTarget(text) != -1)
            {
                var start = scintilla.TargetStart;
                var end = scintilla.TargetEnd;

                scintilla.TargetStart = end;
                scintilla.TargetEnd = scintilla.TextLength;

                if (scintilla.SearchInTarget("%}") == -1)
                {
                    // Search the remainder of the document
                    scintilla.TargetStart = end;
                    scintilla.TargetEnd = scintilla.TextLength;
                    continue;
                }
                var endEnd = scintilla.TargetEnd;

                // Mark the search results with the current indicator
                scintilla.IndicatorFillRange(start, endEnd - start);

                scintilla.TargetStart = endEnd;
                scintilla.TargetEnd = scintilla.TextLength;
            }
        }

        private void ManageCmdKeys()
        {
            scintilla.ClearCmdKey(Keys.Control | Keys.S);
            scintilla.ClearCmdKey(Keys.Control | Keys.U);
            scintilla.ClearCmdKey(Keys.Control | Keys.F);
            scintilla.ClearCmdKey(Keys.Control | Keys.G);
            scintilla.ClearCmdKey(Keys.Control | Keys.H);
            scintilla.ClearCmdKey(Keys.Control | Keys.K);
            //scintilla.ClearCmdKey(Keys.Control | Keys.C);
            scintilla.ClearCmdKey(Keys.Control | Keys.U);
            scintilla.AssignCmdKey(Keys.Shift | Keys.Delete, Command.LineDelete);
        }

        #endregion Methods

        private void CodeEditorForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (item.State == CodeItemState.Draft)
            {
                var result = MessageBox.Show(this,
                    "This item has unsaved content!\n\nAre your sure you want to close it?\n\nYou will loose unsaved content",
                    "Unsaved content", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                e.Cancel = result == DialogResult.No;

                if (!e.Cancel)
                {
                    item.State = CodeItemState.None;
                }
            }

            item.StateChanged -= ItemStateChanged;
        }

        private void tsCodeContent_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem == tsbMinifyJS)
            {
                MinifyJs();
            }
            else if (e.ClickedItem == tsbBeautify)
            {
                Beautify();
            }
            else if (e.ClickedItem == tsbComment)
            {
                CommentSelectedLines();
            }
            else if (e.ClickedItem == tsbnUncomment)
            {
                UncommentSelectedLines();
            }
            else if (e.ClickedItem == tsbGetLatestVersion)
            {
                item.Refresh(service);
                RefreshContent(item.Content);
            }
        }

        private void tsddbEdit_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem == tsmiFind)
            {
                Find(false, null);
            }
            else if (e.ClickedItem == tsmiReplace)
            {
                Find(true, null);
            }
            else if (e.ClickedItem == tsmiGoToLine)
            {
                GoToLine();
            }
        }

        private void tsddbFile_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem == tsmiSave)
            {
                Save();
            }
            else if (e.ClickedItem == tsmiUpdate)
            {
                UpdateItem();
            }
        }
    }
}
using Infragistics.Controls.Editors;
using Infragistics.Documents.RichText;
using Infragistics.Windows.Ribbon;
using PrismOutlook.Core;
using System;
using System.Linq;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace PrismOutlook.Modules.Mail.Menus
{
    /// <summary>
    /// Interaction logic for MessageTab.xaml
    /// </summary>
    public partial class MessageTab : ISupportDataContext, ISupportRichText
    {
        private XamRichTextEditor _richTextEditor;
        private bool _updatingState;

        public static double[] FontSizes
        {
            get
            {
                return new double[] {
                    3.0, 4.0, 5.0, 6.0, 6.5, 7.0, 7.5, 8.0, 8.5, 9.0, 9.5,
                    10.0, 10.5, 11.0, 11.5, 12.0, 12.5, 13.0, 13.5, 14.0, 15.0,
                    16.0, 17.0, 18.0, 19.0, 20.0, 22.0, 24.0, 26.0, 28.0, 30.0,
                    32.0, 34.0, 36.0, 38.0, 40.0, 44.0, 48.0, 52.0, 56.0, 60.0, 64.0, 68.0, 72.0, 76.0,
                    80.0, 88.0, 96.0, 104.0, 112.0, 120.0, 128.0, 136.0, 144.0
                    };
            }
        }

        public XamRichTextEditor RichTextEditor
        {
            get { return _richTextEditor; }
            set
            {
                if (_richTextEditor != null)
                {
                    _richTextEditor.Loaded -= RichTextEditor_Loaded;
                    _richTextEditor.SelectionChanged -= RichTextEditor_SelectionChanged;
                }

                _richTextEditor = value;

                if (_richTextEditor != null)
                {
                    _richTextEditor.Loaded += RichTextEditor_Loaded;
                    _richTextEditor.SelectionChanged += RichTextEditor_SelectionChanged;
                }
            }
        }

        public MessageTab()
        {
            InitializeComponent();
            SetResourceReference(StyleProperty, typeof(RibbonTabItem));

            _fontSizes.ItemsSource = FontSizes;
            _fontNames.ItemsSource = Fonts.SystemFontFamilies.ToList().Select(x => x.Source);
        }

        private void FontSizes_SelectedItemChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<object> e)
        {
            if (_updatingState)
                return;

            RichTextEditor.Selection.ApplyFontSize((double)e.NewValue);
        }

        private void FontNames_SelectedItemChanged(object sender, System.Windows.RoutedPropertyChangedEventArgs<object> e)
        {
            if (_updatingState)
                return;

            if (e.NewValue == null)
                return;

            var fontName = (string)e.NewValue;

            RichTextEditor.Selection.ApplyFont(new RichTextFont(fontName));
        }

        private void RichTextEditor_SelectionChanged(object sender, RichTextSelectionChangedEventArgs e)
        {
            UpdateVisualState();
        }

        private void RichTextEditor_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            UpdateVisualState();
        }

        void UpdateVisualState()
        {
            _updatingState = true;

            DocumentSpan docSpan = RichTextEditor.Selection == null ? new DocumentSpan(0,0) : RichTextEditor.Selection.DocumentSpan;
            var settings = RichTextEditor.Document.GetCommonCharacterSettings(docSpan);

            if (settings == null)
            {
                _updatingState = false;
                return;
            }

            UpdateFontSizes(settings);
            UpdateFontFamily(settings);

            UpdateToggleButton(_boldButton, settings.Bold);
            UpdateToggleButton(_italicButton, settings.Italics);
            UpdateUnderlineState(settings);

            UpdateAlignment(docSpan);
            UpdateParagraphListStyleState(docSpan);

            _updatingState = false;
        }

        private void UpdateAlignment(DocumentSpan docSpan)
        {
            ParagraphSettings pSettings = RichTextEditor.Document.GetCommonParagraphSettings(docSpan);
            if (pSettings.ParagraphAlignment.HasValue)
            {
                var alignment = pSettings.ParagraphAlignment.Value;
                switch (alignment)
                {
                    case ParagraphAlignment.Start:
                        {
                            UpdateToggleButton(_alignLeft, true);
                            break;
                        }
                    case ParagraphAlignment.Center:
                        {
                            UpdateToggleButton(_alignCenter, true);
                            break;
                        }
                    case ParagraphAlignment.End:
                        {
                            UpdateToggleButton(_alignRight, true);
                            break;
                        }
                    case ParagraphAlignment.Justify:
                        {
                            UpdateToggleButton(_alignJustify, true);
                            break;
                        }
                }
            }
        }

        void UpdateUnderlineState(CharacterSettings settings)
        {
            if (settings.UnderlineType.HasValue)
                UpdateToggleButton(_underlineButton, settings.UnderlineType.Value != UnderlineType.None);
        }

        void UpdateToggleButton(ToggleButton button, bool? value)
        {
            button.IsChecked = value.HasValue ? value.Value : false;
        }

        void UpdateFontSizes(CharacterSettings settings)
        {
            _fontSizes.Value = settings.FontSize.HasValue ? settings.FontSize.Value.Points : 12.0;
        }

        void UpdateFontFamily(CharacterSettings settings)
        {
            if (settings.FontSettings == null)
                return;

            _fontNames.Value = (settings.FontSettings.Ascii.HasValue && !string.IsNullOrWhiteSpace(settings.FontSettings.Ascii.Value.Name))
                                ? settings.FontSettings.Ascii.Value.Name : "Arial";
        }

        private void BulletsButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            SetSelectionParagraphListStyle(_bulletsButton);
        }

        private void NumbersButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            SetSelectionParagraphListStyle(_numbersButton);
        }

        private void SetSelectionParagraphListStyle(ToggleButton toggleButton)
        {
            if (toggleButton.IsChecked == true)
                RichTextEditor.Selection.ApplyParagraphListStyle(toggleButton.Tag.ToString());
            else
                RichTextEditor.Selection.ClearParagraphListStyle();
        }

        void UpdateParagraphListStyleState(DocumentSpan documentSpan)
        {
            string listStyleId = RichTextEditor.Document.GetCommonParagraphListStyle(documentSpan);
            UpdateToggleButton(_bulletsButton, _bulletsButton.Tag.ToString() == listStyleId);
            UpdateToggleButton(_numbersButton, _numbersButton.Tag.ToString() == listStyleId);
        }
    }
}


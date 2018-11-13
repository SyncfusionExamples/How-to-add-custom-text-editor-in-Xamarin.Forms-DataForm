using Syncfusion.XForms.DataForm;
using Syncfusion.XForms.DataForm.Editors;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace DataFormCustomTextEditor
{
    public class DataFormBehavior : Behavior<ContentPage>
    {
        SfDataForm dataForm;
        protected override void OnAttachedTo(ContentPage bindable)
        {
            base.OnAttachedTo(bindable);

            dataForm = bindable.FindByName<SfDataForm>("dataForm");
            dataForm.DataObject = new Company();
            dataForm.ValidationMode = ValidationMode.LostFocus;
            dataForm.RegisterEditor("TextBoxEditor", new CustomEntryEditor(dataForm));
            dataForm.RegisterEditor("Name", "TextBoxEditor");
            dataForm.LayoutManager = new DataFormLayoutManagerExt(dataForm);
        }  
    }

    #region DataFormLayoutManagerExt
    public class DataFormLayoutManagerExt : DataFormLayoutManager
    {
        public DataFormLayoutManagerExt(SfDataForm dataForm) : base(dataForm)
        {
        }
        protected override View GenerateViewForLabel(DataFormItem dataFormItem)
        {
            var view = base.GenerateViewForLabel(dataFormItem);
            var textView = (view as Label);
            textView.TextColor = Color.Blue;
            return view;
        }
        protected override void OnEditorCreated(DataFormItem dataFormItem, View editor)
        {
            if (dataFormItem.Name == "Address")
            {
                (editor as Entry).TextColor = Color.YellowGreen;
                (editor as Entry).Placeholder = "Chennai";
                (editor as Entry).PlaceholderColor = Color.Purple;
                base.OnEditorCreated(dataFormItem, editor);
            }
            else
            {
                base.OnEditorCreated(dataFormItem, editor);
            }
        }
    }
    #endregion

    public class Company
    {
        [Required(AllowEmptyStrings = false, ErrorMessage = "Name should not be empty")]
        public string Name { get; set; }
        public string Address { get; set; }
    }

    #region CustomEntryEditor
    public class CustomEntryEditor : DataFormEditor<CustomEntry>
    {
        public CustomEntryEditor(SfDataForm dataForm) : base(dataForm)
        {
        }
        protected override CustomEntry OnCreateEditorView()
        {
            return new CustomEntry();
        }
        protected override void OnInitializeView(DataFormItem dataFormItem, CustomEntry view)
        {
            view.BackgroundColor = Color.Pink;
            view.Placeholder = "Enter value";
            view.PlaceholderColor = Color.DarkBlue;
            view.Text = "Syncfusion";
        }

        protected override void OnWireEvents(CustomEntry view)
        {
            view.TextChanged += View_TextChanged;
            view.Focused += View_Focused;
            view.Unfocused += View_Unfocused;
        }

        private void View_Focused(object sender, FocusEventArgs e)
        {
            var view = (sender as CustomEntry);
            view.TextColor = Color.Green;
        }

        protected override bool OnValidateValue(CustomEntry view)
        {
            return this.DataForm.Validate("Name");
        }
        private void View_Unfocused(object sender, FocusEventArgs e)
        {
            var view = (sender as CustomEntry);
            view.TextColor = Color.Red;
            OnValidateValue(sender as CustomEntry);
        }
        private void View_TextChanged(object sender, TextChangedEventArgs e)
        {
            OnCommitValue(sender as CustomEntry);
        }

        protected override void OnCommitValue(CustomEntry view)
        {
            var dataFormItemView = view.Parent as DataFormItemView;
            var textValue = view.Text;
            this.DataForm.ItemManager.SetValue(dataFormItemView.DataFormItem, view.Text);
        }

        protected override void OnUnWireEvents(CustomEntry view)
        {
            view.TextChanged -= View_TextChanged;
            view.Focused -= View_Focused;
            view.Unfocused -= View_Unfocused;
        }
    }

    public class CustomEntry : Entry
    {
        public CustomEntry()
        {

        }
    }
    #endregion
}

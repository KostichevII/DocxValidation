using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DocxValidation
{
    public partial class MenuForm : Form
    {
        public MenuForm()
        {
            InitializeComponent();
        }

        private void Btemplates_Click(object sender, EventArgs e)
        {

        }

        private void Bvalidation_Click(object sender, EventArgs e)
        {
            //Hide();
            OpenOrCreateForm<ValidationForm>();
        }

        public static T OpenOrCreateForm<T>() where T : Form, new()
        {
            T result;

            // Test if form exists
            foreach (Form form in Application.OpenForms)
            {
                result = form as T;

                if (!Object.ReferenceEquals(null, result))
                {
                    // Form found; and this is the right place 
                    //  to restore form size,
                    //  bring form to front etc.
                    if (result.WindowState == FormWindowState.Minimized)
                        result.WindowState = FormWindowState.Normal;

                    result.BringToFront();

                    return result;
                }
            }

            // Form doesn't exist, let's create it
            result = new T();
            // Probably, you want to show the created form
            result.Show();

            return result;
        }

        private void BJornal_Click(object sender, EventArgs e)
        {
            OpenOrCreateForm<JornalForm>();
        }

        private void Bhistory_Click(object sender, EventArgs e)
        {

        }

        private void Bsettings_Click(object sender, EventArgs e)
        {

        }

        private void Bguide_Click(object sender, EventArgs e)
        {

        }
    }
}

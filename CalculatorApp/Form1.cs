using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CalculatorApp
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            var files = Directory.EnumerateFiles(".", "SBF*.dll", SearchOption.TopDirectoryOnly)
            .Where(s => s.EndsWith(".dll"));
             
            foreach (string s in files)
            {
                string s1 = s.Substring(2);
                cmbDllName.Items.Add(s1);
            }
             
        }

        private void btnExecute_Click(object sender, EventArgs e)
        {
            if (cmbDllName.Text == string.Empty)
            {
                MessageBox.Show("Please select DLL File name");
                return;
            }
             
            if (txtFuncName.Text == string.Empty)
            {
                MessageBox.Show("Please enter function name");
                return;
            }

            if (txtClassName.Text == string.Empty)
            {
                MessageBox.Show("Please enter class name");
                return;
            }

            string dllFile = cmbDllName.Text;
            try
            {
                string curDir = Directory.GetCurrentDirectory();
                string fullPathDllFile = curDir + "\\" + dllFile;
                var assembly = Assembly.LoadFile(fullPathDllFile);
                string functionType = $"SBF.{txtClassName.Text}";
                var type = assembly.GetType(functionType);
                if (type == null)
                {
                    MessageBox.Show("Failed to find function " + txtFuncName + " in namespace SBF file:" + dllFile);
                }
                var obj = Activator.CreateInstance(type);
                var method = type.GetMethod(txtFuncName.Text);
                object result = method.Invoke(obj, new object[] { 1, 2 });
                string strResult = result.ToString();
                MessageBox.Show(strResult);
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }
    }
}
